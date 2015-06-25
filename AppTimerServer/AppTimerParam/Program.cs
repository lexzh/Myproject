using System;
using System.Windows.Forms;

namespace AppServerParam
{
	internal static class Program
	{
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new GpsAppTimerParam(args));
		}
	}
}