using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001441 RID: 5185
	public class CompAbilityEffect_ShowTargetIdeoCertainty : CompAbilityEffect
	{
		// Token: 0x170015DC RID: 5596
		// (get) Token: 0x06007FB8 RID: 32696 RVA: 0x002D938D File Offset: 0x002D758D
		public override bool HideTargetPawnTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007FB9 RID: 32697 RVA: 0x002D9390 File Offset: 0x002D7590
		public override string ExtraLabelMouseAttachment(LocalTargetInfo target)
		{
			if (target.Pawn != null)
			{
				Pawn pawn = target.Pawn;
				if (pawn.Ideo != null)
				{
					return "IdeoCertaintyTooltip".Translate(pawn.Named("PAWN"), pawn.Ideo.Named("IDEO"), pawn.ideo.Certainty.ToStringPercent().Named("PERCENTAGE"));
				}
			}
			return null;
		}
	}
}
