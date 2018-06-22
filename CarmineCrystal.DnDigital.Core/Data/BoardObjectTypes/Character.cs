using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarmineCrystal.DnDigital.Core.Data.BoardObjectTypes
{
	[ProtoContract]
	public class Character : Sprite
	{
		protected Character(uint id) : base(id)
		{

		}
	}
}
