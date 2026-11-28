using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020016CF RID: 5839
	public class RitualOutcomeComp_NumParticipantsWithTag : RitualOutcomeComp_Quality
	{
		// Token: 0x1700191C RID: 6428
		// (get) Token: 0x06009220 RID: 37408 RVA: 0x003560D1 File Offset: 0x003542D1
		public override bool DataRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06009221 RID: 37409 RVA: 0x003560D4 File Offset: 0x003542D4
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			int num = 0;
			foreach (Pawn p in ritual.assignments.Participants)
			{
				if (ritual.PawnTagSet(p, this.tag))
				{
					num++;
				}
			}
			return (float)num;
		}

		// Token: 0x06009222 RID: 37410 RVA: 0x0035613C File Offset: 0x0035433C
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			int num = assignments.Participants.Count((Pawn p) => p.RaceProps.Humanlike);
			float quality = this.curve.Evaluate((float)num);
			return new QualityFactor
			{
				label = this.label.CapitalizeFirst(),
				count = Mathf.Min((float)num, base.MaxValue).ToString() + " / " + base.MaxValue.ToString(),
				qualityChange = this.ExpectedOffsetDesc(num > 0, quality),
				quality = quality,
				positive = (num > 0),
				priority = 4f
			};
		}

		// Token: 0x0400588E RID: 22670
		public string tag;
	}
}
