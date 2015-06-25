using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Windows.Forms;

namespace TimerServer
{
	internal static class Program
	{
		private static void Main()
		{
			try
			{
				Uri uri = new Uri(typeof(string).Assembly.CodeBase);
				string directoryName = Path.GetDirectoryName(uri.LocalPath);
				string str = Path.Combine(directoryName, "InstallUtil.exe");
				string executablePath = Application.ExecutablePath;
				string.Concat("\"", Application.ExecutablePath, "\"");
				string[] commandLineArgs = Environment.GetCommandLineArgs();
				int num = 0;
				while (num < (int)commandLineArgs.Length)
				{
					string str1 = commandLineArgs[num];
					if (str1 == "/uninstall")
					{
						executablePath = string.Concat("/u ", executablePath);
						Process.Start(str, string.Concat("/u \"", Application.ExecutablePath, "\""));
						return;
					}
					else if (str1 != "/install")
					{
						num++;
					}
					else
					{
						Process.Start(str, string.Concat("\"", Application.ExecutablePath, "\""));
						return;
					}
				}
				ServiceBase.Run(new ServiceBase[] { new Service() });
				return;
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.ToString());
				Console.WriteLine();
			}
		}
	}
}