using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarmineCrystal.DnDigital.Core.Data.BoardObjectTypes
{
	[ProtoContract]
	public class Vehicle : Sprite
	{
		protected Vehicle(uint id) : base(id)
		{

		}

		// Empty constructor fo protobuf
		protected Vehicle() : base()
		{

		}
	}
}
