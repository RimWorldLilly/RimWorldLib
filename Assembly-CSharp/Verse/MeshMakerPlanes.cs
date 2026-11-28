using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000503 RID: 1283
	public static class MeshMakerPlanes
	{
		// Token: 0x06002609 RID: 9737 RVA: 0x000E7BF0 File Offset: 0x000E5DF0
		public static Mesh NewPlaneMesh(float size)
		{
			return MeshMakerPlanes.NewPlaneMesh(size, false);
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x000E7BF9 File Offset: 0x000E5DF9
		public static Mesh NewPlaneMesh(float size, bool flipped)
		{
			return MeshMakerPlanes.NewPlaneMesh(size, flipped, false);
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x000E7C03 File Offset: 0x000E5E03
		public static Mesh NewPlaneMesh(float size, bool flipped, bool backLift)
		{
			return MeshMakerPlanes.NewPlaneMesh(new Vector2(size, size), flipped, backLift, false);
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x000E7C14 File Offset: 0x000E5E14
		public static Mesh NewPlaneMesh(float size, bool flipped, bool backLift, bool twist)
		{
			return MeshMakerPlanes.NewPlaneMesh(new Vector2(size, size), flipped, backLift, twist);
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x000E7C28 File Offset: 0x000E5E28
		public static Mesh NewPlaneMesh(Vector2 size, bool flipped, bool backLift, bool twist)
		{
			Vector3[] array = new Vector3[4];
			Vector2[] array2 = new Vector2[4];
			int[] array3 = new int[6];
			array[0] = new Vector3(-0.5f * size.x, 0f, -0.5f * size.y);
			array[1] = new Vector3(-0.5f * size.x, 0f, 0.5f * size.y);
			array[2] = new Vector3(0.5f * size.x, 0f, 0.5f * size.y);
			array[3] = new Vector3(0.5f * size.x, 0f, -0.5f * size.y);
			if (backLift)
			{
				array[1].y = 0.0018292684f;
				array[2].y = 0.0018292684f;
				array[3].y = 0.0007317074f;
			}
			if (twist)
			{
				array[0].y = 0.0009146342f;
				array[1].y = 0.0004573171f;
				array[2].y = 0f;
				array[3].y = 0.0004573171f;
			}
			if (!flipped)
			{
				array2[0] = new Vector2(0f, 0f);
				array2[1] = new Vector2(0f, 1f);
				array2[2] = new Vector2(1f, 1f);
				array2[3] = new Vector2(1f, 0f);
			}
			else
			{
				array2[0] = new Vector2(1f, 0f);
				array2[1] = new Vector2(1f, 1f);
				array2[2] = new Vector2(0f, 1f);
				array2[3] = new Vector2(0f, 0f);
			}
			array3[0] = 0;
			array3[1] = 1;
			array3[2] = 2;
			array3[3] = 0;
			array3[4] = 2;
			array3[5] = 3;
			Mesh mesh = new Mesh();
			mesh.name = "NewPlaneMesh()";
			mesh.vertices = array;
			mesh.uv = array2;
			mesh.SetTriangles(array3, 0);
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			MeshPool.EnsureMetaDataCached(mesh, size, flipped);
			return mesh;
		}

		// Token: 0x0600260E RID: 9742 RVA: 0x000E7E74 File Offset: 0x000E6074
		public static Mesh NewWholeMapPlane()
		{
			Mesh mesh = MeshMakerPlanes.NewPlaneMesh(2000f, false, false);
			Vector2[] array = new Vector2[4];
			for (int i = 0; i < 4; i++)
			{
				array[i] = mesh.uv[i] * 200f;
			}
			mesh.uv = array;
			return mesh;
		}

		// Token: 0x04001B66 RID: 7014
		private const float BackLiftAmount = 0.0018292684f;

		// Token: 0x04001B67 RID: 7015
		private const float TwistAmount = 0.0009146342f;
	}
}
