using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013D9 RID: 5081
	public class CompAbilityEffect_BloodfeederBite : CompAbilityEffect
	{
		// Token: 0x170015A1 RID: 5537
		// (get) Token: 0x06007E69 RID: 32361 RVA: 0x002D2A4A File Offset: 0x002D0C4A
		public new CompProperties_AbilityBloodfeederBite Props
		{
			get
			{
				return (CompProperties_AbilityBloodfeederBite)this.props;
			}
		}

		// Token: 0x06007E6A RID: 32362 RVA: 0x002D2A58 File Offset: 0x002D0C58
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			if (pawn == null)
			{
				return;
			}
			SanguophageUtility.DoBite(this.parent.pawn, pawn, this.Props.hemogenGain, this.Props.nutritionGain, this.Props.targetBloodLoss, this.Props.resistanceGain, this.Props.bloodFilthToSpawnRange, this.Props.thoughtDefToGiveTarget, this.Props.opinionThoughtDefToGiveTarget);
		}

		// Token: 0x06007E6B RID: 32363 RVA: 0x002D2AD7 File Offset: 0x002D0CD7
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Valid(target, false);
		}

		// Token: 0x06007E6C RID: 32364 RVA: 0x002D2AE4 File Offset: 0x002D0CE4
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn == null)
			{
				return false;
			}
			if (!AbilityUtility.ValidateMustBeHumanOrWildMan(pawn, throwMessages, this.parent))
			{
				return false;
			}
			if (pawn.Faction != null && !pawn.IsSlaveOfColony && !pawn.IsPrisonerOfColony)
			{
				if (pawn.Faction.HostileTo(this.parent.pawn.Faction))
				{
					if (!pawn.Downed)
					{
						if (throwMessages)
						{
							Messages.Message("MessageCantUseOnResistingPerson".Translate(this.parent.def.Named("ABILITY")), pawn, MessageTypeDefOf.RejectInput, false);
						}
						return false;
					}
				}
				else if (pawn.IsQuestLodger() || pawn.Faction != this.parent.pawn.Faction)
				{
					if (throwMessages)
					{
						Messages.Message("MessageCannotUseOnOtherFactions".Translate(this.parent.def.Named("ABILITY")), pawn, MessageTypeDefOf.RejectInput, false);
					}
					return false;
				}
			}
			if (pawn.IsWildMan() && !pawn.IsPrisonerOfColony && !pawn.Downed)
			{
				if (throwMessages)
				{
					Messages.Message("MessageCantUseOnResistingPerson".Translate(this.parent.def.Named("ABILITY")), pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (pawn.InMentalState || PrisonBreakUtility.IsPrisonBreaking(pawn))
			{
				if (throwMessages)
				{
					Messages.Message("MessageCantUseOnResistingPerson".Translate(this.parent.def.Named("ABILITY")), pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (ModsConfig.AnomalyActive && pawn.IsMutant && !pawn.mutant.Def.canBleed)
			{
				if (throwMessages)
				{
					Messages.Message("MessageCannotUseOnNonBleeder".Translate(this.parent.def.Named("ABILITY")), pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06007E6D RID: 32365 RVA: 0x002D2CE4 File Offset: 0x002D0EE4
		public override string ExtraLabelMouseAttachment(LocalTargetInfo target)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				string text = null;
				if (pawn.HostileTo(this.parent.pawn) && !pawn.Downed)
				{
					text += "MessageCantUseOnResistingPerson".Translate(this.parent.def.Named("ABILITY"));
				}
				float num = this.BloodlossAfterBite(pawn);
				if (num >= HediffDefOf.BloodLoss.lethalSeverity)
				{
					if (!text.NullOrEmpty())
					{
						text += "\n";
					}
					text += "WillKill".Translate();
				}
				else if (HediffDefOf.BloodLoss.stages[HediffDefOf.BloodLoss.StageAtSeverity(num)].lifeThreatening)
				{
					if (!text.NullOrEmpty())
					{
						text += "\n";
					}
					text += "WillCauseSeriousBloodloss".Translate();
				}
				return text;
			}
			return base.ExtraLabelMouseAttachment(target);
		}

		// Token: 0x06007E6E RID: 32366 RVA: 0x002D2DDC File Offset: 0x002D0FDC
		public override Window ConfirmationDialog(LocalTargetInfo target, Action confirmAction)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (pawn.genes != null && pawn.genes.HasActiveGene(GeneDefOf.Deathless))
				{
					return null;
				}
				float num = this.BloodlossAfterBite(pawn);
				if (num >= HediffDefOf.BloodLoss.lethalSeverity)
				{
					return Dialog_MessageBox.CreateConfirmation("WarningPawnWillDieFromBloodfeeding".Translate(pawn.Named("PAWN")), confirmAction, true, null, WindowLayer.Dialog);
				}
				if (HediffDefOf.BloodLoss.stages[HediffDefOf.BloodLoss.StageAtSeverity(num)].lifeThreatening)
				{
					return Dialog_MessageBox.CreateConfirmation("WarningPawnWillHaveSeriousBloodlossFromBloodfeeding".Translate(pawn.Named("PAWN")), confirmAction, true, null, WindowLayer.Dialog);
				}
			}
			return null;
		}

		// Token: 0x06007E6F RID: 32367 RVA: 0x002D2E88 File Offset: 0x002D1088
		private float BloodlossAfterBite(Pawn target)
		{
			if (target.Dead || !target.RaceProps.IsFlesh)
			{
				return 0f;
			}
			float num = this.Props.targetBloodLoss;
			Hediff firstHediffOfDef = target.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss, false);
			if (firstHediffOfDef != null)
			{
				num += firstHediffOfDef.Severity;
			}
			return num;
		}
	}
}
