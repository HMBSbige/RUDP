using Protocol.Base;
using System;

namespace Protocol.Packets
{
	public abstract class Packet
	{
		public abstract short Id { get; }

		public abstract bool IsReliable { get; }

		internal long Seq { get; set; }

		internal DateTime ResendTime { get; set; }

		protected Packet() { }

		public virtual void ReadPacket(ref RawPacket data)
		{
			if (IsReliable)
				Seq = data.ReadInt64();
		}

		public virtual void WritePacket(ref RawPacket data)
		{
			data.Write(Id);
			if (IsReliable)
				data.Write(Seq);
		}

		public override string ToString()
		{
			return $@"[{GetType().Name}, Id: {Id}, Reliable: {IsReliable}, Magic: {Seq}]";
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Packet packet))
			{
				return false;
			}

			return packet.Id == Id && packet.IsReliable == IsReliable && packet.Seq == Seq;
		}

		public override int GetHashCode()
		{
			return (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + @"#" + Id).GetHashCode();
		}
	}
}
