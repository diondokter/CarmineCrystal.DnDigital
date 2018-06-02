using CarmineCrystal.DnDigital.Core.Data.Drawing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CarmineCrystal.DnDigital.Core.Data.BoardObjectTypes
{
	public class Drawing : BoardObject
	{
		private List<Line> _Lines = new List<Line>();
		public ReadOnlyCollection<Line> Lines => _Lines.AsReadOnly();

		protected Drawing(uint id) : base(id)
		{

		}

		public void AddLine(Line )
		{

		}

		public void RemoveLine()
		{

		}
	}
}
