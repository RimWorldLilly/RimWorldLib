using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001219 RID: 4633
	public class ComplexThreatWorker_FuelNode : ComplexThreatWorker
	{
		// Token: 0x060072F7 RID: 29431 RVA: 0x002836E0 File Offset: 0x002818E0
		protected override bool CanResolveInt(ComplexResolveParams parms)
		{
			IntVec3 intVec;
			return base.CanResolveInt(parms) && ComplexUtility.TryFindRandomSpawnCell(ThingDefOf.AncientFuelNode, parms.room, parms.map, out intVec, 1, null);
		}

		// Token: 0x060072F8 RID: 29432 RVA: 0x0028371C File Offset: 0x0028191C
		protected override void ResolveInt(ComplexResolveParams parms, ref float threatPointsUsed, List<Thing> outSpawnedThings)
		{
			IntVec3 loc;
			ComplexUtility.TryFindRandomSpawnCell(ThingDefOf.AncientFuelNode, parms.room, parms.map, out loc, 1, null);
			Thing thing = GenSpawn.Spawn(ThingDefOf.AncientFuelNode, loc, parms.map, WipeMode.Vanish);
			SignalAction_StartWick signalAction_StartWick = (SignalAction_StartWick)ThingMaker.MakeThing(ThingDefOf.SignalAction_StartWick, null);
			signalAction_StartWick.thingWithWick = thing;
			signalAction_StartWick.signalTag = parms.triggerSignal;
			signalAction_StartWick.completedSignalTag = "CompletedStartWickAction" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
			if (parms.delayTicks != null)
			{
				signalAction_StartWick.delayTicks = parms.delayTicks.Value;
			}
			GenSpawn.Spawn(signalAction_StartWick, parms.room.rects[0].CenterCell, parms.map, WipeMode.Vanish);
			CompExplosive compExplosive = thing.TryGetComp<CompExplosive>();
			float randomInRange = ComplexThreatWorker_FuelNode.ExplosiveRadiusRandomRange.RandomInRange;
			compExplosive.customExplosiveRadius = new float?(randomInRange);
			threatPointsUsed = randomInRange * 10f;
		}

		// Token: 0x040048BC RID: 18620
		private const string TriggerStartWickAction = "TriggerStartWickAction";

		// Token: 0x040048BD RID: 18621
		private const string CompletedStartWickAction = "CompletedStartWickAction";

		// Token: 0x040048BE RID: 18622
		private static readonly FloatRange ExplosiveRadiusRandomRange = new FloatRange(2f, 12f);

		// Token: 0x040048BF RID: 18623
		private const float ExplosiveRadiusThreatPointsFactor = 10f;

		// Token: 0x040048C0 RID: 18624
		private const float RoomEntryTriggerChance = 0.25f;
	}
}
