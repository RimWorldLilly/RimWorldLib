using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B8C RID: 2956
	public class JoyGiver_Meditate : JoyGiver_InPrivateRoom
	{
		// Token: 0x060055C7 RID: 21959 RVA: 0x001FF0E2 File Offset: 0x001FD2E2
		public override Job TryGiveJob(Pawn pawn)
		{
			if (ModsConfig.RoyaltyActive)
			{
				return MeditationUtility.GetMeditationJob(pawn, true);
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x060055C8 RID: 21960 RVA: 0x001FF0FA File Offset: 0x001FD2FA
		public override bool CanBeGivenTo(Pawn pawn)
		{
			return (!ModsConfig.RoyaltyActive || MeditationUtility.CanMeditateNow(pawn)) && base.CanBeGivenTo(pawn);
		}
	}
}
