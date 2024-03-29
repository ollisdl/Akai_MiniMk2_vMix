﻿using NAudio.Midi;
using System.Xml;
using static AKAIVMIX.MidiOutputHandler;

namespace AKAIVMIX
{
    class Looop
    {
        public static IniFile? config;
        private static Input[]? inputs;
        public static bool needClear = true;
        public static MidiOut? midi;
        public static int midiID;
        public static LocToPad locToPad = new LocToPad(true);
        private static Input[] first8AudioSources = new Input[8];
        public static int activeColor = 5;
        public static int previewColor = 45;
        public static int sleepColor = 1;
        public static int audioColor = 32;
        public static int sleepMode = 1;
        public static int defaultColor = 0;
        public static int overlayColor = 13;
        public static int mutedColor = 0;


        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();

            string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AKAIVMIX.INI";
            config = new IniFile(FilePath);

            //Load device from config or display selector
            if (!config.KeyExists("devid"))
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
                while (midi == null)
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


            if (!config.KeyExists("topToBottom"))
            {

            }
            else
            {
                locToPad = new LocToPad(Convert.ToBoolean(int.Parse(config.GetKeyValue("topToBottom"))));
            }

            if (!config.KeyExists("activeColor"))
            {
                activeColor = ClrForm.formResult("Choose a color for the active source");
                if (activeColor == -1)
                {
                    activeColor = 1;
                }
                config.Write("activeColor", activeColor.ToString());
            }
            else
            {
                activeColor = int.Parse(config.Read("activeColor"));
            }

            if (!config.KeyExists("sleepColor"))
            {
                sleepColor = ClrForm.formResult("Choose a color for sleep mode 2 (Static)");
                if (sleepColor == -1)
                {
                    sleepColor = 1;
                }
                config.Write("sleepColor", sleepColor.ToString());
            }
            else
            {
                sleepColor = int.Parse(config.Read("sleepColor"));
            }

            if (!config.KeyExists("sleepMode"))
            {
                sleepMode = SleepModeForm.formResult("Choose a sleep mode:");
                if (sleepMode == -1)
                {
                    sleepMode = 1;
                }
                config.Write("sleepMode", sleepMode.ToString());
            }
            else
            {
                sleepMode = int.Parse(config.Read("sleepMode"));
            }


            SetupForm f = new SetupForm("Test");
            f.ShowDialog();

            //Configure TopToBottom or BottomToTop and generate LocToPad

            Console.WriteLine("Connected to " + MidiOut.DeviceInfo(int.Parse(config.GetKeyValue("devid"))).ProductName);

            //Main loop.
            while (true)
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
                        SleepMode(midi, locToPad, sleepMode, sleepColor);
                        needClear = true;
                    }
                }
                var key = Console.ReadKey();
                if(key.Key == ConsoleKey.Escape)
                {
                    f = new SetupForm("Test");
                    f.ShowDialog();
                }
            }
        }
    }
}