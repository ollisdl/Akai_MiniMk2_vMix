using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AKAIVMIX
{
    internal class SleepModeForm : Form
    {
        public static int returnInt = -1;
        public static Button? confirm;
        public SleepModeForm(string title) 
        {
            
            int counter = 0;
            int row = 0;
            int col = 0;
            Button b1 = new Button();
            b1.Size = new Size(300, 40);
            b1.Text = "Random Color and Pattern";
            b1.Top = row * 40;
            row++;

            Button b2 = new Button();
            b2.Size = new Size(300, 40);
            b2.Text = "Single Color Full Brightness";
            b2.Top = row * 40;
            row++;

            b1.Click += new EventHandler(option_click);
            b2.Click += new EventHandler(option_click);

            this.Controls.Add(b1); 
            this.Controls.Add(b2);




            confirm = new Button();
            confirm.Size = new Size(300, 40);
            confirm.Text = "Click to Confirm";
            confirm.Top = (row+1) * 40;
            confirm.Left = 0;
            confirm.Click += new EventHandler(button_click);
            this.Controls.Add(confirm);
            this.Size = new Size(40*9,40*(row+2));
            this.Text = title;
        }

        private void button_click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void option_click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            returnInt = b.Top / 40;
            MidiOutputHandler.SleepMode(Looop.midi, Looop.locToPad, returnInt, Looop.activeColor);
        }

        public static int formResult(string title)
        {
            using (var f = new SleepModeForm(title))
            {
                f.ShowDialog();
                return returnInt;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ClrForm
            // 
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "ClrForm";
            this.ResumeLayout(false);

        }
    }
}
