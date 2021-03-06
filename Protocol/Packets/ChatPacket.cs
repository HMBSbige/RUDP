﻿using Protocol.Base;
using System;

namespace Protocol.Packets
{
	public class ChatPacket : Packet
	{
		static ChatPacket()
		{
			PacketReader.RegisterPacket(1, typeof(ChatPacket));
		}

		public override short Id => 1;

		public override bool IsReliable => true;

		public Guid ClientID { get; set; }
		public string Message { get; set; }

		public override void ReadPacket(ref RawPacket data)
		{
			base.ReadPacket(ref data);

			// ClientID = data.ReadGuid();
			Message = data.ReadString();
		}

		public override void WritePacket(ref RawPacket data)
		{
			base.WritePacket(ref data);

			// data.Write(ClientID);
			data.Write(Message);
		}
	}
}
