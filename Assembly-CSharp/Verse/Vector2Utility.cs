using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200006C RID: 108
	public static class Vector2Utility
	{
		// Token: 0x060003E6 RID: 998 RVA: 0x000185A0 File Offset: 0x000167A0
		public static Vector2 Rotated(this Vector2 v)
		{
			return new Vector2(v.y, v.x);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x000185B3 File Offset: 0x000167B3
		public static Vector2 RotatedBy(this Vector2 v, Rot4 rot)
		{
			return v.RotatedBy(rot.AsAngle);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x000185C4 File Offset: 0x000167C4
		public static Vector2 RotatedBy(this Vector2 v, float degrees)
		{
			float num = Mathf.Sin(degrees * 0.017453292f);
			float num2 = Mathf.Cos(degrees * 0.017453292f);
			return new Vector2(num2 * v.x - num * v.y, num * v.x + num2 * v.y);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00018612 File Offset: 0x00016812
		public static float AngleTo(this Vector2 a, Vector2 b)
		{
			return Mathf.Atan2(-(b.y - a.y), b.x - a.x) * 57.29578f;
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0001863A File Offset: 0x0001683A
		public static Vector3 ScaledBy(this Vector3 a, Vector3 b)
		{
			return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00018668 File Offset: 0x00016868
		public static Vector2 Moved(this Vector2 v, float angle, float distance)
		{
			return new Vector2(v.x + Mathf.Cos(angle * 0.017453292f) * distance, v.y - Mathf.Sin(angle * 0.017453292f) * distance);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00018699 File Offset: 0x00016899
		public static Vector2 FromAngle(float angle)
		{
			return new Vector2(Mathf.Cos(angle * 0.017453292f), -Mathf.Sin(angle * 0.017453292f));
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x000186B9 File Offset: 0x000168B9
		public static float ToAngle(this Vector2 v)
		{
			return Mathf.Atan2(-v.y, v.x) * 57.29578f;
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x000186D3 File Offset: 0x000168D3
		public static Vector3 ToVector3(this Vector2 v)
		{
			return new Vector3(v.x, 0f, v.y);
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x000186EB File Offset: 0x000168EB
		public static Vector2 ToVector2(this Vector3 v)
		{
			return new Vector2(v.x, v.z);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x000186FE File Offset: 0x000168FE
		public static float Cross(this Vector2 u, Vector2 v)
		{
			return u.x * v.y - u.y * v.x;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0001871C File Offset: 0x0001691C
		public static float DistanceToRect(this Vector2 u, Rect rect)
		{
			if (rect.Contains(u))
			{
				return 0f;
			}
			if (u.x < rect.xMin && u.y < rect.yMin)
			{
				return Vector2.Distance(u, new Vector2(rect.xMin, rect.yMin));
			}
			if (u.x > rect.xMax && u.y < rect.yMin)
			{
				return Vector2.Distance(u, new Vector2(rect.xMax, rect.yMin));
			}
			if (u.x < rect.xMin && u.y > rect.yMax)
			{
				return Vector2.Distance(u, new Vector2(rect.xMin, rect.yMax));
			}
			if (u.x > rect.xMax && u.y > rect.yMax)
			{
				return Vector2.Distance(u, new Vector2(rect.xMax, rect.yMax));
			}
			if (u.x < rect.xMin)
			{
				return rect.xMin - u.x;
			}
			if (u.x > rect.xMax)
			{
				return u.x - rect.xMax;
			}
			if (u.y < rect.yMin)
			{
				return rect.yMin - u.y;
			}
			return u.y - rect.yMax;
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00018881 File Offset: 0x00016A81
		public static Vector3 MultipliedBy(this Vector3 a, Vector3 b)
		{
			return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x000188AF File Offset: 0x00016AAF
		public static Vector2 Cardinalize(this Vector2 vec)
		{
			return vec.Cardinalize(Vector2.zero);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x000188BC File Offset: 0x00016ABC
		public static Vector2 Cardinalize(this Vector2 vec, Vector2 bias)
		{
			if (vec.sqrMagnitude == 0f)
			{
				return vec;
			}
			Vector2 normalized = (vec + bias).normalized;
			float num = Mathf.Abs(normalized.x);
			float num2 = Mathf.Abs(normalized.y);
			if (num > num2)
			{
				if (normalized.x > 0f)
				{
					return Vector2.right;
				}
				return Vector2.left;
			}
			else
			{
				if (normalized.y > 0f)
				{
					return Vector2.up;
				}
				return Vector2.down;
			}
		}
	}
}
