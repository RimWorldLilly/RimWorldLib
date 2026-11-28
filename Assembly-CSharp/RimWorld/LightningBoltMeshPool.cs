using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013A3 RID: 5027
	public static class LightningBoltMeshPool
	{
		// Token: 0x1700153D RID: 5437
		// (get) Token: 0x06007CB0 RID: 31920 RVA: 0x002C8ED0 File Offset: 0x002C70D0
		public static Mesh RandomBoltMesh
		{
			get
			{
				if (LightningBoltMeshPool.boltMeshes.Count < 20)
				{
					Mesh mesh = LightningBoltMeshMaker.NewBoltMesh();
					LightningBoltMeshPool.boltMeshes.Add(mesh);
					return mesh;
				}
				return LightningBoltMeshPool.boltMeshes.RandomElement<Mesh>();
			}
		}

		// Token: 0x04004E7D RID: 20093
		private static List<Mesh> boltMeshes = new List<Mesh>();

		// Token: 0x04004E7E RID: 20094
		private const int NumBoltMeshesMax = 20;
	}
}
