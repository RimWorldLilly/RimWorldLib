using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002074 RID: 8308
	public class PlaceWorker_GravshipThruster : PlaceWorker
	{
		// Token: 0x0600D16B RID: 53611 RVA: 0x004CFC14 File Offset: 0x004CDE14
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_GravshipThruster compProperties = def.GetCompProperties<CompProperties_GravshipThruster>();
			if (compProperties != null)
			{
				compProperties.GetExclusionZone(center, rot, ref PlaceWorker_GravshipThruster.exclusionCells);
				GenDraw.DrawFieldEdges(PlaceWorker_GravshipThruster.exclusionCells, 2900);
			}
		}

		// Token: 0x0600D16C RID: 53612 RVA: 0x004CFC48 File Offset: 0x004CDE48
		public override void PostPlace(Map map, BuildableDef def, IntVec3 loc, Rot4 rot)
		{
			Thing thing;
			bool flag;
			if (CompGravshipThruster.IsBlocked(def as ThingDef, map, loc, rot, out thing, out flag))
			{
				if (thing != null)
				{
					Messages.Message("WarningThrusterBlocked".Translate(def.LabelCap, thing.Label), MessageTypeDefOf.CautionInput, false);
					return;
				}
				if (flag)
				{
					Messages.Message("WarningThrusterBlocked".Translate(def.LabelCap, TerrainDefOf.Substructure.label), MessageTypeDefOf.CautionInput, false);
					return;
				}
			}
			else if (!CompGravshipThruster.IsOutdoors(def as ThingDef, map, loc, rot))
			{
				Messages.Message("WarningThrusterInside".Translate(def.LabelCap), MessageTypeDefOf.CautionInput, false);
			}
		}

		// Token: 0x040084EA RID: 34026
		private static List<IntVec3> exclusionCells = new List<IntVec3>();
	}
}
