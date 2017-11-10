using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenSaver
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var option = GetOptionSelected(args);
            switch (option.selected)
            {
                case Options.preview:
                    //show the screen saver preview
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    //args[1] is the handle to the preview window
                    Application.Run(new Transparent(new IntPtr(long.Parse(args[1]))));

                    break;
                case Options.configure:
                    MessageBox.Show("The transparent screensaver options cannot be seen",
                    "Transparent Screensaver",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                    break;
                default: // ScreenSaverOptions.show
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Show();
                    Application.Run();
                    break;
            }            
        }

        static OptionProvided GetOptionSelected(string[] args)
        {
            OptionProvided option = new OptionProvided();
            // Set to show by default
            option.selected = Options.show;

            if(args.Length > 0)
            {
                if (args[0].ToLower().Trim().Substring(0, 2) == "/p") //preview
                {
                    if(args[1].ToLower().Trim().Length == 0)
                    {
                        MessageBox.Show("Sorry, but the expected window handle was not provided.",
            "ScreenSaver", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        throw new ArgumentException();

                    }
                    //preview the screen saver
                    option.selected = Options.preview;
                }
                else if (args[0].ToLower().Trim().Substring(0, 2) == "/c") //configure
                {
                    //configure the screen saver
                    option.selected = Options.configure;
                }
            }
            return option;
        }

        static void Show()
        {
            foreach (var screen in Screen.AllScreens)
            {
                Transparent screensaver = new Transparent(screen.Bounds);
                screensaver.Show();
            }
        }
    }
}
