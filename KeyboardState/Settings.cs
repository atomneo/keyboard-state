using System;
using System.Windows.Forms;

namespace KeyboardState
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = Program.keyState.Autostart();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Autostart = checkBox1.Checked;
            Properties.Settings.Default.Save();
            Program.keyState.SetStartup();
        }

        private void Settings_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose(true);
        }
    }
}
