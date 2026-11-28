using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200210C RID: 8460
	public class SymbolResolver_RemoveDangerousTerrain : SymbolResolver
	{
		// Token: 0x0600D469 RID: 54377 RVA: 0x004E8238 File Offset: 0x004E6438
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			TerrainGrid terrainGrid = map.terrainGrid;
			foreach (IntVec3 c in rp.rect)
			{
				if (terrainGrid.TerrainAt(c).dangerous)
				{
					terrainGrid.SetTerrain(c, map.BiomeAt(c).TerrainForAffordance(TerrainAffordanceDefOf.Heavy));
				}
			}
		}
	}
}
