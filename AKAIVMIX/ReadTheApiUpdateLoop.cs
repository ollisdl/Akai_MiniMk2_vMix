using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace AKAIVMIX
{
    internal class ReadTheApiUpdateLoop
    {
        internal static Input[] ReadApi()
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
                            else if (reader.Name == "volume")
                            {
                                newInput.hasAudio = true;
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

        internal static Input[] audio(Input[] inputs)
        {
            Input[] inputsWithAudio = new Input[8];
            int count = 0;

            foreach (Input input in inputs)
            {
                if (input.hasAudio)
                {
                    if (count < 7)
                    {
                        inputsWithAudio[count] = input;
                        count++;
                    }
                }
            }
        }
    }
}
