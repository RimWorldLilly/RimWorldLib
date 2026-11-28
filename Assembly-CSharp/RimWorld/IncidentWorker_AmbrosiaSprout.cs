using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001167 RID: 4455
	public class IncidentWorker_AmbrosiaSprout : IncidentWorker
	{
		// Token: 0x06006FEE RID: 28654 RVA: 0x00272B38 File Offset: 0x00270D38
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return PlantUtility.GrowthSeasonNow(map, ThingDefOf.Plant_Ambrosia) && this.TryFindRootCell(map, out intVec);
		}

		// Token: 0x06006FEF RID: 28655 RVA: 0x00272B74 File Offset: 0x00270D74
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			if (!this.TryFindRootCell(map, out intVec))
			{
				return false;
			}
			Thing thing = null;
			int randomInRange = IncidentWorker_AmbrosiaSprout.CountRange.RandomInRange;
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < randomInRange; i++)
			{
				IntVec3 root = intVec;
				Map map2 = map;
				int radius = 6;
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
				Thing thing2 = GenSpawn.Spawn(ThingDefOf.Plant_Ambrosia, intVec2, map, WipeMode.Vanish);
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

		// Token: 0x06006FF0 RID: 28656 RVA: 0x00272C54 File Offset: 0x00270E54
		private bool TryFindRootCell(Map map, out IntVec3 cell)
		{
			return CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => this.CanSpawnAt(x, map) && x.GetRoom(map).CellCount >= 64, map, out cell);
		}

		// Token: 0x06006FF1 RID: 28657 RVA: 0x00272C90 File Offset: 0x00270E90
		private bool CanSpawnAt(IntVec3 c, Map map)
		{
			if (!c.Standable(map) || c.Fogged(map) || map.fertilityGrid.FertilityAt(c) < ThingDefOf.Plant_Ambrosia.plant.fertilityMin || !c.GetRoom(map).PsychologicallyOutdoors || c.GetEdifice(map) != null || !PlantUtility.GrowthSeasonNow(c, map, ThingDefOf.Plant_Ambrosia))
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
				if (thingList[i].def == ThingDefOf.Plant_Ambrosia)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400478B RID: 18315
		private static readonly IntRange CountRange = new IntRange(10, 20);

		// Token: 0x0400478C RID: 18316
		private const int MinRoomCells = 64;

		// Token: 0x0400478D RID: 18317
		private const int SpawnRadius = 6;
	}
}
