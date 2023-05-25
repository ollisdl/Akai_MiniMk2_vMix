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
        private static LocToPad locToPad = new LocToPad(true);

        static void Main(string[] args)
        {
            IniFile config = new IniFile(@"%appdata\AKAIVMIX.ini");

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
                locToPad = new LocToPad(Convert.ToBoolean(config.GetKeyValue("topToBottom")));
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
                    int devId = MidiFinder.MidiOutSelector();
                    midi = new MidiOut(devId);
                    config.Write("devid", devId.ToString());
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

            //Main loop.
            while (true)
            {
                try
                {
                    inputs = ReadTheApiUpdateLoop.ReadApi();
                    if(needClear)
                    {
                        ClearPad(midi, locToPad);
                        needClear= false;
                    }
                    InputHandler.CheckInputsAndHandle(midi, locToPad, inputs);
                    Thread.Sleep(250);
                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine("Failed to connect to vMix or read Input List. Going to sleep.");
                    Console.WriteLine(e.Message);
                    SleepMode(midi, locToPad);
                    needClear = true;
                }
            }
        }
    }
}