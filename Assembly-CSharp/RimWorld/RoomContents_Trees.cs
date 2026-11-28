using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200125B RID: 4699
	public class RoomContents_Trees : RoomContentsWorker
	{
		// Token: 0x06007468 RID: 29800 RVA: 0x0028B79C File Offset: 0x0028999C
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			if (RoomContents_Trees.treeKinds.Empty<ThingDef>())
			{
				RoomContents_Trees.treeKinds.AddRange(new ThingDef[]
				{
					ThingDefOf.Plant_TreeOak,
					ThingDefOf.Plant_TreePoplar,
					ThingDefOf.Plant_TreeBirch,
					ThingDefOf.Plant_TreePine
				});
			}
			ThingDef thingDef = RoomContents_Trees.treeKinds.RandomElement<ThingDef>();
			IntVec3 center;
			if (!room.TryGetRandomCellInRoom(map, out center, 2, 0, null, false))
			{
				return;
			}
			int randomInRange = RoomContents_Trees.DirtAmountRange.RandomInRange;
			int randomInRange2 = RoomContents_Trees.TressRange.RandomInRange;
			int num = 0;
			List<IntVec3> list = GridShapeMaker.IrregularLump(center, map, randomInRange, (IntVec3 x) => room.Contains(x, 1));
			for (int i = 0; i < list.Count; i++)
			{
				IntVec3 intVec = list[i];
				map.terrainGrid.SetTerrain(intVec, TerrainDefOf.Soil);
				if (Rand.DynamicChance(num, randomInRange2, list.Count - i))
				{
					num++;
					Plant plant = (Plant)GenSpawn.Spawn(thingDef, intVec, map, WipeMode.Vanish);
					plant.Growth = RoomContents_Trees.TreeGrowthRange.RandomInRange;
					if (!thingDef.plant.dieIfLeafless)
					{
						plant.MakeLeafless(Plant.LeaflessCause.Cold, true);
					}
				}
			}
		}

		// Token: 0x04004928 RID: 18728
		private static readonly IntRange DirtAmountRange = new IntRange(18, 24);

		// Token: 0x04004929 RID: 18729
		private static readonly IntRange TressRange = new IntRange(2, 4);

		// Token: 0x0400492A RID: 18730
		private static readonly FloatRange TreeGrowthRange = new FloatRange(0.7f, 1f);

		// Token: 0x0400492B RID: 18731
		private static readonly List<ThingDef> treeKinds = new List<ThingDef>();
	}
}
