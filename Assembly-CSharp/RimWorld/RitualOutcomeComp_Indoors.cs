using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016C4 RID: 5828
	public class RitualOutcomeComp_Indoors : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x17001918 RID: 6424
		// (get) Token: 0x060091E2 RID: 37346 RVA: 0x00354D62 File Offset: 0x00352F62
		public override bool DataRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060091E3 RID: 37347 RVA: 0x00354D65 File Offset: 0x00352F65
		public override bool Applies(LordJob_Ritual ritual)
		{
			return true;
		}

		// Token: 0x060091E4 RID: 37348 RVA: 0x00354D68 File Offset: 0x00352F68
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			Room room = ritual.Spot.GetRoom(ritual.Map);
			if (room != null)
			{
				return !room.PsychologicallyOutdoors;
			}
			return 0f;
		}

		// Token: 0x060091E5 RID: 37349 RVA: 0x00354D9C File Offset: 0x00352F9C
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			float quality = 0f;
			bool flag = false;
			if (ritualTarget.Map != null)
			{
				Room room = ritualTarget.Cell.GetRoom(ritualTarget.Map);
				flag = (room != null && !room.PsychologicallyOutdoors);
				quality = (flag ? this.qualityOffset : 0f);
			}
			return new QualityFactor
			{
				label = this.label.CapitalizeFirst(),
				qualityChange = this.ExpectedOffsetDesc(true, quality),
				quality = quality,
				present = flag,
				positive = true,
				priority = 0f
			};
		}
	}
}
