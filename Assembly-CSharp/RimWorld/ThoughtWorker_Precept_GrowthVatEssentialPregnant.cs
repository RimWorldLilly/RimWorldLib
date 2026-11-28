using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DD9 RID: 3545
	public class ThoughtWorker_Precept_GrowthVatEssentialPregnant : ThoughtWorker_Precept
	{
		// Token: 0x0600608C RID: 24716 RVA: 0x0022FAA6 File Offset: 0x0022DCA6
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.IdeologyActive || !ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			return p.health.hediffSet.HasHediff(HediffDefOf.PregnantHuman, false);
		}
	}
}
