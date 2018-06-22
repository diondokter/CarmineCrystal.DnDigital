using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;

namespace CarmineCrystal.DnDigital.Core.Data.Drawing
{
	[ProtoContract]
    public class Line
	{
		[ProtoMember(1)]
		private readonly List<LinePoint> _LinePoints = new List<LinePoint>();
		public ReadOnlyCollection<LinePoint> LinePoints => _LinePoints.AsReadOnly();

		public event Action<Line, LinePoint> LinePointAdded;
		public event Action<Line, LinePoint> LinePointRemoved;

		[ProtoMember(2)]
		private Brush _Linebrush;
		public Brush LineBrush
		{
			get => _Linebrush;
			set
			{
				_Linebrush = value;
				BrushChanged?.Invoke(this);
			}
		}

		public event Action<Line> BrushChanged;

		public void Add(LinePoint item)
		{
			_LinePoints.Add(item);
			LinePointAdded?.Invoke(this, item);
		}

		public bool Remove(LinePoint item)
		{
			if (_LinePoints.Remove(item))
			{
				LinePointRemoved?.Invoke(this, item);
				return true;
			}

			return false;
		}

		public void RemoveAt(int index)
		{
			LinePoint item = _LinePoints[index];
			_LinePoints.RemoveAt(index);
			LinePointRemoved?.Invoke(this, item);
		}
	}
}
