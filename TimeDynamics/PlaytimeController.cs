using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31.ZestKit;

namespace Unitilities.TimeDynamics {

	public class PlaytimeController : MonoBehaviour, ITweenTarget<float> {
		
		private float _fixedDeltaTime;
		private float _timeScale = 1f;
		private float _defaultScale;
		
		// FPS tracking
		public float _updateRateSeconds = 4f;
	    private int _frameCount = 0;
	    private float _dt = 0f;
	    private float _fps = 0f;

		private Playtime _playtime;

		public void setTweenedValue(float value) {
			SetTimeScale(value);
			_playtime.SetTimescaleModifier(value);
		}
		public float getTweenedValue() {
			return _timeScale;
		}
		public object getTargetObject() {
			return this;
		}
		public void AddBounce(float duration, float timeScale = 0f, EaseType easeType = EaseType.ElasticOut) {
			SetTimeScale(timeScale);
			new FloatTween(this, _timeScale, _defaultScale, duration)
				.setIsTimeScaleIndependent()
				.start();
		}
		public void AddFreeze(float duration, float timeScale = 0f) {
			SetTimeScale(timeScale);
			new FloatTween(this, _timeScale, _timeScale, duration)
				.setIsTimeScaleIndependent()
				.setContext( this )
				.setCompletionHandler( tween =>	{
					var self = (PlaytimeController)tween.context;
					self.Reset();
				} )
				.start();
		}
		public void AddFreezeFrames(int frames, float timeScale = 0f) {
			float duration = 1f / _fps * (float)frames;
			AddFreeze(duration, timeScale);
		}

		private void Reset() {
			UnityEngine.Time.timeScale = _timeScale = _defaultScale;
			_playtime.Reset(_defaultScale);
		}

		private void SetTimeScale(float value) {
			_timeScale = Mathf.Clamp(value, 0.0001f, 100f);
		}
		private void ApplyTimeScale(float timeScale) {
			UnityEngine.Time.timeScale = timeScale;
			UnityEngine.Time.fixedDeltaTime = _fixedDeltaTime * UnityEngine.Time.timeScale;
		}
	    void Awake() {
	        _fixedDeltaTime = UnityEngine.Time.fixedDeltaTime;
			_defaultScale = UnityEngine.Time.timeScale;
			_playtime = new Playtime(_defaultScale);
	    }
		private void LateUpdate() {
			ApplyTimeScale(_playtime.GetTimescaleModifier());
			// GraphDbg.Log(Time.timeScale);
		}



	    void Update() {
	        // if (Input.GetKeyDown(KeyCode.A))  AddBounce(2f, 2f);
	        // if (Input.GetKeyDown(KeyCode.S))  AddBounce(1f, 0.3f, EaseType.BounceIn);
	        // if (Input.GetKeyDown(KeyCode.D))  AddBounce(0.05f, 0.3f, EaseType.QuintInOut);

	        // if (Input.GetKeyDown(KeyCode.Q))  AddFreeze(2f, 2f);
	        // if (Input.GetKeyDown(KeyCode.W))  AddFreeze(1f, 0.3f);
	        // if (Input.GetKeyDown(KeyCode.E))  AddFreeze(0.05f);

	        // if (Input.GetKeyDown(KeyCode.Z))  AddFreezeFrames(60, 2f);
	        // if (Input.GetKeyDown(KeyCode.X))  AddFreezeFrames(30, 0.3f);
	        // if (Input.GetKeyDown(KeyCode.C))  AddFreezeFrames(10);

			

			_frameCount++;
			_dt += UnityEngine.Time.unscaledDeltaTime;
			if (_dt > 1.0 / _updateRateSeconds) {
				_fps = _frameCount / _dt;
				_frameCount = 0;
				_dt -= 1.0F / _updateRateSeconds;
			}

			_playtime.SetTimescaleModifier(_timeScale);
	    }
	}
}