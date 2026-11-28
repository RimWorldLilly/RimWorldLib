using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012DC RID: 4828
	public class RoadDefGenStep_DryWithFallback : RoadDefGenStep
	{
		// Token: 0x060077C7 RID: 30663 RVA: 0x002A6676 File Offset: 0x002A4876
		public override void Place(Map map, IntVec3 position, TerrainDef rockDef, IntVec3 origin, GenStep_Roads.DistanceElement[,] distance)
		{
			RoadDefGenStep_DryWithFallback.PlaceWorker(map, position, this.fallback);
		}

		// Token: 0x060077C8 RID: 30664 RVA: 0x002A6688 File Offset: 0x002A4888
		public static void PlaceWorker(Map map, IntVec3 position, TerrainDef fallback)
		{
			while (map.terrainGrid.TerrainAt(position).driesTo != null)
			{
				map.terrainGrid.SetTerrain(position, map.terrainGrid.TerrainAt(position).driesTo);
			}
			TerrainDef terrainDef = map.terrainGrid.TerrainAt(position);
			if (terrainDef.passability == Traversability.Impassable || terrainDef.IsRiver)
			{
				map.terrainGrid.SetTerrain(position, fallback);
			}
		}

		// Token: 0x04004B3A RID: 19258
		public TerrainDef fallback;
	}
}
