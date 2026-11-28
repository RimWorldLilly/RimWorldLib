using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020016CC RID: 5836
	public class RitualOutcomeComp_LovedOnePresent : RitualOutcomeComp_Quality
	{
		// Token: 0x0600920D RID: 37389 RVA: 0x00355948 File Offset: 0x00353B48
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			Pawn pawn = this.LovedOneParticipating(assignments);
			bool flag = pawn != null;
			float quality = flag ? this.qualityOffset : 0f;
			return new QualityFactor
			{
				label = "RitualOutcomeLovedOnePresentLabel".Translate(),
				present = flag,
				qualityChange = this.ExpectedOffsetDesc(true, quality),
				quality = quality,
				positive = true,
				priority = 4f,
				count = ((pawn != null) ? pawn.LabelShort : null),
				toolTip = "RitualOutcomeLovedOnePresentTip".Translate(assignments.FirstAssignedPawn(this.lovedOneOfRole).Named("PAWN"))
			};
		}

		// Token: 0x0600920E RID: 37390 RVA: 0x003559FC File Offset: 0x00353BFC
		private Pawn LovedOneParticipating(RitualRoleAssignments assignments)
		{
			Pawn lovedOnOf = assignments.FirstAssignedPawn(this.lovedOneOfRole);
			if (lovedOnOf == null)
			{
				return null;
			}
			RitualOutcomeComp_LovedOnePresent.lovedOnesTmp.Clear();
			RitualOutcomeComp_LovedOnePresent.lovedOnesTmp.AddRange(from p in SocialCardUtility.PawnsForSocialInfo(lovedOnOf)
			where assignments.PawnParticipating(p) && lovedOnOf.relations.OpinionOf(p) >= 20
			select p);
			if (RitualOutcomeComp_LovedOnePresent.lovedOnesTmp.Count > 0)
			{
				RitualOutcomeComp_LovedOnePresent.lovedOnesTmp.SortByDescending((Pawn l) => lovedOnOf.relations.OpinionOf(l));
				return RitualOutcomeComp_LovedOnePresent.lovedOnesTmp.First<Pawn>();
			}
			return null;
		}

		// Token: 0x0600920F RID: 37391 RVA: 0x00355A98 File Offset: 0x00353C98
		public override string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
		{
			string str = (this.qualityOffset < 0f) ? "" : "+";
			return ((this.LovedOneParticipating(ritual.assignments) != null) ? this.label : this.labelNotMet).CapitalizeFirst().Formatted(Array.Empty<NamedArgument>()) + ": " + "OutcomeBonusDesc_QualitySingleOffset".Translate(str + this.qualityOffset.ToStringPercent()) + ".";
		}

		// Token: 0x06009210 RID: 37392 RVA: 0x00355B28 File Offset: 0x00353D28
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			if (this.LovedOneParticipating(ritual.assignments) == null)
			{
				return 0f;
			}
			return this.qualityOffset;
		}

		// Token: 0x06009211 RID: 37393 RVA: 0x00355B44 File Offset: 0x00353D44
		protected override string ExpectedOffsetDesc(bool positive, float quality = 0f)
		{
			if (!positive)
			{
				return "";
			}
			return quality.ToStringWithSign("0.#%");
		}

		// Token: 0x06009212 RID: 37394 RVA: 0x00355B5A File Offset: 0x00353D5A
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return this.LovedOneParticipating(ritual.assignments) != null;
		}

		// Token: 0x04005889 RID: 22665
		[MustTranslate]
		public string labelNotMet;

		// Token: 0x0400588A RID: 22666
		[NoTranslate]
		public string lovedOneOfRole;

		// Token: 0x0400588B RID: 22667
		private static List<Pawn> lovedOnesTmp = new List<Pawn>();
	}
}
