using Protocol.Packets;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Protocol.Base
{
	public class UdpListener
	{
		private UdpSocket m_server;
		private EndPoint m_serverEP;

		public bool Active { get; private set; }

		public UdpListener(IPAddress ip, int port)
		{
			m_serverEP = new IPEndPoint(ip, port);
			m_server = new UdpSocket();
		}

		public void Start()
		{
			if (Active)
			{
				return;
			}

			m_server.Bind(m_serverEP);

			Active = true;
		}

		public void Stop()
		{
			m_server.Close();

			Active = false;

			m_server = new UdpSocket();
		}

		public Task<Tuple<Packet, IPEndPoint>> ReceivePacketAsync()
		{
			return m_server.ReceivePacketAsync();
		}

		public Task<int> SendPacketAsync(Packet packet, EndPoint clientEP)
		{
			return m_server.SendPacketAsync(packet, clientEP);
		}

	}
}
