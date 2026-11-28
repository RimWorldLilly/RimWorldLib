using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200006B RID: 107
	public static class Vector3Utility
	{
		// Token: 0x060003D8 RID: 984 RVA: 0x00018316 File Offset: 0x00016516
		public static Vector3 HorizontalVectorFromAngle(float angle)
		{
			return Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00018330 File Offset: 0x00016530
		public static float AngleFlat(this Vector3 v)
		{
			if (v.x == 0f && v.z == 0f)
			{
				return 0f;
			}
			return Quaternion.LookRotation(v).eulerAngles.y;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00018370 File Offset: 0x00016570
		public static Vector3 RandomHorizontalOffset(float maxDist)
		{
			float d = Rand.Range(0f, maxDist);
			float y = (float)Rand.Range(0, 360);
			return Quaternion.Euler(new Vector3(0f, y, 0f)) * Vector3.forward * d;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x000183BB File Offset: 0x000165BB
		public static Vector3 Yto0(this Vector3 v3)
		{
			return new Vector3(v3.x, 0f, v3.z);
		}

		// Token: 0x060003DC RID: 988 RVA: 0x000183D3 File Offset: 0x000165D3
		public static Vector3 WithYOffset(this Vector3 v3, float offset)
		{
			return new Vector3(v3.x, v3.y + offset, v3.z);
		}

		// Token: 0x060003DD RID: 989 RVA: 0x000183EE File Offset: 0x000165EE
		public static Vector3 WithY(this Vector3 v3, float y)
		{
			return new Vector3(v3.x, y, v3.z);
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00018402 File Offset: 0x00016602
		public static Vector3 SetToAltitude(this Vector3 v3, AltitudeLayer altitude)
		{
			v3.y = altitude.AltitudeFor();
			return v3;
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00018412 File Offset: 0x00016612
		public static Vector3 RotatedBy(this Vector3 v3, float angle)
		{
			return Quaternion.AngleAxis(angle, Vector3.up) * v3;
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00018428 File Offset: 0x00016628
		public static Vector3 RotatedBy(this Vector3 orig, Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
				return orig;
			case 1:
				return new Vector3(orig.z, orig.y, -orig.x);
			case 2:
				return new Vector3(-orig.x, orig.y, -orig.z);
			case 3:
				return new Vector3(-orig.z, orig.y, orig.x);
			default:
				return orig;
			}
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x000184A4 File Offset: 0x000166A4
		public static float AngleToFlat(this Vector3 a, Vector3 b)
		{
			return new Vector2(a.x, a.z).AngleTo(new Vector2(b.x, b.z));
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x000184D0 File Offset: 0x000166D0
		public static Vector3 FromAngleFlat(float angle)
		{
			Vector2 vector = Vector2Utility.FromAngle(angle);
			return new Vector3(vector.x, 0f, vector.y);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x000184FA File Offset: 0x000166FA
		public static float ToAngleFlat(this Vector3 v)
		{
			return new Vector2(v.x, v.z).ToAngle();
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00018512 File Offset: 0x00016712
		public static Vector3 Abs(this Vector3 v)
		{
			return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0001853C File Offset: 0x0001673C
		public static Vector3 ClosestTo(this IEnumerable<Vector3> pts, Vector3 target)
		{
			float num = float.MaxValue;
			Vector3 result = pts.FirstOrDefault<Vector3>();
			foreach (Vector3 vector in pts)
			{
				float num2 = Vector3.Distance(vector, target);
				if (num2 < num)
				{
					num = num2;
					result = vector;
				}
			}
			return result;
		}
	}
}
