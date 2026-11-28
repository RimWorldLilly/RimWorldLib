using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001522 RID: 5410
	public static class RecruitUtility
	{
		// Token: 0x06008464 RID: 33892 RVA: 0x002FAD5C File Offset: 0x002F8F5C
		public static void Recruit(Pawn pawn, Faction faction, Pawn recruiter = null)
		{
			Pawn_ApparelTracker apparel = pawn.apparel;
			if (apparel != null)
			{
				apparel.UnlockAll();
			}
			if (pawn.royalty != null)
			{
				foreach (RoyalTitle royalTitle in pawn.royalty.AllTitlesForReading)
				{
					if (royalTitle.def.replaceOnRecruited != null)
					{
						pawn.royalty.SetTitle(royalTitle.faction, royalTitle.def.replaceOnRecruited, false, false, false);
					}
				}
			}
			if (pawn.guest != null)
			{
				pawn.guest.SetGuestStatus(null, GuestStatus.Guest);
			}
			if (pawn.Faction != faction)
			{
				pawn.SetFaction(faction, recruiter);
			}
			if (pawn.guest != null)
			{
				pawn.guest.Notify_PawnRecruited();
			}
		}
	}
}
