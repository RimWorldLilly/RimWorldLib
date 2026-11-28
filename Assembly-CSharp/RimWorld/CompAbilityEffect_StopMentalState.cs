using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001459 RID: 5209
	public class CompAbilityEffect_StopMentalState : CompAbilityEffect
	{
		// Token: 0x170015EE RID: 5614
		// (get) Token: 0x06008005 RID: 32773 RVA: 0x002DA854 File Offset: 0x002D8A54
		public new CompProperties_AbilityStopMentalState Props
		{
			get
			{
				return (CompProperties_AbilityStopMentalState)this.props;
			}
		}

		// Token: 0x170015EF RID: 5615
		// (get) Token: 0x06008006 RID: 32774 RVA: 0x002DA861 File Offset: 0x002D8A61
		public override bool HideTargetPawnTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008007 RID: 32775 RVA: 0x002DA864 File Offset: 0x002D8A64
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.CatatonicBreakdown, false);
			if (firstHediffOfDef != null)
			{
				pawn.health.RemoveHediff(firstHediffOfDef);
			}
			if (pawn != null)
			{
				MentalState mentalState = pawn.MentalState;
				if (mentalState == null)
				{
					return;
				}
				mentalState.RecoverFromState();
			}
		}

		// Token: 0x06008008 RID: 32776 RVA: 0x002DA8BA File Offset: 0x002D8ABA
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Valid(target, false);
		}

		// Token: 0x06008009 RID: 32777 RVA: 0x002DA8C4 File Offset: 0x002D8AC4
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (!AbilityUtility.ValidateHasMentalState(pawn, throwMessages, this.parent))
				{
					return false;
				}
				if (pawn.MentalStateDef != null && this.Props.exceptions.Contains(pawn.MentalStateDef))
				{
					if (throwMessages)
					{
						Messages.Message("AbilityDoesntWorkOnMentalState".Translate(this.parent.def.label, pawn.MentalStateDef.label), pawn, MessageTypeDefOf.RejectInput, false);
					}
					return false;
				}
				float num = this.PsyfocusCostForTarget(target);
				if (num > this.parent.pawn.psychicEntropy.CurrentPsyfocus + 0.0005f)
				{
					Pawn pawn2 = this.parent.pawn;
					if (throwMessages)
					{
						TaggedString value = ("MentalBreakIntensity" + this.TargetMentalBreakIntensity(target).ToString()).Translate();
						Messages.Message("CommandPsycastNotEnoughPsyfocusForMentalBreak".Translate(num.ToStringPercent(), value, pawn2.psychicEntropy.CurrentPsyfocus.ToStringPercent("0.#"), this.parent.def.label.Named("PSYCASTNAME"), pawn2.Named("CASTERNAME")), pawn, MessageTypeDefOf.RejectInput, false);
					}
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600800A RID: 32778 RVA: 0x002DAA30 File Offset: 0x002D8C30
		public MentalBreakIntensity TargetMentalBreakIntensity(LocalTargetInfo target)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				MentalStateDef mentalStateDef = pawn.MentalStateDef;
				if (mentalStateDef != null)
				{
					List<MentalBreakDef> allDefsListForReading = DefDatabase<MentalBreakDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (allDefsListForReading[i].mentalState == mentalStateDef)
						{
							return allDefsListForReading[i].intensity;
						}
					}
				}
				else if (pawn.health.hediffSet.HasHediff(HediffDefOf.CatatonicBreakdown, false))
				{
					return MentalBreakIntensity.Extreme;
				}
			}
			return MentalBreakIntensity.Minor;
		}

		// Token: 0x0600800B RID: 32779 RVA: 0x002DAAA4 File Offset: 0x002D8CA4
		public override float PsyfocusCostForTarget(LocalTargetInfo target)
		{
			switch (this.TargetMentalBreakIntensity(target))
			{
			case MentalBreakIntensity.Minor:
				return this.Props.psyfocusCostForMinor;
			case MentalBreakIntensity.Major:
				return this.Props.psyfocusCostForMajor;
			case MentalBreakIntensity.Extreme:
				return this.Props.psyfocusCostForExtreme;
			default:
				return 0f;
			}
		}

		// Token: 0x0600800C RID: 32780 RVA: 0x002DAAF8 File Offset: 0x002D8CF8
		public override string ExtraLabelMouseAttachment(LocalTargetInfo target)
		{
			if (target.Pawn != null && this.Valid(target, false))
			{
				return "AbilityPsyfocusCost".Translate() + ": " + this.PsyfocusCostForTarget(target).ToStringPercent("0.#");
			}
			return null;
		}
	}
}
