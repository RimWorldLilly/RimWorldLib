using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003E3 RID: 995
	public class Gene_PollutionRush : Gene
	{
		// Token: 0x06001D50 RID: 7504 RVA: 0x000B2F40 File Offset: 0x000B1140
		public override void PostRemove()
		{
			Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PollutionStimulus, false);
			if (firstHediffOfDef != null)
			{
				this.pawn.health.RemoveHediff(firstHediffOfDef);
			}
			base.PostRemove();
		}
	}
}
