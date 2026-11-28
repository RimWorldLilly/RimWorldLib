using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AD9 RID: 2777
	public class JobGiver_AIFollowMaster : JobGiver_AIFollowPawn
	{
		// Token: 0x17000EFD RID: 3837
		// (get) Token: 0x0600532E RID: 21294 RVA: 0x001EF83A File Offset: 0x001EDA3A
		protected override int FollowJobExpireInterval
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x0600532F RID: 21295 RVA: 0x001EF841 File Offset: 0x001EDA41
		protected override Pawn GetFollowee(Pawn pawn)
		{
			if (pawn.playerSettings == null)
			{
				return null;
			}
			return pawn.playerSettings.Master;
		}

		// Token: 0x06005330 RID: 21296 RVA: 0x001EF858 File Offset: 0x001EDA58
		protected override float GetRadius(Pawn pawn)
		{
			if (pawn.playerSettings.Master.playerSettings.animalsReleased && pawn.training.HasLearned(TrainableDefOf.Release))
			{
				return 50f;
			}
			return 3f;
		}

		// Token: 0x04003201 RID: 12801
		public const float RadiusUnreleased = 3f;

		// Token: 0x04003202 RID: 12802
		public const float RadiusReleased = 50f;
	}
}
