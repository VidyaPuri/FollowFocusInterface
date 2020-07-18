using System;
using System.IO.Ports;
using System.Diagnostics;
using Caliburn.Micro;
using FollowFocusInterface.Models;

namespace FollowFocusInterface.Networking
{
    public class SerialCommunication
    {

        private SerialPort port;
        private readonly SerialStatusModel serialStatus = new SerialStatusModel();
        private SerialReceivedModel serialReceived = new SerialReceivedModel();
        private int idx;
        public IEventAggregator _eventAggregator { get; }

        public SerialCommunication(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            idx = 0;
        }

        /// <summary>
        /// Open new serial port
        /// </summary>
        public void OpenSerialPort()
        {
            port = new SerialPort
            {
                PortName = "COM3",
                BaudRate = 1000000
            };

            try
            {
                if (!port.IsOpen)
                {
                    port.Open();
                    serialStatus.USBSerialStatus = port.IsOpen;
                    serialStatus.ComType = "USB";
                    _eventAggregator.BeginPublishOnUIThread(serialStatus);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            // DataReceived event handler
            port.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
        }

        /// <summary>
        /// Close the port
        /// </summary>
        public void CloseSerialPort()
        {
            if (port.IsOpen)
            {
                port.Close();
                serialStatus.USBSerialStatus = port.IsOpen;
                serialStatus.ComType = "USB";
                _eventAggregator.BeginPublishOnUIThread(serialStatus);
            }
        }

        /// <summary>
        /// Received data from Arduino
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                idx++;
                SerialPort spl = (SerialPort)sender;
                //string received = spl.ReadLine();
                serialReceived.Message = spl.ReadLine();
                serialReceived.Timestamp = DateTime.Now.ToString("HH:mm:ss");
                serialReceived.Idx = idx;

                Debug.WriteLine($"Data {spl.ReadLine()} \n");
                //Debug.WriteLine($"Received {serialReceived.Message}");
                _eventAggregator.BeginPublishOnUIThread(serialReceived);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Send value to port method
        /// </summary>
        /// <param name="value"></param>
        public void SendToPort(double value)
        {
            if (port != null)
            {
                if (port.IsOpen)
                {
                    port.WriteLine(value.ToString());
                }
            }
        }
    }
}
