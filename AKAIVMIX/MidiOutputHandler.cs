﻿using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace AKAIVMIX
{
    internal static class MidiOutputHandler
    {

        public static void WriteWithBlinkBrightnessMidi(MidiOut midiOut, LocToPad locToPad, int index, int color, int birghtorblink)
        {
            var noteOnEvent = new NoteOnEvent(0L, birghtorblink, locToPad.getNoteLocFromIndex(index - 1), color, 0);
            midiOut.Send(noteOnEvent.GetAsShortMessage());
            midiOut.Send(noteOnEvent.OffEvent.GetAsShortMessage());
            return;
        }

        public static void WriteLittle(MidiOut midiOut, int index, bool on, bool blink)
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
            WriteWithBlinkBrightnessMidi(midiOut, locToPad, index, color, 6);
        }

        public static void WriteDefault(MidiOut midiOut, LocToPad locToPad, int index)
        {
            WriteMidi(midiOut, locToPad, index, Looop.defaultColor);
        }

        public static void WritePreview(MidiOut midiOut, LocToPad locToPad, int index)
        {
            WriteMidi(midiOut, locToPad, index, Looop.previewColor);
        }

        public static void WriteActive(MidiOut midiOut, LocToPad locToPad, int index)
        {
            WriteMidi(midiOut, locToPad, index, Looop.activeColor);
        }

        public static void WritePreviewAndOverlay(MidiOut midiOut, LocToPad locToPad, int index)
        {
            WriteWithBlinkBrightnessMidi(midiOut, locToPad, index, Looop.previewColor, 9);
        }

        public static void WriteOverlay(MidiOut midiOut, LocToPad locToPad, int index)
        {
            WriteMidi(midiOut, locToPad, index, Looop.overlayColor);
        }

        public static void WriteAudioActive(MidiOut midiOut, LocToPad locToPad, int index)
        {
            WriteMidi(midiOut, locToPad, index + 32, Looop.audioColor);
        }

        public static void WriteAudioMuted(MidiOut midiOut, LocToPad locToPad, int index)
        {
            WriteMidi(midiOut, locToPad, index + 32, Looop.mutedColor);
        }

        public static async void WritePreviewActive(MidiOut midiOut, LocToPad locToPad, int index)
        {
            WriteMidi(midiOut, locToPad, index, Looop.previewColor);
            Thread.Sleep(50);
            WriteMidi(midiOut, locToPad, index, Looop.activeColor);
            Thread.Sleep(50);
            return;
        }

        public static void ClearPad(MidiOut midiOut, LocToPad locToPad)
        {
            for (int i = 0; i <= 64; i++)
            {
                WriteMidi(midiOut, locToPad, i, Looop.defaultColor);
            }

            for (int i = 100; i < 119; i++)
            {
                WriteLittle(midiOut, i, false, false);
            }
        }

        public static void ClearIndex(MidiOut midiOut, LocToPad locToPad, int index)
        {
            WriteMidi(midiOut, locToPad, index, Looop.defaultColor);
        }

        readonly static int SleepModes = 2;
        public static void SleepMode(MidiOut midiOut, LocToPad locToPad, int sleepMode, int color)
        {
            if (sleepMode == 1)
                SleepMode1(midiOut, locToPad);
            else if (sleepMode == 2)
                SleepMode2(midiOut, locToPad, color);
            else
                SleepMode1(midiOut, locToPad);

        }

        private static void SleepMode1(MidiOut midiOut, LocToPad locToPad)
        {
            Random r = new Random();
            int[] possibleAmounts = { 1, 2, 4, 8, 16, 32 };
            bool theSame = Convert.ToBoolean(r.Next(2));
            int amountpercycle = possibleAmounts[r.Next(possibleAmounts.Length)];
            int clr = r.Next(127);
            int mode = r.Next(10) + 1;
            for (int i = 1; i <= 64; i++)
            {
                WriteWithBlinkBrightnessMidi(midiOut, locToPad, i, clr, mode);
                if (i % amountpercycle == 0)
                {
                    Thread.Sleep(50 * amountpercycle);
                    if (!theSame)
                    {
                        clr = r.Next(127);
                    }
                }
            }
            for (int i = 100; i <= 117; i++)
            {
                WriteLittle(midiOut, i, Convert.ToBoolean(r.Next(2) - 1), Convert.ToBoolean(r.Next(2) - 1));
            }

        }

        private static void SleepMode2(MidiOut midiOut, LocToPad locToPad, int color)
        {
            if (color > 127 || color < 0)
                color = 1;
            for (int i = 1; i <= 64; i++)
            {
                WriteWithBlinkBrightnessMidi(midiOut, locToPad, i, color, 6);
            }

            for (int i = 100; i <= 117; i++)
            {
                WriteLittle(midiOut, i, true, false);
            }

        }
    }
}
