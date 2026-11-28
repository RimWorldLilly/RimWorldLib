using System;
using System.Collections.Generic;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02001F2B RID: 7979
	public static class LineMeshGenerator
	{
		// Token: 0x0600CB8E RID: 52110 RVA: 0x004B0070 File Offset: 0x004AE270
		public static Mesh Generate(Vector2[] points, float width)
		{
			Vector3[] array = new Vector3[points.Length * 2];
			Vector2[] array2 = new Vector2[array.Length];
			int[] array3 = new int[2 * (points.Length - 1) * 3];
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < points.Length; i++)
			{
				Vector2 vector = Vector2.zero;
				if (i < points.Length - 1)
				{
					vector += points[(i + 1) % points.Length] - points[i];
				}
				if (i > 0)
				{
					vector += points[i] - points[(i - 1 + points.Length) % points.Length];
				}
				vector.Normalize();
				Vector2 a = new Vector2(-vector.y, vector.x);
				Vector2 vector2 = points[i] + a * (width * 0.5f);
				Vector2 vector3 = points[i] - a * (width * 0.5f);
				array[num] = new Vector3(vector2.x, 0f, vector2.y);
				array[num + 1] = new Vector3(vector3.x, 0f, vector3.y);
				float num3 = (float)i / (float)(points.Length - 1);
				float y = 1f - Mathf.Abs(2f * num3 - 1f);
				array2[num] = new Vector2(0f, y);
				array2[num + 1] = new Vector2(1f, y);
				if (i < points.Length - 1)
				{
					array3[num2] = num;
					array3[num2 + 1] = (num + 2) % array.Length;
					array3[num2 + 2] = num + 1;
					array3[num2 + 3] = num + 1;
					array3[num2 + 4] = (num + 2) % array.Length;
					array3[num2 + 5] = (num + 3) % array.Length;
				}
				num += 2;
				num2 += 6;
			}
			return new Mesh
			{
				vertices = array,
				triangles = array3,
				uv = array2
			};
		}

		// Token: 0x0600CB8F RID: 52111 RVA: 0x004B0270 File Offset: 0x004AE470
		public static Vector2[] CalculateEvenlySpacedPoints(List<Vector2> points, float spacing, float resolution = 1f)
		{
			List<Vector2> list = new List<Vector2>
			{
				points[0]
			};
			Vector2 a = points[0];
			int num = points.Count / 3;
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				Vector2[] pointsInSegment = LineMeshGenerator.GetPointsInSegment(points, i);
				float num3 = Vector2.Distance(pointsInSegment[0], pointsInSegment[1]) + Vector2.Distance(pointsInSegment[1], pointsInSegment[2]) + Vector2.Distance(pointsInSegment[2], pointsInSegment[3]);
				int num4 = Mathf.CeilToInt((Vector2.Distance(pointsInSegment[0], pointsInSegment[3]) + num3 / 2f) * resolution * 10f);
				float num5 = 0f;
				while (num5 <= 1f)
				{
					num5 += 1f / (float)num4;
					Vector2 vector = Bezier.EvaluateCubic(pointsInSegment[0], pointsInSegment[1], pointsInSegment[2], pointsInSegment[3], num5);
					num2 += Vector2.Distance(a, vector);
					while (num2 >= spacing)
					{
						float num6 = num2 - spacing;
						Vector2 vector2 = vector + (a - vector).normalized * num6;
						list.Add(vector2);
						num2 = num6;
						a = vector2;
					}
					a = vector;
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600CB90 RID: 52112 RVA: 0x004B03D4 File Offset: 0x004AE5D4
		private static Vector2[] GetPointsInSegment(List<Vector2> points, int i)
		{
			return new Vector2[]
			{
				points[i * 3],
				points[i * 3 + 1],
				points[i * 3 + 2],
				points[LineMeshGenerator.LoopIndex(points, i * 3 + 3)]
			};
		}

		// Token: 0x0600CB91 RID: 52113 RVA: 0x004B0433 File Offset: 0x004AE633
		private static int LoopIndex(List<Vector2> points, int i)
		{
			return (i + points.Count) % points.Count;
		}
	}
}
