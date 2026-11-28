using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001460 RID: 5216
	public class CompAbilityEffect_TransferEntropy : CompAbilityEffect
	{
		// Token: 0x170015F2 RID: 5618
		// (get) Token: 0x06008021 RID: 32801 RVA: 0x002DB25E File Offset: 0x002D945E
		public new CompProperties_AbilityTransferEntropy Props
		{
			get
			{
				return (CompProperties_AbilityTransferEntropy)this.props;
			}
		}

		// Token: 0x06008022 RID: 32802 RVA: 0x002DB26C File Offset: 0x002D946C
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				Pawn pawn2 = this.parent.pawn;
				if (this.Props.targetReceivesEntropy)
				{
					pawn.psychicEntropy.TryAddEntropy(pawn2.psychicEntropy.EntropyValue, pawn2, false, true);
				}
				pawn2.psychicEntropy.RemoveAllEntropy();
				MoteMaker.MakeInteractionOverlay(ThingDefOf.Mote_PsychicLinkPulse, this.parent.pawn, pawn);
				return;
			}
			Log.Error("CompAbilityEffect_TransferEntropy is only applicable to pawns.");
		}

		// Token: 0x06008023 RID: 32803 RVA: 0x002DB2F6 File Offset: 0x002D94F6
		public override bool GizmoDisabled(out string reason)
		{
			if (this.parent.pawn.psychicEntropy.EntropyValue <= 0f)
			{
				reason = "AbilityNoEntropyToDump".Translate();
				return true;
			}
			return base.GizmoDisabled(out reason);
		}

		// Token: 0x06008024 RID: 32804 RVA: 0x002DB330 File Offset: 0x002D9530
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn == null || AbilityUtility.ValidateNoMentalState(pawn, throwMessages, this.parent);
		}
	}
}
