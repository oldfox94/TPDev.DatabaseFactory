using DbNotifyer.Models;

namespace DbNotifyer.Events
{
    public class SLNotifyerEvents
    {
        public delegate void SLNotifyerEventHandler(SLNotifyerEventArgs args);

        public static event SLNotifyerEventHandler DoNotifyerAction;

        public static void FireDoNotifyerAction(SLNotifyerEventArgs args)
        {
            if (DoNotifyerAction != null) DoNotifyerAction(args);
        }
    }
}
