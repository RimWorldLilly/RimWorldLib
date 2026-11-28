using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000504 RID: 1284
	public static class ShadowMeshPool
	{
		// Token: 0x0600260F RID: 9743 RVA: 0x000E7EC6 File Offset: 0x000E60C6
		public static Mesh GetShadowMesh(ShadowData sha)
		{
			return ShadowMeshPool.GetShadowMesh(sha.BaseX, sha.BaseZ, sha.BaseY);
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x000E7EDF File Offset: 0x000E60DF
		public static Mesh GetShadowMesh(float baseEdgeLength, float tallness)
		{
			return ShadowMeshPool.GetShadowMesh(baseEdgeLength, baseEdgeLength, tallness);
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x000E7EEC File Offset: 0x000E60EC
		public static Mesh GetShadowMesh(float baseWidth, float baseHeight, float tallness)
		{
			int key = ShadowMeshPool.HashOf(baseWidth, baseHeight, tallness);
			Mesh mesh;
			if (!ShadowMeshPool.shadowMeshDict.TryGetValue(key, out mesh))
			{
				mesh = MeshMakerShadows.NewShadowMesh(baseWidth, baseHeight, tallness);
				ShadowMeshPool.shadowMeshDict.Add(key, mesh);
			}
			return mesh;
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x000E7F28 File Offset: 0x000E6128
		private static int HashOf(float baseWidth, float baseheight, float tallness)
		{
			int num = (int)(baseWidth * 1000f);
			int num2 = (int)(baseheight * 1000f);
			int num3 = (int)(tallness * 1000f);
			return num * 391 ^ 261231 ^ num2 * 612331 ^ num3 * 456123;
		}

		// Token: 0x04001B68 RID: 7016
		private static Dictionary<int, Mesh> shadowMeshDict = new Dictionary<int, Mesh>();
	}
}
