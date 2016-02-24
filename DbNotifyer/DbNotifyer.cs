using Hardcodet.Wpf.TaskbarNotification;
using System.Drawing;

namespace DbNotifyer
{
    public class DbNotifyer
    {
        private TaskbarIcon m_tbNotify { get; set; }
        public DbNotifyer()
        {
            m_tbNotify = new TaskbarIcon();
            m_tbNotify.Icon = new Icon("/DbNotifyer;component/Images/Database.ico");
        }
    }
}
