using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D8C RID: 7564
	public static class SelectorUtility
	{
		// Token: 0x0600C438 RID: 50232 RVA: 0x0047D248 File Offset: 0x0047B448
		public static void SortInColonistBarOrder(List<Thing> things)
		{
			SelectorUtility.tmp_thingsToSort.Clear();
			SelectorUtility.tmp_thingsToSort.AddRange(things);
			things.Clear();
			foreach (Pawn item in Find.ColonistBar.GetColonistsInOrder())
			{
				int num = SelectorUtility.tmp_thingsToSort.IndexOf(item);
				if (num != -1)
				{
					things.Add(item);
					SelectorUtility.tmp_thingsToSort.RemoveAt(num);
				}
			}
			things.AddRange(SelectorUtility.tmp_thingsToSort);
			SelectorUtility.tmp_thingsToSort.Clear();
		}

		// Token: 0x0600C439 RID: 50233 RVA: 0x0047D2EC File Offset: 0x0047B4EC
		public static bool IsEquivalentRace(Pawn a, Pawn b)
		{
			return (ModsConfig.AnomalyActive && ((a.def == ThingDefOf.Human && b.def == ThingDefOf.CreepJoiner) || (b.def == ThingDefOf.Human && a.def == ThingDefOf.CreepJoiner))) || a.RaceProps == b.RaceProps;
		}

		// Token: 0x0400701A RID: 28698
		private static readonly List<Thing> tmp_thingsToSort = new List<Thing>();
	}
}
