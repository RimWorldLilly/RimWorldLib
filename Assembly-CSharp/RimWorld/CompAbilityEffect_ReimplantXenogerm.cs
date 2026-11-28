using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001433 RID: 5171
	public class CompAbilityEffect_ReimplantXenogerm : CompAbilityEffect
	{
		// Token: 0x170015D1 RID: 5585
		// (get) Token: 0x06007F81 RID: 32641 RVA: 0x002D870F File Offset: 0x002D690F
		private new CompProperties_AbilityReimplantXenogerm Props
		{
			get
			{
				return (CompProperties_AbilityReimplantXenogerm)this.props;
			}
		}

		// Token: 0x06007F82 RID: 32642 RVA: 0x002D871C File Offset: 0x002D691C
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (!ModLister.CheckBiotech("xenogerm reimplantation"))
			{
				return;
			}
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			if (pawn == null)
			{
				return;
			}
			GeneUtility.ReimplantXenogerm(this.parent.pawn, pawn);
			FleckMaker.AttachedOverlay(pawn, FleckDefOf.FlashHollow, new Vector3(0f, 0f, 0.26f), 1f, -1f);
			if (PawnUtility.ShouldSendNotificationAbout(this.parent.pawn) || PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				int max = HediffDefOf.XenogerminationComa.CompProps<HediffCompProperties_Disappears>().disappearsAfterTicks.max;
				int max2 = HediffDefOf.XenogermLossShock.CompProps<HediffCompProperties_Disappears>().disappearsAfterTicks.max;
				Find.LetterStack.ReceiveLetter("LetterLabelGenesImplanted".Translate(), "LetterTextGenesImplanted".Translate(this.parent.pawn.Named("CASTER"), pawn.Named("TARGET"), max.ToStringTicksToPeriod(true, false, true, true, false).Named("COMADURATION"), max2.ToStringTicksToPeriod(true, false, true, true, false).Named("SHOCKDURATION")), LetterDefOf.NeutralEvent, new LookTargets(new TargetInfo[]
				{
					this.parent.pawn,
					pawn
				}), null, null, null, null, 0, true);
			}
		}

		// Token: 0x06007F83 RID: 32643 RVA: 0x002D8870 File Offset: 0x002D6A70
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn == null)
			{
				return base.Valid(target, throwMessages);
			}
			if (pawn.IsQuestLodger())
			{
				if (throwMessages)
				{
					Messages.Message("MessageCannotImplantInTempFactionMembers".Translate(), pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (pawn.HostileTo(this.parent.pawn) && !pawn.Downed)
			{
				if (throwMessages)
				{
					Messages.Message("MessageCantUseOnResistingPerson".Translate(this.parent.def.Named("ABILITY")), pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (!this.parent.pawn.genes.Xenogenes.Any<Gene>())
			{
				if (throwMessages)
				{
					Messages.Message("MessagePawnHasNoXenogenes".Translate(this.parent.pawn), this.parent.pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (GeneUtility.SameXenotype(pawn, this.parent.pawn))
			{
				if (throwMessages)
				{
					Messages.Message("MessageCannotUseOnSameXenotype".Translate(pawn), pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (!CompAbilityEffect_ReimplantXenogerm.PawnIdeoCanAcceptReimplant(this.parent.pawn, pawn))
			{
				if (throwMessages)
				{
					Messages.Message("MessageCannotBecomeNonPreferredXenotype".Translate(pawn), pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return base.Valid(target, throwMessages);
		}

		// Token: 0x06007F84 RID: 32644 RVA: 0x002D89F2 File Offset: 0x002D6BF2
		public override Window ConfirmationDialog(LocalTargetInfo target, Action confirmAction)
		{
			if (GeneUtility.PawnWouldDieFromReimplanting(this.parent.pawn))
			{
				return Dialog_MessageBox.CreateConfirmation("WarningPawnWillDieFromReimplanting".Translate(this.parent.pawn.Named("PAWN")), confirmAction, true, null, WindowLayer.Dialog);
			}
			return null;
		}

		// Token: 0x06007F85 RID: 32645 RVA: 0x002D8A30 File Offset: 0x002D6C30
		public override IEnumerable<Mote> CustomWarmupMotes(LocalTargetInfo target)
		{
			CompAbilityEffect_ReimplantXenogerm.<CustomWarmupMotes>d__5 <CustomWarmupMotes>d__ = new CompAbilityEffect_ReimplantXenogerm.<CustomWarmupMotes>d__5(-2);
			<CustomWarmupMotes>d__.<>3__target = target;
			return <CustomWarmupMotes>d__;
		}

		// Token: 0x06007F86 RID: 32646 RVA: 0x002D8A40 File Offset: 0x002D6C40
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompAbilityEffect_ReimplantXenogerm.<CompGetGizmosExtra>d__7 <CompGetGizmosExtra>d__ = new CompAbilityEffect_ReimplantXenogerm.<CompGetGizmosExtra>d__7(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06007F87 RID: 32647 RVA: 0x002D8A50 File Offset: 0x002D6C50
		public static bool PawnIdeoCanAcceptReimplant(Pawn implanter, Pawn implantee)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return true;
			}
			if (!IdeoUtility.DoerWillingToDo(HistoryEventDefOf.PropagateBloodfeederGene, implantee))
			{
				if (implanter.genes.Xenogenes.Any((Gene x) => x.def == GeneDefOf.Bloodfeeder))
				{
					return false;
				}
			}
			return IdeoUtility.DoerWillingToDo(HistoryEventDefOf.BecomeNonPreferredXenotype, implantee) || implantee.Ideo.IsPreferredXenotype(implanter);
		}

		// Token: 0x04004FD4 RID: 20436
		private static readonly CachedTexture ReimplantIcon = new CachedTexture("UI/Icons/Genes/Gene_XenogermReimplanter");
	}
}
