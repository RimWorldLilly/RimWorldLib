using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016D2 RID: 5842
	public class RitualOutcomeComp_PawnStatScaled : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x0600922D RID: 37421 RVA: 0x0035647A File Offset: 0x0035467A
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return this.Count(ritual, data);
		}

		// Token: 0x0600922E RID: 37422 RVA: 0x00356484 File Offset: 0x00354684
		protected float StatValue(Pawn pawn)
		{
			if (this.curve == null)
			{
				return pawn.GetStatValue(this.statDef, true, -1);
			}
			return this.curve.Evaluate(pawn.GetStatValue(this.statDef, true, -1));
		}

		// Token: 0x0600922F RID: 37423 RVA: 0x003564B8 File Offset: 0x003546B8
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			Pawn pawn = ritual.PawnWithRole(this.roleId);
			if (pawn == null)
			{
				return 0f;
			}
			if (this.statDef.Worker.IsDisabledFor(pawn))
			{
				return 0f;
			}
			return this.StatValue(pawn) * this.scaledBy;
		}

		// Token: 0x06009230 RID: 37424 RVA: 0x00356504 File Offset: 0x00354704
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
			float num = 0f;
			if (!this.statDef.Worker.IsDisabledFor(pawn))
			{
				num = this.StatValue(pawn);
			}
			float num2 = num * this.scaledBy;
			string str = (num2 < 0f) ? "" : "+";
			return this.LabelForDesc.Formatted(pawn.Named("PAWN")) + ": " + "OutcomeBonusDesc_QualitySingleOffset".Translate(str + num2.ToStringPercent()) + ".";
		}

		// Token: 0x06009231 RID: 37425 RVA: 0x003565C4 File Offset: 0x003547C4
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			Pawn pawn = assignments.FirstAssignedPawn(this.roleId);
			if (pawn == null)
			{
				return null;
			}
			float f = 0f;
			float num = 0f;
			if (!this.statDef.Worker.IsDisabledFor(pawn))
			{
				f = pawn.GetStatValue(this.statDef, true, -1);
				num = this.StatValue(pawn) * this.scaledBy;
			}
			return new QualityFactor
			{
				label = this.label.Formatted(pawn.Named("PAWN")),
				count = f.ToStringPercent(),
				qualityChange = ((Math.Abs(num) > float.Epsilon) ? "OutcomeBonusDesc_QualitySingleOffset".Translate(num.ToStringWithSign("0.#%")).Resolve() : " - "),
				positive = (num >= 0f),
				quality = num,
				priority = 0f
			};
		}

		// Token: 0x06009232 RID: 37426 RVA: 0x003566B2 File Offset: 0x003548B2
		public override bool Applies(LordJob_Ritual ritual)
		{
			return true;
		}

		// Token: 0x04005894 RID: 22676
		[NoTranslate]
		public string roleId;

		// Token: 0x04005895 RID: 22677
		public StatDef statDef;

		// Token: 0x04005896 RID: 22678
		public float scaledBy = 1f;
	}
}
