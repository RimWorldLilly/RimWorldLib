using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E2A RID: 3626
	public class ThoughtWorker_Hediff : ThoughtWorker
	{
		// Token: 0x06006147 RID: 24903 RVA: 0x00232540 File Offset: 0x00230740
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(this.def.hediff, false);
			if (((firstHediffOfDef != null) ? firstHediffOfDef.def.stages : null) == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(Mathf.Min(new int[]
			{
				firstHediffOfDef.CurStageIndex,
				firstHediffOfDef.def.stages.Count - 1,
				this.def.stages.Count - 1
			}));
		}

		// Token: 0x06006148 RID: 24904 RVA: 0x002325C8 File Offset: 0x002307C8
		public override string PostProcessDescription(Pawn p, string description)
		{
			string text = base.PostProcessDescription(p, description);
			Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(this.def.hediff, false);
			if (firstHediffOfDef == null || !firstHediffOfDef.Visible)
			{
				return text;
			}
			return text + "\n\n" + "CausedBy".Translate() + ": " + firstHediffOfDef.LabelBase.CapitalizeFirst();
		}
	}
}
