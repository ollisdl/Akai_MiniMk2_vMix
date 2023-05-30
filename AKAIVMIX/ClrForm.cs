using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AKAIVMIX
{
    internal class ClrForm : Form
    {
        public static int returnInt = -1;
        public static Button? confirm;
        public static Button? colorPreview;
        public ClrForm(string title) 
        {
            
            int counter = 0;
            int row = 0;
            int col = 0;
            foreach (Color color in RefThings.colors)
            {
               
                Button button = new Button();
                button.Size = new Size(40, 40);
                button.Top = row*40;
                button.Left = col;
                col += 40;

                if ((counter+1)%8 == 0)
                {
                    row ++;
                    col = 0;
                }
                button.Text = counter.ToString();
                counter++;
                
                button.BackColor = color;
                button.ForeColor = color;
                this.Controls.Add(button);
                button.Click += new EventHandler(color_click);
            }
            confirm = new Button();
            confirm.Size = new Size(200, 40);
            confirm.Text = "Select This Color";
            confirm.Top = row * 40;
            confirm.Left = 20;
            colorPreview = new Button();
            colorPreview.Size = new Size(100, 40);
            colorPreview.Text = "Select This Color";
            colorPreview.Top = row * 40;
            colorPreview.Left = 220;
            confirm.Click += new EventHandler(button_click);
            this.Controls.Add(confirm);
            this.Controls.Add(colorPreview);
            this.Size = new Size(40*9,40*(row+2));
            this.Text = title;
        }

        private void button_click(object sender, EventArgs e)
        {
            Button b = colorPreview;
            if(b != null )
            {
                if (b.Text != null)
                    returnInt = int.Parse(b.Text);
            }
            this.Close();
            
        }

        private void color_click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if(b != null )
            {
                if(colorPreview != null)
                {
                    colorPreview.BackColor = b.BackColor;
                    colorPreview.ForeColor = b.ForeColor;
                    colorPreview.Text = b.Text;
                    MidiOutputHandler.SleepMode(Looop.midi, Looop.locToPad, 2, int.Parse(colorPreview.Text));
                }
            }
        }

        public static int formResult(string title)
        {
            using (var f = new ClrForm(title))
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
            this.ClientSize = new System.Drawing.Size(324, 509);
            this.Name = "ClrForm";
            this.ResumeLayout(false);

        }
    }
}
