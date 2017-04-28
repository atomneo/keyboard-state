using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

namespace KeyboardState
{
    public class KeyState
    {
        private ControlContainer container = new ControlContainer();
        private NotifyIcon numIcon;
        private NotifyIcon capsIcon;
        private Timer timer;
        private bool oldNum, oldCaps;
        private int baloonTime = 0;

        private void Initialize()
        {
            SetStartup();

            numIcon = new NotifyIcon(container);
            capsIcon = new NotifyIcon(container);

            MenuItem closeMI = new MenuItem(MenuMerge.Add, 1, Shortcut.None, "Exit", ExitClick, null, null, null);
            MenuItem settingsMI = new MenuItem(MenuMerge.Add, 1, Shortcut.None, "Settings", SettingsClick, null, null, null);
            MenuItem[] menuItems = new MenuItem[2];

            menuItems[0] = settingsMI;
            menuItems[1] = closeMI;
            
            ContextMenu contextMenu = new ContextMenu(menuItems);

            numIcon.ContextMenu = contextMenu;
            capsIcon.ContextMenu = contextMenu;

            firstTick();

            numIcon.Text = "NUM LOCK state";
            numIcon.Visible = true;
            capsIcon.Text = "CAPS LOCK state";
            capsIcon.Visible = true;

            timer = new Timer(container);
            timer.Interval = 250;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Enabled = true;
        }

        private void SettingsClick(object sender, EventArgs e)
        {
            Settings s =new Settings();
            s.Show();
        }

        void firstTick()
        {
            bool num = Control.IsKeyLocked(Keys.NumLock);
            bool caps = Control.IsKeyLocked(Keys.CapsLock);

            Icon icon;
            if (num)
            {
                icon = Icon.FromHandle(Properties.Resources.num_on.GetHicon());
            }
            else
            {
                icon = Icon.FromHandle(Properties.Resources.num_off.GetHicon());
            }
            numIcon.Icon = icon;

            if (caps)
            {
                icon = Icon.FromHandle(Properties.Resources.caps_on.GetHicon());
            }
            else
            {
                icon = Icon.FromHandle(Properties.Resources.caps_off.GetHicon());
            }
            capsIcon.Icon = icon;

            oldNum = num;
            oldCaps = caps;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            bool num = Control.IsKeyLocked(Keys.NumLock);
            bool caps = Control.IsKeyLocked(Keys.CapsLock);
            if ((num == oldNum) && (caps == oldCaps))
                return;

            Icon icon;

            if (num != oldNum)
            {
                if (num)
                {
                    icon = Icon.FromHandle(Properties.Resources.num_on.GetHicon());
                    numIcon.ShowBalloonTip(baloonTime, "NUM LOCK ON", " ", ToolTipIcon.Info);
                }
                else
                {
                    icon = Icon.FromHandle(Properties.Resources.num_off.GetHicon());
                    numIcon.ShowBalloonTip(baloonTime, "NUM LOCK OFF", " ", ToolTipIcon.Info);
                }
                numIcon.Icon = icon;
            }

            if (caps != oldCaps)
            {
                if (caps)
                {
                    icon = Icon.FromHandle(Properties.Resources.caps_on.GetHicon());
                    capsIcon.ShowBalloonTip(baloonTime, "CAPS LOCK ON", " ", ToolTipIcon.Info);
                }
                else
                {
                    icon = Icon.FromHandle(Properties.Resources.caps_off.GetHicon());
                    capsIcon.ShowBalloonTip(baloonTime, "CAPS LOCK OFF", " ", ToolTipIcon.Info);
                }
                capsIcon.Icon = icon;
            }

            oldNum = num;
            oldCaps = caps;

            //if ((num) && (caps))
            //    icon = Icon.FromHandle(Properties.Resources.num_caps.GetHicon());
            //else if (num)
            //    icon = Icon.FromHandle(Properties.Resources.num.GetHicon());
            //else if (caps)
            //    icon = Icon.FromHandle(Properties.Resources.caps.GetHicon());
            //else 
            //    icon = Icon.FromHandle(Properties.Resources.none.GetHicon());
            //numIcon.Icon = icon;
        }

        private void ExitClick(object sender, EventArgs eventArgs)
        {
            timer.Enabled = false;
            Application.Exit();
        }

        public KeyState()
        {
            Initialize();
        }

        public void SetStartup()
        {
            SetStartup(Properties.Settings.Default.ApplicationName, Properties.Settings.Default.Autostart);
        }

        /// <summary>
        /// Add/Remove registry entries for windows startup.
        /// </summary>
        /// <param name="AppName">Name of the application.</param>
        /// <param name="enable">if set to <c>true</c> [enable].</param>
        private void SetStartup(string AppName, bool enable)
        {
            string runKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

            RegistryKey startupKey;

            if (enable)
            {
                if (!Autostart(AppName))
                {
                    startupKey = Registry.CurrentUser.OpenSubKey(runKey, true);
                    // Add startup reg key
                    startupKey.SetValue(AppName, Application.ExecutablePath.ToString());
                    startupKey.Close();
                }
            }
            else
            {
                //Debugger.Break();
                // remove startup
                startupKey = Registry.CurrentUser.OpenSubKey(runKey, true);
                startupKey.DeleteValue(AppName, false);
                startupKey.Close();
            }
        }

        public bool Autostart()
        {
            return Autostart(Properties.Settings.Default.ApplicationName);
        }

        private bool Autostart(string AppName)
        {
            string runKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
            RegistryKey startupKey = Registry.CurrentUser.OpenSubKey(runKey);
            bool startup = startupKey.GetValue(AppName) != null;
            startupKey.Close();
            return startup;
        }
    }
}
