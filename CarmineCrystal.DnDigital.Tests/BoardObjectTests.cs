using System;
using System.Collections.Generic;
using System.Numerics;
using CarmineCrystal.DnDigital.Core.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarmineCrystal.DnDigital.Tests
{
	[TestClass]
	public class BoardObjectTests
	{
		[TestMethod]
		public void ParentingPosition()
		{
			BoardObject.Initialize(new Dictionary<uint, BoardObject>());

			BoardObject parent = BoardObject.Create();
			BoardObject child = BoardObject.Create();

			child.Parent = parent;

			parent.Position = new Vector2(1, 0);
			child.Position = new Vector2(1, 0);

			Assert.AreEqual(new Vector2(2, 0), child.GlobalPosition, "Child position is wrong.");

			parent.Position = new Vector2(2, 1);
			child.Position = new Vector2(2, 1);

			Assert.AreEqual(new Vector2(4, 2), child.GlobalPosition, "Child position is wrong.");

			Assert.ThrowsException<InvalidOperationException>(() => child.Parent = child, "Illegal setting an object as its own parent.");
		}

		[TestMethod]
		public void ParentingRotation()
		{
			BoardObject.Initialize(new Dictionary<uint, BoardObject>());

			BoardObject parent = BoardObject.Create();
			BoardObject child = BoardObject.Create();

			child.Parent = parent;

			parent.Rotation = Quaternion.CreateFromYawPitchRoll(0, 0, 20);
			child.Rotation = Quaternion.CreateFromYawPitchRoll(0, 0, 20);

			Assert.AreEqual(Quaternion.CreateFromYawPitchRoll(0, 0, 40).W, child.GlobalRotation.W, 0.01, "Child rotation W is wrong.");
			Assert.AreEqual(Quaternion.CreateFromYawPitchRoll(0, 0, 40).X, child.GlobalRotation.X, 0.01, "Child rotation X is wrong.");
			Assert.AreEqual(Quaternion.CreateFromYawPitchRoll(0, 0, 40).Y, child.GlobalRotation.Y, 0.01, "Child rotation Y is wrong.");
			Assert.AreEqual(Quaternion.CreateFromYawPitchRoll(0, 0, 40).Z, child.GlobalRotation.Z, 0.01, "Child rotation Z is wrong.");
		}

		[TestMethod]
		public void ParentingPositionRotation()
		{
			BoardObject.Initialize(new Dictionary<uint, BoardObject>());

			BoardObject parent = BoardObject.Create();
			BoardObject child = BoardObject.Create();

			child.Parent = parent;

			// Put both at 1
			parent.Position = new Vector2(1, 0);
			child.Position = new Vector2(1, 0);

			// Make the parent cancel out the child so it will be at 0,0
			parent.Rotation = Quaternion.CreateFromYawPitchRoll(0, 0, (float)Math.PI);
			child.Rotation = Quaternion.CreateFromYawPitchRoll(0, 0, 0);

			Assert.AreEqual(0, child.GlobalPosition.X, 0.01);
			Assert.AreEqual(0, child.GlobalPosition.Y, 0.01);
			
			// Child rotation shouldn't factor in here
			child.Rotation = Quaternion.CreateFromYawPitchRoll(51, 32, 21);

			Assert.AreEqual(0, child.GlobalPosition.X, 0.01);
			Assert.AreEqual(0, child.GlobalPosition.Y, 0.01);
		}

		[TestMethod]
		public void Events()
		{
			BoardObject.Initialize(new Dictionary<uint, BoardObject>());

			BoardObject testObject = BoardObject.Create();

			bool positionChanged = false;
			bool levelChanged = false;
			bool rotationChanged = false;
			bool sizeChanged = false;
			bool parentChanged = false;

			void ResetBools()
			{
				positionChanged = false;
				levelChanged = false;
				rotationChanged = false;
				sizeChanged = false;
				parentChanged = false;
			}

			testObject.PositionChanged += (x) => positionChanged = true;
			testObject.LevelChanged += (x) => levelChanged = true;
			testObject.RotationChanged += (x) => rotationChanged = true;
			testObject.SizeChanged += (x) => sizeChanged = true;
			testObject.ParentChanged += (x) => parentChanged = true;

			testObject.Position = new Vector2(1, 0);
			Assert.IsTrue(positionChanged);
			Assert.IsFalse(levelChanged);
			Assert.IsFalse(rotationChanged);
			Assert.IsFalse(sizeChanged);
			Assert.IsFalse(parentChanged);
			ResetBools();

			testObject.Level = 1;
			Assert.IsFalse(positionChanged);
			Assert.IsTrue(levelChanged);
			Assert.IsFalse(rotationChanged);
			Assert.IsFalse(sizeChanged);
			Assert.IsFalse(parentChanged);
			ResetBools();

			testObject.Rotation = new Quaternion(1, 0, 0, 0);
			Assert.IsFalse(positionChanged);
			Assert.IsFalse(levelChanged);
			Assert.IsTrue(rotationChanged);
			Assert.IsFalse(sizeChanged);
			Assert.IsFalse(parentChanged);
			ResetBools();

			testObject.Size = new Vector2(1, 0);
			Assert.IsFalse(positionChanged);
			Assert.IsFalse(levelChanged);
			Assert.IsFalse(rotationChanged);
			Assert.IsTrue(sizeChanged);
			Assert.IsFalse(parentChanged);
			ResetBools();

			BoardObject parentObject = BoardObject.Create();
			testObject.Parent = parentObject;
			Assert.IsFalse(positionChanged);
			Assert.IsFalse(levelChanged);
			Assert.IsFalse(rotationChanged);
			Assert.IsFalse(sizeChanged);
			Assert.IsTrue(parentChanged);
			ResetBools();


			testObject.Position = new Vector2(1, 0);
			Assert.IsFalse(positionChanged);
			Assert.IsFalse(levelChanged);
			Assert.IsFalse(rotationChanged);
			Assert.IsFalse(sizeChanged);
			Assert.IsFalse(parentChanged);
			ResetBools();

			testObject.Level = 1;
			Assert.IsFalse(positionChanged);
			Assert.IsFalse(levelChanged);
			Assert.IsFalse(rotationChanged);
			Assert.IsFalse(sizeChanged);
			Assert.IsFalse(parentChanged);
			ResetBools();

			testObject.Rotation = new Quaternion(1, 0, 0, 0);
			Assert.IsFalse(positionChanged);
			Assert.IsFalse(levelChanged);
			Assert.IsFalse(rotationChanged);
			Assert.IsFalse(sizeChanged);
			Assert.IsFalse(parentChanged);
			ResetBools();

			testObject.Size = new Vector2(1, 0);
			Assert.IsFalse(positionChanged);
			Assert.IsFalse(levelChanged);
			Assert.IsFalse(rotationChanged);
			Assert.IsFalse(sizeChanged);
			Assert.IsFalse(parentChanged);
			ResetBools();

			testObject.Parent = parentObject;
			Assert.IsFalse(positionChanged);
			Assert.IsFalse(levelChanged);
			Assert.IsFalse(rotationChanged);
			Assert.IsFalse(sizeChanged);
			Assert.IsFalse(parentChanged);
			ResetBools();

			// Changing the parent should also create the events
			parentObject.Position = new Vector2(1, 0);
			Assert.IsTrue(positionChanged);
			Assert.IsFalse(levelChanged);
			Assert.IsFalse(rotationChanged);
			Assert.IsFalse(sizeChanged);
			Assert.IsFalse(parentChanged);
			ResetBools();

			parentObject.Level = 1;
			Assert.IsFalse(positionChanged);
			Assert.IsTrue(levelChanged);
			Assert.IsFalse(rotationChanged);
			Assert.IsFalse(sizeChanged);
			Assert.IsFalse(parentChanged);
			ResetBools();

			parentObject.Rotation = new Quaternion(1, 0, 0, 0);
			Assert.IsFalse(positionChanged);
			Assert.IsFalse(levelChanged);
			Assert.IsTrue(rotationChanged);
			Assert.IsFalse(sizeChanged);
			Assert.IsFalse(parentChanged);
			ResetBools();

			parentObject.Size = new Vector2(1, 0);
			Assert.IsFalse(positionChanged);
			Assert.IsFalse(levelChanged);
			Assert.IsFalse(rotationChanged);
			Assert.IsTrue(sizeChanged);
			Assert.IsFalse(parentChanged);
			ResetBools();

			BoardObject parent2Object = BoardObject.Create();
			parentObject.Parent = parent2Object;
			Assert.IsFalse(positionChanged);
			Assert.IsFalse(levelChanged);
			Assert.IsFalse(rotationChanged);
			Assert.IsFalse(sizeChanged);
			Assert.IsTrue(parentChanged);
			ResetBools();


			parentObject.Position = new Vector2(1, 0);
			Assert.IsFalse(positionChanged);
			Assert.IsFalse(levelChanged);
			Assert.IsFalse(rotationChanged);
			Assert.IsFalse(sizeChanged);
			Assert.IsFalse(parentChanged);
			ResetBools();

			parentObject.Level = 1;
			Assert.IsFalse(positionChanged);
			Assert.IsFalse(levelChanged);
			Assert.IsFalse(rotationChanged);
			Assert.IsFalse(sizeChanged);
			Assert.IsFalse(parentChanged);
			ResetBools();

			parentObject.Rotation = new Quaternion(1, 0, 0, 0);
			Assert.IsFalse(positionChanged);
			Assert.IsFalse(levelChanged);
			Assert.IsFalse(rotationChanged);
			Assert.IsFalse(sizeChanged);
			Assert.IsFalse(parentChanged);
			ResetBools();

			parentObject.Size = new Vector2(1, 0);
			Assert.IsFalse(positionChanged);
			Assert.IsFalse(levelChanged);
			Assert.IsFalse(rotationChanged);
			Assert.IsFalse(sizeChanged);
			Assert.IsFalse(parentChanged);
			ResetBools();

			parentObject.Parent = parent2Object;
			Assert.IsFalse(positionChanged);
			Assert.IsFalse(levelChanged);
			Assert.IsFalse(rotationChanged);
			Assert.IsFalse(sizeChanged);
			Assert.IsFalse(parentChanged);
			ResetBools();
		}

	}
}
