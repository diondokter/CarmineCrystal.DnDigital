using CarmineCrystal.Networking;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarmineCrystal.DnDigital.Core.Messages.Events
{
	[ProtoContract]
    public class BoardObjectLevelChangedMessage : Message
    {
		[ProtoMember(1)]
		public uint ID;
		[ProtoMember(2)]
		public int NewLevel;
    }
}
