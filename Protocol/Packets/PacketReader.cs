using Protocol.Base;
using System;
using System.Collections.Concurrent;

namespace Protocol.Packets
{
	public static class PacketReader
	{
		private static ConcurrentDictionary<short, Type> PacketTypes = new ConcurrentDictionary<short, Type>();

		public static void RegisterPacket(short id, Type packetType)
		{
			PacketTypes.AddOrUpdate(id, packetType, (b, t) => packetType);
		}

		public static Packet PacketFromType(Type packetType)
		{
			if (!typeof(Packet).IsAssignableFrom(packetType))
				throw new InvalidCastException(@"Type must inherit from IPacket");

			return (Packet)Activator.CreateInstance(packetType);
		}

		public static Packet GetPacket(RawPacket data)
		{
			var id = data.ReadInt16();

			if (!PacketTypes.TryGetValue(id, out var type))
			{
				throw new InvalidOperationException($@"Invalid packet id {id}");
			}

			var packet = PacketFromType(type);
			packet.ReadPacket(ref data);

			return packet;
		}
	}
}
