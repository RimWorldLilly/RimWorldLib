using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001217 RID: 4631
	public class ComplexThreatWorker_Ambush : ComplexThreatWorker
	{
		// Token: 0x060072F0 RID: 29424 RVA: 0x00283229 File Offset: 0x00281429
		protected override bool CanResolveInt(ComplexResolveParams parms)
		{
			return base.CanResolveInt(parms) && (parms.hostileFaction == null || (this.def.signalActionAmbushType == SignalActionAmbushType.Mechanoids && parms.hostileFaction == Faction.OfMechanoids));
		}

		// Token: 0x060072F1 RID: 29425 RVA: 0x00283260 File Offset: 0x00281460
		protected override void ResolveInt(ComplexResolveParams parms, ref float threatPointsUsed, List<Thing> outSpawnedThings)
		{
			SignalAction_Ambush signalAction_Ambush = (SignalAction_Ambush)ThingMaker.MakeThing(ThingDefOf.SignalAction_Ambush, null);
			signalAction_Ambush.signalTag = parms.triggerSignal;
			signalAction_Ambush.points = parms.points;
			signalAction_Ambush.ambushType = this.def.signalActionAmbushType;
			signalAction_Ambush.useDropPods = this.def.useDropPods;
			if (this.def.spawnAroundComplex)
			{
				signalAction_Ambush.spawnAround = parms.complexRect.ExpandedBy(5);
			}
			GenSpawn.Spawn(signalAction_Ambush, parms.room.rects[0].CenterCell, parms.map, WipeMode.Vanish);
			threatPointsUsed += parms.points;
		}

		// Token: 0x040048B8 RID: 18616
		private const int SpawnAroundDistance = 5;
	}
}
