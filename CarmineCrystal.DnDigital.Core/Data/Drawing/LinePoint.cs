using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CarmineCrystal.DnDigital.Core.Data.Drawing
{
	[ProtoContract]
    public struct LinePoint
    {
		[ProtoMember(1)]
		public readonly Vector2 Point;
		[ProtoMember(2)]
		public readonly float Pressure;
		[ProtoMember(3)]
		public readonly float TiltX;
		[ProtoMember(4)]
		public readonly float TiltY;

		public LinePoint(Vector2 point, float pressure, float tiltX, float tiltY)
		{
			Point = point;
			Pressure = pressure;
			TiltX = tiltX;
			TiltY = tiltY;
		}
    }
}
