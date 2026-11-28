using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B59 RID: 7001
	public static class SkyfallerUtility
	{
		// Token: 0x0600B3D2 RID: 46034 RVA: 0x003F6510 File Offset: 0x003F4710
		public static bool CanPossiblyFallOnColonist(ThingDef skyfaller, IntVec3 c, Map map)
		{
			CellRect cellRect = GenAdj.OccupiedRect(c, Rot4.North, skyfaller.size);
			int dist = Mathf.Max(Mathf.CeilToInt(skyfaller.skyfaller.explosionRadius) + 7, 14);
			foreach (IntVec3 c2 in cellRect.ExpandedBy(dist))
			{
				if (c2.InBounds(map))
				{
					List<Thing> thingList = c2.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Pawn pawn = thingList[i] as Pawn;
						if (pawn != null && pawn.IsColonist)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
