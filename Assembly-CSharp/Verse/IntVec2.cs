using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000067 RID: 103
	public struct IntVec2 : IEquatable<IntVec2>
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000379 RID: 889 RVA: 0x0001745E File Offset: 0x0001565E
		public bool IsInvalid
		{
			get
			{
				return this.x < -500;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600037A RID: 890 RVA: 0x0001746D File Offset: 0x0001566D
		public bool IsValid
		{
			get
			{
				return this.x >= -500;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600037B RID: 891 RVA: 0x0001747F File Offset: 0x0001567F
		public static IntVec2 Zero
		{
			get
			{
				return new IntVec2(0, 0);
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600037C RID: 892 RVA: 0x00017488 File Offset: 0x00015688
		public static IntVec2 One
		{
			get
			{
				return new IntVec2(1, 1);
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600037D RID: 893 RVA: 0x00017491 File Offset: 0x00015691
		public static IntVec2 Two
		{
			get
			{
				return new IntVec2(2, 2);
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600037E RID: 894 RVA: 0x0001749A File Offset: 0x0001569A
		public static IntVec2 North
		{
			get
			{
				return new IntVec2(0, 1);
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600037F RID: 895 RVA: 0x000174A3 File Offset: 0x000156A3
		public static IntVec2 East
		{
			get
			{
				return new IntVec2(1, 0);
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000380 RID: 896 RVA: 0x000174AC File Offset: 0x000156AC
		public static IntVec2 South
		{
			get
			{
				return new IntVec2(0, -1);
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000381 RID: 897 RVA: 0x000174B5 File Offset: 0x000156B5
		public static IntVec2 West
		{
			get
			{
				return new IntVec2(-1, 0);
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000382 RID: 898 RVA: 0x000174BE File Offset: 0x000156BE
		public float Magnitude
		{
			get
			{
				return Mathf.Sqrt((float)(this.x * this.x + this.z * this.z));
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000383 RID: 899 RVA: 0x000174E1 File Offset: 0x000156E1
		public int MagnitudeManhattan
		{
			get
			{
				return Mathf.Abs(this.x) + Mathf.Abs(this.z);
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000384 RID: 900 RVA: 0x000174FA File Offset: 0x000156FA
		public int Area
		{
			get
			{
				return Mathf.Abs(this.x) * Mathf.Abs(this.z);
			}
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00017513 File Offset: 0x00015713
		public IntVec2(int newX, int newZ)
		{
			this.x = newX;
			this.z = newZ;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00017523 File Offset: 0x00015723
		public IntVec2(Vector2 v2)
		{
			this.x = (int)v2.x;
			this.z = (int)v2.y;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0001753F File Offset: 0x0001573F
		public Vector2 ToVector2()
		{
			return new Vector2((float)this.x, (float)this.z);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00017554 File Offset: 0x00015754
		public Vector3 ToVector3()
		{
			return new Vector3((float)this.x, 0f, (float)this.z);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0001756E File Offset: 0x0001576E
		public IntVec2 Rotated()
		{
			return new IntVec2(this.z, this.x);
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00017584 File Offset: 0x00015784
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.x.ToString(),
				", ",
				this.z.ToString(),
				")"
			});
		}

		// Token: 0x0600038B RID: 907 RVA: 0x000175D0 File Offset: 0x000157D0
		public string ToStringCross()
		{
			return this.x.ToString() + " x " + this.z.ToString();
		}

		// Token: 0x0600038C RID: 908 RVA: 0x000175F4 File Offset: 0x000157F4
		public static IntVec2 FromString(string str)
		{
			str = str.TrimStart('(');
			str = str.TrimEnd(')');
			string[] array = str.Split(',', StringSplitOptions.None);
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			int newX = Convert.ToInt32(array[0], invariantCulture);
			int newZ = Convert.ToInt32(array[1], invariantCulture);
			return new IntVec2(newX, newZ);
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600038D RID: 909 RVA: 0x0001763E File Offset: 0x0001583E
		public static IntVec2 Invalid
		{
			get
			{
				return new IntVec2(-1000, -1000);
			}
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0001764F File Offset: 0x0001584F
		public Vector2 ToVector2Shifted()
		{
			return new Vector2((float)this.x + 0.5f, (float)this.z + 0.5f);
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00017670 File Offset: 0x00015870
		public static IntVec2 operator +(IntVec2 a, IntVec2 b)
		{
			return new IntVec2(a.x + b.x, a.z + b.z);
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00017691 File Offset: 0x00015891
		public static IntVec2 operator -(IntVec2 a, IntVec2 b)
		{
			return new IntVec2(a.x - b.x, a.z - b.z);
		}

		// Token: 0x06000391 RID: 913 RVA: 0x000176B2 File Offset: 0x000158B2
		public static IntVec2 operator *(IntVec2 a, int b)
		{
			return new IntVec2(a.x * b, a.z * b);
		}

		// Token: 0x06000392 RID: 914 RVA: 0x000176C9 File Offset: 0x000158C9
		public static IntVec2 operator *(IntVec2 a, float b)
		{
			return new IntVec2(Mathf.RoundToInt((float)a.x * b), Mathf.RoundToInt((float)a.z * b));
		}

		// Token: 0x06000393 RID: 915 RVA: 0x000176EC File Offset: 0x000158EC
		public static IntVec2 operator /(IntVec2 a, int b)
		{
			return new IntVec2(a.x / b, a.z / b);
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000394 RID: 916 RVA: 0x00017703 File Offset: 0x00015903
		public IntVec3 ToIntVec3
		{
			get
			{
				return new IntVec3(this.x, 0, this.z);
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00017717 File Offset: 0x00015917
		public static bool operator ==(IntVec2 a, IntVec2 b)
		{
			return a.x == b.x && a.z == b.z;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00017738 File Offset: 0x00015938
		public static bool operator !=(IntVec2 a, IntVec2 b)
		{
			return a.x != b.x || a.z != b.z;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00017759 File Offset: 0x00015959
		public override bool Equals(object obj)
		{
			return obj is IntVec2 && this.Equals((IntVec2)obj);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00017771 File Offset: 0x00015971
		public bool Equals(IntVec2 other)
		{
			return this.x == other.x && this.z == other.z;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00017791 File Offset: 0x00015991
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.x, this.z);
		}

		// Token: 0x0400017A RID: 378
		public int x;

		// Token: 0x0400017B RID: 379
		public int z;
	}
}
