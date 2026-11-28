using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002040 RID: 8256
	public class PlaceWorker_MultiCellDoor : PlaceWorker
	{
		// Token: 0x0600D0D8 RID: 53464 RVA: 0x004CCEE4 File Offset: 0x004CB0E4
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			foreach (IntVec3 center2 in DoorUtility.WallRequirementCells(def, center, rot))
			{
				GhostDrawer.DrawGhostThing(center2, Rot4.South, ThingDefOf.Wall, null, Color.grey, AltitudeLayer.Blueprint, null, false, null);
			}
		}

		// Token: 0x0600D0D9 RID: 53465 RVA: 0x004CCF48 File Offset: 0x004CB148
		public override void PostPlace(Map map, BuildableDef def, IntVec3 loc, Rot4 rot)
		{
			ThingDef thingDef = def as ThingDef;
			if (thingDef == null)
			{
				return;
			}
			using (IEnumerator<IntVec3> enumerator = DoorUtility.WallRequirementCells(thingDef, loc, rot).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!DoorUtility.EncapsulatingWallAt(enumerator.Current, map, true))
					{
						Messages.Message("MessageBuildingRequiresAdjacentWalls".Translate(def).CapitalizeFirst(), MessageTypeDefOf.CautionInput, false);
						break;
					}
				}
			}
		}
	}
}
