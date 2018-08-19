using Protocol.Packets;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Client
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		private RudpClient client;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void InitializeClient(string username, string server)
		{
			Title = $@"RUDP Client (Server: {server} Username: {username})";
			client = new RudpClient(server, 12345, this);
			client.Start();
			client.SendPacket(new LoginPacket { Username = username });
		}

		private void Window_Initialized(object sender, EventArgs e)
		{
			var login = new LoginWindow();
			if (login.ShowDialog() == true)
			{
				InitializeClient(login.Username, login.Server);
			}
			else
			{
				Environment.Exit(0);
			}
		}

		public void AddLog(string msg)
		{
			//if (!Log_TextBox.Dispatcher.CheckAccess())
			{
				Log_TextBox.Dispatcher.Invoke(() =>
				{
					Log_TextBox.Text += $@"{msg}{Environment.NewLine}";

				});
			}

		}

		private void Send_Button_Click(object sender, RoutedEventArgs e)
		{
			SendMsg();
		}

		private void SendMsg()
		{
			client.SendPacket(new ChatPacket { Message = msg_TextBox.Text });

			msg_TextBox.Clear();
		}

		private void msg_TextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				Send_Button.PerformClick();
			}
		}
	}
}
