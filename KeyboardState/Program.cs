using System;
using System.Windows.Forms;

namespace KeyboardState
{
    public static class Program
    {
        public static KeyState keyState;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            keyState = new KeyState();

            Application.Run();
        }
    }
}
