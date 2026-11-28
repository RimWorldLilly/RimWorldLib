using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000069 RID: 105
	public struct IntVec3 : IEquatable<IntVec3>
	{
		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600039C RID: 924 RVA: 0x000177CE File Offset: 0x000159CE
		public IntVec2 ToIntVec2
		{
			get
			{
				return new IntVec2(this.x, this.z);
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600039D RID: 925 RVA: 0x000177E1 File Offset: 0x000159E1
		public bool IsValid
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.y >= 0;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600039E RID: 926 RVA: 0x000177EF File Offset: 0x000159EF
		public int LengthHorizontalSquared
		{
			get
			{
				return this.x * this.x + this.z * this.z;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600039F RID: 927 RVA: 0x0001780C File Offset: 0x00015A0C
		public float LengthHorizontal
		{
			get
			{
				return GenMath.Sqrt((float)(this.x * this.x + this.z * this.z));
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x0001782F File Offset: 0x00015A2F
		public int LengthManhattan
		{
			get
			{
				return ((this.x >= 0) ? this.x : (-this.x)) + ((this.z >= 0) ? this.z : (-this.z));
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x00017862 File Offset: 0x00015A62
		public float Magnitude
		{
			get
			{
				return Mathf.Sqrt((float)(this.x * this.x + this.z * this.z));
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x00017885 File Offset: 0x00015A85
		public float SqrMagnitude
		{
			get
			{
				return (float)(this.x * this.x + this.z * this.z);
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x000178A3 File Offset: 0x00015AA3
		public bool IsCardinal
		{
			get
			{
				return this.x == 0 || this.z == 0;
			}
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x000178B8 File Offset: 0x00015AB8
		public static implicit operator int3(IntVec3 value)
		{
			return new int3(value.x, value.y, value.z);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x000178D1 File Offset: 0x00015AD1
		public static implicit operator float3(IntVec3 value)
		{
			return new float3((float)value.x, (float)value.y, (float)value.z);
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x000178ED File Offset: 0x00015AED
		public static implicit operator IntVec3(int3 value)
		{
			return new IntVec3(value.x, value.y, value.z);
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00017906 File Offset: 0x00015B06
		public static implicit operator IntVec3(float3 value)
		{
			return new IntVec3(Mathf.FloorToInt(value.x), Mathf.FloorToInt(value.y), Mathf.FloorToInt(value.z));
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x00017930 File Offset: 0x00015B30
		public float AngleFlat
		{
			get
			{
				if (this.x == 0 && this.z == 0)
				{
					return 0f;
				}
				return Quaternion.LookRotation(this.ToVector3()).eulerAngles.y;
			}
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0001796B File Offset: 0x00015B6B
		public IntVec3(int newX, int newY, int newZ)
		{
			this.x = newX;
			this.y = newY;
			this.z = newZ;
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00017982 File Offset: 0x00015B82
		public IntVec3(Vector3 v)
		{
			this.x = (int)v.x;
			this.y = 0;
			this.z = (int)v.z;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x000179A5 File Offset: 0x00015BA5
		public IntVec3(Vector2 v)
		{
			this.x = (int)v.x;
			this.y = 0;
			this.z = (int)v.y;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x000179C8 File Offset: 0x00015BC8
		public static IntVec3 operator -(IntVec3 a)
		{
			return new IntVec3(-a.x, -a.y, -a.z);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x000179E4 File Offset: 0x00015BE4
		public static IntVec3 FromString(string str)
		{
			str = str.TrimStart('(');
			str = str.TrimEnd(')');
			string[] array = str.Split(',', StringSplitOptions.None);
			IntVec3 result;
			try
			{
				CultureInfo invariantCulture = CultureInfo.InvariantCulture;
				int newX = Convert.ToInt32(array[0], invariantCulture);
				int newY = Convert.ToInt32(array[1], invariantCulture);
				int newZ = Convert.ToInt32(array[2], invariantCulture);
				result = new IntVec3(newX, newY, newZ);
			}
			catch (Exception ex)
			{
				string str2 = str;
				string str3 = " is not a valid IntVec3 format. Exception: ";
				Exception ex2 = ex;
				Log.Warning(str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
				result = IntVec3.Invalid;
			}
			return result;
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00017A7C File Offset: 0x00015C7C
		public Vector2 ToVector2()
		{
			return new Vector2((float)this.x, (float)this.z);
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00017A91 File Offset: 0x00015C91
		public Vector3 ToVector3()
		{
			return new Vector3((float)this.x, (float)this.y, (float)this.z);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00017AAD File Offset: 0x00015CAD
		public Vector3 ToVector3Shifted()
		{
			return new Vector3((float)this.x + 0.5f, (float)this.y, (float)this.z + 0.5f);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00017AD5 File Offset: 0x00015CD5
		public Vector3 ToVector3ShiftedWithAltitude(AltitudeLayer AltLayer)
		{
			return this.ToVector3ShiftedWithAltitude(AltLayer.AltitudeFor());
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00017AE3 File Offset: 0x00015CE3
		public Vector3 ToVector3ShiftedWithAltitude(float AddedAltitude)
		{
			return new Vector3((float)this.x + 0.5f, (float)this.y + AddedAltitude, (float)this.z + 0.5f);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00017B10 File Offset: 0x00015D10
		public bool InHorDistOf(IntVec3 otherLoc, float maxDist)
		{
			float num = (float)(this.x - otherLoc.x);
			float num2 = (float)(this.z - otherLoc.z);
			return num * num + num2 * num2 <= maxDist * maxDist;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00017B48 File Offset: 0x00015D48
		public static IntVec3 FromVector3(Vector3 v)
		{
			return IntVec3.FromVector3(v, 0);
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00017B51 File Offset: 0x00015D51
		public static IntVec3 FromVector3(Vector3 v, int newY)
		{
			return new IntVec3((int)v.x, newY, (int)v.z);
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00017B68 File Offset: 0x00015D68
		public static IntVec3 FromPolar(float angle, float distance)
		{
			int num = (int)(Mathf.Cos(angle * 0.017453292f) * distance);
			float num2 = Mathf.Sin(angle * 0.017453292f) * distance;
			return new IntVec3(num, 0, (int)num2);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00017B9B File Offset: 0x00015D9B
		public Vector2 ToUIPosition()
		{
			return this.ToVector3Shifted().MapToUIPosition();
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00017BA8 File Offset: 0x00015DA8
		public Rect ToUIRect()
		{
			Vector2 vector = this.ToVector3().MapToUIPosition();
			Vector2 vector2 = (this + IntVec3.NorthEast).ToVector3().MapToUIPosition();
			return new Rect(vector.x, vector2.y, vector2.x - vector.x, vector.y - vector2.y);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00017C0C File Offset: 0x00015E0C
		public bool AdjacentToCardinal(IntVec3 other)
		{
			return this.IsValid && ((other.z == this.z && (other.x == this.x + 1 || other.x == this.x - 1)) || (other.x == this.x && (other.z == this.z + 1 || other.z == this.z - 1)));
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00017C84 File Offset: 0x00015E84
		public bool CardinalTo(IntVec3 other)
		{
			return this.IsValid && other.IsValid && (other.z == this.z || other.x == this.x);
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00017CB7 File Offset: 0x00015EB7
		public bool AdjacentToDiagonal(IntVec3 other)
		{
			return this.IsValid && Mathf.Abs(this.x - other.x) == 1 && Mathf.Abs(this.z - other.z) == 1;
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00017CEF File Offset: 0x00015EEF
		public bool AdjacentTo(IntVec3 other)
		{
			return this.AdjacentToDiagonal(other) || this.AdjacentToCardinal(other);
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00017D04 File Offset: 0x00015F04
		public bool AdjacentToCardinal(District district)
		{
			if (!this.IsValid)
			{
				return false;
			}
			Map map = district.Map;
			if (this.InBounds(map) && this.GetDistrict(map, RegionType.Set_All) == district)
			{
				return true;
			}
			IntVec3[] cardinalDirections = GenAdj.CardinalDirections;
			for (int i = 0; i < cardinalDirections.Length; i++)
			{
				IntVec3 intVec = this + cardinalDirections[i];
				if (intVec.InBounds(map) && intVec.GetDistrict(map, RegionType.Set_All) == district)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00017D82 File Offset: 0x00015F82
		public IntVec3 ClampInsideMap(Map map)
		{
			return this.ClampInsideRect(CellRect.WholeMap(map));
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00017D90 File Offset: 0x00015F90
		public IntVec3 ClampMagnitude(float magnitude)
		{
			float lengthHorizontal = this.LengthHorizontal;
			if (lengthHorizontal <= magnitude)
			{
				return this;
			}
			float num = (float)this.x / lengthHorizontal;
			float num2 = (float)this.z / lengthHorizontal;
			this.x = Mathf.RoundToInt(num * magnitude);
			this.z = Mathf.RoundToInt(num2 * magnitude);
			return this;
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00017DE8 File Offset: 0x00015FE8
		public IntVec3 ClampInsideRect(CellRect rect)
		{
			this.x = Mathf.Clamp(this.x, rect.minX, rect.maxX);
			this.y = 0;
			this.z = Mathf.Clamp(this.z, rect.minZ, rect.maxZ);
			return this;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00017E3C File Offset: 0x0001603C
		public static IntVec3 operator +(IntVec3 a, IntVec3 b)
		{
			return new IntVec3(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00017E6A File Offset: 0x0001606A
		public static IntVec3 operator -(IntVec3 a, IntVec3 b)
		{
			return new IntVec3(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00017E98 File Offset: 0x00016098
		public static IntVec3 operator *(IntVec3 a, int i)
		{
			return new IntVec3(a.x * i, a.y * i, a.z * i);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00017EB7 File Offset: 0x000160B7
		public static IntVec3 operator *(int i, IntVec3 a)
		{
			return new IntVec3(a.x * i, a.y * i, a.z * i);
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00017ED6 File Offset: 0x000160D6
		public static IntVec3 operator /(IntVec3 a, int i)
		{
			return new IntVec3(a.x / i, a.y / i, a.z / i);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00017EF5 File Offset: 0x000160F5
		public static bool operator ==(IntVec3 a, IntVec3 b)
		{
			return a.x == b.x && a.z == b.z && a.y == b.y;
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00017F24 File Offset: 0x00016124
		public static bool operator !=(IntVec3 a, IntVec3 b)
		{
			return a.x != b.x || a.z != b.z || a.y != b.y;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00017F53 File Offset: 0x00016153
		public override bool Equals(object obj)
		{
			return obj is IntVec3 && this.Equals((IntVec3)obj);
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x00017F6B File Offset: 0x0001616B
		public bool Equals(IntVec3 other)
		{
			return this.x == other.x && this.z == other.z && this.y == other.y;
		}

		// Token: 0x060003CA RID: 970 RVA: 0x00017F99 File Offset: 0x00016199
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(Gen.HashCombineInt(Gen.HashCombineInt(0, this.x), this.y), this.z);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00017FBD File Offset: 0x000161BD
		public ulong UniqueHashCode()
		{
			return (ulong)(0L + (long)this.x + 4096L * (long)this.z + 16777216L * (long)this.y);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00017FE8 File Offset: 0x000161E8
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.x.ToString(),
				", ",
				this.y.ToString(),
				", ",
				this.z.ToString(),
				")"
			});
		}

		// Token: 0x0400017C RID: 380
		public int x;

		// Token: 0x0400017D RID: 381
		public int y;

		// Token: 0x0400017E RID: 382
		public int z;

		// Token: 0x0400017F RID: 383
		public static readonly IntVec3 Zero = new IntVec3(0, 0, 0);

		// Token: 0x04000180 RID: 384
		public static readonly IntVec3 North = new IntVec3(0, 0, 1);

		// Token: 0x04000181 RID: 385
		public static readonly IntVec3 East = new IntVec3(1, 0, 0);

		// Token: 0x04000182 RID: 386
		public static readonly IntVec3 South = new IntVec3(0, 0, -1);

		// Token: 0x04000183 RID: 387
		public static readonly IntVec3 West = new IntVec3(-1, 0, 0);

		// Token: 0x04000184 RID: 388
		public static readonly IntVec3 NorthWest = new IntVec3(-1, 0, 1);

		// Token: 0x04000185 RID: 389
		public static readonly IntVec3 NorthEast = new IntVec3(1, 0, 1);

		// Token: 0x04000186 RID: 390
		public static readonly IntVec3 SouthWest = new IntVec3(-1, 0, -1);

		// Token: 0x04000187 RID: 391
		public static readonly IntVec3 SouthEast = new IntVec3(1, 0, -1);

		// Token: 0x04000188 RID: 392
		public static readonly IntVec3 Invalid = new IntVec3(-1000, -1000, -1000);
	}
}
