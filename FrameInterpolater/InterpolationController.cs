/*
The MIT License

Copyright (c) 2016 Martin Pichlmair
Based on http://www.kinematicsoup.com/news/2016/8/9/rrypp5tkubynjwxhxjzd42s3o034o8 by Scott Sewell

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/


using UnityEngine;
using System.Collections;

/*
	Execution Order:
	InterpolationController -100
	InterpolatedTransform -50
	InterpolatedTransformUpdater 100

	Source: http://www.kinematicsoup.com/news/2016/8/9/rrypp5tkubynjwxhxjzd42s3o034o8

	HOWTO:
	Attach InterpolationController to a single gameobject in the scene.
	Attach InterpolatedTransform + Updater to any objects moved during a FixedUpdate.


	Do all movement/state update in FixedUpdate!
*/

namespace Unitilities.FrameInterpolater {

	public class InterpolationController : MonoBehaviour {
		private float[] m_lastFixedUpdateTimes;
		private int m_newTimeIndex;

		private static float m_interpolationFactor;

		public static float InterpolationFactor {
			get { return m_interpolationFactor; }
		}

		public void Start() {
			m_lastFixedUpdateTimes = new float[2];
			m_newTimeIndex = 0;
		}

		public void FixedUpdate() {
			m_newTimeIndex = OldTimeIndex();
			m_lastFixedUpdateTimes[m_newTimeIndex] = UnityEngine.Time.fixedTime;
		}

		public void Update() {
			float newerTime = m_lastFixedUpdateTimes[m_newTimeIndex];
			float olderTime = m_lastFixedUpdateTimes[OldTimeIndex()];

			if (newerTime != olderTime) {
				m_interpolationFactor = (UnityEngine.Time.time - newerTime) / (newerTime - olderTime);
			} else {
				m_interpolationFactor = 1;
			}
		}

		private int OldTimeIndex() {
			return (m_newTimeIndex == 0 ? 1 : 0);
		}
	}

}