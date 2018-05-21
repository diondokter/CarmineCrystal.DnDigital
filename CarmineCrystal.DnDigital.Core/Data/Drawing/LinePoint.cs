using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CarmineCrystal.DnDigital.Core.Data.Drawing
{
	[ProtoContract]
    public class LinePoint
    {
		[ProtoMember(1)]
		public Vector2 Point;
		[ProtoMember(2)]
		public float Pressure;
		[ProtoMember(3)]
		public float TiltX;
		[ProtoMember(4)]
		public float TiltY;

		public LinePoint(Vector2 point, float pressure, float tiltX, float tiltY)
		{
			Point = point;
			Pressure = pressure;
			TiltX = tiltX;
			TiltY = tiltY;
		}

		/// <summary>
		/// Private constructor for protobuf.
		/// </summary>
		private LinePoint()
		{

		}
    }
}
