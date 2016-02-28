using DbLogger.Events;
using DbNotifyer.Events;
using DbNotifyer.Models;

namespace DbInterface
{
    public class SLNotify
    {
        public static DbNotifyer.DbNotifyer NotifyIcon { get; set; }
        public static void CreateEvents()
        {
            SLNotifyerEvents.DoNotifyerAction += OnDoNotifyAction;
        }

        private static void OnDoNotifyAction(SLNotifyerEventArgs args)
        {
            switch(args.Action)
            {
                case NotifyActionTypes.ShowLog:
                    SLLogEvents.FireShowLogFile();
                    break;
            }
        }
    }
}
