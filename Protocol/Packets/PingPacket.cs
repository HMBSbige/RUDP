using Protocol.Base;

namespace Protocol.Packets
{
	public class PingPacket : Packet
	{
		static PingPacket()
		{
			PacketReader.RegisterPacket(2, typeof(PingPacket));
		}

		public override short Id => 2;

		public override bool IsReliable => true;

		public long SendTime { get; set; }

		public override void ReadPacket(ref RawPacket data)
		{
			base.ReadPacket(ref data);

			SendTime = data.ReadInt64();
		}

		public override void WritePacket(ref RawPacket data)
		{
			base.WritePacket(ref data);

			data.Write(SendTime);
		}
	}
}
