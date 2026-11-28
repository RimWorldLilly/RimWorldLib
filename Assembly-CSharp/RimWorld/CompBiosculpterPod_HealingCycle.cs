using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001835 RID: 6197
	public abstract class CompBiosculpterPod_HealingCycle : CompBiosculpterPod_Cycle
	{
		// Token: 0x17001A7B RID: 6779
		// (get) Token: 0x06009B33 RID: 39731 RVA: 0x0037F52F File Offset: 0x0037D72F
		public new CompProperties_BiosculpterPod_HealingCycle Props
		{
			get
			{
				return (CompProperties_BiosculpterPod_HealingCycle)this.props;
			}
		}

		// Token: 0x17001A7C RID: 6780
		// (get) Token: 0x06009B34 RID: 39732
		public abstract bool Regenerate { get; }

		// Token: 0x06009B35 RID: 39733 RVA: 0x0037F53C File Offset: 0x0037D73C
		public override string Description(Pawn tunedFor)
		{
			string text = base.Description(tunedFor);
			if (!this.Props.extraRequiredIngredients.NullOrEmpty<ThingDefCountClass>())
			{
				text += "\n\n" + "BiosculpterRequiresIngredients".Translate() + ":\n" + (from x in this.Props.extraRequiredIngredients
				select x.Summary).ToLineList("  - ", true);
			}
			string healingDescriptionForPawn = this.GetHealingDescriptionForPawn(tunedFor);
			if (!healingDescriptionForPawn.NullOrEmpty())
			{
				text = text + "\n\n" + healingDescriptionForPawn;
			}
			return text;
		}

		// Token: 0x06009B36 RID: 39734 RVA: 0x0037F5EC File Offset: 0x0037D7EC
		public string GetHealingDescriptionForPawn(Pawn pawn)
		{
			string result;
			try
			{
				string text = "";
				if (pawn != null)
				{
					foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
					{
						if (this.WillHeal(pawn, hediff))
						{
							this.tmpWillHealHediffs.Add(CompBiosculpterPod_HealingCycle.<GetHealingDescriptionForPawn>g__HediffLabel|7_0(hediff));
						}
						else if (this.CanPotentiallyHeal(pawn, hediff) && !this.tmpCanHealHediffs.Contains(CompBiosculpterPod_HealingCycle.<GetHealingDescriptionForPawn>g__HediffLabel|7_0(hediff)))
						{
							this.tmpCanHealHediffs.Add(CompBiosculpterPod_HealingCycle.<GetHealingDescriptionForPawn>g__HediffLabel|7_0(hediff));
						}
					}
					if (this.tmpCanHealHediffs.Count == 1)
					{
						this.tmpWillHealHediffs.AddRange(this.tmpCanHealHediffs);
						this.tmpCanHealHediffs.Clear();
					}
					string text2 = string.Empty;
					if (this.tmpWillHealHediffs.Any<string>())
					{
						text2 += "HealingCycleWillHeal".Translate() + ":\n" + this.tmpWillHealHediffs.ToLineList("  - ", true);
					}
					if (this.tmpCanHealHediffs.Any<string>())
					{
						if (!text2.NullOrEmpty())
						{
							text2 += "\n\n";
						}
						text2 += "HealingCycleOneWillHeal".Translate() + ":\n" + this.tmpCanHealHediffs.ToLineList("  - ", true);
					}
					if (!text2.NullOrEmpty())
					{
						text += text2;
					}
				}
				result = text;
			}
			finally
			{
				this.tmpWillHealHediffs.Clear();
				this.tmpCanHealHediffs.Clear();
			}
			return result;
		}

		// Token: 0x06009B37 RID: 39735 RVA: 0x0037F7B8 File Offset: 0x0037D9B8
		public override void CycleCompleted(Pawn pawn)
		{
			if (pawn.health == null)
			{
				return;
			}
			this.tmpHediffs.Clear();
			this.tmpHediffs.AddRange(pawn.health.hediffSet.hediffs);
			try
			{
				foreach (Hediff hediff in this.tmpHediffs)
				{
					if (this.WillHeal(pawn, hediff))
					{
						HealthUtility.Cure(hediff);
					}
					else
					{
						Hediff_MissingPart hediff_MissingPart = hediff as Hediff_MissingPart;
						if (hediff_MissingPart != null && hediff_MissingPart.IsFresh)
						{
							hediff_MissingPart.IsFresh = false;
							pawn.health.Notify_HediffChanged(hediff_MissingPart);
						}
					}
				}
				this.tmpHediffs.Clear();
				foreach (Hediff hediff2 in pawn.health.hediffSet.hediffs)
				{
					if (this.CanPotentiallyHeal(pawn, hediff2))
					{
						this.tmpHediffs.Add(hediff2);
					}
				}
				Hediff hediff3;
				if (this.tmpHediffs.TryRandomElement(out hediff3))
				{
					HealthUtility.Cure(hediff3);
					Messages.Message("BiosculpterHealCompletedWithCureMessage".Translate(pawn.Named("PAWN"), hediff3.Named("HEDIFF")), pawn, MessageTypeDefOf.PositiveEvent, true);
				}
				else
				{
					Messages.Message("BiosculpterHealCompletedMessage".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent, true);
				}
			}
			finally
			{
				this.tmpHediffs.Clear();
			}
		}

		// Token: 0x06009B38 RID: 39736 RVA: 0x0037F98C File Offset: 0x0037DB8C
		private bool WillHeal(Pawn pawn, Hediff hediff)
		{
			if (!hediff.def.everCurableByItem)
			{
				return false;
			}
			if (hediff.def.chronic)
			{
				return false;
			}
			if (hediff.def.countsAsAddedPartOrImplant)
			{
				return false;
			}
			if (hediff.def == HediffDefOf.BloodLoss)
			{
				return true;
			}
			if (this.Regenerate && hediff.Part != null && this.Props.bodyPartsToRestore != null && this.Props.bodyPartsToRestore.Contains(hediff.Part.def) && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(hediff.Part))
			{
				return hediff.Part.def != BodyPartDefOf.Eye || pawn.Ideo == null || !pawn.Ideo.IdeoApprovesOfBlindness();
			}
			return hediff is Hediff_Injury && !hediff.IsPermanent();
		}

		// Token: 0x06009B39 RID: 39737 RVA: 0x0037FA68 File Offset: 0x0037DC68
		private bool CanPotentiallyHeal(Pawn pawn, Hediff hediff)
		{
			return hediff.def.everCurableByItem && !hediff.def.countsAsAddedPartOrImplant && ((this.Props.conditionsToPossiblyCure != null && this.Props.conditionsToPossiblyCure.Contains(hediff.def)) || (this.Regenerate && hediff is Hediff_Injury && hediff.IsPermanent()));
		}

		// Token: 0x06009B3B RID: 39739 RVA: 0x0037FAFE File Offset: 0x0037DCFE
		[CompilerGenerated]
		internal static string <GetHealingDescriptionForPawn>g__HediffLabel|7_0(Hediff hediff)
		{
			if (hediff.Part != null && !hediff.def.cureAllAtOnceIfCuredByItem)
			{
				return hediff.Part.Label + " (" + hediff.Label + ")";
			}
			return hediff.Label;
		}

		// Token: 0x04005C4D RID: 23629
		private List<string> tmpWillHealHediffs = new List<string>();

		// Token: 0x04005C4E RID: 23630
		private List<string> tmpCanHealHediffs = new List<string>();

		// Token: 0x04005C4F RID: 23631
		private List<Hediff> tmpHediffs = new List<Hediff>();
	}
}
