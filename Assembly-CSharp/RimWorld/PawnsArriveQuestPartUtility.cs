using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001118 RID: 4376
	public static class PawnsArriveQuestPartUtility
	{
		// Token: 0x06006D78 RID: 28024 RVA: 0x00265B23 File Offset: 0x00263D23
		public static IEnumerable<Pawn> GetQuestLookTargets(IEnumerable<Pawn> pawns)
		{
			PawnsArriveQuestPartUtility.<GetQuestLookTargets>d__0 <GetQuestLookTargets>d__ = new PawnsArriveQuestPartUtility.<GetQuestLookTargets>d__0(-2);
			<GetQuestLookTargets>d__.<>3__pawns = pawns;
			return <GetQuestLookTargets>d__;
		}

		// Token: 0x06006D79 RID: 28025 RVA: 0x00265B34 File Offset: 0x00263D34
		public static bool IncreasesPopulation(IEnumerable<Pawn> pawns, bool joinPlayer, bool makePrisoners)
		{
			foreach (Pawn pawn in pawns)
			{
				if (pawn.RaceProps.Humanlike && !pawn.Destroyed && (pawn.Faction == Faction.OfPlayer || pawn.IsPrisonerOfColony || pawn.Downed || joinPlayer || makePrisoners))
				{
					return true;
				}
			}
			return false;
		}
	}
}
