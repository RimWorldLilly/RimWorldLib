using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200140F RID: 5135
	public class CompAbilityEffect_GiveHediffPsychic : CompAbilityEffect_GiveHediff
	{
		// Token: 0x170015BF RID: 5567
		// (get) Token: 0x06007F1B RID: 32539 RVA: 0x002D6D15 File Offset: 0x002D4F15
		public new CompProperties_AbilityGiveHediffPsychic Props
		{
			get
			{
				return (CompProperties_AbilityGiveHediffPsychic)this.props;
			}
		}

		// Token: 0x06007F1C RID: 32540 RVA: 0x002D6D22 File Offset: 0x002D4F22
		protected override bool TryResist(Pawn pawn)
		{
			return Rand.Chance(1f - pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1));
		}
	}
}
