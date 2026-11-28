using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E8A RID: 3722
	public class ThoughtWorker_SleepSuppressor : ThoughtWorker
	{
		// Token: 0x0600625E RID: 25182 RVA: 0x00235A7C File Offset: 0x00233C7C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (p.MapHeld == null)
			{
				return false;
			}
			if (!p.health.capacities.CapableOf(PawnCapacityDefOf.Hearing))
			{
				return false;
			}
			foreach (Thing thing in p.MapHeld.listerThings.ThingsOfDef(ThingDefOf.SleepSuppressor))
			{
				CompNoiseSource compNoiseSource = thing.TryGetComp<CompNoiseSource>();
				if (compNoiseSource.Active && p.PositionHeld.InHorDistOf(thing.Position, compNoiseSource.Props.radius))
				{
					return true;
				}
			}
			return false;
		}
	}
}
