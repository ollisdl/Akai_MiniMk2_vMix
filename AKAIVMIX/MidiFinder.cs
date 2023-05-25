using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Midi;

namespace AKAIVMIX
{
    internal class MidiFinder
    {
        public static MidiOut[] FindDevices()
        {
            MidiOut[] midiOuts = new MidiOut[MidiOut.NumberOfDevices];
            Console.WriteLine("Begining discovery of MIDI output devices.");
            for(int i = 0; i < MidiOut.NumberOfDevices; i++)
            {
                Console.WriteLine(1 + ": " + MidiOut.DeviceInfo(i).ProductName);
            }
            return midiOuts;
        }

        public static int MidiOutSelector()
        {
            Console.Clear();
            Console.WriteLine("Select a MIDI device to output to:");
            for (int i = 0; i < MidiOut.NumberOfDevices; i++)
            {
                Console.WriteLine(i + ": " + MidiOut.DeviceInfo(i).ProductName + " by " + MidiOut.DeviceInfo(i).Manufacturer);
            }
            int retint = -1;
            
            while (retint < 0)
            {
                Console.Write("Enter device number, then press Enter: ");
                bool goodInt = int.TryParse(Console.ReadLine(), out retint);
                if(goodInt)
                {
                    if(retint >= 0)
                    {
                        if(retint < MidiOut.NumberOfDevices)
                        {
                            return retint;
                        }
                    }
                }
                Console.WriteLine("Something was wrong with your input... lets try again.");
                retint = -1;
            }
            return retint;
        }
    }
}
