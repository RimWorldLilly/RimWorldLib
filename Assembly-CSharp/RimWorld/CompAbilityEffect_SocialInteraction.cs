using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001447 RID: 5191
	public class CompAbilityEffect_SocialInteraction : CompAbilityEffect
	{
		// Token: 0x170015E0 RID: 5600
		// (get) Token: 0x06007FCB RID: 32715 RVA: 0x002D984A File Offset: 0x002D7A4A
		public new CompProperties_AbilitySocialInteraction Props
		{
			get
			{
				return (CompProperties_AbilitySocialInteraction)this.props;
			}
		}

		// Token: 0x06007FCC RID: 32716 RVA: 0x002D9858 File Offset: 0x002D7A58
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			if (pawn != null && this.parent.pawn != pawn)
			{
				Pawn_InteractionsTracker interactions = this.parent.pawn.interactions;
				if (interactions == null)
				{
					return;
				}
				interactions.TryInteractWith(pawn, this.Props.interactionDef);
			}
		}

		// Token: 0x06007FCD RID: 32717 RVA: 0x002D98AD File Offset: 0x002D7AAD
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Valid(target, false);
		}

		// Token: 0x06007FCE RID: 32718 RVA: 0x002D98B8 File Offset: 0x002D7AB8
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (!this.Props.canApplyToMentallyBroken && !AbilityUtility.ValidateNoMentalState(pawn, throwMessages, this.parent))
				{
					return false;
				}
				if (!this.Props.canApplyToUnconscious && !AbilityUtility.ValidateIsConscious(pawn, throwMessages, this.parent))
				{
					return false;
				}
			}
			return true;
		}
	}
}
