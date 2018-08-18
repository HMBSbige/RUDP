using System;
using System.Threading;

namespace Server
{
	public static class Program
	{
		private static void Run()
		{
			var server = new RudpServer(12345);
			server.Start();

			while (Console.ReadKey().Key != ConsoleKey.C)
			{
				Thread.Sleep(0);
			}

			Console.WriteLine(@"'C' pressed, exiting..");

			server.Stop();
		}

		public static void Main(string[] args)
		{
			Run();
		}
	}
}
