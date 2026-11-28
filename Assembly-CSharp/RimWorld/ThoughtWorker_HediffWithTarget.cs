using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E19 RID: 3609
	public class ThoughtWorker_HediffWithTarget : ThoughtWorker_Hediff
	{
		// Token: 0x0600611E RID: 24862 RVA: 0x00231CD8 File Offset: 0x0022FED8
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			HediffWithTarget hediffWithTarget = (HediffWithTarget)p.health.hediffSet.GetFirstHediffOfDef(this.def.hediff, false);
			if (hediffWithTarget == null || hediffWithTarget.target != other)
			{
				return ThoughtState.Inactive;
			}
			return this.CurrentStateInternal(p);
		}
	}
}
