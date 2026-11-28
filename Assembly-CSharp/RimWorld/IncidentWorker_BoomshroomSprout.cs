using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200116A RID: 4458
	public class IncidentWorker_BoomshroomSprout : IncidentWorker
	{
		// Token: 0x06006FFC RID: 28668 RVA: 0x002731B4 File Offset: 0x002713B4
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return PlantUtility.GrowthSeasonNow(map, ThingDefOf.Boomshroom) && this.TryFindRootCell(map, out intVec);
		}

		// Token: 0x06006FFD RID: 28669 RVA: 0x002731F0 File Offset: 0x002713F0
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			if (!this.TryFindRootCell(map, out intVec))
			{
				return false;
			}
			Thing thing = null;
			int randomInRange = IncidentWorker_BoomshroomSprout.CountRange.RandomInRange;
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < randomInRange; i++)
			{
				IntVec3 root = intVec;
				Map map2 = map;
				int radius = 4;
				Predicate<IntVec3> extraValidator;
				if ((extraValidator = <>9__0) == null)
				{
					extraValidator = (<>9__0 = ((IntVec3 x) => this.CanSpawnAt(x, map)));
				}
				IntVec3 intVec2;
				if (!CellFinder.TryRandomClosewalkCellNear(root, map2, radius, out intVec2, extraValidator))
				{
					break;
				}
				Plant plant = intVec2.GetPlant(map);
				if (plant != null)
				{
					plant.Destroy(DestroyMode.Vanish);
				}
				Thing thing2 = GenSpawn.Spawn(ThingDefOf.Boomshroom, intVec2, map, WipeMode.Vanish);
				if (thing == null)
				{
					thing = thing2;
				}
			}
			if (thing == null)
			{
				return false;
			}
			base.SendStandardLetter(parms, thing, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x06006FFE RID: 28670 RVA: 0x002732D0 File Offset: 0x002714D0
		private bool TryFindRootCell(Map map, out IntVec3 cell)
		{
			return CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => this.CanSpawnAt(x, map) && x.GetRoom(map).CellCount >= 64, map, out cell);
		}

		// Token: 0x06006FFF RID: 28671 RVA: 0x0027330C File Offset: 0x0027150C
		private bool CanSpawnAt(IntVec3 c, Map map)
		{
			if (!c.Standable(map) || c.Fogged(map) || map.fertilityGrid.FertilityAt(c) < ThingDefOf.Boomshroom.plant.fertilityMin || !c.GetRoom(map).PsychologicallyOutdoors || c.GetEdifice(map) != null || !PlantUtility.GrowthSeasonNow(c, map, ThingDefOf.Boomshroom))
			{
				return false;
			}
			Thing thing;
			if (!ThingDefOf.Boomshroom.CanEverPlantAt(c, map, out thing, false, true, true))
			{
				return false;
			}
			Plant plant = c.GetPlant(map);
			if (plant != null && plant.def.plant.growDays > 10f)
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def == ThingDefOf.Boomshroom)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400478F RID: 18319
		private static readonly IntRange CountRange = new IntRange(6, 10);

		// Token: 0x04004790 RID: 18320
		private const int MinRoomCells = 64;

		// Token: 0x04004791 RID: 18321
		private const int SpawnRadius = 4;
	}
}
