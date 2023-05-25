using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKAIVMIX
{
    internal static class MidiOutputHandler
    {

        private static int defaultColor = 0;
        private static int defaultPreviewColor = 21;
        private static int defaultActiveColor = 5;
        private static int defaultOverlayColor = 13;
        private static int defaultAudioActiveColor = 46;
        private static int defaultAudioDisabledColor = 0;


        public static void writeWithBlinkBrightnessMidi(MidiOut midiOut, LocToPad locToPad, int index, int color, int birghtorblink)
        {
            var noteOnEvent = new NoteOnEvent(0L, birghtorblink, locToPad.getNoteLocFromIndex(index - 1), color, 0);
            midiOut.Send(noteOnEvent.GetAsShortMessage());
            midiOut.Send(noteOnEvent.OffEvent.GetAsShortMessage());
            return;
        }

        public static void writeLittle(MidiOut midiOut, int index, bool on, bool blink)
        {
            var noteOnEvent = new NoteOnEvent(0L, 1, index, 0, 0);
            if (on)
            {
                if (blink)
                {
                    noteOnEvent = new NoteOnEvent(0L, 1, index, 2, 0);
                }
                else
                {
                    noteOnEvent = new NoteOnEvent(0L, 1, index, 8, 0);
                }

            }
            midiOut.Send(noteOnEvent.GetAsShortMessage());
            midiOut.Send(noteOnEvent.OffEvent.GetAsShortMessage());
            return;
        }

        public static void WriteMidi(MidiOut midiOut, LocToPad locToPad, int index, int color)
        {
            writeWithBlinkBrightnessMidi(midiOut,locToPad,index,color, 6);
        }

        public static void WriteDefault(MidiOut midiOut, LocToPad locToPad, int index)
        {
            WriteMidi(midiOut,locToPad,index,defaultColor);
        }

        public static void writePreview(MidiOut midiOut, LocToPad locToPad, int index)
        {
            WriteMidi(midiOut, locToPad, index, defaultPreviewColor);
        }

        public static void writeActive(MidiOut midiOut, LocToPad locToPad, int index)
        {
            WriteMidi(midiOut, locToPad, index, defaultActiveColor);
        }

        public static void writePreviewAndOverlay(MidiOut midiOut, LocToPad locToPad, int index)
        {
            writeWithBlinkBrightnessMidi(midiOut, locToPad, index, defaultPreviewColor, 9);
        }

        public static void writeOverlay(MidiOut midiOut, LocToPad locToPad, int index)
        {
            WriteMidi(midiOut, locToPad, index, defaultOverlayColor);
        }

        public static void writeAudioActive(MidiOut midiOut, LocToPad locToPad, int index)
        {
            WriteMidi(midiOut, locToPad, index + 32, defaultAudioActiveColor);          
        }

        public static void 







    }
}
