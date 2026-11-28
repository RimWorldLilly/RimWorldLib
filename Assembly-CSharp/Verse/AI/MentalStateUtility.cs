using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000879 RID: 2169
	public static class MentalStateUtility
	{
		// Token: 0x060043C2 RID: 17346 RVA: 0x001AA1A2 File Offset: 0x001A83A2
		public static MentalStateDef GetWanderToOwnRoomStateOrFallback(Pawn pawn)
		{
			if (MentalStateDefOf.Wander_OwnRoom.Worker.StateCanOccur(pawn))
			{
				return MentalStateDefOf.Wander_OwnRoom;
			}
			if (MentalStateDefOf.Wander_Sad.Worker.StateCanOccur(pawn))
			{
				return MentalStateDefOf.Wander_Sad;
			}
			return null;
		}

		// Token: 0x060043C3 RID: 17347 RVA: 0x001AA1D8 File Offset: 0x001A83D8
		public static void TryTransitionToWanderOwnRoom(MentalState mentalState)
		{
			MentalStateDef wanderToOwnRoomStateOrFallback = MentalStateUtility.GetWanderToOwnRoomStateOrFallback(mentalState.pawn);
			if (wanderToOwnRoomStateOrFallback != null)
			{
				mentalState.pawn.mindState.mentalStateHandler.TryStartMentalState(wanderToOwnRoomStateOrFallback, null, false, false, mentalState.causedByMood, null, true, false, false);
				return;
			}
			mentalState.RecoverFromState();
		}

		// Token: 0x060043C4 RID: 17348 RVA: 0x001AA220 File Offset: 0x001A8420
		public static void StartMentalState(Pawn pawn, MentalStateDef stateDef, MentalStateDef mechStateDef = null)
		{
			MentalStateDef stateDef2 = pawn.RaceProps.IsMechanoid ? (mechStateDef ?? stateDef) : stateDef;
			if (pawn.mindState.mentalStateHandler.TryStartMentalState(stateDef2, null, false, true, false, null, false, false, false))
			{
				RestUtility.WakeUp(pawn, true);
			}
		}

		// Token: 0x060043C5 RID: 17349 RVA: 0x001AA268 File Offset: 0x001A8468
		public static bool IsHavingMentalBreak(Pawn pawn)
		{
			if (!pawn.mindState.mentalStateHandler.InMentalState)
			{
				return false;
			}
			MentalStateDef mentalStateDef = pawn.MentalStateDef;
			if (mentalStateDef == null)
			{
				return false;
			}
			List<MentalBreakDef> allDefsListForReading = DefDatabase<MentalBreakDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].mentalState == mentalStateDef)
				{
					return true;
				}
			}
			return false;
		}
	}
}
