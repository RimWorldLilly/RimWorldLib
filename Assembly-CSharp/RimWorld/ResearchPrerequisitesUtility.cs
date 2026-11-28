using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DA1 RID: 7585
	public static class ResearchPrerequisitesUtility
	{
		// Token: 0x0600C53B RID: 50491 RVA: 0x00487BEC File Offset: 0x00485DEC
		private static Dictionary<Def, List<ResearchProjectDef>> ComputeResearchPrerequisites()
		{
			Dictionary<Def, List<ResearchProjectDef>> dictionary = new Dictionary<Def, List<ResearchProjectDef>>();
			foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefs)
			{
				foreach (Def key in researchProjectDef.UnlockedDefs)
				{
					List<ResearchProjectDef> list;
					if (!dictionary.TryGetValue(key, out list))
					{
						list = new List<ResearchProjectDef>();
						dictionary.Add(key, list);
					}
					list.Add(researchProjectDef);
				}
			}
			return dictionary;
		}

		// Token: 0x0600C53C RID: 50492 RVA: 0x00487C9C File Offset: 0x00485E9C
		public static List<Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>> UnlockedDefsGroupedByPrerequisites(ResearchProjectDef rd)
		{
			Dictionary<Def, List<ResearchProjectDef>> dictionary = ResearchPrerequisitesUtility.ComputeResearchPrerequisites();
			List<Pair<Def, ResearchPrerequisitesUtility.UnlockedHeader>> list = new List<Pair<Def, ResearchPrerequisitesUtility.UnlockedHeader>>();
			foreach (Def def in rd.UnlockedDefs)
			{
				list.Add(new Pair<Def, ResearchPrerequisitesUtility.UnlockedHeader>(def, new ResearchPrerequisitesUtility.UnlockedHeader(dictionary[def].Except(rd).ToList<ResearchProjectDef>())));
			}
			return (from pair in list
			orderby pair.Second.unlockedBy.Count
			group pair.First by pair.Second).Select(new Func<IGrouping<ResearchPrerequisitesUtility.UnlockedHeader, Def>, Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>>(GenCollection.ConvertIGroupingToPair<ResearchPrerequisitesUtility.UnlockedHeader, Def>)).ToList<Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>>();
		}

		// Token: 0x02003B0D RID: 15117
		public class UnlockedHeader : IEquatable<ResearchPrerequisitesUtility.UnlockedHeader>
		{
			// Token: 0x0601540C RID: 87052 RVA: 0x006BEFB9 File Offset: 0x006BD1B9
			public UnlockedHeader(List<ResearchProjectDef> unlockedBy)
			{
				this.unlockedBy = unlockedBy;
			}

			// Token: 0x0601540D RID: 87053 RVA: 0x006BEFC8 File Offset: 0x006BD1C8
			public bool Equals(ResearchPrerequisitesUtility.UnlockedHeader other)
			{
				return other != null && (this == other || this.unlockedBy.SequenceEqual(other.unlockedBy));
			}

			// Token: 0x0601540E RID: 87054 RVA: 0x006BEFE6 File Offset: 0x006BD1E6
			public override bool Equals(object obj)
			{
				return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((ResearchPrerequisitesUtility.UnlockedHeader)obj)));
			}

			// Token: 0x0601540F RID: 87055 RVA: 0x006BF014 File Offset: 0x006BD214
			public override int GetHashCode()
			{
				if (!this.unlockedBy.Any<ResearchProjectDef>())
				{
					return 23;
				}
				return this.unlockedBy.First<ResearchProjectDef>().GetHashCode();
			}

			// Token: 0x0400EA76 RID: 60022
			public List<ResearchProjectDef> unlockedBy;
		}
	}
}
