using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E8B RID: 3723
	public class ThoughtWorker_BioferriteGenerator : ThoughtWorker
	{
		// Token: 0x06006260 RID: 25184 RVA: 0x00235B60 File Offset: 0x00233D60
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
			foreach (Thing thing in p.MapHeld.listerThings.ThingsOfDef(ThingDefOf.BioferriteGenerator))
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
