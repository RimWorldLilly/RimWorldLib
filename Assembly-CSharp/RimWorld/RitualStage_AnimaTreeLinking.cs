using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200167D RID: 5757
	public class RitualStage_AnimaTreeLinking : RitualStage
	{
		// Token: 0x06009056 RID: 36950 RVA: 0x0034FC4D File Offset: 0x0034DE4D
		public override TargetInfo GetSecondFocus(LordJob_Ritual ritual)
		{
			return ritual.selectedTarget;
		}

		// Token: 0x06009057 RID: 36951 RVA: 0x0034FC58 File Offset: 0x0034DE58
		public override float ProgressPerTick(LordJob_Ritual ritual)
		{
			int num = 1;
			foreach (Pawn p in ritual.assignments.SpectatorsForReading)
			{
				if (ritual.IsParticipating(p))
				{
					num++;
				}
			}
			return RitualStage_AnimaTreeLinking.ProgressPerParticipantCurve.Evaluate((float)num);
		}

		// Token: 0x0400581D RID: 22557
		public static readonly SimpleCurve ProgressPerParticipantCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(2f, 1.2f),
				true
			},
			{
				new CurvePoint(4f, 1.5f),
				true
			},
			{
				new CurvePoint(6f, 2f),
				true
			},
			{
				new CurvePoint(8f, 3f),
				true
			}
		};
	}
}
