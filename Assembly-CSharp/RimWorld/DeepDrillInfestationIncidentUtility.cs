using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001170 RID: 4464
	public static class DeepDrillInfestationIncidentUtility
	{
		// Token: 0x06007014 RID: 28692 RVA: 0x00273A5C File Offset: 0x00271C5C
		public static void GetUsableDeepDrills(Map map, List<Thing> outDrills)
		{
			outDrills.Clear();
			List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.CreatesInfestations);
			Faction ofPlayer = Faction.OfPlayer;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Faction == ofPlayer && list[i].TryGetComp<CompCreatesInfestations>().CanCreateInfestationNow)
				{
					outDrills.Add(list[i]);
				}
			}
		}
	}
}
