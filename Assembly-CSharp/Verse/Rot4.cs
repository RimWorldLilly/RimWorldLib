using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000071 RID: 113
	public struct Rot4 : IEquatable<Rot4>
	{
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x00018B00 File Offset: 0x00016D00
		public bool IsValid
		{
			get
			{
				return this.rotInt < 100;
			}
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00018B0C File Offset: 0x00016D0C
		public static implicit operator Rot4(RotEnum rot)
		{
			if (rot == RotEnum.East)
			{
				return Rot4.East;
			}
			if (rot == RotEnum.South)
			{
				return Rot4.South;
			}
			if (rot == RotEnum.West)
			{
				return Rot4.West;
			}
			return Rot4.North;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00018B31 File Offset: 0x00016D31
		public static implicit operator RotEnum(Rot4 rot)
		{
			if (rot == Rot4.East)
			{
				return RotEnum.East;
			}
			if (rot == Rot4.South)
			{
				return RotEnum.South;
			}
			if (rot == Rot4.West)
			{
				return RotEnum.West;
			}
			return RotEnum.North;
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x00018B61 File Offset: 0x00016D61
		// (set) Token: 0x06000401 RID: 1025 RVA: 0x00018B69 File Offset: 0x00016D69
		public byte AsByte
		{
			get
			{
				return this.rotInt;
			}
			set
			{
				this.rotInt = (byte)GenMath.PositiveMod((int)value, 4);
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x00018B79 File Offset: 0x00016D79
		// (set) Token: 0x06000403 RID: 1027 RVA: 0x00018B81 File Offset: 0x00016D81
		public int AsInt
		{
			get
			{
				return (int)this.rotInt;
			}
			set
			{
				this.rotInt = (byte)GenMath.PositiveMod(value, 4);
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x00018B94 File Offset: 0x00016D94
		public float AsAngle
		{
			get
			{
				switch (this.AsInt)
				{
				case 0:
					return 0f;
				case 1:
					return 90f;
				case 2:
					return 180f;
				case 3:
					return 270f;
				default:
					return 0f;
				}
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x00018BE0 File Offset: 0x00016DE0
		public SpectateRectSide AsSpectateSide
		{
			get
			{
				switch (this.AsInt)
				{
				case 0:
					return SpectateRectSide.Up;
				case 1:
					return SpectateRectSide.Right;
				case 2:
					return SpectateRectSide.Down;
				case 3:
					return SpectateRectSide.Left;
				default:
					return SpectateRectSide.None;
				}
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x00018C18 File Offset: 0x00016E18
		public Quaternion AsQuat
		{
			get
			{
				switch (this.rotInt)
				{
				case 0:
					return Quaternion.identity;
				case 1:
					return Quaternion.LookRotation(Vector3.right);
				case 2:
					return Quaternion.LookRotation(Vector3.back);
				case 3:
					return Quaternion.LookRotation(Vector3.left);
				default:
					Log.Error("ToQuat with Rot = " + this.AsInt.ToString());
					return Quaternion.identity;
				}
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x00018C90 File Offset: 0x00016E90
		public Vector2 AsVector2
		{
			get
			{
				switch (this.rotInt)
				{
				case 0:
					return Vector2.up;
				case 1:
					return Vector2.right;
				case 2:
					return Vector2.down;
				case 3:
					return Vector2.left;
				default:
					throw new Exception("rotInt's value cannot be >3 but it is:" + this.rotInt.ToString());
				}
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x00018CF0 File Offset: 0x00016EF0
		public IntVec3 AsIntVec3
		{
			get
			{
				switch (this.rotInt)
				{
				case 0:
					return IntVec3.North;
				case 1:
					return IntVec3.East;
				case 2:
					return IntVec3.South;
				case 3:
					return IntVec3.West;
				default:
					throw new Exception("rotInt's value cannot be >3 but it is:" + this.rotInt.ToString());
				}
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x00018D4E File Offset: 0x00016F4E
		public bool IsHorizontal
		{
			get
			{
				return this.rotInt == 1 || this.rotInt == 3;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x00018D64 File Offset: 0x00016F64
		public bool IsVertical
		{
			get
			{
				return this.rotInt == 0 || this.rotInt == 2;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x00018D79 File Offset: 0x00016F79
		public static Rot4 Random
		{
			get
			{
				return new Rot4(Rand.RangeInclusive(0, 3));
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x00018D87 File Offset: 0x00016F87
		public static IEnumerable<Rot4> AllRotations
		{
			get
			{
				return new Rot4.<get_AllRotations>d__38(-2);
			}
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00018D90 File Offset: 0x00016F90
		public Rot4(byte newRot)
		{
			this.rotInt = (byte)GenMath.PositiveMod((int)newRot, 4);
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00018DA0 File Offset: 0x00016FA0
		public Rot4(int newRot)
		{
			this.rotInt = (byte)GenMath.PositiveMod(newRot, 4);
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x00018DB0 File Offset: 0x00016FB0
		public IntVec3 FacingCell
		{
			get
			{
				switch (this.AsInt)
				{
				case 0:
					return new IntVec3(0, 0, 1);
				case 1:
					return new IntVec3(1, 0, 0);
				case 2:
					return new IntVec3(0, 0, -1);
				case 3:
					return new IntVec3(-1, 0, 0);
				default:
					return default(IntVec3);
				}
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x00018E0C File Offset: 0x0001700C
		public IntVec3 RighthandCell
		{
			get
			{
				switch (this.AsInt)
				{
				case 0:
					return new IntVec3(1, 0, 0);
				case 1:
					return new IntVec3(0, 0, -1);
				case 2:
					return new IntVec3(-1, 0, 0);
				case 3:
					return new IntVec3(0, 0, 1);
				default:
					return default(IntVec3);
				}
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x00018E68 File Offset: 0x00017068
		public Rot4 Opposite
		{
			get
			{
				switch (this.AsInt)
				{
				case 0:
					return new Rot4(2);
				case 1:
					return new Rot4(3);
				case 2:
					return new Rot4(0);
				case 3:
					return new Rot4(1);
				default:
					return default(Rot4);
				}
			}
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00018EB9 File Offset: 0x000170B9
		public void Rotate(RotationDirection RotDir)
		{
			this.AsByte = (byte)(this.AsByte + RotDir);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00018ECC File Offset: 0x000170CC
		public Rot4 Rotated(RotationDirection RotDir)
		{
			Rot4 result = this;
			result.Rotate(RotDir);
			return result;
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00018EEC File Offset: 0x000170EC
		public static Rot4 FromAngleFlat(float angle)
		{
			angle = GenMath.PositiveMod(angle, 360f);
			if (angle < 45f)
			{
				return Rot4.North;
			}
			if (angle < 135f)
			{
				return Rot4.East;
			}
			if (angle < 225f)
			{
				return Rot4.South;
			}
			if (angle < 315f)
			{
				return Rot4.West;
			}
			return Rot4.North;
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00018F44 File Offset: 0x00017144
		public static Rot4 FromIntVec3(IntVec3 offset)
		{
			if (offset.x == 1)
			{
				return Rot4.East;
			}
			if (offset.x == -1)
			{
				return Rot4.West;
			}
			if (offset.z == 1)
			{
				return Rot4.North;
			}
			if (offset.z == -1)
			{
				return Rot4.South;
			}
			string str = "FromIntVec3 with bad offset ";
			IntVec3 intVec = offset;
			Log.Error(str + intVec.ToString());
			return Rot4.North;
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00018FB0 File Offset: 0x000171B0
		public static Rot4 FromIntVec2(IntVec2 offset)
		{
			return Rot4.FromIntVec3(offset.ToIntVec3);
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00018FC0 File Offset: 0x000171C0
		public static RotationDirection GetRelativeRotation(Rot4 from, Rot4 to)
		{
			if (!from.IsValid || !to.IsValid)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Both from (");
				defaultInterpolatedStringHandler.AppendFormatted<Rot4>(from);
				defaultInterpolatedStringHandler.AppendLiteral(") and to (");
				defaultInterpolatedStringHandler.AppendFormatted<Rot4>(to);
				defaultInterpolatedStringHandler.AppendLiteral(") must be valid.");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return RotationDirection.None;
			}
			return (RotationDirection)GenMath.PositiveMod((int)(to.AsByte - from.AsByte), 4);
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00019041 File Offset: 0x00017241
		public static bool operator ==(Rot4 a, Rot4 b)
		{
			return a.AsInt == b.AsInt;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00019053 File Offset: 0x00017253
		public static bool operator !=(Rot4 a, Rot4 b)
		{
			return a.AsInt != b.AsInt;
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00019068 File Offset: 0x00017268
		public override int GetHashCode()
		{
			switch (this.rotInt)
			{
			case 0:
				return 235515;
			case 1:
				return 5612938;
			case 2:
				return 1215650;
			case 3:
				return 9231792;
			default:
				return (int)this.rotInt;
			}
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x000190B2 File Offset: 0x000172B2
		public override string ToString()
		{
			return this.rotInt.ToString();
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x000190C0 File Offset: 0x000172C0
		public string ToStringHuman()
		{
			switch (this.rotInt)
			{
			case 0:
				return "North".Translate();
			case 1:
				return "East".Translate();
			case 2:
				return "South".Translate();
			case 3:
				return "West".Translate();
			default:
				return "error";
			}
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00019134 File Offset: 0x00017334
		public string ToStringWord()
		{
			switch (this.rotInt)
			{
			case 0:
				return "North";
			case 1:
				return "East";
			case 2:
				return "South";
			case 3:
				return "West";
			default:
				return "error";
			}
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00019180 File Offset: 0x00017380
		public static Rot4 FromString(string str)
		{
			int num;
			byte newRot;
			if (int.TryParse(str, out num))
			{
				newRot = (byte)num;
			}
			else if (!(str == "North"))
			{
				if (!(str == "East"))
				{
					if (!(str == "South"))
					{
						if (!(str == "West"))
						{
							newRot = 0;
							Log.Error("Invalid rotation: " + str);
						}
						else
						{
							newRot = 3;
						}
					}
					else
					{
						newRot = 2;
					}
				}
				else
				{
					newRot = 1;
				}
			}
			else
			{
				newRot = 0;
			}
			return new Rot4(newRot);
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x000191FA File Offset: 0x000173FA
		public override bool Equals(object obj)
		{
			return obj is Rot4 && this.Equals((Rot4)obj);
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00019212 File Offset: 0x00017412
		public bool Equals(Rot4 other)
		{
			return this.rotInt == other.rotInt;
		}

		// Token: 0x04000198 RID: 408
		public const int NorthInt = 0;

		// Token: 0x04000199 RID: 409
		public const int EastInt = 1;

		// Token: 0x0400019A RID: 410
		public const int SouthInt = 2;

		// Token: 0x0400019B RID: 411
		public const int WestInt = 3;

		// Token: 0x0400019C RID: 412
		public const int RotationCount = 4;

		// Token: 0x0400019D RID: 413
		private byte rotInt;

		// Token: 0x0400019E RID: 414
		public static readonly Rot4 North = new Rot4(0);

		// Token: 0x0400019F RID: 415
		public static readonly Rot4 East = new Rot4(1);

		// Token: 0x040001A0 RID: 416
		public static readonly Rot4 South = new Rot4(2);

		// Token: 0x040001A1 RID: 417
		public static readonly Rot4 West = new Rot4(3);

		// Token: 0x040001A2 RID: 418
		public static readonly Rot4 Invalid = new Rot4
		{
			rotInt = 200
		};
	}
}
