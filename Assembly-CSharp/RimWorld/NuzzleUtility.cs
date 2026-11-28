using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001520 RID: 5408
	public static class NuzzleUtility
	{
		// Token: 0x0600845D RID: 33885 RVA: 0x002FA344 File Offset: 0x002F8544
		public static float GetNuzzleMTBHours(Pawn pawn)
		{
			float num = pawn.RaceProps.nuzzleMtbHours;
			if (ModsConfig.OdysseyActive)
			{
				Pawn_TrainingTracker training = pawn.training;
				if (training != null && training.HasLearned(TrainableDefOf.Comfort))
				{
					num *= 0.75f;
				}
			}
			return num;
		}

		// Token: 0x0600845E RID: 33886 RVA: 0x002FA386 File Offset: 0x002F8586
		public static int GetNuzzleStageIndex(Pawn pawn)
		{
			if (ModsConfig.OdysseyActive)
			{
				Pawn_TrainingTracker training = pawn.training;
				if (training != null && training.HasLearned(TrainableDefOf.Comfort))
				{
					return 1;
				}
			}
			return 0;
		}
	}
}
