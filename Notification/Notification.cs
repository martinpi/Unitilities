
// Standard notification class.  For specific needs subclass
public class Notification
{
    public NotificationType type;
    public object userInfo;

    public Notification( NotificationType type )
    {
        this.type = type;
		this.userInfo = null;
    }


    public Notification( NotificationType type, object userInfo )
    {
        this.type = type;
        this.userInfo = userInfo;
    }
}
