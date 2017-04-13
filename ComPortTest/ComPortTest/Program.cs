using System;

namespace ComPortTest
{
    class Program
    {
        public static bool running = true;
        public static string instructions = @"Enter commands prefixed by a switch eg. /s hello
/l - gets a list of available COM ports
/c <x> - connects to COM port x
/x - closes the currently opened COM port
/s <message> - sends a message over the open COM port
/r <message> re-sends a message at 5 second intervals
/k - stop sending repeat messages
/h - list of commands
/q - exits the program";

        static void Main(string[] args)
        {
            Console.WriteLine(instructions);
            while (running)
            {
                var command = Console.ReadLine();
                ProcessCommand(command);
            }
        }

        private static void ProcessCommand(string command)
        {
            if (!command.StartsWith("/") && command.Length < 2)
            {
                Console.WriteLine("Invalid Command");
            }
            else
            {
                var commandSwitch = command.Substring(0, 2);
                command = command.Remove(0, 2);

                switch (commandSwitch)
                {
                    case "/l":
                        Serial.GetPorts();
                        break;

                    case "/c":
                        Serial.OpenSerialPort(command);
                        break;

                    case "/x":
                        Serial.CloseSerialPort();
                        break;

                    case "/s":
                        Serial.WriteToSerialPort(command);
                        break;

                    case "/r":
                        Serial.RepeatWrite(command);
                        break;

                    case "/k":
                        Serial.KillTimer();
                        break;

                    case "/h":
                        Console.WriteLine(instructions);
                        break;

                    case "/q":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Unrecognized Command");
                        break;
                }
            }
        }
    }
}
