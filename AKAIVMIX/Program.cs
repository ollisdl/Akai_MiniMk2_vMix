using NAudio.Midi;
using System.Xml;
using static AKAIVMIX.MidiOutputHandler;

namespace AKAIVMIX
{
    class Looop
    {
        private static Input[]? inputs;
        public static bool needClear = true;
        public static MidiOut? midi;
        public static LocToPad locToPad = new LocToPad(true);
        private static Input[] first8AudioSources = new Input[8];
        public static int activeColor = 1;
        public static int previewColor = 69;
        public static int sleepColor = 1;
        public static int audioColor = 51;
        public static int sleepMode = 1;
        public static int defaultColor = 1;
        public static int overlayColor = 1;
        public static int mutedColor = 1;


        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();

            string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AKAIVMIX.INI";
            IniFile config = new IniFile(FilePath);

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

            if(!config.KeyExists("activeColor"))
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


            Console.WriteLine("Connected to " + MidiOut.DeviceInfo(int.Parse(config.GetKeyValue("devid"))).ProductName);

            //Main loop.
            while (true)
            {
                try
                {
                    inputs = ReadTheApiUpdateLoop.ReadApi();
                    first8AudioSources = ReadTheApiUpdateLoop.audio(inputs); 
                    if(needClear)
                    {
                        ClearPad(midi, locToPad);
                        needClear= false;
                        Console.Clear();
                        int inputCounts = 0;
                        foreach (var input in inputs)
                        {
                            if(input != null)
                                inputCounts++;
                        }


                        Console.WriteLine("Connected to vMix with " + inputCounts + " inputs.");
                        Console.WriteLine("\tSilders are assinged to:");
                        foreach(var input in first8AudioSources)
                        {
                            if(input != null)
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
        }
    }
}