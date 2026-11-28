using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001457 RID: 5207
	public class CompAbilityEffect_StopManhunter : CompAbilityEffect
	{
		// Token: 0x170015EA RID: 5610
		// (get) Token: 0x06007FFA RID: 32762 RVA: 0x002DA5D4 File Offset: 0x002D87D4
		public new CompProperties_StopManhunter Props
		{
			get
			{
				return (CompProperties_StopManhunter)this.props;
			}
		}

		// Token: 0x06007FFB RID: 32763 RVA: 0x002DA5E4 File Offset: 0x002D87E4
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (!ModLister.CheckIdeology("Animal calm"))
			{
				return;
			}
			Pawn pawn = target.Pawn;
			pawn.MentalState.RecoverFromState();
			Messages.Message(this.Props.successMessage.Formatted(this.parent.pawn.Named("INITIATOR"), pawn.Named("RECIPIENT")), new LookTargets(new Pawn[]
			{
				this.parent.pawn,
				pawn
			}), MessageTypeDefOf.PositiveEvent, true);
		}

		// Token: 0x06007FFC RID: 32764 RVA: 0x002DA670 File Offset: 0x002D8870
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn != null && AbilityUtility.ValidateMustBeAnimal(pawn, throwMessages, this.parent) && AbilityUtility.ValidateIsMaddened(pawn, throwMessages, this.parent) && AbilityUtility.ValidateIsAwake(pawn, throwMessages, this.parent);
		}

		// Token: 0x06007FFD RID: 32765 RVA: 0x002DA6BE File Offset: 0x002D88BE
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return AbilityUtility.ValidateIsMaddened(target.Pawn, false, this.parent) && base.CanApplyOn(target, dest);
		}
	}
}
