using Protocol;
using Protocol.Packets;

namespace Client
{
	public static class PacketHandlers
	{
		public static void HandleChatPacket(RUdpClient client, Packet packet)
		{
			var chat = (ChatPacket)packet;
			var myclient = (RudpClient)client;

			myclient.AddText(chat.Message);
		}

		public static void HandleLoginPacket(RUdpClient client, Packet packet)
		{
			var login = (LoginPacket)packet;
			var myclient = (RudpClient)client;

			myclient.AddText($@"{login.Username} logged in!");
		}

	}
}