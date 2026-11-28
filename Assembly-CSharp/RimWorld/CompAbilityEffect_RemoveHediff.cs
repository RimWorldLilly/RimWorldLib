using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001437 RID: 5175
	public class CompAbilityEffect_RemoveHediff : CompAbilityEffect
	{
		// Token: 0x170015D4 RID: 5588
		// (get) Token: 0x06007F90 RID: 32656 RVA: 0x002D8BA9 File Offset: 0x002D6DA9
		public new CompProperties_AbilityRemoveHediff Props
		{
			get
			{
				return (CompProperties_AbilityRemoveHediff)this.props;
			}
		}

		// Token: 0x06007F91 RID: 32657 RVA: 0x002D8BB8 File Offset: 0x002D6DB8
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			if (this.Props.applyToSelf)
			{
				this.RemoveHediff(this.parent.pawn);
			}
			if (target.Pawn != null && this.Props.applyToTarget && target.Pawn != this.parent.pawn)
			{
				this.RemoveHediff(target.Pawn);
			}
		}

		// Token: 0x06007F92 RID: 32658 RVA: 0x002D8C24 File Offset: 0x002D6E24
		private void RemoveHediff(Pawn pawn)
		{
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(this.Props.hediffDef, false);
			if (firstHediffOfDef != null)
			{
				pawn.health.RemoveHediff(firstHediffOfDef);
			}
		}
	}
}
