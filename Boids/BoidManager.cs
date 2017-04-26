using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unitilities.Boids;
using Vectrosity;

public class BoidManager : MonoBehaviour {

	private List<Transform> _hostiles;

	private Swarm _swarm;
	[SerializeField] int numberOfBoids = 20;

	[SerializeField] float activateAfter = 60f;

	[SerializeField] float speed = 0.5f;
	[SerializeField] float sight = 200f;
	[SerializeField] float flockDistance = 10f;
	[SerializeField] float collisionSize = 1f;
	[SerializeField] float huntDistance = 500;
	[SerializeField] float agility = 0.3f;
	[SerializeField] Vector2 area;
	[SerializeField] int type;

	[SerializeField] GameObject Prefab;

	[SerializeField] Transform target;
	private GameManager _manager;
	private bool _isActive;

	void Start () {
		_manager = FindObjectOfType<GameManager>();
		StartCoroutine(InitSwarms());
		StartCoroutine(ActivateSwarm());
	}

	IEnumerator InitSwarms() {
		Vector2 pos = transform.position.Vector2XZ();
		Rect areaRect = new Rect(pos-area/2f, area);
		_swarm = new Swarm(0.25f, areaRect, numberOfBoids, type);

		ObjectPool.instance.Init();

		_hostiles = new List<Transform>();
		for (var i = 0; i < numberOfBoids; i++) {
			GameObject o = ObjectPool.instance.GetObjectForType(Prefab.name, false);
			o.SetActive(false);
			_hostiles.Add(o.transform);
		}

		yield return null;
	}

	IEnumerator PutbackAfter(GameObject g, float t) {
		yield return new WaitForSeconds(t);
		ObjectPool.instance.PoolObject(g);
	}

	IEnumerator ActivateSwarm() {
		yield return new WaitForSeconds(activateAfter);

		_isActive = true;

		_swarm.RegisterSwarm();
		for (var i = 0; i < numberOfBoids; i++) {
			_hostiles[i].gameObject.SetActive(true);
		}
		UpdateSwarmParameters();
		for (var i = 0; i < numberOfBoids; i++) 
			_swarm.Boids[i].Reset();
	}

	void UpdateSwarmParameters() {
		_swarm.speed = speed;
		_swarm.sight = sight;
		_swarm.flockDistance = flockDistance;
		_swarm.collisionSize = collisionSize;
		_swarm.huntDistance = huntDistance;
		_swarm.agility = Mathf.Clamp01(agility);

		Camera c = Camera.main;
		Vector2 bl = c.ViewportToWorldPoint(new Vector3(0f,0f, c.transform.position.y)).Vector2XZ();
		Vector2 tr = c.ViewportToWorldPoint(new Vector3(1f,1f, c.transform.position.y)).Vector2XZ();
		_swarm.cameraArea = new Rect(bl, tr-bl);

		_manager.RegisterKilled(_swarm.killed);
	}

	void Update () {
		if (!_isActive) return;

		UpdateSwarmParameters();

		Vector2 targetPos2D = target.position.Vector2XZ();
//		_swarm.MoveBoids(targetPos2D, Time.deltaTime);

		for (var i = 0; i < numberOfBoids; i++) {
			Vector3 pos = _hostiles[i].position;
			pos.x = _swarm.Boids[i].pos.x;
			pos.z = _swarm.Boids[i].pos.y;
			_hostiles[i].position = pos;
			_hostiles[i].rotation = Quaternion.LookRotation(_swarm.Boids[i].faceDirection.Vector3XZ(), Vector3.up);

			float targetDist = Vector2.Distance(targetPos2D, _swarm.Boids[i].pos);
			if (targetDist < _swarm.collisionSize) _manager.GameOver();

			if (_swarm.Boids[i].exploded) {
				_swarm.Boids[i].Respawn();

				GameObject g = ObjectPool.instance.GetObjectForType("Explosion", true);

				if (g != null) {
					g.transform.position = pos;
					g.transform.rotation = transform.rotation;

					StartCoroutine(PutbackAfter(g, 5f));
				}
			}
		}
	}
}
