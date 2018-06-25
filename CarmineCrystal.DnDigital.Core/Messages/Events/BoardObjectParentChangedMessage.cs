using CarmineCrystal.Networking;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CarmineCrystal.DnDigital.Core.Messages.Events
{
	[ProtoContract]
    public class BoardObjectParentChangedMessage : Message
    {
		[ProtoMember(1)]
		public uint ID;
		[ProtoMember(2)]
		public uint? NewParent;
    }
}
