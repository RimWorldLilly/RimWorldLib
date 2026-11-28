using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001431 RID: 5169
	public class CompAbilityEffect_Reassure : CompAbilityEffect
	{
		// Token: 0x170015CF RID: 5583
		// (get) Token: 0x06007F7A RID: 32634 RVA: 0x002D852D File Offset: 0x002D672D
		public new CompProperties_AbilityReassure Props
		{
			get
			{
				return (CompProperties_AbilityReassure)this.props;
			}
		}

		// Token: 0x170015D0 RID: 5584
		// (get) Token: 0x06007F7B RID: 32635 RVA: 0x002D853A File Offset: 0x002D673A
		public override bool HideTargetPawnTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007F7C RID: 32636 RVA: 0x002D8540 File Offset: 0x002D6740
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn != null && AbilityUtility.ValidateMustBeHuman(pawn, throwMessages, this.parent) && AbilityUtility.ValidateNoMentalState(pawn, throwMessages, this.parent) && AbilityUtility.ValidateSameIdeo(this.parent.pawn, pawn, throwMessages, this.parent);
		}

		// Token: 0x06007F7D RID: 32637 RVA: 0x002D8599 File Offset: 0x002D6799
		private float CertaintyGain(Pawn initiator, Pawn recipient)
		{
			return this.Props.baseCertaintyGain * initiator.GetStatValue(StatDefOf.NegotiationAbility, true, -1);
		}

		// Token: 0x06007F7E RID: 32638 RVA: 0x002D85B4 File Offset: 0x002D67B4
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (!ModLister.CheckIdeology("Ideoligion certainty"))
			{
				return;
			}
			Pawn pawn = this.parent.pawn;
			Pawn pawn2 = target.Pawn;
			float certaintyGain = this.CertaintyGain(pawn, pawn2);
			float certainty = pawn2.ideo.Certainty;
			pawn2.ideo.Reassure(certaintyGain);
			Messages.Message(this.Props.successMessage.Formatted(pawn.Named("INITIATOR"), pawn2.Named("RECIPIENT"), certainty.ToStringPercent().Named("BEFORECERTAINTY"), pawn2.ideo.Certainty.ToStringPercent().Named("AFTERCERTAINTY"), pawn.Ideo.name.Named("IDEO")), new LookTargets(new Pawn[]
			{
				pawn,
				pawn2
			}), MessageTypeDefOf.PositiveEvent, true);
			PlayLogEntry_Interaction entry = new PlayLogEntry_Interaction(InteractionDefOf.Reassure, this.parent.pawn, pawn2, null);
			Find.PlayLog.Add(entry);
			if (this.Props.sound != null)
			{
				this.Props.sound.PlayOneShot(new TargetInfo(target.Cell, this.parent.pawn.Map, false));
			}
		}
	}
}
