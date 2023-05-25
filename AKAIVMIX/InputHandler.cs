﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Midi;
using static AKAIVMIX.MidiOutputHandler;

namespace AKAIVMIX
{
    internal static class InputHandler
    {
        public static void CheckInputsAndHandle(MidiOut midi, LocToPad locToPad, Input[] inputs)
        {
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
                        if (!item.preview && !item.active && (item.overlay == -1))
                        {
                            WritePreviewActive(midi, locToPad, item.number);
                        }
                        if (item.preview && item.active)
                        {
                            WritePreviewActive(midi, locToPad, item.number);
                        }
                        if (item.preview && !item.active)
                        {
                            WritePreview(midi, locToPad, item.number);
                        }
                        if (item.active)
                        {
                            WriteActive(midi, locToPad, item.number);
                        }
                        if (item.overlay != -1)
                        {
                            if (item.preview)
                            {
                                WritePreviewAndOverlay(midi, locToPad, item.number);
                            }
                            else
                                WriteOverlay(midi, locToPad, item.number);
                        }
                        if (!item.muted)
                        {
                            WriteAudioActive(midi, locToPad, item.number);
                        }
                        if (item.muted)
                        {
                            WriteAudioMuted(midi, locToPad, item.number);
                        }
                    }
                }
                else if (item == null)
                {
                    if (i <= 32)
                        WriteDefault(midi, locToPad, i + 1);
                }
            }
        }
    }
}