using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200006A RID: 106
	public static class IntVec3Utility
	{
		// Token: 0x060003CE RID: 974 RVA: 0x000180E7 File Offset: 0x000162E7
		public static IntVec3 ToIntVec3(this Vector3 vect)
		{
			return new IntVec3(vect);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x000180F0 File Offset: 0x000162F0
		public static float DistanceTo(this IntVec3 a, IntVec3 b)
		{
			return (a - b).LengthHorizontal;
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0001810C File Offset: 0x0001630C
		public static int DistanceToSquared(this IntVec3 a, IntVec3 b)
		{
			return (a - b).LengthHorizontalSquared;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00018128 File Offset: 0x00016328
		public static IntVec3 RotatedBy(this IntVec3 orig, RotationDirection rot)
		{
			switch (rot)
			{
			case RotationDirection.None:
				return orig;
			case RotationDirection.Clockwise:
				return new IntVec3(orig.z, orig.y, -orig.x);
			case RotationDirection.Opposite:
				return new IntVec3(-orig.x, orig.y, -orig.z);
			case RotationDirection.Counterclockwise:
				return new IntVec3(-orig.z, orig.y, orig.x);
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x000181A0 File Offset: 0x000163A0
		public static IntVec3 RotatedBy(this IntVec3 orig, Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
				return orig;
			case 1:
				return new IntVec3(orig.z, orig.y, -orig.x);
			case 2:
				return new IntVec3(-orig.x, orig.y, -orig.z);
			case 3:
				return new IntVec3(-orig.z, orig.y, orig.x);
			default:
				return orig;
			}
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0001821C File Offset: 0x0001641C
		public static IntVec3 Inverse(this IntVec3 orig)
		{
			return new IntVec3(-orig.x, -orig.y, -orig.z);
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00018238 File Offset: 0x00016438
		public static int ManhattanDistanceFlat(IntVec3 a, IntVec3 b)
		{
			return Math.Abs(a.x - b.x) + Math.Abs(a.z - b.z);
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0001825F File Offset: 0x0001645F
		public static IntVec3 RandomHorizontalOffset(float maxDist)
		{
			return Vector3Utility.RandomHorizontalOffset(maxDist).ToIntVec3();
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0001826C File Offset: 0x0001646C
		public static int DistanceToEdge(this IntVec3 v, Map map)
		{
			return Mathf.Max(Mathf.Min(Mathf.Min(Mathf.Min(v.x, v.z), map.Size.x - v.x - 1), map.Size.z - v.z - 1), 0);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x000182C4 File Offset: 0x000164C4
		public static int Determinant(this IntVec3 p, IntVec3 p1, IntVec3 p2)
		{
			int num = (p2.x - p1.x) * (p.z - p1.z) - (p.x - p1.x) * (p2.z - p1.z);
			if (num > 0)
			{
				return -1;
			}
			if (num < 0)
			{
				return 1;
			}
			return 0;
		}
	}
}
