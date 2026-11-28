using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016D4 RID: 5844
	public class RitualOutcomeComp_PawnSkill : RitualOutcomeComp_Quality
	{
		// Token: 0x0600923A RID: 37434 RVA: 0x00356872 File Offset: 0x00354A72
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return this.curve.Evaluate(this.Count(ritual, data));
		}

		// Token: 0x0600923B RID: 37435 RVA: 0x00356887 File Offset: 0x00354A87
		protected float SkillValue(Pawn pawn)
		{
			return (float)pawn.skills.GetSkill(this.skillDef).Level;
		}

		// Token: 0x0600923C RID: 37436 RVA: 0x003568A0 File Offset: 0x00354AA0
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			Pawn pawn = ritual.PawnWithRole(this.roleId);
			if (pawn == null)
			{
				return 0f;
			}
			return this.SkillValue(pawn);
		}

		// Token: 0x0600923D RID: 37437 RVA: 0x003568CC File Offset: 0x00354ACC
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
			float x = this.SkillValue(pawn);
			float num = this.curve.Evaluate(x);
			string str = (num < 0f) ? "" : "+";
			return this.label.CapitalizeFirst().Formatted(pawn.Named("PAWN")) + ": " + "OutcomeBonusDesc_QualitySingleOffset".Translate(str + num.ToStringPercent()) + ".";
		}

		// Token: 0x0600923E RID: 37438 RVA: 0x0035697C File Offset: 0x00354B7C
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			Pawn pawn = assignments.FirstAssignedPawn(this.roleId);
			if (pawn == null)
			{
				return null;
			}
			float x = this.SkillValue(pawn);
			float num = this.curve.Evaluate(x);
			return new QualityFactor
			{
				label = this.label.Formatted(pawn.Named("PAWN")),
				count = x.ToString(),
				qualityChange = ((Math.Abs(num) > float.Epsilon) ? "OutcomeBonusDesc_QualitySingleOffset".Translate(num.ToStringWithSign("0.#%")).Resolve() : " - "),
				positive = (num >= 0f),
				quality = num,
				priority = 0f
			};
		}

		// Token: 0x0600923F RID: 37439 RVA: 0x00356A43 File Offset: 0x00354C43
		public override bool Applies(LordJob_Ritual ritual)
		{
			return true;
		}

		// Token: 0x04005898 RID: 22680
		[NoTranslate]
		public string roleId;

		// Token: 0x04005899 RID: 22681
		public SkillDef skillDef;
	}
}
