using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace CarmineCrystal.DnDigital.Core.Data.Drawing
{
	[ProtoContract]
    public class Line : ICollection<LinePoint>, IList<LinePoint>, INotifyCollectionChanged
	{
		[ProtoMember(1)]
		private readonly List<LinePoint> LinePoints = new List<LinePoint>();

		public LinePoint this[int index] { get => LinePoints[index]; set => LinePoints[index] = value; }

		public int Count => LinePoints.Count;

		public bool IsReadOnly => ((ICollection<LinePoint>)LinePoints).IsReadOnly;

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public void Add(LinePoint item)
		{
			LinePoints.Add(item);
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
		}

		public void Clear()
		{
			LinePoints.Clear();
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
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
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
		}

		public bool Remove(LinePoint item)
		{
			if (LinePoints.Remove(item))
			{
				CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
				return true;
			}

			return false;
		}

		public void RemoveAt(int index)
		{
			LinePoint point = LinePoints[index];
			LinePoints.RemoveAt(index);
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, point, index));
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((ICollection<LinePoint>)LinePoints).GetEnumerator();
		}
	}
}
