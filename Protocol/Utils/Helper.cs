using System.Net;
using System.Net.Sockets;

namespace Protocol.Utils
{
	internal static class Helper
	{
		public static bool IsValidTcpPort(int port) => port >= IPEndPoint.MinPort + 1 && port <= IPEndPoint.MaxPort;

		public static EndPoint GetEp(AddressFamily family) => GetEp(family, 0);

		public static EndPoint GetEp(AddressFamily family, int port) => new IPEndPoint(family == AddressFamily.InterNetwork ? IPAddress.Any : IPAddress.IPv6Any, port);

		public static bool IsValidPacketId(short id) => id > 0 && id < 30000;
	}
}