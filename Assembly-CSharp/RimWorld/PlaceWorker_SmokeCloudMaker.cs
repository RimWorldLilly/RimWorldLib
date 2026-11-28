using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002056 RID: 8278
	public class PlaceWorker_SmokeCloudMaker : PlaceWorker
	{
		// Token: 0x0600D111 RID: 53521 RVA: 0x004CE148 File Offset: 0x004CC348
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_SmokeCloudMaker compProperties_SmokeCloudMaker;
			if ((compProperties_SmokeCloudMaker = def.GetCompProperties<CompProperties_SmokeCloudMaker>()) == null)
			{
				ThingDef thingDef = def.entityDefToBuild as ThingDef;
				compProperties_SmokeCloudMaker = ((thingDef != null) ? thingDef.GetCompProperties<CompProperties_SmokeCloudMaker>() : null);
			}
			CompProperties_SmokeCloudMaker compProperties_SmokeCloudMaker2 = compProperties_SmokeCloudMaker;
			if (compProperties_SmokeCloudMaker2 != null)
			{
				GenDraw.DrawCircleOutline(center.ToVector3Shifted(), compProperties_SmokeCloudMaker2.cloudRadius);
			}
		}
	}
}
