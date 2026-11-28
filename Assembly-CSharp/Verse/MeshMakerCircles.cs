using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000502 RID: 1282
	public static class MeshMakerCircles
	{
		// Token: 0x06002606 RID: 9734 RVA: 0x000E7954 File Offset: 0x000E5B54
		public static void MakePieMeshes(Mesh[] meshes)
		{
			if (meshes.Length < 361)
			{
				throw new ArgumentException();
			}
			Vector3[] array = new Vector3[362];
			array[0] = new Vector3(0f, 0f, 0f);
			for (int i = 0; i <= 360; i++)
			{
				float num = (float)i / 180f * 3.1415927f;
				float x = (float)(0.550000011920929 * Math.Cos((double)num));
				float z = (float)(0.550000011920929 * Math.Sin((double)num));
				array[i + 1] = new Vector3(x, 0f, z);
			}
			int[] array2 = new int[1080];
			for (int j = 0; j < 360; j++)
			{
				array2[j * 3] = j + 2;
				array2[j * 3 + 1] = j + 1;
				array2[j * 3 + 2] = 0;
			}
			Bounds bounds = default(Bounds);
			Vector3 vector = 0.55f * new Vector3(1f, 0f, 1f);
			bounds.SetMinMax(-vector, vector);
			for (int k = 0; k < 361; k++)
			{
				meshes[k] = new Mesh();
				meshes[k].vertices = array;
				meshes[k].SetTriangles(array2, 0, 3 * k, 0, false, 0);
				meshes[k].RecalculateNormals();
				meshes[k].bounds = bounds;
			}
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x000E7AC0 File Offset: 0x000E5CC0
		public static Mesh MakeCircleMesh(float radius)
		{
			MeshMakerCircles.tmpCircleVerts[0] = Vector3.zero;
			int i = 0;
			int num = 1;
			while (i <= 360)
			{
				float f = (float)i / 180f * 3.1415927f;
				MeshMakerCircles.tmpCircleVerts[num] = new Vector3(radius * Mathf.Cos(f), 0f, radius * Mathf.Sin(f));
				i += 4;
				num++;
			}
			for (int j = 1; j < MeshMakerCircles.tmpCircleVerts.Length; j++)
			{
				int num2 = (j - 1) * 3;
				MeshMakerCircles.tmpTris[num2] = 0;
				MeshMakerCircles.tmpTris[num2 + 1] = (j + 1) % MeshMakerCircles.tmpCircleVerts.Length;
				MeshMakerCircles.tmpTris[num2 + 2] = j;
			}
			Mesh mesh = new Mesh();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 1);
			defaultInterpolatedStringHandler.AppendLiteral("CircleMesh_");
			defaultInterpolatedStringHandler.AppendFormatted<float>(radius, "0.#");
			mesh.name = defaultInterpolatedStringHandler.ToStringAndClear();
			mesh.SetVertices(MeshMakerCircles.tmpCircleVerts);
			mesh.SetTriangles(MeshMakerCircles.tmpTris, 0);
			return mesh;
		}

		// Token: 0x04001B5F RID: 7007
		private const float PieRadius = 0.55f;

		// Token: 0x04001B60 RID: 7008
		private const int MaxDegreesWide = 361;

		// Token: 0x04001B61 RID: 7009
		private const int CircleVertexCount = 92;

		// Token: 0x04001B62 RID: 7010
		private static readonly List<Vector3> tmpVerts = new List<Vector3>(361);

		// Token: 0x04001B63 RID: 7011
		private static readonly Vector2[] emptyUVs = new Vector2[361];

		// Token: 0x04001B64 RID: 7012
		private static readonly Vector3[] tmpCircleVerts = new Vector3[92];

		// Token: 0x04001B65 RID: 7013
		private static readonly int[] tmpTris = new int[273];
	}
}
