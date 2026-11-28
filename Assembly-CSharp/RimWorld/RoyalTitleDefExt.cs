using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200200E RID: 8206
	public static class RoyalTitleDefExt
	{
		// Token: 0x0600CFDD RID: 53213 RVA: 0x004C73DC File Offset: 0x004C55DC
		public static RoyalTitleDef GetNextTitle(this RoyalTitleDef currentTitle, Faction faction)
		{
			int num = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle);
			if (num == -1 && currentTitle != null)
			{
				return null;
			}
			int num2 = (currentTitle == null) ? 0 : (num + 1);
			if (faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.Count <= num2)
			{
				return null;
			}
			return faction.def.RoyalTitlesAwardableInSeniorityOrderForReading[num2];
		}

		// Token: 0x0600CFDE RID: 53214 RVA: 0x004C7434 File Offset: 0x004C5634
		public static RoyalTitleDef GetPreviousTitle(this RoyalTitleDef currentTitle, Faction faction)
		{
			if (currentTitle == null)
			{
				return null;
			}
			int num = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle) - 1;
			if (num >= faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.Count || num < 0)
			{
				return null;
			}
			return faction.def.RoyalTitlesAwardableInSeniorityOrderForReading[num];
		}

		// Token: 0x0600CFDF RID: 53215 RVA: 0x004C7484 File Offset: 0x004C5684
		public static RoyalTitleDef GetPreviousTitle_IncludeNonRewardable(this RoyalTitleDef currentTitle, Faction faction)
		{
			if (currentTitle == null)
			{
				return null;
			}
			int num = faction.def.RoyalTitlesAllInSeniorityOrderForReading.IndexOf(currentTitle) - 1;
			if (num >= faction.def.RoyalTitlesAllInSeniorityOrderForReading.Count || num < 0)
			{
				return null;
			}
			return faction.def.RoyalTitlesAllInSeniorityOrderForReading[num];
		}

		// Token: 0x0600CFE0 RID: 53216 RVA: 0x004C74D4 File Offset: 0x004C56D4
		public static bool TryInherit(this RoyalTitleDef title, Pawn from, Faction faction, out RoyalTitleInheritanceOutcome outcome)
		{
			outcome = default(RoyalTitleInheritanceOutcome);
			if (title.GetInheritanceWorker(faction) == null)
			{
				return false;
			}
			Pawn heir = from.royalty.GetHeir(faction);
			if (heir == null || heir.Destroyed)
			{
				return false;
			}
			RoyalTitleDef currentTitle = heir.royalty.GetCurrentTitle(faction);
			bool heirTitleHigher = currentTitle != null && currentTitle.seniority >= title.seniority;
			outcome = new RoyalTitleInheritanceOutcome
			{
				heir = heir,
				heirCurrentTitle = currentTitle,
				heirTitleHigher = heirTitleHigher
			};
			return true;
		}
	}
}
