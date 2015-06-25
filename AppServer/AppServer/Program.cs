namespace AppServer
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.ServiceProcess;
    using System.Threading;
    using System.Windows.Forms;

    internal static class Program
    {
        private static void HandleException(Exception ex)
        {
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.txt"), true);
                writer.WriteLine(string.Concat(new object[] { DateTime.Now, " ", ex.Message, ex.ToString() }));
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        private static void Main()
        {
#if (!DEBUG)
            try
            {
                Uri uri = new Uri(typeof(string).Assembly.CodeBase);
                string fileName = Path.Combine(Path.GetDirectoryName(uri.LocalPath), "InstallUtil.exe");
                string executablePath = Application.ExecutablePath;
                string text1 = "\"" + Application.ExecutablePath + "\"";
                foreach (string str4 in Environment.GetCommandLineArgs())
                {
                    switch (str4)
                    {
                        case "/uninstall":
                            executablePath = "/u " + executablePath;
                            Process.Start(fileName, "/u \"" + Application.ExecutablePath + "\"");
                            return;

                        case "/install":
                            Process.Start(fileName, "\"" + Application.ExecutablePath + "\"");
                            return;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                Console.WriteLine();
                return;
            }
            Application.ThreadException += (sender, args1) => HandleException(args1.Exception);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += delegate(object sender, UnhandledExceptionEventArgs args2)
            {
                Exception ex = args2.ExceptionObject as Exception;
                if (ex != null)
                {
                    HandleException(ex);
                }
            };
            //System.Diagnostics.Trace.Write("appserver - Main ServiceBase.Run");
            ServiceBase.Run(new ServiceBase[] { new Service() });
#else
            //µ÷ÊÔ³ÌÐò
            Service sev = new Service();
            sev.Main1();
#endif
            
        }
    }
}

