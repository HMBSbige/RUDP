using Protocol;
using Protocol.Packets;

namespace Client
{
	public class RudpClient : RUdpClient
	{
		private MainWindow m_client;

		public RudpClient(string host, int port, MainWindow client) : base(host, port)
		{
			m_client = client;
		}

		protected override void RegisterPacketHandlers()
		{
			base.RegisterPacketHandlers();

			RegisterPacketHandler(typeof(ChatPacket), PacketHandlers.HandleChatPacket);
			RegisterPacketHandler(typeof(LoginPacket), PacketHandlers.HandleLoginPacket);
		}

		public void AddText(string msg) => m_client.AddLog(msg);
	}
}
