using Caliburn.Micro;
using FollowFocusInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowFocusInterface.ViewModels
{
    public class LoggerViewModel : Screen, IHandle<SerialReceivedModel>
    {
        #region Private Members

        // EventAggregator
        private IEventAggregator _eventAggregator { get; }

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

        private BindableCollection<SerialReceivedModel> _ReceivedMessageLog = new BindableCollection<SerialReceivedModel>();

        #endregion

        #region Property Initialisation

        /// <summary>
        /// Recieved Message Log
        /// </summary>
        public BindableCollection<SerialReceivedModel> ReceivedMessageLog
        {
            get { return _ReceivedMessageLog; }
            set => Set(ref _ReceivedMessageLog, value);
        }

        #endregion

        #endregion

        #region Handlers

        /// <summary>
        /// Received from serial package
        /// </summary>
        /// <param name="message"></param>
        public void Handle(SerialReceivedModel message)
        {
            //ReceivedMessageLog.Add(message);
            ReceivedMessageLog.Insert(0, message);
        }

        #endregion


    }
}
