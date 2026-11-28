using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020012CF RID: 4815
	public class GenStep_Terrain : GenStep
	{
		// Token: 0x1700144D RID: 5197
		// (get) Token: 0x06007774 RID: 30580 RVA: 0x002A240D File Offset: 0x002A060D
		public override int SeedPart
		{
			get
			{
				return 262606459;
			}
		}

		// Token: 0x06007775 RID: 30581 RVA: 0x002A2414 File Offset: 0x002A0614
		public override void Generate(Map map, GenStepParams parms)
		{
			TerrainGrid terrainGrid = map.terrainGrid;
			List<IntVec3> list = new List<IntVec3>();
			using (map.pathing.DisableIncrementalScope())
			{
				foreach (IntVec3 intVec in map.AllCells)
				{
					Building edifice = intVec.GetEdifice(map);
					TerrainDef naturalTerrainAt = MapGenUtility.GetNaturalTerrainAt(intVec, map);
					if (!naturalTerrainAt.supportsRock && edifice != null)
					{
						list.Add(edifice.Position);
						edifice.Destroy(DestroyMode.Vanish);
					}
					terrainGrid.SetTerrain(intVec, naturalTerrainAt);
				}
				RoofCollapseCellsFinder.RemoveBulkCollapsingRoofs(list, map);
				foreach (TerrainPatchMaker terrainPatchMaker in map.Biome.terrainPatchMakers)
				{
					terrainPatchMaker.Cleanup();
				}
			}
		}
	}
}
