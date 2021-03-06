﻿using System;
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
        private LogModel serialReceived = new LogModel();
        public IEventAggregator _eventAggregator { get; }

        public SerialCommunication(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
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
                SerialPort spl = (SerialPort)sender;
                serialReceived.Message = spl.ReadLine();

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
