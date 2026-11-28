using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200030F RID: 783
	public static class RoofUtility
	{
		// Token: 0x06001813 RID: 6163 RVA: 0x00090908 File Offset: 0x0008EB08
		public static Thing FirstBlockingThing(IntVec3 pos, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(pos);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.plant != null && list[i].def.plant.interferesWithRoof)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x00090968 File Offset: 0x0008EB68
		public static bool IsAnyCellUnderRoof(Thing thing)
		{
			CellRect cellRect = thing.OccupiedRect();
			bool result = false;
			RoofGrid roofGrid = thing.Map.roofGrid;
			foreach (IntVec3 c in cellRect)
			{
				if (roofGrid.Roofed(c))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x000909D8 File Offset: 0x0008EBD8
		public static bool CanHandleBlockingThing(Thing blocker, Pawn worker, bool forced = false)
		{
			if (blocker == null)
			{
				return true;
			}
			if (blocker.def.category == ThingCategory.Plant)
			{
				if (!PlantUtility.PawnWillingToCutPlant_Job(blocker, worker))
				{
					return false;
				}
				if (worker.CanReserveAndReach(blocker, PathEndMode.ClosestTouch, worker.NormalMaxDanger(), 1, -1, null, forced))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001816 RID: 6166 RVA: 0x00090A14 File Offset: 0x0008EC14
		public static Job HandleBlockingThingJob(Thing blocker, Pawn worker, bool forced = false)
		{
			if (blocker == null)
			{
				return null;
			}
			if (blocker.def.category != ThingCategory.Plant || !worker.CanReserveAndReach(blocker, PathEndMode.ClosestTouch, worker.NormalMaxDanger(), 1, -1, null, forced))
			{
				return null;
			}
			if (!PlantUtility.PawnWillingToCutPlant_Job(blocker, worker))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.CutPlant, blocker);
		}
	}
}
