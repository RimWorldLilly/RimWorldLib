using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E6D RID: 3693
	public class ThoughtWorker_TravelAnticipation : ThoughtWorker
	{
		// Token: 0x060061FB RID: 25083 RVA: 0x002347EC File Offset: 0x002329EC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.IsColonist || p.jobs == null || p.jobs.StartedFormingCaravanTick < 0)
			{
				return ThoughtState.Inactive;
			}
			int num = (Find.TickManager.TicksGame - p.jobs.StartedFormingCaravanTick) / 2500;
			if (num < 8)
			{
				return ThoughtState.Inactive;
			}
			if (num < 16)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			return ThoughtState.ActiveAtStage(1);
		}

		// Token: 0x04003611 RID: 13841
		private const int stage0Hours = 8;

		// Token: 0x04003612 RID: 13842
		private const int stage1Hours = 16;
	}
}
