using UnityEngine;

namespace Utils {

	public static class DebugX {
		public static void Log(bool onoff, object message ) {
			if (onoff) UnityEngine.Debug.Log(message);
		}

		public static void Assert(bool ass, object message) {
			if (!ass) throw new UnityException(message.ToString());
		}
	}
}
