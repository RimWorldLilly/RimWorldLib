using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001340 RID: 4928
	public class TileMutatorWorker_Patches : TileMutatorWorker
	{
		// Token: 0x06007A06 RID: 31238 RVA: 0x002B6E01 File Offset: 0x002B5001
		public TileMutatorWorker_Patches(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007A07 RID: 31239 RVA: 0x002B6E0C File Offset: 0x002B500C
		public override void GeneratePostTerrain(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			bool flag = map.TileInfo.Mutators.Any((TileMutatorDef m) => m.preventsPondGeneration);
			if (map.TileInfo.Mutators.Any((TileMutatorDef m) => m.preventPatches))
			{
				return;
			}
			foreach (IntVec3 intVec in map.AllCells)
			{
				TerrainDef terrain = intVec.GetTerrain(map);
				if (terrain.categoryType != TerrainDef.TerrainCategoryType.Stone && !terrain.IsWater && terrain != MapGenUtility.BeachTerrainAt(intVec, map) && terrain != MapGenUtility.LakeshoreTerrainAt(intVec, map))
				{
					foreach (TerrainPatchMaker terrainPatchMaker in this.def.terrainPatchMakers)
					{
						if (!terrainPatchMaker.isPond || !flag)
						{
							TerrainDef terrainDef = terrainPatchMaker.TerrainAt(intVec, map, intVec.GetFertility(map));
							if (terrainDef != null)
							{
								map.terrainGrid.SetTerrain(intVec, terrainDef);
							}
						}
					}
				}
			}
			foreach (TerrainPatchMaker terrainPatchMaker2 in this.def.terrainPatchMakers)
			{
				terrainPatchMaker2.Cleanup();
			}
		}
	}
}
