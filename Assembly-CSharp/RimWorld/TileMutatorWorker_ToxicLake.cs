using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001335 RID: 4917
	public class TileMutatorWorker_ToxicLake : TileMutatorWorker_Lake
	{
		// Token: 0x060079D3 RID: 31187 RVA: 0x002B59FD File Offset: 0x002B3BFD
		public TileMutatorWorker_ToxicLake(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x060079D4 RID: 31188 RVA: 0x002B5A08 File Offset: 0x002B3C08
		protected override void ProcessCell(IntVec3 cell, Map map)
		{
			float valAt = this.GetValAt(cell, map);
			if (this.GenerateDeepWater && valAt > 0.75f)
			{
				map.terrainGrid.SetTerrain(cell, TerrainDefOf.ToxicWaterDeep);
				return;
			}
			if (valAt > 0.5f)
			{
				map.terrainGrid.SetTerrain(cell, TerrainDefOf.ToxicWaterShallow);
				return;
			}
			if (valAt > 0.45f && MapGenUtility.ShouldGenerateBeachSand(cell, map))
			{
				map.terrainGrid.SetTerrain(cell, MapGenUtility.LakeshoreTerrainAt(cell, map));
			}
		}
	}
}
