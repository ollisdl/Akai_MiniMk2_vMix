using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKAIVMIX
{
    internal class LocToPad
    {
        protected bool topToBottom = true;

        public LocToPad(bool topToBottom)
        {
            this.topToBottom = topToBottom;
            return;
        }

        public int getNoteLocFromIndex(int index)
        {
            if(!topToBottom)
            {
                return index;
            }
            int originRow = index / 8;
            int positionInNewRow = index % 8;
            int newRow = 7 - originRow;
            return (((newRow) * 8) + positionInNewRow);
        }

        public int getAudioNoteLocFromIndex(int index)
        {
            return getNoteLocFromIndex(index + 32);
        }
    }
}
