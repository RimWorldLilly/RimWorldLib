using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D2B RID: 3371
	public static class PaintUtility
	{
		// Token: 0x06005E6E RID: 24174 RVA: 0x002282A4 File Offset: 0x002264A4
		public static List<Thing> FindNearbyDyes(Pawn pawn, bool forced = false)
		{
			List<Thing> list = new List<Thing>();
			List<Thing> list2 = pawn.Map.listerThings.ThingsOfDef(ThingDefOf.Dye);
			for (int i = 0; i < list2.Count; i++)
			{
				if (!list2[i].IsForbidden(pawn) && pawn.CanReserveAndReach(list2[i], PathEndMode.ClosestTouch, Danger.Deadly, 1, -1, null, forced))
				{
					list.Add(list2[i]);
				}
			}
			return list;
		}
	}
}
