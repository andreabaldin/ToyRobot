/*
 * ToyRobot - by Andrea Baldin - 04/05/2016 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ToyRobot.Models;

namespace ToyRobot
{
    class Program
    {
        static int NumRows { get; set; }
        static int NumCols { get; set; }
        static bool TestMode { get; set; }

        static int Main(string[] args)
        {
            // ... Defaults setting (application universe)
            NumRows = 5;
            NumCols = 5;
            TestMode = true;
            string path = Directory.GetCurrentDirectory();
            string filename = "\\ToyRobot_Status.txt";

            Console.WriteLine("ToyRobot 5X5 - 04/05/2016 by ab");

            Robot robot = new Robot();
            if (GetCurrentRobotPosition(path, filename, ref robot))
                Console.WriteLine("  Existing Status File: " + path + filename);
            else
                Console.WriteLine("  New Status File: " + path + filename);

            // ... CommandLine Arguments
            if (args.Length < 1)
            {
                InteractiveMode(path + filename, ref robot);

                return 0;
            }
            else
            {
                // ... Help requested
                if (args.Contains("-h", StringComparer.OrdinalIgnoreCase))
                {
                    Console.WriteLine("\nHELP");
                    Console.WriteLine("  ToyRobot.exe                   interactive mode");
                    Console.WriteLine("  ToyRobot.exe <path+filename>   command file mode");
                    Console.WriteLine("  ToyRobot.exe -h                this help page");
                }
                else if (File.Exists(args[1]))
                {
                    CommandFileMode(path + filename, args[1], ref robot);

                }


                filename = "";
                path = "";
                Environment.CurrentDirectory = (path);
            }

            return 1;
        }


        // ... get the Current Robot Position from the Status File
        static bool GetCurrentRobotPosition(string path, string filename, ref Robot robot)
        {

            Detail detail;

            if (File.Exists(path + filename))
            {
                int _x = -1,
                    _y = -1, 
                    _c = -1;
                string _face = "", 
                    _lastAction = "";
                bool _lastActionAccepted = false;

                // ... Read current Robot Position if any
                bool flag = true;
                try
                {
                    StreamReader reader = new StreamReader(path + filename);
                    do
                    {
                        string line = reader.ReadLine();
                        string[] segments = line.Split(':');
                        if (Enum.TryParse(segments[0], true, out detail))
                        {

                            switch (detail)
                            {
                                case Detail.X:
                                    if (segments.Count() < 2 || !int.TryParse(segments[1], out _x))
                                        flag = false;
                                    break;

                                case Detail.Y:
                                    if (segments.Count() < 2 || !int.TryParse(segments[1], out _y))
                                        flag = false;
                                    break;

                                case Detail.MOVECOUNTER:
                                    if (segments.Count() < 2 || !int.TryParse(segments[1], out _c))
                                        flag = false;
                                    break;

                                case Detail.FACE:
                                    _face = segments[1];
                                    if (segments.Count() < 2 )
                                        flag = false;
                                    break;

                                case Detail.LASTACTION:
                                    _lastAction = segments[1];
                                    if (segments.Count() < 2)
                                        flag = false;
                                    break;

                                case Detail.LASTACTIONACCEPTED:
                                    if (segments.Count() < 2 || !bool.TryParse(segments[1], out _lastActionAccepted))
                                        flag = false;
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                    while (reader.Peek() != -1);
                    reader.Close();

                    if (flag && robot.CheckValidity(_x, _y, _face))
                    {
                        robot.X = _x;
                        robot.Y = _y;
                        robot.Face = robot._F;
                        robot.MoveCounter = _c;
                        robot.LastAction = _lastAction;
                        robot.LastActionAccepted = _lastActionAccepted;
                    }
                    else
                    {
                        Console.WriteLine("File: " + path + filename + " corrupted !");
                        return false;
                    }
                
                    return true;

                }
                catch
                {
                    Console.WriteLine("I/O error, file: " + path + filename);
                    return false;
                }

            }

            return false;
        }


        // ... Persist the Current Robot Position on Status File
        static void SaveCurrentRobotPosition(string statusToyRobotilename, Robot robot)
        {

            // ... Persist the current Robot Position
            try
            {
                StreamWriter writer = new StreamWriter(statusToyRobotilename);
                writer.WriteLine(Detail.X.ToString() + ": " + robot.X.ToString());
                writer.WriteLine(Detail.Y.ToString() + ": " + robot.Y.ToString());
                writer.WriteLine(Detail.FACE.ToString() + ": " + robot.Face.ToString());
                writer.WriteLine(Detail.MOVECOUNTER.ToString() + ": " + robot.MoveCounter.ToString());
                writer.WriteLine(Detail.LASTACTION.ToString() + ": " + robot.LastAction);
                writer.WriteLine(Detail.LASTACTIONACCEPTED.ToString() + ": " + robot.LastActionAccepted);
                writer.Close();
            }
            catch
            {
                Console.WriteLine("I/O error, file: " + statusToyRobotilename);
            }

        }


        // ... Interactive Mode - accept command input from keyboard.  Program terminates at wrong command
        static void InteractiveMode(string statusToyRobotilename, ref Robot robot)
        {
            Console.WriteLine("  Interactive mode");

            if (robot.IsValid())
                Console.WriteLine(robot.Report());
            else
                Console.WriteLine("Current position: undefined");

            bool flag;
            do
            {
                string s = Console.ReadLine();
                flag = ExecuteCommand(s, ref robot);
                SaveCurrentRobotPosition(statusToyRobotilename, robot);
            } while (flag);

        }


        // ... Command File Mode, accept a txt file containing commands at each line
        static void CommandFileMode(string statusToyRobotilename, string commandToyRobotilename, ref Robot robot)
        {
            Console.WriteLine("  Command File Mode");
            Console.WriteLine(robot.Report());

            if (File.Exists(commandToyRobotilename))
            {
                // ... Read current Robot Position if any
                try
                {
                    string commandString;
                    StreamReader reader = new StreamReader(commandToyRobotilename);
                    while ((commandString = reader.ReadLine()) != null)
                    {
                        ExecuteCommand(commandString, ref robot);
                        SaveCurrentRobotPosition(statusToyRobotilename, robot);
                    }
                }
                catch
                {
                    Console.WriteLine("I/O error, file: " + commandToyRobotilename);
                }
            }

        }


        // ... Execute Command by string
        static bool ExecuteCommand(string commandString, ref Robot robot)
        {
            bool flag = false;
            bool commandFlag = false;
            ActionEnum command;
            string[] commandSegments = commandString.Split(' ');

            if (Enum.TryParse(commandSegments[0], true, out command))
            {
                commandFlag = true;
                switch (command)
                {
                    case ActionEnum.PLACE:
                        if (commandSegments.Count() > 0)
                        {
                            string[] parameters = commandSegments[1].Split(',');
                            if (parameters.Count() > 2)
                            {
                                int _x, _y;
                                Direction _f;
                                if (int.TryParse(parameters[0], out _x) &&
                                        int.TryParse(parameters[1], out _y))
                                    flag = robot.Place(_x, _y, parameters[2]);
                            }
                        }
                        break;

                    case ActionEnum.MOVE:
                        flag = robot.Move();
                        break;

                    case ActionEnum.LEFT:
                        flag = robot.Left();
                        break;

                    case ActionEnum.RIGHT:
                        flag = robot.Right();
                        break;

                    case ActionEnum.REPORT:
                        flag = true;
                        Console.WriteLine(robot.Report());
                        break;

                    default:
                        // ... not possible
                        commandFlag = false;
                        break;
                }
            }

            if (TestMode && !string.IsNullOrEmpty(robot.LastAction))
                Console.WriteLine("\t\t" + robot.LastAction);

            return commandFlag;
        }

    }
}
