using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001557 RID: 5463
	public static class RestraintsUtility
	{
		// Token: 0x060085A0 RID: 34208 RVA: 0x00303900 File Offset: 0x00301B00
		public static bool InRestraints(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return false;
			}
			if (pawn.HostFaction == null)
			{
				return false;
			}
			Lord lord = pawn.GetLord();
			return (lord == null || lord.LordJob == null || !lord.LordJob.NeverInRestraints) && (pawn.guest == null || !pawn.guest.Released) && !pawn.IsSlave;
		}

		// Token: 0x060085A1 RID: 34209 RVA: 0x00303964 File Offset: 0x00301B64
		public static bool ShouldShowRestraintsInfo(Pawn pawn)
		{
			return pawn.IsPrisonerOfColony && RestraintsUtility.InRestraints(pawn);
		}
	}
}
