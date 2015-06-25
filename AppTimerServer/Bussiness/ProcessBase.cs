using System;
using Library;

namespace Bussiness
{
	public abstract class ProcessBase
	{
		public LogHelper logHelper = new LogHelper();

		protected ProcessBase()
		{
		}

		public abstract void start();

		public abstract void stop();
	}
}