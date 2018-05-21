using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CarmineCrystal.DnDigital.Core.Data.Drawing
{
	[ProtoContract]
    public class Line : ICollection<LinePoint>, IList<LinePoint>
	{
		[ProtoMember(1)]
		private readonly List<LinePoint> LinePoints = new List<LinePoint>();

		public LinePoint this[int index] { get => ((IList<LinePoint>)LinePoints)[index]; set => ((IList<LinePoint>)LinePoints)[index] = value; }

		public int Count => ((ICollection<LinePoint>)LinePoints).Count;

		public bool IsReadOnly => ((ICollection<LinePoint>)LinePoints).IsReadOnly;

		public void Add(LinePoint item)
		{
			LinePoints.Add(item);
		}

		public void Clear()
		{
			LinePoints.Clear();
		}

		public bool Contains(LinePoint item)
		{
			return LinePoints.Contains(item);
		}

		public void CopyTo(LinePoint[] array, int arrayIndex)
		{
			LinePoints.CopyTo(array, arrayIndex);
		}

		public IEnumerator<LinePoint> GetEnumerator()
		{
			return ((ICollection<LinePoint>)LinePoints).GetEnumerator();
		}

		public int IndexOf(LinePoint item)
		{
			return LinePoints.IndexOf(item);
		}

		public void Insert(int index, LinePoint item)
		{
			LinePoints.Insert(index, item);
		}

		public bool Remove(LinePoint item)
		{
			return LinePoints.Remove(item);
		}

		public void RemoveAt(int index)
		{
			LinePoints.RemoveAt(index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((ICollection<LinePoint>)LinePoints).GetEnumerator();
		}
	}
}
