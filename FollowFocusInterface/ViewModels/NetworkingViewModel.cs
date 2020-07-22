using Caliburn.Micro;
using FollowFocusInterface.Models;
using FollowFocusInterface.Networking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowFocusInterface.ViewModels
{
    public class NetworkingViewModel : Screen, IHandle<SerialStatusModel>
    {
        #region Private Members

        // EventAggregator
        private IEventAggregator _eventAggregator { get; }
        private SerialCommunication _serial;

        #endregion

        #region Constructor
        public NetworkingViewModel(
            IEventAggregator eventAggregator
            )
        {
            // Event Aggregator Initialisation
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            _serial = new SerialCommunication(eventAggregator);
        }

        #endregion

        #region USB Serial Communication 

        #region Private Members

        private int _SliderValue;
        private string _SerialInput;
        private bool _USBSerialStatus;
        private string _USBConnectBtnText = "Open Port";

        #endregion

        #region USB Property Initialisation 

        /// <summary>
        /// USB Serial status
        /// </summary>
        public bool USBSerialStatus
        {
            get { return _USBSerialStatus; }
            set => Set(ref _USBSerialStatus, value);
        }

        /// <summary>
        /// USB Connect serial btn text
        /// </summary>
        public string USBConnectBtnText
        {
            get { return _USBConnectBtnText; }
            set => Set(ref _USBConnectBtnText, value);
        }

        /// <summary>
        /// SerialInput property
        /// </summary>
        public string SerialInput
        {
            get { return _SerialInput; }
            set => Set(ref _SerialInput, value);
        }

        /// <summary>
        /// Slider Value Property
        /// </summary>
        public int SliderValue
        {
            get { return _SliderValue; }
            set
            {
                _SliderValue = value;
                _serial.SendToPort(value);
                NotifyOfPropertyChange(() => SliderValue);
            }
        }

        #endregion

        #region USB Serial Communication Methods

        /// <summary>
        /// Open up the serial port
        /// </summary>
        public void USBSerialConnect()
        {
            try
            {
                // Async Task for connecting
                Task.Run(() =>
                {
                    if (!USBSerialStatus)
                    {
                        _serial.OpenSerialPort();
                    }
                    else if (USBSerialStatus)
                    {
                        _serial.CloseSerialPort();
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Send to Arduino method
        /// </summary>
        public void SendSerial()
        {
            var value = Convert.ToDouble(SerialInput);
            _serial.SendToPort(value);
        }

        #endregion

        #endregion

        #region Handlers

        /// <summary>
        /// Handlers
        /// </summary>
        /// <param name="message"></param>
        public void Handle(SerialStatusModel message)
        {
            USBSerialStatus = message.USBSerialStatus;

            // USB Serial Status
            if (USBSerialStatus)
                USBConnectBtnText = "Close Port";
            else if (!USBSerialStatus)
                USBConnectBtnText = "Open Port";
        }

        #endregion

    }
}
