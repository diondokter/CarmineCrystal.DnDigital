using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarmineCrystal.DnDigital.Core.Data.Drawing
{
	[ProtoContract]
    public class Brush
    {
		[ProtoMember(1)]
		private Color _SolidColor;
		public Color SolidColor
		{
			get => _SolidColor;
			set
			{
				_SolidColor = value;
				SolidColorChanged?.Invoke(this);
			}
		}
		public event Action<Brush> SolidColorChanged;

		[ProtoMember(2)]
		private float _Height;
		public float Height
		{
			get => _Height;
			set
			{
				_Height = value;
				HeightChanged?.Invoke(this);
			}
		}
		public event Action<Brush> HeightChanged;

		[ProtoMember(3)]
		private float _Width;
		public float Width
		{
			get => _Width;
			set
			{
				_Width = value;
				WidthChanged?.Invoke(this);
			}
		}
		public event Action<Brush> WidthChanged;
	}
}
