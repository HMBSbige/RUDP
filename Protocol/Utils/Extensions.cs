using Protocol.Threading;
using System;

namespace Protocol.Utils
{
	public static class Extensions
	{
		public static void RemoveAll<T>(this ConcurrentList<T> self, Func<T, bool> predicate)
		{
			for (var i = self.Count - 1; i >= 0; --i)
			{
				if (predicate(self[i]))
				{
					self.RemoveAt(i);
				}
			}
		}
	}
}