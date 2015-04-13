using UnityEngine;
using System.Collections;

public class SuperNotification : Notification
{
	public float varFloat;
	public int varInt;

	
	public SuperNotification( NotificationType type, float varFloat, int varInt ) : base( type )
	{
		this.varFloat = varFloat;
		this.varInt = varInt;
	}
}
