using Caliburn.Micro;
using FollowFocusInterface.Models;
using FollowFocusInterface.Networking;
using RobotClient.Networking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowFocusInterface.ViewModels
{
    public class NetworkingViewModel : Screen, IHandle<SerialStatusModel>, IHandle<SocketServerModel>
    {
        #region Private Members

        // EventAggregator
        private IEventAggregator _eventAggregator { get; }
        private SerialCommunication _serial;
        private LogModel logMessage = new LogModel();

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

            // Server
            _roboServer = new SocketServer(eventAggregator);
        }

        #endregion

        #region TCP Socket Server

        #region Private Members

        private SocketServer _roboServer;
        public int RoboPort { get; set; } = 11000;
        private bool _SocketServerStatus;
        private string _SocketServerBtnTxt = "Start server";
        private int _NoClientsConnected = 0;

        #endregion

        #region TCP Property Initialisation

        /// <summary>
        /// Socket Server Status
        /// </summary>
        public bool SocketServerStatus
        {
            get { return _SocketServerStatus; }
            set => Set(ref _SocketServerStatus, value);
        }

        /// <summary>
        /// Socket Server Button Text
        /// </summary>
        public string SocketServerBtnTxt
        {
            get { return _SocketServerBtnTxt; }
            set => Set(ref _SocketServerBtnTxt, value);
        }

        /// <summary>
        /// Number of connected clients
        /// </summary>
        public int NoClientsConnected
        {
            get { return _NoClientsConnected; }
            set => Set(ref _NoClientsConnected, value);
        }


        #endregion

        #region TCP Server Methods

        /// <summary>
        /// Create Socket Server
        /// </summary>
        public void StartSocketServer()
        {
            if (!SocketServerStatus)
            {
                Task.Run(() =>
                {
                    _roboServer.StartListening(RoboPort);
                });
            }
            else
            {
                _roboServer.CloseServer();
                NoClientsConnected = 0;
            }
        }

        #endregion

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

        #region Follow Focus Targets

        #region Private Members

        private BindableCollection<FocusModel> _FocusList = new BindableCollection<FocusModel>();

        private int _ReceivedFocusTarget;
        private int _SelectedFocusTargetIdx = 0;
        private int TargetIdx = 0;


        #endregion

        #region Target Properties

        /// <summary>
        /// List with Focus Targets
        /// </summary>
        public BindableCollection<FocusModel> FocusList
        {
            get { return _FocusList; }
            set => Set(ref _FocusList, value);
        }

        /// <summary>
        /// Idx of selected focus target
        /// </summary>
        public int SelectedFocusTargetIdx
        {
            get { return _SelectedFocusTargetIdx; }
            set => Set(ref _SelectedFocusTargetIdx, value);
        }

        /// <summary>
        /// The target that was received from UR
        /// </summary>
        public int ReceivedFocusTarget
        {
            get { return _ReceivedFocusTarget; }
            set
            {
                _ReceivedFocusTarget = value;
                //Debug.WriteLine($"I have received the order to execute servo focus position #{ReceivedFocusTarget}");
                try
                {
                    _serial.SendToPort(FocusList[value].Val);
                    logMessage.Message = $"Move to position #{ReceivedFocusTarget} at {FocusList[value].Val}";
                    _eventAggregator.PublishOnUIThread(logMessage);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                NotifyOfPropertyChange(() => ReceivedFocusTarget);
            }
        }

        #endregion

        #region Target Methods

        /// <summary>
        /// Adds new focus target to the list
        /// </summary>
        public void AddFocusTarget()
        {
            var target = new FocusModel
            {
                Name = $"FollowFocus Target #{TargetIdx}",
                Idx = TargetIdx,
                Val = SliderValue
            };
            FocusList.Add(target);

            TargetIdx++;
        }

        /// <summary>
        /// Insert new focus target to the list
        /// </summary>
        public void InsertFocusTarget()
        {
            var target = new FocusModel
            {
                Name = $"FocusTarget {TargetIdx}",
                Idx = TargetIdx,
                Val = SliderValue
            };
            FocusList.Insert(SelectedFocusTargetIdx, target);

            TargetIdx++;
        }

        /// <summary>
        /// Edits selected focus target
        /// </summary>
        public void EditFocusTarget()
        {
            if (FocusList.Count > 0)
                FocusList[SelectedFocusTargetIdx].Val = SliderValue;

            FocusList.Refresh();
        }

        /// <summary>
        /// Removes selected focus target from the list
        /// </summary>
        public void RemoveFocusTarget()
        {
            if (FocusList.Count > 0)
                FocusList.RemoveAt(SelectedFocusTargetIdx);
            SelectedFocusTargetIdx = 0;
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


        /// <summary>
        /// Handling the Socket server status and data
        /// </summary>
        /// <param name="message"></param>
        public void Handle(SocketServerModel message)
        {
            SocketServerStatus = message.IsServerListening;
            ReceivedFocusTarget = message.FocusTargetIndex;
            NoClientsConnected = message.NoClientsConnected;

            // USB Serial Status
            if (SocketServerStatus)
                SocketServerBtnTxt = "Stop Server";
            else if (!SocketServerStatus)
                SocketServerBtnTxt = "Start Server";
        }

        #endregion

    }
}
