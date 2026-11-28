using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001416 RID: 5142
	public class CompAbilityEffect_GiveRandomHediff : CompAbilityEffect
	{
		// Token: 0x170015C1 RID: 5569
		// (get) Token: 0x06007F2B RID: 32555 RVA: 0x002D710F File Offset: 0x002D530F
		public new CompProperties_AbilityGiveRandomHediff Props
		{
			get
			{
				return (CompProperties_AbilityGiveRandomHediff)this.props;
			}
		}

		// Token: 0x06007F2C RID: 32556 RVA: 0x002D711C File Offset: 0x002D531C
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			HediffOption hediffOption = this.GetApplicableHediffs(target.Pawn).RandomElement<HediffOption>();
			BodyPartRecord partRecord = this.GetAcceptablePartsForHediff(target.Pawn, hediffOption).RandomElement<BodyPartRecord>();
			Hediff hediff = HediffMaker.MakeHediff(hediffOption.hediffDef, target.Pawn, partRecord);
			target.Pawn.health.AddHediff(hediff, null, null, null);
			if (base.SendLetter)
			{
				Find.LetterStack.ReceiveLetter(this.Props.customLetterLabel.Formatted(hediff.def.LabelCap), this.Props.customLetterText.Formatted(this.parent.pawn, target.Pawn, hediff.def.label), LetterDefOf.PositiveEvent, new LookTargets(target.Pawn), null, null, null, null, 0, true);
			}
		}

		// Token: 0x06007F2D RID: 32557 RVA: 0x002D720E File Offset: 0x002D540E
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return target.Pawn != null && this.GetApplicableHediffs(target.Pawn).Any<HediffOption>() && base.CanApplyOn(target, dest);
		}

		// Token: 0x06007F2E RID: 32558 RVA: 0x002D723C File Offset: 0x002D543C
		private List<HediffOption> GetApplicableHediffs(Pawn target)
		{
			List<HediffOption> list = new List<HediffOption>();
			foreach (HediffOption hediffOption in this.Props.options)
			{
				if (!this.GetAcceptablePartsForHediff(target, hediffOption).EnumerableNullOrEmpty<BodyPartRecord>())
				{
					list.Add(hediffOption);
				}
			}
			return list;
		}

		// Token: 0x06007F2F RID: 32559 RVA: 0x002D72AC File Offset: 0x002D54AC
		private IEnumerable<BodyPartRecord> GetAcceptablePartsForHediff(Pawn target, HediffOption option)
		{
			if (!this.Props.allowDuplicates && (from x in target.health.hediffSet.hediffs
			where x.def == option.hediffDef
			select x).Any<Hediff>())
			{
				return null;
			}
			return from p in target.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where (option.bodyPart == null || p.def == option.bodyPart) && !target.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(p)
			select p;
		}
	}
}
