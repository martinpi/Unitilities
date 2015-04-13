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

public enum NotificationType {
	None,
	
	Init = 1,
	PostInit,
	PostPostInit,
	InitDone,

	Refresh,

	Shutdown,
	
	TotalNotifications
};

public delegate void OnNotificationDelegate (Notification note);

public class NotificationCenter {
	public static NotificationCenter Instance = new NotificationCenter();
	private ArrayList[] listeners = new ArrayList[(int)NotificationType.TotalNotifications];
	private List<Notification> _scheduledNotifications = new List<Notification> ();
	private ArrayList[] toRemove = new ArrayList[(int)NotificationType.TotalNotifications];

	public NotificationCenter () { }
	
	public void AddListener (OnNotificationDelegate newListenerDelegate, NotificationType type) {
		int typeInt = (int)type;

		if (listeners [typeInt] == null)
			listeners [typeInt] = new ArrayList ();

		listeners [typeInt].Add (newListenerDelegate);
	}

	private void DoRemoveListener (OnNotificationDelegate listenerDelegate, NotificationType type) {
		int typeInt = (int)type;

		if (listeners [typeInt] == null)
			return;

		if (listeners [typeInt].Contains (listenerDelegate))
			listeners [typeInt].Remove (listenerDelegate);

		if (listeners [typeInt].Count == 0)
			listeners [typeInt] = null;
	}

	private void DoRemoveListeners() {
		for (int typeInt = 0; typeInt < (int)NotificationType.TotalNotifications; typeInt++) {
			if (toRemove [typeInt] != null) {
				foreach (OnNotificationDelegate dele in toRemove [typeInt])
					DoRemoveListener(dele, (NotificationType)typeInt);

				toRemove [typeInt].Clear();
				toRemove [typeInt] = null;
			}
		}
	}

	public void RemoveListener (OnNotificationDelegate listenerDelegate, NotificationType type) {
		int typeInt = (int)type;

		if (toRemove [typeInt] == null)
			toRemove [typeInt] = new ArrayList();
		
		toRemove [typeInt].Add(listenerDelegate);
	}

	public void RemoveListener (OnNotificationDelegate listenerDelegate) {
		for (int typeInt = 0; typeInt < (int)NotificationType.TotalNotifications; typeInt++) {
			if (listeners [typeInt] == null)
				continue;
			
			if (listeners [typeInt].Contains (listenerDelegate)) {
				if (toRemove [typeInt] == null)
					toRemove [typeInt] = new ArrayList();

				toRemove [typeInt].Add(listenerDelegate);
			}
		}
	}

	public void PostNotification (Notification note) {
		int typeInt = (int)note.type;

		if (listeners [typeInt] == null)
			return;

		foreach (OnNotificationDelegate delegateCall in listeners[typeInt]) {
			delegateCall (note);
		}
	}

	public void PostNotification ( NotificationType typeInt, object userInfo=null ) {
		PostNotification( new Notification(typeInt,userInfo) );
	}
    
	public void ScheduleNotification (Notification note) {
		_scheduledNotifications.Add (note);
	}

	public void ScheduleNotification ( NotificationType typeInt, object userInfo=null ) {
		_scheduledNotifications.Add ( new Notification(typeInt,userInfo) );
	}

	public void SendScheduledNotifications () {
		DoRemoveListeners();

		foreach (Notification note in _scheduledNotifications)
			PostNotification (note);
		_scheduledNotifications.Clear ();
	}
}


// Usage:
// NotificationCenter.Instance.AddListener( onNotification, NotificationType.Init );
// NotificationCenter.Instance.PostNotification( new Notification( NotificationType.Init, this ) );
// NotificationCenter.Instance.RemoveListener( onNotification, NotificationType.Init );


