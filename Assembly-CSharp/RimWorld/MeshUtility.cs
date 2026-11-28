using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F2D RID: 7981
	public static class MeshUtility
	{
		// Token: 0x0600CB9D RID: 52125 RVA: 0x004B066C File Offset: 0x004AE86C
		public static void RemoveVertices(List<Vector3> verts, List<TriangleIndices> tris, Predicate<Vector3> predicate)
		{
			int i = 0;
			int count = tris.Count;
			while (i < count)
			{
				TriangleIndices triangleIndices = tris[i];
				if (predicate(verts[triangleIndices.v1]) || predicate(verts[triangleIndices.v2]) || predicate(verts[triangleIndices.v3]))
				{
					tris[i] = new TriangleIndices(-1, -1, -1);
				}
				i++;
			}
			tris.RemoveAll((TriangleIndices x) => x.v1 == -1);
			MeshUtility.RemoveUnusedVertices(verts, tris);
		}

		// Token: 0x0600CB9E RID: 52126 RVA: 0x004B070C File Offset: 0x004AE90C
		public static void RemoveUnusedVertices(List<Vector3> verts, List<TriangleIndices> tris)
		{
			MeshUtility.vertIsUsed.Clear();
			int i = 0;
			int count = verts.Count;
			while (i < count)
			{
				MeshUtility.vertIsUsed.Add(false);
				i++;
			}
			int j = 0;
			int count2 = tris.Count;
			while (j < count2)
			{
				TriangleIndices triangleIndices = tris[j];
				MeshUtility.vertIsUsed[triangleIndices.v1] = true;
				MeshUtility.vertIsUsed[triangleIndices.v2] = true;
				MeshUtility.vertIsUsed[triangleIndices.v3] = true;
				j++;
			}
			int num = 0;
			MeshUtility.offsets.Clear();
			int k = 0;
			int count3 = verts.Count;
			while (k < count3)
			{
				if (!MeshUtility.vertIsUsed[k])
				{
					num++;
				}
				MeshUtility.offsets.Add(num);
				k++;
			}
			int l = 0;
			int count4 = tris.Count;
			while (l < count4)
			{
				TriangleIndices triangleIndices2 = tris[l];
				tris[l] = new TriangleIndices(triangleIndices2.v1 - MeshUtility.offsets[triangleIndices2.v1], triangleIndices2.v2 - MeshUtility.offsets[triangleIndices2.v2], triangleIndices2.v3 - MeshUtility.offsets[triangleIndices2.v3]);
				l++;
			}
			verts.RemoveAll((Vector3 elem, int index) => !MeshUtility.vertIsUsed[index]);
		}

		// Token: 0x0600CB9F RID: 52127 RVA: 0x004B0877 File Offset: 0x004AEA77
		public static bool Visible(Vector3 point, float radius, Vector3 viewCenter, float viewAngle)
		{
			return viewAngle >= 180f || Vector3.Angle(viewCenter * radius, point) <= viewAngle;
		}

		// Token: 0x0600CBA0 RID: 52128 RVA: 0x004B0898 File Offset: 0x004AEA98
		public static bool VisibleForWorldgen(Vector3 point, float radius, Vector3 viewCenter, float viewAngle)
		{
			if (viewAngle >= 180f)
			{
				return true;
			}
			float num = Vector3.Angle(viewCenter * radius, point) + -1E-05f;
			if (Mathf.Abs(num - viewAngle) < 1E-06f)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(73, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Angle difference ");
				defaultInterpolatedStringHandler.AppendFormatted<float>(num - viewAngle);
				defaultInterpolatedStringHandler.AppendLiteral(" is within epsilon; recommend adjusting visibility tweak");
				Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return num <= viewAngle;
		}

		// Token: 0x0600CBA1 RID: 52129 RVA: 0x004B0911 File Offset: 0x004AEB11
		public static Color32 MutateAlpha(this Color32 input, byte newAlpha)
		{
			input.a = newAlpha;
			return input;
		}

		// Token: 0x040081C3 RID: 33219
		private static List<int> offsets = new List<int>();

		// Token: 0x040081C4 RID: 33220
		private static List<bool> vertIsUsed = new List<bool>();
	}
}
