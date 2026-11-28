using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001253 RID: 4691
	public class RoomContents_Harbinger : RoomContentsWorker
	{
		// Token: 0x06007451 RID: 29777 RVA: 0x0028B1C0 File Offset: 0x002893C0
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			IntVec3 intVec;
			if (!room.TryGetRandomCellInRoom(map, out intVec, 2, 0, null, false))
			{
				return;
			}
			((Plant)GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Plant_TreeHarbinger, null), intVec, map, WipeMode.Vanish)).Growth = RoomContents_Harbinger.TreeGrowthRange.RandomInRange;
			foreach (IntVec3 c in GridShapeMaker.IrregularLump(intVec, map, RoomContents_Harbinger.DirtAmount.RandomInRange, (IntVec3 x) => room.Contains(x, 1)))
			{
				map.terrainGrid.SetTerrain(c, TerrainDefOf.Soil);
			}
		}

		// Token: 0x04004920 RID: 18720
		private static readonly IntRange DirtAmount = new IntRange(10, 14);

		// Token: 0x04004921 RID: 18721
		private static readonly FloatRange TreeGrowthRange = new FloatRange(0.5f, 0.9f);
	}
}
