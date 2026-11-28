using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200146D RID: 5229
	public abstract class CompAbilityEffect_WithDuration : CompAbilityEffect
	{
		// Token: 0x17001602 RID: 5634
		// (get) Token: 0x06008061 RID: 32865 RVA: 0x002DC874 File Offset: 0x002DAA74
		public new CompProperties_AbilityEffectWithDuration Props
		{
			get
			{
				return (CompProperties_AbilityEffectWithDuration)this.props;
			}
		}

		// Token: 0x06008062 RID: 32866 RVA: 0x002DC884 File Offset: 0x002DAA84
		public float GetDurationSeconds(Pawn target)
		{
			if (this.Props.durationSecondsOverride != FloatRange.Zero)
			{
				return this.Props.durationSecondsOverride.RandomInRange;
			}
			float num = this.parent.def.GetStatValueAbstract(StatDefOf.Ability_Duration, this.parent.pawn);
			if (this.Props.durationMultiplier != null)
			{
				num *= target.GetStatValue(this.Props.durationMultiplier, true, -1);
			}
			return num;
		}
	}
}
