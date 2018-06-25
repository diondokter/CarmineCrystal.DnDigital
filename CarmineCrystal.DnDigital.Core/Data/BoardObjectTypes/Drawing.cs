using CarmineCrystal.DnDigital.Core.Data.Drawing;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CarmineCrystal.DnDigital.Core.Data.BoardObjectTypes
{
	[ProtoContract]
	public class Drawing : BoardObject
	{
		[ProtoMember(1)]
		private readonly List<Line> _Lines = new List<Line>();
		public ReadOnlyCollection<Line> Lines => _Lines.AsReadOnly();

		public event Action<BoardObject, Line> LineAdded;
		public event Action<BoardObject, Line> LineRemoved;

		protected Drawing(uint id) : base(id)
		{

		}

		// Empty constructor fo protobuf
		protected Drawing() : base()
		{

		}

		public void AddLine(Line newLine)
		{
			_Lines.Add(newLine);
			LineAdded?.Invoke(this, newLine);
		}

		public bool RemoveLine(Line oldLine)
		{
			if (_Lines.Remove(oldLine))
			{
				LineRemoved?.Invoke(this, oldLine);
				return true;
			}

			return false;
		}

		public void RemoveLineAt(int index)
		{
			Line oldLine = _Lines[index];
			_Lines.RemoveAt(index);
			LineRemoved?.Invoke(this, oldLine);
		}
	}
}
