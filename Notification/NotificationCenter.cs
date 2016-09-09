/*
The MIT License

Copyright (c) 2015 Martin Pichlmair

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

/*
Original source from: http://wiki.unity3d.com/index.php?title=CSharpNotificationCenter
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Unitilities {
	public delegate void OnNotificationDelegate (Notification note);

	public sealed class NotificationCenter {
		Dictionary<string, List<OnNotificationDelegate>> _listeners = new Dictionary<string, List<OnNotificationDelegate>>();
		List<Pair<string,OnNotificationDelegate>>  _toRemove = new List<Pair<string, OnNotificationDelegate>>();
		List<Notification> _scheduledNotifications = new List<Notification> ();

		static readonly NotificationCenter instance = new NotificationCenter();
		static NotificationCenter() { }
		public static NotificationCenter Instance { get { return instance; } }

		public void AddListener (OnNotificationDelegate newListenerDelegate, string key) {
			if (!_listeners.ContainsKey(key))
				_listeners[key] = new List<OnNotificationDelegate>();
			_listeners[key].Add(newListenerDelegate);
		}

		private void DoRemoveListener (OnNotificationDelegate listenerDelegate, string key) {
			if (!_listeners.ContainsKey(key)) return;
			if (_listeners[key].Contains (listenerDelegate))
				_listeners[key].Remove (listenerDelegate);
		}

		private void DoRemoveListeners() {
			foreach (Pair<string,OnNotificationDelegate> pair in _toRemove)
				DoRemoveListener(pair.Second, pair.First);
		}

		public void RemoveListener (OnNotificationDelegate listenerDelegate, string key) {
			_toRemove.Add(new Pair<string, OnNotificationDelegate>(key, listenerDelegate));
		}

		public void RemoveListener (OnNotificationDelegate listenerDelegate) {
			foreach (var kvp in _listeners) {
				if (kvp.Value.Contains (listenerDelegate)) 
					RemoveListener(listenerDelegate, kvp.Key);
			}
		}

		public void PostNotification (Notification note) {
			foreach (OnNotificationDelegate delegateCall in _listeners[note.key]) {
				delegateCall(note);
			}
		}

		public void PostNotification ( string key, object userInfo=null ) {
			PostNotification( new Notification(key, userInfo) );
		}

		public void ScheduleNotification (Notification note) {
			_scheduledNotifications.Add (note);
		}

		public void ScheduleNotification ( string key, object userInfo=null ) {
			_scheduledNotifications.Add ( new Notification(key, userInfo) );
		}

		public void SendScheduledNotifications () {
			DoRemoveListeners();

			foreach (Notification note in _scheduledNotifications)
				PostNotification (note);
			_scheduledNotifications.Clear ();
		}

		public void Clear() {
			_listeners.Clear();
			_toRemove.Clear();
			_scheduledNotifications.Clear();
		}
	}
}



// Usage:
// NotificationCenter.Instance.AddListener( onNotification, NotificationType.Init );
// NotificationCenter.Instance.PostNotification( new Notification( NotificationType.Init, this ) );
// NotificationCenter.Instance.RemoveListener( onNotification, NotificationType.Init );


