using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AFB RID: 6907
	public static class MonumentMarkerUtility
	{
		// Token: 0x0600B0A8 RID: 45224 RVA: 0x003E42CC File Offset: 0x003E24CC
		public static Building GetFirstAdjacentBuilding(SketchEntity entity, IntVec3 offset, List<Thing> monumentThings, Map map)
		{
			if (entity.IsSameSpawnedOrBlueprintOrFrame(entity.pos + offset, map))
			{
				return null;
			}
			foreach (IntVec3 c in entity.OccupiedRect.MovedBy(offset).ExpandedBy(1).EdgeCells)
			{
				if (c.InBounds(map))
				{
					Building firstBuilding = c.GetFirstBuilding(map);
					if (firstBuilding != null && !monumentThings.Contains(firstBuilding) && (firstBuilding.Faction == null || firstBuilding.Faction == Faction.OfPlayer))
					{
						return firstBuilding;
					}
				}
			}
			return null;
		}
	}
}
