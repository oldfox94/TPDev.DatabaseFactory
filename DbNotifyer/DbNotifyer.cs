using DbLogger.Events;
using DbLogger.Models;
using DbNotifyer.Models;
using DbNotifyer.Properties;
using Hardcodet.Wpf.TaskbarNotification;

namespace DbNotifyer
{
    public class DbNotifyer
    {
        private TaskbarIcon m_tbNotify { get; set; }
        private NotifyData m_Data { get; set; }
        public DbNotifyer(NotifyData data)
        {
            m_Data = data;

            m_tbNotify = new TaskbarIcon();
            m_tbNotify.Icon = Resources.Database;
            m_tbNotify.ToolTipText = data.Title;

            SLLogEvents.ShowBallonTipp += OnShowLogBallonTipp;
        }

        public void SetOkayState()
        {
            if (m_tbNotify == null) return;
            m_tbNotify.Icon = Resources.DatabaseCheck;

            m_tbNotify.ToolTipText = m_Data.Title + " - DatabaseFactory is Okay!";
        }

        public void SetWarningState()
        {
            if (m_tbNotify == null) return;
            m_tbNotify.Icon = Resources.DatabaseAlert;
            m_tbNotify.ToolTipText = m_Data.Title + " - DatabaseFactory has Warnings!";
        }

        public void SetDownState()
        {
            if (m_tbNotify == null) return;
            m_tbNotify.Icon = Resources.DatabaseDown;
            m_tbNotify.ToolTipText = m_Data.Title + " - DatabaseFactory is Down!";
        }

        public void WriteError(string titel, string message)
        {
            if (m_tbNotify == null) return;
            if (!m_Data.NotifyOnError) return;

            m_tbNotify.ShowBalloonTip(titel, message, BalloonIcon.Error);
            SetWarningState();

            m_tbNotify.ToolTipText = m_Data.Title + " - DatabaseFactory has Errors!";
        }

        private void OnShowLogBallonTipp(SLLogEventArgs args)
        {
            if (m_tbNotify == null) return;

            switch (args.Type)
            {
                case LogType.Info:
                    SetOkayState();
                    if (!m_Data.NotifyOnInfo) return;
                    m_tbNotify.ShowBalloonTip(args.Titel, args.LogMessage, BalloonIcon.Info);
                    break;

                case LogType.Warning:
                    SetWarningState();
                    if (!m_Data.NotifyOnInfo) return;
                    m_tbNotify.ShowBalloonTip(args.Titel, args.LogMessage, BalloonIcon.Warning);
                    break;

                case LogType.Error:
                    SetWarningState();
                    if (!m_Data.NotifyOnError) return;
                    m_tbNotify.ShowBalloonTip(args.Titel, args.LogMessage, BalloonIcon.Error);
                    break;
            }
        }
    }
}
