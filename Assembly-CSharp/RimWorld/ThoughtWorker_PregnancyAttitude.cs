using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E81 RID: 3713
	public class ThoughtWorker_PregnancyAttitude : ThoughtWorker
	{
		// Token: 0x06006240 RID: 25152 RVA: 0x002353F4 File Offset: 0x002335F4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			Hediff_Pregnant hediff_Pregnant = PregnancyUtility.GetPregnancyHediff(p) as Hediff_Pregnant;
			if (hediff_Pregnant == null || hediff_Pregnant.CurStageIndex > 0)
			{
				return ThoughtState.Inactive;
			}
			PregnancyAttitude? attitude = hediff_Pregnant.Attitude;
			if (attitude != null)
			{
				PregnancyAttitude value = attitude.Value;
				if (value == PregnancyAttitude.Positive)
				{
					return ThoughtState.ActiveAtStage(0);
				}
				if (value == PregnancyAttitude.Negative)
				{
					return ThoughtState.ActiveAtStage(1);
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
