using NAudio.Midi;
using System.Xml;

namespace ReadXMLfromURL
{
    public class locToPad
    {
        public static int getNoteLocFromIndex(int index)
        {
            int originRow = index / 8;
            int positionInNewRow = index % 8;
            int newRow = 7 - originRow;
            return (((newRow) * 8) + positionInNewRow);
        }
    }

    public class Input
    {
        public int number;
        public string? title;
        public bool muted = true;
        public bool active = false;
        public bool preview = false;
        public int overlay = -1;
        public override string ToString()
        {
            string overlaystring = "False";
            if (overlay != -1)
            {
                overlaystring = (overlay + 1).ToString();
            }
            return (this.number + ": " + this.title + "\t\tActive: " + this.active + "\t\tPreview: " + this.preview + "\t\tMuted: " + this.muted + "\t\tOverlay: " + overlaystring);
        }
    }

    public class ReadTheApiUpdateLoop
    {
        public static Input[] RunThisShit()
        {
            Input[] inputs = new Input[64];
            String URLString = "http://localhost:8088/api";
            XmlTextReader reader = new XmlTextReader(URLString);
            int overlaynum = 0;
            while (reader.Read())
            {

                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "input")
                    {
                        Input newInput = new Input();
                        while (reader.MoveToNextAttribute())
                        {
                            if (reader.Name == "number")
                            {
                                newInput.number = int.Parse(reader.Value);
                            }
                            else if (reader.Name == "title")
                            {
                                newInput.title = reader.Value;
                            }
                            else if (reader.Name == "muted")
                            {
                                newInput.muted = bool.Parse(reader.Value);
                            }
                            if (newInput.number != 0)
                            {
                                inputs[newInput.number - 1] = newInput;
                            }

                        }
                    }
                    else if (reader.Name == "overlay")
                    {
                        while (reader.MoveToNextAttribute())
                        {
                            if (reader.Name == "number")
                            {
                                overlaynum = int.Parse(reader.Value);
                            }
                            reader.Read();
                            if (reader.Value != "")
                            {
                                inputs[int.Parse(reader.Value) - 1].overlay = overlaynum - 1;
                            }
                        }
                    }
                    else if (reader.Name == "active")
                    {
                        reader.Read();
                        inputs[int.Parse(reader.Value) - 1].active = true;
                    }
                    else if (reader.Name == "preview")
                    {
                        reader.Read();
                        inputs[int.Parse(reader.Value) - 1].preview = true;
                    }
                }
            }
            return inputs;
        }
    }
    class Looop
    {
        private static Input[] inputs;
        private static int defaultColor = 0;
        private static int defaultPreviewColor = 21;
        private static int defaultActiveColor = 5;
        private static int defaultOverlayColor = 13;
        private static int defaultAudioActiveColor = 46;
        private bool init = true;

        private static void write(MidiOut midiOut, int index, int color)
        {
            var noteOnEvent = new NoteOnEvent(0L, 6, locToPad.getNoteLocFromIndex(index - 1), color, 0);
            midiOut.Send(noteOnEvent.GetAsShortMessage());
            midiOut.Send(noteOnEvent.OffEvent.GetAsShortMessage());
            return;
        }

        private static void writeBlinkBrightness(MidiOut midiOut, int index, int color, int birghtorblink)
        {
            var noteOnEvent = new NoteOnEvent(0L, birghtorblink, locToPad.getNoteLocFromIndex(index - 1), color, 0);
            midiOut.Send(noteOnEvent.GetAsShortMessage());
            midiOut.Send(noteOnEvent.OffEvent.GetAsShortMessage());
            return;
        }

        private static void writeDefault(MidiOut midiOut, int index)
        {
            write(midiOut, index, defaultColor);
        }

        private static void writeLittle(MidiOut midiOut, int index, bool on, bool blink)
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
        private static void writePreview(MidiOut midiOut, int index)
        {
            var noteOnEvent = new NoteOnEvent(0L, 6, locToPad.getNoteLocFromIndex(index - 1), defaultPreviewColor, 0);
            midiOut.Send(noteOnEvent.GetAsShortMessage());
            midiOut.Send(noteOnEvent.OffEvent.GetAsShortMessage());
            return;
        }

        private static void writeActive(MidiOut midiOut, int index)
        {
            var noteOnEvent = new NoteOnEvent(0L, 6, locToPad.getNoteLocFromIndex(index - 1), defaultActiveColor, 0);
            midiOut.Send(noteOnEvent.GetAsShortMessage());
            midiOut.Send(noteOnEvent.OffEvent.GetAsShortMessage());
            return;
        }
        private static void writePreviewOverlay(MidiOut midiOut, int index)
        {
            var noteOnEvent = new NoteOnEvent(0L, 9, locToPad.getNoteLocFromIndex(index - 1), defaultPreviewColor, 0);
            midiOut.Send(noteOnEvent.GetAsShortMessage());
            midiOut.Send(noteOnEvent.OffEvent.GetAsShortMessage());
            return;
        }
        private static void writeOverlay(MidiOut midiOut, int index)
        {
            write(midiOut, index, defaultOverlayColor); return;
        }

        private static void writeAudioActive(MidiOut midiOut, int index)
        {
            //Console.WriteLine(locToPad.getNoteLocFromIndex(index - 1 + 32));
            var noteOnEvent = new NoteOnEvent(0L, 6, locToPad.getNoteLocFromIndex(index - 1 + 32), defaultAudioActiveColor, 0);
            midiOut.Send(noteOnEvent.GetAsShortMessage());
            midiOut.Send(noteOnEvent.OffEvent.GetAsShortMessage());
            return;
        }

        private static void writeAudioMuted(MidiOut midiOut, int index)
        {

            var noteOnEvent = new NoteOnEvent(0L, 6, locToPad.getNoteLocFromIndex(index - 1 + 32), defaultColor, 0);
            midiOut.Send(noteOnEvent.GetAsShortMessage());
            midiOut.Send(noteOnEvent.OffEvent.GetAsShortMessage());
            return;
        }
        private static async void WritePreviewActive(MidiOut midiOut, int index)
        {
            var noteOnEvent = new NoteOnEvent(0L, 6, locToPad.getNoteLocFromIndex(index - 1), defaultPreviewColor, 0);
            midiOut.Send(noteOnEvent.GetAsShortMessage());
            midiOut.Send(noteOnEvent.OffEvent.GetAsShortMessage());
            Thread.Sleep(50);
            noteOnEvent = new NoteOnEvent(0L, 6, locToPad.getNoteLocFromIndex(index - 1), defaultActiveColor, 0);
            midiOut.Send(noteOnEvent.GetAsShortMessage());
            midiOut.Send(noteOnEvent.OffEvent.GetAsShortMessage());
            Thread.Sleep(50);
            return;
        }
        private static void clearPad(MidiOut midiOut)
        {
            for (int i = 0; i < 64; i++)
            {
                var noteOnEvent = new NoteOnEvent(0L, 6, i, defaultColor, 0);
                midiOut.Send(noteOnEvent.GetAsShortMessage());
                midiOut.Send(noteOnEvent.OffEvent.GetAsShortMessage());
            }
            for (int i = 100; i < 119; i++)
            {
                writeLittle(midiOut, i, false, false);
            }
        }

        private static void clearIndex(MidiOut midiOut, int index)
        {
            var noteOnEvent = new NoteOnEvent(0L, 6, locToPad.getNoteLocFromIndex(index - 1), defaultColor, 0);
            midiOut.Send(noteOnEvent.GetAsShortMessage());
            midiOut.Send(noteOnEvent.OffEvent.GetAsShortMessage());
        }

        private static void sleepMode(MidiOut midiOut)
        {
            Random r = new Random();
            int[] possibleAmounts = { 1, 2, 4, 8, 16, 32 };
            bool theSame = Convert.ToBoolean(r.Next(2));
            int amountpercycle = possibleAmounts[r.Next(possibleAmounts.Length)];
            int clr = r.Next(127);
            int mode = r.Next(10) + 1;
            for (int i = 1; i <= 64; i++)
            {
                writeBlinkBrightness(midiOut, i, clr, mode);
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
                writeLittle(midiOut, i, Convert.ToBoolean(r.Next(2) - 1), Convert.ToBoolean(r.Next(2) - 1));
            }
        }

        static void Main(string[] args)
        {
            int midiIn = MidiIn.NumberOfDevices;
            bool needClear = false;
            MidiOut midi = new MidiOut(1);
            clearPad(midi);
            while (true)
            {
                try
                {
                    inputs = ReadTheApiUpdateLoop.RunThisShit();
                    if (inputs[0] != null)
                    {
                        for (int j = 112; j < 114; j++)
                        {
                            writeLittle(midi, j, true, false);
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
                                    clearPad(midi);
                                    needClear = false;
                                    Console.Clear();
                                    Console.WriteLine("Connected to vMix.");
                                    for (int j = 112; j < 114; j++)
                                    {
                                        writeLittle(midi, j, true, false);
                                    }
                                }
                                if (!item.preview && !item.active && (item.overlay == -1))
                                {
                                    writeDefault(midi, item.number);
                                }
                                if (item.preview && item.active)
                                {
                                    WritePreviewActive(midi, item.number);
                                }
                                if (item.preview)
                                {
                                    writePreview(midi, item.number);
                                }
                                if (item.active)
                                {
                                    writeActive(midi, item.number);
                                }
                                if (item.overlay != -1)
                                {
                                    if (item.preview)
                                    {
                                        writePreviewOverlay(midi, item.number);
                                    }
                                    else
                                        writeOverlay(midi, item.number);
                                }
                                if (!item.muted)
                                {
                                    writeAudioActive(midi, item.number);
                                }
                                if (item.muted)
                                {
                                    writeAudioMuted(midi, item.number);
                                }
                            }
                        }
                        else if (item == null)
                        {
                            if (i <= 32)
                                writeDefault(midi, i + 1);
                        }
                    }
                    Thread.Sleep(250);
                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine("Failed to connect to vMix or read Input List. Going to sleep.");
                    Console.WriteLine(e.Message);
                    sleepMode(midi);
                    needClear = true;
                }
            }
        }
    }
}