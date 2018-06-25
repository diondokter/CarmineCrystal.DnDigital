using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarmineCrystal.DnDigital.Core.Data.BoardObjectTypes
{
	[ProtoContract]
    public abstract class Sprite : BoardObject
    {
		[ProtoMember(1)]
		private string _Source;
		public string Source
		{
			get => _Source;
			set
			{
				if (_Source != value)
				{
					_Source = value;
					SourceChanged?.Invoke(this);
				}
			}
		}
		public event Action<Sprite> SourceChanged;

		protected Sprite(uint id) : base(id)
		{

		}

		// Empty constructor fo protobuf
		protected Sprite() : base()
		{

		}
	}
}
