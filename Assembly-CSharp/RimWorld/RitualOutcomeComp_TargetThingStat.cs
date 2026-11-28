using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016C3 RID: 5827
	public class RitualOutcomeComp_TargetThingStat : RitualOutcomeComp_Quality
	{
		// Token: 0x17001917 RID: 6423
		// (get) Token: 0x060091DC RID: 37340 RVA: 0x00354C5A File Offset: 0x00352E5A
		public override bool DataRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060091DD RID: 37341 RVA: 0x00354C5D File Offset: 0x00352E5D
		public override bool Applies(LordJob_Ritual ritual)
		{
			return ritual.selectedTarget.HasThing && (!this.mustBeBed || ritual.selectedTarget.Thing.def.IsBed);
		}

		// Token: 0x060091DE RID: 37342 RVA: 0x00354C8D File Offset: 0x00352E8D
		private float StatValue(TargetInfo ritualTarget)
		{
			if (ritualTarget.HasThing)
			{
				return ritualTarget.Thing.GetStatValue(this.statDef, true, -1);
			}
			return 0f;
		}

		// Token: 0x060091DF RID: 37343 RVA: 0x00354CB2 File Offset: 0x00352EB2
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return this.StatValue(ritual.selectedTarget);
		}

		// Token: 0x060091E0 RID: 37344 RVA: 0x00354CC0 File Offset: 0x00352EC0
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			float x = this.StatValue(ritualTarget);
			float quality = this.curve.Evaluate(x);
			return new QualityFactor
			{
				label = this.label.CapitalizeFirst(),
				count = x.ToString(),
				present = (!this.mustBeBed || (ritualTarget.HasThing && ritualTarget.Thing.def.IsBed)),
				qualityChange = this.ExpectedOffsetDesc(true, quality),
				quality = quality,
				positive = true,
				priority = 0f
			};
		}

		// Token: 0x04005880 RID: 22656
		public StatDef statDef;

		// Token: 0x04005881 RID: 22657
		public bool mustBeBed;
	}
}
