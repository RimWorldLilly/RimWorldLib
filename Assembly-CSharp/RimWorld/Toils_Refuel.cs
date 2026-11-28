using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A8E RID: 2702
	public class Toils_Refuel
	{
		// Token: 0x06005163 RID: 20835 RVA: 0x001E701C File Offset: 0x001E521C
		public static Toil FinalizeRefueling(TargetIndex refuelableInd, TargetIndex fuelInd)
		{
			Toil toil = ToilMaker.MakeToil("FinalizeRefueling");
			toil.initAction = delegate()
			{
				Job curJob = toil.actor.CurJob;
				Thing thing = curJob.GetTarget(refuelableInd).Thing;
				if (toil.actor.CurJob.placedThings.NullOrEmpty<ThingCountClass>())
				{
					thing.TryGetComp<CompRefuelable>().Refuel(new List<Thing>
					{
						curJob.GetTarget(fuelInd).Thing
					});
					return;
				}
				thing.TryGetComp<CompRefuelable>().Refuel((from p in toil.actor.CurJob.placedThings
				select p.thing).ToList<Thing>());
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}
	}
}
