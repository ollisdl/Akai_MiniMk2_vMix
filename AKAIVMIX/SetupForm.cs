using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AKAIVMIX
{
    internal class SetupForm : Form
    {
        public static int returnInt = -1;
        public static Button? confirm;
        private Button button1;
        private CheckBox checkBox1;
        private ComboBox comboBox1;
        private Label label1;
        private PictureBox ActiveColorDisplay;
        private GroupBox groupBox1;
        private Button activeColorButton;
        private Button defaultColorButton;
        private PictureBox DefaultColorDisplay;
        private Button audioActiveColorButton;
        private PictureBox AudioActiveColorDisplay;
        private Button sleepColorButton;
        private PictureBox SleepColorDisplay;
        private Button mutedColorButton;
        private PictureBox MutedColorDisplay;
        public static Button? exit;

        public SetupForm(string title) 
        {           
            this.Text= title;
            InitializeComponent();
            this.ActiveColorDisplay.BackColor = RefThings.colors[Looop.activeColor];
            this.DefaultColorDisplay.BackColor = RefThings.colors[Looop.defaultColor];
            this.SleepColorDisplay.BackColor = RefThings.colors[Looop.sleepColor];
            this.AudioActiveColorDisplay.BackColor = RefThings.colors[Looop.audioColor];
            this.MutedColorDisplay.BackColor = RefThings.colors[Looop.mutedColor];
            this.checkBox1.Checked = Looop.locToPad.topToBottom;

        }

        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ActiveColorDisplay = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mutedColorButton = new System.Windows.Forms.Button();
            this.MutedColorDisplay = new System.Windows.Forms.PictureBox();
            this.audioActiveColorButton = new System.Windows.Forms.Button();
            this.AudioActiveColorDisplay = new System.Windows.Forms.PictureBox();
            this.sleepColorButton = new System.Windows.Forms.Button();
            this.SleepColorDisplay = new System.Windows.Forms.PictureBox();
            this.defaultColorButton = new System.Windows.Forms.Button();
            this.DefaultColorDisplay = new System.Windows.Forms.PictureBox();
            this.activeColorButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ActiveColorDisplay)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MutedColorDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AudioActiveColorDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SleepColorDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DefaultColorDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button1.Location = new System.Drawing.Point(641, 453);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(140, 45);
            this.button1.TabIndex = 0;
            this.button1.Text = "Submit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(86, 77);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(169, 19);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "Use \"Top To Bottom\" Mode";
            this.checkBox1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.AllowDrop = true;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(178, 114);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.comboBox1.Size = new System.Drawing.Size(121, 23);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.Text = "Sleep Mode";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(86, 117);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Sleep Mode";
            // 
            // ActiveColorDisplay
            // 
            this.ActiveColorDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ActiveColorDisplay.Location = new System.Drawing.Point(19, 42);
            this.ActiveColorDisplay.Name = "ActiveColorDisplay";
            this.ActiveColorDisplay.Size = new System.Drawing.Size(21, 23);
            this.ActiveColorDisplay.TabIndex = 4;
            this.ActiveColorDisplay.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mutedColorButton);
            this.groupBox1.Controls.Add(this.MutedColorDisplay);
            this.groupBox1.Controls.Add(this.audioActiveColorButton);
            this.groupBox1.Controls.Add(this.AudioActiveColorDisplay);
            this.groupBox1.Controls.Add(this.sleepColorButton);
            this.groupBox1.Controls.Add(this.SleepColorDisplay);
            this.groupBox1.Controls.Add(this.defaultColorButton);
            this.groupBox1.Controls.Add(this.DefaultColorDisplay);
            this.groupBox1.Controls.Add(this.activeColorButton);
            this.groupBox1.Controls.Add(this.ActiveColorDisplay);
            this.groupBox1.Location = new System.Drawing.Point(86, 170);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(695, 294);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // mutedColorButton
            // 
            this.mutedColorButton.Location = new System.Drawing.Point(455, 100);
            this.mutedColorButton.Name = "mutedColorButton";
            this.mutedColorButton.Size = new System.Drawing.Size(174, 23);
            this.mutedColorButton.TabIndex = 11;
            this.mutedColorButton.Text = "Choose Muted Color";
            this.mutedColorButton.UseVisualStyleBackColor = true;
            // 
            // MutedColorDisplay
            // 
            this.MutedColorDisplay.BackColor = System.Drawing.Color.Gray;
            this.MutedColorDisplay.Location = new System.Drawing.Point(406, 97);
            this.MutedColorDisplay.Name = "MutedColorDisplay";
            this.MutedColorDisplay.Size = new System.Drawing.Size(21, 23);
            this.MutedColorDisplay.TabIndex = 10;
            this.MutedColorDisplay.TabStop = false;
            // 
            // audioActiveColorButton
            // 
            this.audioActiveColorButton.Location = new System.Drawing.Point(455, 45);
            this.audioActiveColorButton.Name = "audioActiveColorButton";
            this.audioActiveColorButton.Size = new System.Drawing.Size(174, 23);
            this.audioActiveColorButton.TabIndex = 11;
            this.audioActiveColorButton.Text = "Choose Audio Active Color";
            this.audioActiveColorButton.UseVisualStyleBackColor = true;
            // 
            // AudioActiveColorDisplay
            // 
            this.AudioActiveColorDisplay.BackColor = System.Drawing.Color.Gray;
            this.AudioActiveColorDisplay.Location = new System.Drawing.Point(406, 42);
            this.AudioActiveColorDisplay.Name = "AudioActiveColorDisplay";
            this.AudioActiveColorDisplay.Size = new System.Drawing.Size(21, 23);
            this.AudioActiveColorDisplay.TabIndex = 10;
            this.AudioActiveColorDisplay.TabStop = false;
            // 
            // sleepColorButton
            // 
            this.sleepColorButton.Location = new System.Drawing.Point(68, 160);
            this.sleepColorButton.Name = "sleepColorButton";
            this.sleepColorButton.Size = new System.Drawing.Size(174, 23);
            this.sleepColorButton.TabIndex = 9;
            this.sleepColorButton.Text = "Choose Sleep Color";
            this.sleepColorButton.UseVisualStyleBackColor = true;
            // 
            // SleepColorDisplay
            // 
            this.SleepColorDisplay.BackColor = System.Drawing.Color.Gray;
            this.SleepColorDisplay.Location = new System.Drawing.Point(19, 157);
            this.SleepColorDisplay.Name = "SleepColorDisplay";
            this.SleepColorDisplay.Size = new System.Drawing.Size(21, 23);
            this.SleepColorDisplay.TabIndex = 8;
            this.SleepColorDisplay.TabStop = false;
            // 
            // defaultColorButton
            // 
            this.defaultColorButton.Location = new System.Drawing.Point(68, 100);
            this.defaultColorButton.Name = "defaultColorButton";
            this.defaultColorButton.Size = new System.Drawing.Size(174, 23);
            this.defaultColorButton.TabIndex = 7;
            this.defaultColorButton.Text = "Choose Default Color";
            this.defaultColorButton.UseVisualStyleBackColor = true;
            this.defaultColorButton.Click += new System.EventHandler(this.defaultColorButton_Click);
            // 
            // DefaultColorDisplay
            // 
            this.DefaultColorDisplay.BackColor = System.Drawing.Color.Gray;
            this.DefaultColorDisplay.Location = new System.Drawing.Point(19, 97);
            this.DefaultColorDisplay.Name = "DefaultColorDisplay";
            this.DefaultColorDisplay.Size = new System.Drawing.Size(21, 23);
            this.DefaultColorDisplay.TabIndex = 6;
            this.DefaultColorDisplay.TabStop = false;
            // 
            // activeColorButton
            // 
            this.activeColorButton.Location = new System.Drawing.Point(68, 45);
            this.activeColorButton.Name = "activeColorButton";
            this.activeColorButton.Size = new System.Drawing.Size(174, 23);
            this.activeColorButton.TabIndex = 5;
            this.activeColorButton.Text = "Choose Active Color";
            this.activeColorButton.UseVisualStyleBackColor = true;
            this.activeColorButton.Click += new System.EventHandler(this.activeColorButton_Click);
            // 
            // SetupForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(853, 510);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button1);
            this.Name = "SetupForm";
            ((System.ComponentModel.ISupportInitialize)(this.ActiveColorDisplay)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MutedColorDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AudioActiveColorDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SleepColorDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DefaultColorDisplay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void activeColorButton_Click(object sender, EventArgs e)
        {
            int activeColor = ClrForm.formResult("Choose a color for the active source");
            if (activeColor == -1)
            {
                activeColor = 1;
            }
            Looop.config.Write("activeColor", activeColor.ToString());
            Looop.activeColor = activeColor;
            this.ActiveColorDisplay.BackColor = RefThings.colors[activeColor];
        }

        private void defaultColorButton_Click(object sender, EventArgs e)
        {
            int defaultClr = ClrForm.formResult("Choose a default color");
            if (defaultClr == -1)
            {
                defaultClr = 1;
            }
            Looop.config.Write("defaultColor", defaultClr.ToString());
            Looop.defaultColor = defaultClr;
            this.DefaultColorDisplay.BackColor = RefThings.colors[defaultClr];
        }
    }
}
