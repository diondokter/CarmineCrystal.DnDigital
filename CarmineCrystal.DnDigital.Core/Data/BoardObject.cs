using ProtoBuf;
using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Reflection;
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
		private static bool IsMaster;

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

		public event Action<BoardObject> Destroyed;

		public static void Initialize(Dictionary<uint, BoardObject> objects = null, bool isMaster = true)
		{
			if (objects == null)
			{
				objects = new Dictionary<uint, BoardObject>();
			}

			IsMaster = isMaster;

			Objects = objects;
			ReadOnlyObjects = new ReadOnlyDictionary<uint, BoardObject>(objects);

			if (Objects.Count > 0)
			{
				MaxID = Objects.Max(x => x.Key) + 1;
			}

			ProtoInitialize();

			Initialized = true;
		}

		/// <summary>
		/// Adds all child classes as protoincludes for BoardObject
		/// </summary>
		/// <param name="AdditionalAssemblies"></param>
		private static void ProtoInitialize(params Assembly[] AdditionalAssemblies)
		{
			List<Assembly> TargetAssemblies = AdditionalAssemblies.ToList();
			TargetAssemblies.Add(Assembly.GetExecutingAssembly());

			TypeInfo[] MessageTypes = TargetAssemblies.SelectMany(x => x.DefinedTypes).Select(x => x.GetTypeInfo()).Where(x => x.IsSubclassOf(typeof(BoardObject))).ToArray();

			Dictionary<Type, int> SubTypeCount = new Dictionary<Type, int>();

			for (int i = 0; i < MessageTypes.Length; i++)
			{
				MetaType ProtobufType;

				if (!SubTypeCount.ContainsKey(MessageTypes[i].BaseType))
				{
					ProtobufType = RuntimeTypeModel.Default.Add(MessageTypes[i].BaseType, true);
					SubTypeCount[MessageTypes[i].BaseType] = 101;
				}
				else
				{
					ProtobufType = RuntimeTypeModel.Default[MessageTypes[i].BaseType];
				}

				ProtobufType.AddSubType(SubTypeCount[MessageTypes[i].BaseType], MessageTypes[i].AsType());
				SubTypeCount[MessageTypes[i].BaseType]++;
			}
		}

		public static T Create<T>() where T: BoardObject
		{
			if (!IsMaster)
			{
				throw new Exception("Program is not initialized as the master. Objects can only be added, not created.");
			}

			T newObject = (T)Activator.CreateInstance(typeof(T), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { MaxID++ }, null);
			Objects[newObject.ID] = newObject;
			return newObject;
		}

		public static void AddObject(BoardObject newObject)
		{
			if (IsMaster)
			{
				throw new Exception("Program is initialized as the master. Objects can only be created, not added.");
			}

			if (Objects.ContainsKey(newObject.ID))
			{
				throw new Exception($"A {nameof(BoardObject)} with the {nameof(ID)}: {newObject.ID} already exists.");
			}

			Objects[newObject.ID] = newObject;
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

		protected BoardObject(uint id)
		{
			if (!Initialized)
			{
				throw new Exception($"{nameof(BoardObject)} has not been initialized yet.");
			}

			this.ID = id;
		}

		public void Destroy()
		{
			foreach(BoardObject child in Children)
			{
				child.Position += Position;
				child.Rotation *= Rotation;
				child.Parent = Parent;
			}

			Objects.Remove(ID);

			PositionChanged = null;
			LevelChanged = null;
			RotationChanged = null;
			SizeChanged = null;
			ParentChanged = null;

			Destroyed?.Invoke(this);
		}
	}
}
