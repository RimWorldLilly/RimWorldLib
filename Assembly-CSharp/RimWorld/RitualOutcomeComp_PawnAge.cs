using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016D3 RID: 5843
	public class RitualOutcomeComp_PawnAge : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x06009234 RID: 37428 RVA: 0x003566C8 File Offset: 0x003548C8
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return this.curve.Evaluate(this.Count(ritual, data));
		}

		// Token: 0x06009235 RID: 37429 RVA: 0x003566E0 File Offset: 0x003548E0
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			Pawn pawn = ritual.PawnWithRole(this.roleId);
			if (pawn != null)
			{
				return pawn.ageTracker.AgeBiologicalYearsFloat;
			}
			return 0f;
		}

		// Token: 0x06009236 RID: 37430 RVA: 0x00356710 File Offset: 0x00354910
		public override string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
		{
			if (ritual == null)
			{
				return this.labelAbstract;
			}
			Pawn pawn = (ritual != null) ? ritual.PawnWithRole(this.roleId) : null;
			if (pawn == null)
			{
				return null;
			}
			float num = this.curve.Evaluate(pawn.ageTracker.AgeBiologicalYearsFloat);
			string str = (num < 0f) ? "" : "+";
			return this.LabelForDesc.Formatted(pawn.Named("PAWN")) + ": " + "OutcomeBonusDesc_QualitySingleOffset".Translate(str + num.ToStringPercent()) + ".";
		}

		// Token: 0x06009237 RID: 37431 RVA: 0x003567BC File Offset: 0x003549BC
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			Pawn pawn = assignments.FirstAssignedPawn(this.roleId);
			if (pawn == null)
			{
				return null;
			}
			float num = this.curve.Evaluate(pawn.ageTracker.AgeBiologicalYearsFloat);
			return new QualityFactor
			{
				label = this.label.Formatted(pawn.Named("PAWN")),
				count = pawn.ageTracker.AgeBiologicalYears.ToString(),
				qualityChange = this.ExpectedOffsetDesc(num > 0f, num),
				positive = (num > 0f),
				quality = num,
				priority = 0f
			};
		}

		// Token: 0x06009238 RID: 37432 RVA: 0x00356867 File Offset: 0x00354A67
		public override bool Applies(LordJob_Ritual ritual)
		{
			return true;
		}

		// Token: 0x04005897 RID: 22679
		[NoTranslate]
		public string roleId;
	}
}
