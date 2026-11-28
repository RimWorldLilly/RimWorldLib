using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AD3 RID: 2771
	public class JobGiver_AIDefendMaster : JobGiver_AIDefendPawn
	{
		// Token: 0x06005308 RID: 21256 RVA: 0x001EEE66 File Offset: 0x001ED066
		protected override Pawn GetDefendee(Pawn pawn)
		{
			return pawn.playerSettings.Master;
		}

		// Token: 0x06005309 RID: 21257 RVA: 0x001EEE73 File Offset: 0x001ED073
		protected override float GetFlagRadius(Pawn pawn)
		{
			if (pawn.playerSettings.Master.playerSettings.animalsReleased && pawn.training.HasLearned(TrainableDefOf.Release))
			{
				return 50f;
			}
			return 5f;
		}

		// Token: 0x040031F7 RID: 12791
		private const float RadiusUnreleased = 5f;
	}
}
