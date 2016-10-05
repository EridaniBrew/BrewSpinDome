using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EnhancedSerialPort1.SerialPort;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public EnhancedSerialPort.SerialPort SerialPort = new EnhancedSerialPort.SerialPort();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SerialPort.DataReceived += SerialPort_DataReceived;
            SerialPort.PinChanged += SerialPort_PinChanged;
            SerialPort.ErrorReceived += SerialPort_ErrorReceived;
            SerialPort.Watchdog += SerialPort_Watchdog;

        }
        private void SerialPort_PinChanged(System.IO.Ports.SerialPinChangedEventArgs e)
        {

        }
        private void SerialPort_Watchdog(EnhancedSerialPort.SerialPort.WatchdogFlags Flag)
        {

        }
        private void SerialPort_ErrorReceived(System.IO.Ports.SerialErrorReceivedEventArgs e)
        {
        }
        private void SerialPort_DataReceived(int BytesAvailable)
        {
            
        }
    }
}
