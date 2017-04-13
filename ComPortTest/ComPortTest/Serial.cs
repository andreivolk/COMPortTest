using System;
using System.Text;
using System.IO.Ports;
using System.Timers;

namespace ComPortTest
{
    class Serial
    {
        private static SerialPort _serialPort = new SerialPort("COM999", 9600, Parity.None, 8, StopBits.One);
        private static Timer repeatTimer;
        private static string repeatCommand;

        public static void GetPorts()
        {
            Console.WriteLine("Getting list of COM ports...");
            var ports = SerialPort.GetPortNames();
            foreach (var port in ports)
            {
                Console.WriteLine(port);
            }
        }
        public static void OpenSerialPort(string selectedPort)
        {
            _serialPort.PortName = "COM" + selectedPort.Trim();
            try
            {
                Console.WriteLine("Trying to open Serial Port: {0}", _serialPort.PortName);
                _serialPort.Open();
                Console.WriteLine("Port {0} open, enter command", _serialPort.PortName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to open Serial Port {0}: {1}", _serialPort.PortName, ex);
            }
        }
        public static void CloseSerialPort()
        {
            try
            {
                _serialPort.Close();
                Console.WriteLine("Port {0} closed", _serialPort.PortName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to close Serial Port {0}: {1}", _serialPort.PortName, ex);
            }
        }
        public static void WriteToSerialPort(string command)
        {
            if (_serialPort.IsOpen)
            {
                try
                {
                    byte[] instruction = Encoding.ASCII.GetBytes(command.Trim());
                    _serialPort.Write(instruction, 0, instruction.Length);
                    Console.WriteLine("Sending message: {0}", DateTime.Now);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to write to Port {0}: {1}", _serialPort.PortName, ex);
                }
            }
            else
            {
                Console.WriteLine("No open COM port");
            }
        }
        public static void RepeatWrite(string command)
        {            
            repeatCommand = command.Trim();
            repeatTimer = new Timer(5000);
            repeatTimer.Elapsed += OnTimedEvent;
            repeatTimer.AutoReset = true;
            repeatTimer.Enabled = true;
        }

        private static void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            WriteToSerialPort(repeatCommand);
        }

        public static void KillTimer()
        {
            repeatTimer.Stop();
            repeatTimer.Dispose();
            Console.WriteLine("Stopped sending repeat messages");
        }
    }
}
