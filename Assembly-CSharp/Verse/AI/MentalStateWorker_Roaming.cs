using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200089D RID: 2205
	public class MentalStateWorker_Roaming : MentalStateWorker
	{
		// Token: 0x06004445 RID: 17477 RVA: 0x001ABC40 File Offset: 0x001A9E40
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && MentalStateWorker_Roaming.CanRoamNow(pawn);
		}

		// Token: 0x06004446 RID: 17478 RVA: 0x001ABC54 File Offset: 0x001A9E54
		public static bool CanRoamNow(Pawn pawn)
		{
			return pawn.Spawned && pawn.Map.IsPlayerHome && GenTicks.TicksGame >= 300000 && pawn.Roamer && pawn.Faction == Faction.OfPlayer && !pawn.roping.IsRoped && !pawn.mindState.InRoamingCooldown && pawn.CanReachMapEdge();
		}

		// Token: 0x04002C42 RID: 11330
		public const int GracePeriodSinceGameStartedDays = 5;
	}
}
