using Protocol.Packets;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Protocol.Base
{
	public class UdpClient
	{
		protected UdpSocket m_socket;

		public bool Active => m_socket.Active;
		public bool Available => m_socket.Available > 0;

		public EndPoint LocalEndPoint => m_socket.LocalEndPoint;

		public UdpClient()
		{
			m_socket = new UdpSocket();
		}

		public void Connect(string hostname, int port) => m_socket.Connect(hostname, port);

		public void Connect(IPEndPoint ep) => m_socket.Connect(ep);

		public void Connect(IPAddress ip, int port) => m_socket.Connect(ip, port);

		public Task<int> SendPacketAsync(Packet packet)
		{
			return m_socket.SendPacketAsync(packet);
		}

		public Task<Tuple<Packet, IPEndPoint>> ReceivePacketAsync()
		{
			return m_socket.ReceivePacketAsync();
		}

		public void Close() => m_socket.Close();
	}
}
