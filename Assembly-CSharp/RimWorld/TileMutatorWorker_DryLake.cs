using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001334 RID: 4916
	public class TileMutatorWorker_DryLake : TileMutatorWorker_Lake
	{
		// Token: 0x170014C7 RID: 5319
		// (get) Token: 0x060079CF RID: 31183 RVA: 0x002B59A0 File Offset: 0x002B3BA0
		protected override float LakeRadius
		{
			get
			{
				return 0.6f;
			}
		}

		// Token: 0x170014C8 RID: 5320
		// (get) Token: 0x060079D0 RID: 31184 RVA: 0x002B59A7 File Offset: 0x002B3BA7
		protected override bool GenerateDeepWater
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060079D1 RID: 31185 RVA: 0x002B59AA File Offset: 0x002B3BAA
		public TileMutatorWorker_DryLake(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x060079D2 RID: 31186 RVA: 0x002B59B4 File Offset: 0x002B3BB4
		protected override void ProcessCell(IntVec3 cell, Map map)
		{
			float valAt = this.GetValAt(cell, map);
			if (valAt > 0.5f)
			{
				map.terrainGrid.SetTerrain(cell, TerrainDefOf.DryLakeBed);
				return;
			}
			if (valAt > 0.45f)
			{
				map.terrainGrid.SetTerrain(cell, TerrainDefOf.Sand);
			}
		}
	}
}
