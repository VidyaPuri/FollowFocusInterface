using Caliburn.Micro;
using FollowFocusInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowFocusInterface.ViewModels
{
    public class LoggerViewModel : Screen, IHandle<LogModel>
    {
        #region Private Members

        // EventAggregator
        private IEventAggregator _eventAggregator { get; }
        private int Idx = 0;

        #endregion

        #region Constructor

        public LoggerViewModel(
            IEventAggregator eventAggregator
            )
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        #endregion

        #region Logger Received

        #region Private Members

        private BindableCollection<LogModel> _ReceivedMessageLog = new BindableCollection<LogModel>();
        private LogModel LogPackage = new LogModel();

        #endregion

        #region Property Initialisation

        /// <summary>
        /// Recieved Message Log
        /// </summary>
        public BindableCollection<LogModel> ReceivedMessageLog
        {
            get { return _ReceivedMessageLog; }
            set => Set(ref _ReceivedMessageLog, value);
        }

        /// <summary>
        /// Log Package Initialisation
        /// </summary>
        public LogModel _LogPackage
        {
            get { return LogPackage; }
            set => Set(ref LogPackage, value);
        }

        #endregion

        #endregion

        #region Handlers

        /// <summary>
        /// Received from serial package
        /// </summary>
        /// <param name="message"></param>
        public void Handle(LogModel message)
        {
            LogPackage.Message = message.Message;
            LogPackage.Idx = Idx;
            LogPackage.Timestamp = DateTime.Now.ToString("HH:mm:ss");

            ReceivedMessageLog.Insert(0, LogPackage);
            Idx++;
        }

        #endregion


    }
}
