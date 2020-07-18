using Caliburn.Micro;
using FollowFocusInterface.Models;
using FollowFocusInterface.Networking;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace FollowFocusInterface.ViewModels
{
    public class ShellViewModel : Screen, IHandle<SerialStatusModel>
    {

        #region Window Control

        private WindowState windowState;
        public WindowState WindowState
        {
            get { return windowState; }
            set
            {
                windowState = value;
                NotifyOfPropertyChange(() => WindowState);
            }
        }

        public void MaximizeWindow()
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;
        }

        public void MinimizeWindow()
        {
            WindowState = WindowState.Minimized;
        }

        public bool myCondition { get { return (false); } }

        public void CloseWindow()
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region Constructor

        public ShellViewModel(
            IEventAggregator eventAggregator)
        {
            _serial = new SerialCommunication(eventAggregator);

            // Event Aggregator
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        #endregion

        #region Private Members

        private SerialCommunication _serial;

        // EventAggregator
        private IEventAggregator _eventAggregator { get; }

        #endregion

        #region USB Serial Communication Methods

        #region Private Members

        private bool _USBSerialStatus;
        private string _USBConnectBtnText = "Open Port";
        private string _SerialInput;

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
        /// Serial Input Property
        /// </summary>
        public string SerialInput
        {
            get { return _SerialInput; }
            set => Set(ref _SerialInput, value);
        }

        #endregion

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
                        //BTSerialStatus = true;
                    }
                    else if (USBSerialStatus)
                    {
                        _serial.CloseSerialPort();
                        //BTSerialStatus = false;
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Send serial method
        /// </summary>
        public void SendSerial()
        {
            var value = Convert.ToDouble(SerialInput);
            _serial.SendToPort(value);
        }

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
