using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013D7 RID: 5079
	public class CompAbilityEffect_AnimalRoar : CompAbilityEffect
	{
		// Token: 0x170015A0 RID: 5536
		// (get) Token: 0x06007E63 RID: 32355 RVA: 0x002D299D File Offset: 0x002D0B9D
		public new CompProperties_AbilityAnimalRoar Props
		{
			get
			{
				return (CompProperties_AbilityAnimalRoar)this.props;
			}
		}

		// Token: 0x06007E64 RID: 32356 RVA: 0x002D29AA File Offset: 0x002D0BAA
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			return target.Pawn != null && Rand.Chance(this.Props.chanceFromHearingCurve.Evaluate(target.Pawn.health.capacities.GetLevel(PawnCapacityDefOf.Hearing)));
		}

		// Token: 0x06007E65 RID: 32357 RVA: 0x002D29E7 File Offset: 0x002D0BE7
		public override void PostApplied(List<LocalTargetInfo> targets, Map map)
		{
			Pawn_CallTracker caller = this.parent.pawn.caller;
			if (caller == null)
			{
				return;
			}
			caller.DoCall(true);
		}
	}
}
