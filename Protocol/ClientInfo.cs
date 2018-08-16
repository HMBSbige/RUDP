using Protocol.Packets;
using Protocol.Threading;
using System.Collections.Concurrent;
using System.Net;

namespace Protocol
{
	public class ClientInfo
	{
		public EndPoint EndPoint { get; }

		internal ConcurrentQueue<Packet> SendQueue { get; } = new ConcurrentQueue<Packet>();

		internal ConcurrentList<Packet> ReliablePackets { get; } = new ConcurrentList<Packet>();

		public bool IsActive { get; internal set; }

		private long m_seq;

		public ClientInfo(EndPoint ep)
		{
			EndPoint = ep;
			m_seq = 0;
		}

		public long GetNextSeqNumber() => m_seq++;

		public void ResetSeq() => m_seq = 0;
	}
}
