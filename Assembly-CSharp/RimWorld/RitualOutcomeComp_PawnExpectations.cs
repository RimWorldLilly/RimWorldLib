using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016D6 RID: 5846
	public class RitualOutcomeComp_PawnExpectations : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x1700191F RID: 6431
		// (get) Token: 0x06009244 RID: 37444 RVA: 0x00356AA0 File Offset: 0x00354CA0
		public PawnExpectationsQualityOffset SlaveExpectationCached
		{
			get
			{
				if (this.slaveExpectationCached == null)
				{
					this.slaveExpectationCached = new PawnExpectationsQualityOffset
					{
						labelOverride = "Slave".Translate().CapitalizeFirst(),
						offset = this.offsetForSlave
					};
				}
				return this.slaveExpectationCached;
			}
		}

		// Token: 0x06009245 RID: 37445 RVA: 0x00356AEF File Offset: 0x00354CEF
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return this.Count(ritual, data);
		}

		// Token: 0x06009246 RID: 37446 RVA: 0x00356AFC File Offset: 0x00354CFC
		protected PawnExpectationsQualityOffset ExpectationOffset(Pawn pawn)
		{
			if (pawn.IsSlaveOfColony)
			{
				return this.SlaveExpectationCached;
			}
			ExpectationDef expectations = ExpectationsUtility.CurrentExpectationFor(pawn);
			return this.offsetPerExpectation.FirstOrDefault((PawnExpectationsQualityOffset e) => e.expectation == expectations);
		}

		// Token: 0x06009247 RID: 37447 RVA: 0x00356B44 File Offset: 0x00354D44
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			Pawn pawn = ritual.assignments.FirstAssignedPawn(this.roleId);
			if (pawn == null)
			{
				return 0f;
			}
			PawnExpectationsQualityOffset pawnExpectationsQualityOffset = this.ExpectationOffset(pawn);
			if (pawnExpectationsQualityOffset == null)
			{
				return 0f;
			}
			return pawnExpectationsQualityOffset.offset;
		}

		// Token: 0x06009248 RID: 37448 RVA: 0x00356B84 File Offset: 0x00354D84
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
			PawnExpectationsQualityOffset pawnExpectationsQualityOffset = this.ExpectationOffset(pawn);
			string str = (pawnExpectationsQualityOffset.offset < 0f) ? "" : "+";
			return this.LabelForDesc.Formatted(pawn.Named("PAWN")) + ": " + "OutcomeBonusDesc_QualitySingleOffset".Translate(str + pawnExpectationsQualityOffset.offset.ToStringPercent()) + ".";
		}

		// Token: 0x06009249 RID: 37449 RVA: 0x00356C2C File Offset: 0x00354E2C
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			Pawn pawn = assignments.FirstAssignedPawn(this.roleId);
			if (pawn == null)
			{
				return null;
			}
			PawnExpectationsQualityOffset pawnExpectationsQualityOffset = this.ExpectationOffset(pawn);
			if (pawnExpectationsQualityOffset != null)
			{
				return new QualityFactor
				{
					label = "RitualPredictedOutcomeDescPawnExpectations".Translate(pawn.Named("PAWN")),
					count = pawnExpectationsQualityOffset.Label,
					qualityChange = ((Math.Abs(pawnExpectationsQualityOffset.offset) > float.Epsilon) ? "OutcomeBonusDesc_QualitySingleOffset".Translate(pawnExpectationsQualityOffset.offset.ToStringWithSign("0.#%")).Resolve() : " - "),
					positive = (pawnExpectationsQualityOffset.offset >= 0f),
					quality = pawnExpectationsQualityOffset.offset,
					priority = 0f
				};
			}
			return null;
		}

		// Token: 0x0400589D RID: 22685
		[NoTranslate]
		public string roleId;

		// Token: 0x0400589E RID: 22686
		public float offsetForSlave;

		// Token: 0x0400589F RID: 22687
		public List<PawnExpectationsQualityOffset> offsetPerExpectation = new List<PawnExpectationsQualityOffset>();

		// Token: 0x040058A0 RID: 22688
		private PawnExpectationsQualityOffset slaveExpectationCached;
	}
}
