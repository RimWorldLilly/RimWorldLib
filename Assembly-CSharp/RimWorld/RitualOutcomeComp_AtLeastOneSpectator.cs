using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016CB RID: 5835
	public class RitualOutcomeComp_AtLeastOneSpectator : RitualOutcomeComp_ParticipantCount
	{
		// Token: 0x06009208 RID: 37384 RVA: 0x00355804 File Offset: 0x00353A04
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			int num = assignments.Participants.Count((Pawn p) => this.Counts(assignments, p));
			float quality = (num > 0) ? this.qualityOffset : 0f;
			return new QualityFactor
			{
				label = this.label.CapitalizeFirst(),
				present = (num > 0),
				qualityChange = this.ExpectedOffsetDesc(true, quality),
				quality = quality,
				positive = true,
				priority = 4f
			};
		}

		// Token: 0x06009209 RID: 37385 RVA: 0x0035589C File Offset: 0x00353A9C
		public override string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
		{
			return ((this.Count(ritual, data) > 0f) ? this.label : this.labelNotMet).CapitalizeFirst().Formatted(Array.Empty<NamedArgument>()) + ": " + "OutcomeBonusDesc_QualitySingleOffset".Translate(this.qualityOffset.ToStringPercent()) + ".";
		}

		// Token: 0x0600920A RID: 37386 RVA: 0x0035590D File Offset: 0x00353B0D
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			if (this.Count(ritual, data) <= 0f)
			{
				return 0f;
			}
			return this.qualityOffset;
		}

		// Token: 0x0600920B RID: 37387 RVA: 0x0035592A File Offset: 0x00353B2A
		protected override string ExpectedOffsetDesc(bool positive, float quality = 0f)
		{
			if (!positive)
			{
				return "";
			}
			return quality.ToStringWithSign("0.#%");
		}

		// Token: 0x04005888 RID: 22664
		[MustTranslate]
		public string labelNotMet;
	}
}
