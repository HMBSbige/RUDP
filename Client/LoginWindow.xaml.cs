using System;
using System.Windows;

namespace Client
{
	/// <summary>
	/// LoginWindow.xaml 的交互逻辑
	/// </summary>
	public partial class LoginWindow : Window
	{
		public string Username => Username_TextBox.Text;
		public string Server => Sever_TextBox.Text;

		public LoginWindow()
		{
			InitializeComponent();
		}

		private void Login_Button_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}
