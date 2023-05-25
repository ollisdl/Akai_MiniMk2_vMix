using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKAIVMIX
{
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
}
