using System.Reflection;
using System.Windows.Controls.Primitives;

namespace Client
{
	public static class Extensions
	{
		public static void PerformClick(this ButtonBase button)
		{
			var method = button.GetType().GetMethod(@"OnClick", BindingFlags.NonPublic | BindingFlags.Instance);

			if (method != null)
			{
				method.Invoke(button, null);
			}
		}
	}
}
