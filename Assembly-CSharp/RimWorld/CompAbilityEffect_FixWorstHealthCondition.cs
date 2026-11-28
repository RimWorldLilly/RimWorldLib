using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001403 RID: 5123
	public class CompAbilityEffect_FixWorstHealthCondition : CompAbilityEffect
	{
		// Token: 0x170015BA RID: 5562
		// (get) Token: 0x06007EF9 RID: 32505 RVA: 0x002D638B File Offset: 0x002D458B
		public new CompProperties_AbilityFixWorstHealthCondition Props
		{
			get
			{
				return (CompProperties_AbilityFixWorstHealthCondition)this.props;
			}
		}

		// Token: 0x06007EFA RID: 32506 RVA: 0x002D6398 File Offset: 0x002D4598
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (target.Pawn == null)
			{
				return false;
			}
			return target.Pawn.health.hediffSet.hediffs.Any((Hediff x) => x.def.isBad && x.def.everCurableByItem && x.Visible) && base.CanApplyOn(target, dest);
		}

		// Token: 0x06007EFB RID: 32507 RVA: 0x002D63F8 File Offset: 0x002D45F8
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			TaggedString value = HealthUtility.FixWorstHealthCondition(target.Pawn, Array.Empty<HediffDef>());
			if (base.SendLetter)
			{
				Find.LetterStack.ReceiveLetter(this.Props.customLetterLabel, this.Props.customLetterText.Formatted(this.parent.pawn, target.Pawn, value), LetterDefOf.PositiveEvent, new LookTargets(target.Pawn), null, null, null, null, 0, true);
			}
		}
	}
}
