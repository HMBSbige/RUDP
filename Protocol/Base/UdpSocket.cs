﻿using Protocol.Packets;
using Protocol.Utils;
using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace Protocol.Base
{
	public class UdpSocket : IDisposable
	{
		private const int MAX_BUFFER_SIZE = 8 * 1024;

		protected AddressFamily m_family = AddressFamily.InterNetwork;
		private byte[] m_buffer = new byte[MAX_BUFFER_SIZE];
		private Socket m_client;

		public int Available => m_client.Available;

		public EndPoint LocalEndPoint => m_client.LocalEndPoint;

		public bool Active { get; set; }

		public short Ttl
		{
			get { return m_client.Ttl; }
			set { m_client.Ttl = value; }
		}

		public bool ExclusiveAddressUse
		{
			get { return m_client.ExclusiveAddressUse; }
			set { m_client.ExclusiveAddressUse = value; }
		}

		public bool DontFragment
		{
			get { return m_client.DontFragment; }
			set { m_client.DontFragment = value; }
		}

		public void AllowNatTraversal(bool allowed) => m_client.SetIPProtectionLevel(allowed ? IPProtectionLevel.Unrestricted : IPProtectionLevel.EdgeRestricted);

		public UdpSocket() : this(AddressFamily.InterNetwork)
		{ }

		public UdpSocket(AddressFamily family)
		{
			if (family != AddressFamily.InterNetwork && family != AddressFamily.InterNetworkV6) throw new ArgumentException("Invalid protocol family", nameof(family));

			m_family = family;

			m_client = CreateClient();
		}

		public void Connect(string hostname, int port)
		{
			if (string.IsNullOrEmpty(hostname)) throw new ArgumentNullException(nameof(hostname));
			if (!Helper.IsValidTcpPort(port)) throw new ArgumentOutOfRangeException(nameof(port));

			var addresses = Dns.GetHostAddresses(hostname);
			Exception ex = null;

			foreach (var addr in addresses)
			{
				if (addr.AddressFamily != m_family)
					continue;

				try
				{
					Connect(new IPEndPoint(addr, port));
					break;
				}
				catch (Exception e)
				{
					ex = e;
				}
			}

			if (Active)
				return;

			throw (ex != null) ? ex : new SocketException((int)SocketError.NotConnected);
		}

		public void Connect(IPAddress ip, int port)
		{
			if (ip == null) throw new ArgumentNullException(nameof(ip));
			if (!Helper.IsValidTcpPort(port)) throw new ArgumentOutOfRangeException(nameof(port));

			IPEndPoint ep = new IPEndPoint(ip, port);

			Connect(ep);
		}

		public void Connect(IPEndPoint endPoint)
		{
			if (endPoint == null) throw new ArgumentNullException(nameof(endPoint));

			m_client.Connect(endPoint);

			Active = true;
		}

		public void Bind(int port)
		{
			if (!Helper.IsValidTcpPort(port)) throw new ArgumentOutOfRangeException(nameof(port));

			Bind(Helper.GetEp(m_family, port));
		}

		public void Bind(EndPoint localEP)
		{
			if (localEP == null) throw new ArgumentNullException(nameof(localEP));

			m_client.Bind(localEP);

			Active = true;
		}

		public int Send(byte[] buffer)
		{
			if (buffer == null) throw new ArgumentNullException(nameof(buffer));
			if (!Active) throw new InvalidOperationException("Not connected");

			return m_client.Send(buffer, 0, buffer.Length, SocketFlags.None);
		}

		public int SendTo(byte[] buffer, EndPoint ep)
		{
			if (buffer == null) throw new ArgumentNullException(nameof(buffer));
			if (!Active) throw new InvalidOperationException("Not connected");

			return m_client.SendTo(buffer, 0, buffer.Length, SocketFlags.None, ep);
		}

		public byte[] Receive(ref IPEndPoint remoteEP)
		{
			var ep = Helper.GetEp(m_family);

			int rcvd = m_client.ReceiveFrom(m_buffer, SocketFlags.None, ref ep);

			if (rcvd < MAX_BUFFER_SIZE)
			{
				byte[] buffer = new byte[rcvd];
				Buffer.BlockCopy(m_buffer, 0, buffer, 0, rcvd);

				return buffer;
			}

			return m_buffer;
		}

		public void Close() => m_client.Close(5);

		public void Disconnect(bool reuse)
		{
			m_client.Disconnect(reuse);
		}

#if NETCOREAPP

#else
		[HostProtection(ExternalThreading = true)]
#endif
		public IAsyncResult BeginReceive(AsyncCallback callBack, object state)
		{
			var ep = Helper.GetEp(m_family);

			return m_client.BeginReceiveFrom(m_buffer, 0, MAX_BUFFER_SIZE, SocketFlags.None, ref ep, callBack, state);
		}

		public byte[] EndReceive(IAsyncResult result, ref IPEndPoint remoteEP)
		{
			var ep = Helper.GetEp(m_family);
			int rcvd = 0;
			try
			{
				rcvd = m_client.EndReceiveFrom(result, ref ep);
			}
			catch
			{
				//
			}

			remoteEP = (IPEndPoint)ep;

			if (rcvd < MAX_BUFFER_SIZE)
			{
				byte[] buffer = new byte[rcvd];
				Buffer.BlockCopy(m_buffer, 0, buffer, 0, rcvd);

				return buffer;
			}

			return m_buffer;
		}

#if NETCOREAPP

#else
		[HostProtection(ExternalThreading = true)]
#endif
		public IAsyncResult BeginSend(byte[] buffer, AsyncCallback callBack, object state)
		{
			if (!Active) throw new InvalidOperationException("Not connected");

			return m_client.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, callBack, state);
		}

		public int EndSend(IAsyncResult result)
		{
			return m_client.EndSend(result);
		}

#if NETCOREAPP

#else
		[HostProtection(ExternalThreading = true)]
#endif
		public IAsyncResult BeginSendTo(byte[] buffer, EndPoint ep, AsyncCallback callBack, object state)
		{
			if (!Active) throw new InvalidOperationException("Not connected");

			return m_client.BeginSendTo(buffer, 0, buffer.Length, SocketFlags.None, ep, callBack, state);
		}

		public int EndSendTo(IAsyncResult result)
		{
			return m_client.EndSendTo(result);
		}

#if NETCOREAPP

#else
		[HostProtection(ExternalThreading = true)]
#endif
		public Task<int> SendAsync(byte[] buffer)
		{
			return Task<int>.Factory.FromAsync(BeginSend, EndSend, buffer, null);
		}

#if NETCOREAPP

#else
		[HostProtection(ExternalThreading = true)]
#endif
		public Task<int> SendToAsync(byte[] buffer, EndPoint ep)
		{
			return Task<int>.Factory.FromAsync(BeginSendTo, EndSendTo, buffer, ep, null);
		}

#if NETCOREAPP

#else
		[HostProtection(ExternalThreading = true)]
#endif
		public Task<int> SendPacketAsync(Packet packet)
		{
			var raw = new RawPacket(256);

			packet.WritePacket(ref raw);

			return SendAsync(raw.ToBuffer());
		}

#if NETCOREAPP

#else
		[HostProtection(ExternalThreading = true)]
#endif
		public Task<int> SendPacketAsync(Packet packet, EndPoint ep)
		{
			var raw = new RawPacket(256);

			packet.WritePacket(ref raw);

			return SendToAsync(raw.ToBuffer(), ep);
		}

		//[HostProtection(ExternalThreading = true)]
		//public Task<UdpReceiveResult> ReceiveAsync()
		//{
		//    return Task<UdpReceiveResult>.Factory.FromAsync((cb, state) => BeginReceive(cb, state), (ar) =>
		//    {
		//        IPEndPoint remoteEP = null;
		//        byte[] buffer = EndReceive(ar, ref remoteEP);
		//        return new UdpReceiveResult(buffer, remoteEP);

		//    }, null);
		//}

#if NETCOREAPP

#else
		[HostProtection(ExternalThreading = true)]
#endif
		public Task<Tuple<Packet, IPEndPoint>> ReceivePacketAsync()
		{
			return Task.Factory.FromAsync(BeginReceive, (ar) =>
			{
				IPEndPoint remoteEP = null;
				byte[] buffer = EndReceive(ar, ref remoteEP);

				RawPacket raw = new RawPacket(buffer);

				return new Tuple<Packet, IPEndPoint>(PacketReader.GetPacket(raw), remoteEP);

			}, null);
		}

		private Socket CreateClient()
		{
			return new Socket(m_family, SocketType.Dgram, ProtocolType.Udp);
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
					m_client.Shutdown(SocketShutdown.Both);
					m_client.Close();

					m_client = null;

					m_buffer = null;
					Active = false;
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~UdpSocket() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion
	}
}
