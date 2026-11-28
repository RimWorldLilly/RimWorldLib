using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x020002FE RID: 766
	public static class RegionListersUpdater
	{
		// Token: 0x06001761 RID: 5985 RVA: 0x0008C904 File Offset: 0x0008AB04
		public static void DeregisterInRegions(Thing thing, Map map)
		{
			if (!ListerThings.EverListable(thing.def, ListerThingsUse.Region))
			{
				return;
			}
			RegionListersUpdater.GetTouchableRegions(thing, map, RegionListersUpdater.tmpRegions, true);
			for (int i = 0; i < RegionListersUpdater.tmpRegions.Count; i++)
			{
				ListerThings listerThings = RegionListersUpdater.tmpRegions[i].ListerThings;
				if (listerThings.Contains(thing))
				{
					listerThings.Remove(thing);
				}
			}
			RegionListersUpdater.tmpRegions.Clear();
		}

		// Token: 0x06001762 RID: 5986 RVA: 0x0008C970 File Offset: 0x0008AB70
		public static void RegisterInRegions(Thing thing, Map map)
		{
			if (!ListerThings.EverListable(thing.def, ListerThingsUse.Region))
			{
				return;
			}
			RegionListersUpdater.GetTouchableRegions(thing, map, RegionListersUpdater.tmpRegions, false);
			for (int i = 0; i < RegionListersUpdater.tmpRegions.Count; i++)
			{
				ListerThings listerThings = RegionListersUpdater.tmpRegions[i].ListerThings;
				if (!listerThings.Contains(thing))
				{
					listerThings.Add(thing);
				}
			}
			RegionListersUpdater.tmpRegions.Clear();
		}

		// Token: 0x06001763 RID: 5987 RVA: 0x0008C9DC File Offset: 0x0008ABDC
		public static void RegisterAllAt(IntVec3 c, Map map, HashSet<Thing> processedThings = null)
		{
			List<Thing> thingList = c.GetThingList(map);
			int count = thingList.Count;
			for (int i = 0; i < count; i++)
			{
				Thing thing = thingList[i];
				if (processedThings == null || processedThings.Add(thing))
				{
					RegionListersUpdater.RegisterInRegions(thing, map);
				}
			}
		}

		// Token: 0x06001764 RID: 5988 RVA: 0x0008CA20 File Offset: 0x0008AC20
		public static void GetTouchableRegions(Thing thing, Map map, List<Region> outRegions, bool allowAdjacentEvenIfCantTouch = false)
		{
			outRegions.Clear();
			CellRect cellRect = thing.OccupiedRect();
			CellRect cellRect2 = cellRect;
			if (RegionListersUpdater.CanRegisterInAdjacentRegions(thing))
			{
				cellRect2 = cellRect2.ExpandedBy(1);
			}
			foreach (IntVec3 intVec in cellRect2)
			{
				if (intVec.InBounds(map))
				{
					Region validRegionAt_NoRebuild = map.regionGrid.GetValidRegionAt_NoRebuild(intVec);
					if (validRegionAt_NoRebuild != null && validRegionAt_NoRebuild.type.Passable() && !outRegions.Contains(validRegionAt_NoRebuild))
					{
						if (cellRect.Contains(intVec))
						{
							outRegions.Add(validRegionAt_NoRebuild);
						}
						else if (allowAdjacentEvenIfCantTouch || ReachabilityImmediate.CanReachImmediate(intVec, thing, map, PathEndMode.Touch, null))
						{
							outRegions.Add(validRegionAt_NoRebuild);
						}
					}
				}
			}
		}

		// Token: 0x06001765 RID: 5989 RVA: 0x0008CAEC File Offset: 0x0008ACEC
		private static bool CanRegisterInAdjacentRegions(Thing thing)
		{
			return true;
		}

		// Token: 0x040013DE RID: 5086
		private static List<Region> tmpRegions = new List<Region>();
	}
}
