using Protocol;
using Protocol.Packets;
using System.Net;

namespace Server
{
	public class RudpServer : RUdpServer
	{
		public RudpServer(IPAddress ip, int port) : base(ip, port) { }

		public RudpServer(int port) : base(port) { }

		protected override void RegisterPacketHandlers()
		{
			base.RegisterPacketHandlers();

			RegisterPacketHandler(typeof(LoginPacket), PacketHandlers.LoginPacketHandler);
			RegisterPacketHandler(typeof(ChatPacket), PacketHandlers.ChatPacketHandler);
		}
	}
}
