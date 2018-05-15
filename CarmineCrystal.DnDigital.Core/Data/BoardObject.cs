using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;

namespace CarmineCrystal.DnDigital.Core.Data
{
	[ProtoContract]
	public class BoardObject
	{
		public static uint MaxID { get; private set; } = 0;
		private static bool Initialized = false;
		private static Dictionary<uint, BoardObject> Objects { get; set; }
		public static ReadOnlyDictionary<uint, BoardObject> ReadOnlyObjects { get; private set; }

		[ProtoMember(1)]
		public readonly uint ID;

		[ProtoMember(2)]
		private Vector2 _Position = Vector2.Zero;
		public Vector2 Position
		{
			get => _Position;
			set
			{
				if (_Position != value)
				{
					_Position = value;
					PositionChanged?.Invoke(this);
				}
			}
		}
		public Vector2 GlobalPosition
		{
			get
			{
				Vector2 parentPosition = Parent?.GlobalPosition ?? Vector2.Zero;
				Quaternion parentRotation = Parent?.GlobalRotation ?? Quaternion.Identity;
				Vector2 localTransformedPosition = Vector2.Transform(Position, parentRotation);
				return parentPosition + localTransformedPosition;
			}
		}
		public event Action<BoardObject> PositionChanged;

		[ProtoMember(3)]
		private int _Level = 0;
		public int Level
		{
			get => _Level;
			set
			{
				if (_Level != value)
				{
					_Level = value;
					LevelChanged?.Invoke(this);
				}
			}
		}
		public event Action<BoardObject> LevelChanged;

		[ProtoMember(4)]
		private Quaternion _Rotation = Quaternion.Identity;
		public Quaternion Rotation
		{
			get => _Rotation;
			set
			{
				if (_Rotation != value)
				{
					_Rotation = value;
					RotationChanged?.Invoke(this);
				}
			}
		}
		public Quaternion GlobalRotation => (Parent?.GlobalRotation ?? Quaternion.Identity) * Rotation;
		public event Action<BoardObject> RotationChanged;

		[ProtoMember(5)]
		private Vector2 _Size = Vector2.One;
		public Vector2 Size
		{
			get => _Size;
			set
			{
				if (_Size != value)
				{
					_Size = value;
					SizeChanged?.Invoke(this);
				}
			}
		}
		public event Action<BoardObject> SizeChanged;

		[ProtoMember(6)]
		private uint? ParentID = null;
		public BoardObject Parent
		{
			get => ParentID != null ? Objects[ParentID.Value] : null;
			set
			{
				if (ID == value?.ID)
				{
					throw new InvalidOperationException($"Cannot set a {nameof(BoardObject)} as its own parent.");
				}

				if (ParentID != value?.ID)
				{
					if (ParentID != null)
					{
						Parent.PositionChanged -= PositionChanged;
						Parent.LevelChanged -= LevelChanged;
						Parent.RotationChanged -= RotationChanged;
						Parent.SizeChanged -= SizeChanged;
						Parent.ParentChanged -= ParentChanged;
					}

					ParentID = value?.ID;

					if (ParentID != null)
					{
						Parent.PositionChanged += PositionChanged;
						Parent.LevelChanged += LevelChanged;
						Parent.RotationChanged += RotationChanged;
						Parent.SizeChanged += SizeChanged;
						Parent.ParentChanged += ParentChanged;
					}

					ParentChanged?.Invoke(this);
				}
			}
		}
		public IEnumerable<BoardObject> Children => Objects.Select(x => x.Value).Where(x => x.ParentID == ID);
		public event Action<BoardObject> ParentChanged;


		public static void Initialize(Dictionary<uint, BoardObject> objects = null)
		{
			if (objects == null)
			{
				objects = new Dictionary<uint, BoardObject>();
			}

			Objects = objects;
			ReadOnlyObjects = new ReadOnlyDictionary<uint, BoardObject>(objects);

			if (Objects.Count > 0)
			{
				MaxID = Objects.Max(x => x.Key) + 1;
			}

			Initialized = true;
		}

		public static BoardObject Create()
		{
			BoardObject newObject = new BoardObject(MaxID++);
			Objects[newObject.ID] = newObject;
			return newObject;
		}

		/// <summary>
		/// Constructor meant for Protobuf object deserialization
		/// </summary>
		private BoardObject()
		{
			if (!Initialized)
			{
				throw new Exception($"{nameof(BoardObject)} has not been initialized yet.");
			}
		}

		private BoardObject(uint id)
		{
			if (!Initialized)
			{
				throw new Exception($"{nameof(BoardObject)} has not been initialized yet.");
			}

			this.ID = id;
		}
	}
}
