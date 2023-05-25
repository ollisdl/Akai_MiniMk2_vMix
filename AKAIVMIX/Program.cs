using NAudio.Midi;
using System.Xml;
using static AKAIVMIX.MidiOutputHandler;

namespace AKAIVMIX
{
    class Looop
    {
        private static Input[] inputs;
        private static int defaultColor = 0;
        private static int defaultPreviewColor = 21;
        private static int defaultActiveColor = 5;
        private static int defaultOverlayColor = 13;
        private static int defaultAudioActiveColor = 46;
        private bool init = true;
        


        static void Main(string[] args)
        {
            int midiIn = MidiIn.NumberOfDevices;
            bool needClear = false;
            LocToPad locToPad = new LocToPad(true);
            MidiOut midi = new MidiOut(1);
            ClearPad(midi, locToPad);
            while (true)
            {
                try
                {
                    inputs = ReadTheApiUpdateLoop.RunLoopOnce();
                    if (inputs[0] != null)
                    {
                        for (int j = 112; j < 114; j++)
                        {
                            WriteLittle(midi, j, true, false);
                        }
                    }
                    for (int i = 0; i < inputs.Length; i++)
                    {

                        var item = inputs[i];
                        if (item != null)
                        {
                            if (item.number <= 32)
                            {
                                if (needClear)
                                {
                                    ClearPad(midi, locToPad);
                                    needClear = false;
                                    Console.Clear();
                                    Console.WriteLine("Connected to vMix.");
                                    for (int j = 112; j < 114; j++)
                                    {
                                        WriteLittle(midi, j, true, false);
                                    }
                                }
                                if (!item.preview && !item.active && (item.overlay == -1))
                                {
                                    WriteDefault(midi, locToPad, item.number);
                                }
                                if (item.preview && item.active)
                                {
                                    WritePreviewActive(midi, locToPad, item.number);
                                }
                                if (item.preview)
                                {
                                    WriteDefault(midi, locToPad, item.number);
                                }
                                if (item.active)
                                {
                                    WriteDefault(midi, locToPad, item.number);
                                }
                                if (item.overlay != -1)
                                {
                                    if (item.preview)
                                    {
                                        WriteDefault(midi, locToPad, item.number);
                                    }
                                    else
                                        WriteDefault(midi, locToPad, item.number);
                                }
                                if (!item.muted)
                                {
                                    WriteDefault(midi, locToPad, item.number);
                                }
                                if (item.muted)
                                {
                                    WriteDefault(midi, locToPad, item.number);
                                }
                            }
                        }
                        else if (item == null)
                        {
                            if (i <= 32)
                                WriteDefault(midi, locToPad, i + 1);
                        }
                    }
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