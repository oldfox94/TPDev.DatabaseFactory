using DbNotifyer.Models;
using Hardcodet.Wpf.TaskbarNotification;
using System.Drawing;
using System.Windows.Media;

namespace DbNotifyer
{
    public class DbNotifyer
    {
        private TaskbarIcon m_tbNotify { get; set; }
        public DbNotifyer(NotifyData data)
        {
            m_tbNotify = new TaskbarIcon();

            //m_tbNotify.Icon = new Icon("/DbNotifyer;component/Images/Database.ico");
            //m_tbNotify.Icon =
            var x = (ImageSource)new ImageSourceConverter().ConvertFromString("/DbNotifyer;component/Images/Database.ico");
            m_tbNotify.IconSource = x;
        }
    }
}
