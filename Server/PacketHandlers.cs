using Protocol;
using Protocol.Packets;
using System;

namespace Server
{
	internal static class PacketHandlers
	{
		public static void LoginPacketHandler(RUdpServer server, Packet packet, ClientInfo client)
		{
			Console.WriteLine($@"Connection from {client.EndPoint}");

			var login = (LoginPacket)packet;

			Console.WriteLine($@"Logged in: {login.Username}");

			foreach (var c in server.Clients)
			{
				// if (!c.EndPoint.Equals(client.EndPoint))
				server.SendPacket(login, c);
			}

		}

		public static void ChatPacketHandler(RUdpServer server, Packet packet, ClientInfo client)
		{
			Console.WriteLine($@"Connection from {client.EndPoint}");

			var chat = (ChatPacket)packet;

			Console.WriteLine($@"Received chat message from {client.EndPoint}: {chat.Message}");

			foreach (var c in server.Clients)
			{
				// if (!c.EndPoint.Equals(client.EndPoint))
				server.SendPacket(chat, c);
			}
		}
	}
}
