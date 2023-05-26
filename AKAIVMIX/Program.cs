﻿using NAudio.Midi;
using System.Xml;
using static AKAIVMIX.MidiOutputHandler;

namespace AKAIVMIX
{
    class Looop
    {
        private static Input[]? inputs;
        private static bool needClear = true;
        private static MidiOut? midi;
        private static int midiID;
        private static LocToPad locToPad = new LocToPad(true);
        private static Input[] first8AudioSources = new Input[8];
        private static IniFile config = new IniFile();

        static void menu()
        {
            Console.Clear();
            Console.WriteLine("Main Menu");
            Console.WriteLine("1: Change Volume Silder");
            Console.WriteLine("2: Change Midi Device");
            Console.WriteLine("3: Toggle Audio Mode");
            Console.WriteLine("4: Exit");
            Console.WriteLine("Enter your selection or press ESC to return to program");
            needClear = true;
            switch (Console.ReadKey(true).Key)
            {
                default:
                    Console.Clear();
                    Console.ReadLine();
                    break;
                case ConsoleKey.Escape:
                    Console.Clear();
                    break;
                case ConsoleKey.D1:
                    Console.Clear();
                    break;
                case ConsoleKey.D2:
                    Console.Clear();
                    midi.Close();
                    midiID = MidiFinder.MidiOutSelector();
                    midi = new MidiOut(midiID);
                    config.Write("devid", midiID.ToString());
                    break;
                case ConsoleKey.D3:
                    Console.Clear();
                    break;
                case ConsoleKey.D4:
                    Console.Clear();
                    break;

            }
        }
        static void Main(string[] args)
        {
            string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AKAIVMIX.INI";
            config = new IniFile(FilePath);

            //Configure TopToBottom or BottomToTop and generate LocToPad
            if(!config.KeyExists("topToBottom"))
            {
                int topToBottom = -1;
                while(topToBottom < 0)
                {
                    Console.Clear();
                    Console.WriteLine("Would you like rows to go bottom up or top down?");
                    Console.WriteLine("0: Bottom Up");
                    Console.WriteLine("1: Top Down");
                    Console.WriteLine("Enter selection number, then press Enter:");
                    bool valid = int.TryParse(Console.ReadLine(), out topToBottom);
                    if(valid) 
                    {
                        if(topToBottom < 0 || topToBottom > 1)
                        {
                            Console.WriteLine("Invalid Selection. Let's Try again:");
                        }
                        else
                        {
                            config.Write("topToBottom", topToBottom.ToString());
                            locToPad = new LocToPad(Convert.ToBoolean(topToBottom));
                        }
                    }
                }
            }
            else
            {
                locToPad = new LocToPad(Convert.ToBoolean(int.Parse(config.GetKeyValue("topToBottom"))));
            }

            //Load device from config or display selector
            if(!config.KeyExists("devid"))
            {
                MidiOut[] devices = MidiFinder.FindDevices();
                if (devices.Length == 0)
                {
                    throw new Exception("No suitable MIDI outputs found. Closing. Ensure vMix is not attached to the device you wish to use");
                }
                else
                {
                    midiID = MidiFinder.MidiOutSelector();
                    midi = new MidiOut(midiID);
                    config.Write("devid", midiID.ToString());
                }
            }
            else
            {
                //Attempt to load midi device
                while(midi == null)
                {
                    int deviceID = int.Parse(config.GetKeyValue("devid"));
                    string deviceName = MidiOut.DeviceInfo(deviceID).ProductName;

                    try
                    {
                        Console.WriteLine("Attempting to attach to " + deviceName);
                        midi = new MidiOut(deviceID);
                    }
                    catch
                    {
                        Console.WriteLine("Failed to attach. Make sure device is not busy or selected by vMix");

                        midi = new MidiOut(MidiFinder.MidiOutSelector());
                    }
                    config.Write("devid", deviceID.ToString());
                }
            }
            Console.WriteLine("Connected to " + MidiOut.DeviceInfo(int.Parse(config.GetKeyValue("devid"))).ProductName);

            //Main loop.
            while (true)
            {
                do
                {
                    while (!Console.KeyAvailable)
                    {
                        try
                        {
                            inputs = ReadTheApiUpdateLoop.ReadApi();
                            first8AudioSources = ReadTheApiUpdateLoop.audio(inputs);
                            if (needClear)
                            {
                                ClearPad(midi, locToPad);
                                needClear = false;
                                Console.Clear();
                                int inputCounts = 0;
                                foreach (var input in inputs)
                                {
                                    if (input != null)
                                        inputCounts++;
                                }


                                Console.WriteLine("Connected to vMix with " + inputCounts + " inputs.");
                                Console.WriteLine("\tSilders are assinged to:");
                                foreach (var input in first8AudioSources)
                                {
                                    if (input != null)
                                        Console.WriteLine("\t" + input.number + ": " + input.title);
                                }
                                Console.WriteLine("\t 0: Master Volume");
                                Console.WriteLine("Press ESC to open menu");
                            }
                            InputHandler.CheckInputsAndHandle(midi, locToPad, inputs);
                            InputHandler.HandleSliders(midi, first8AudioSources);
                            Thread.Sleep(250);
                        }
                        catch (Exception e)
                        {
                            Console.Clear();
                            Console.WriteLine("Failed to connect to vMix or read Input List. Going to sleep.");
                            Console.WriteLine(e.Message);
                            Task.Run(() => SleepMode(midi, locToPad));
                            needClear = true;
                        }
                    }
                } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
                menu();
            }
        }
    }
}