using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021DF RID: 8671
	public class QuestNode_GetSitePartDefsByTagsAndFaction : QuestNode
	{
		// Token: 0x0600D7AE RID: 55214 RVA: 0x00500780 File Offset: 0x004FE980
		protected override bool TestRunInt(Slate slate)
		{
			return this.TrySetVars(slate);
		}

		// Token: 0x0600D7AF RID: 55215 RVA: 0x00500789 File Offset: 0x004FE989
		protected override void RunInt()
		{
			if (!this.TrySetVars(QuestGen.slate))
			{
				Log.Error("Could not resolve site parts.");
			}
		}

		// Token: 0x0600D7B0 RID: 55216 RVA: 0x005007A4 File Offset: 0x004FE9A4
		private bool TrySetVars(Slate slate)
		{
			float points = slate.Get<float>("points", 0f, false);
			Faction faction = slate.Get<Faction>("enemyFaction", null, false);
			Pawn asker = slate.Get<Pawn>("asker", null, false);
			Thing mustBeHostileToFactionOfResolved = this.mustBeHostileToFactionOf.GetValue(slate);
			Func<SitePartDef, bool> <>9__3;
			Func<string, IEnumerable<SitePartDef>> <>9__1;
			Predicate<Faction> <>9__2;
			for (int i = 0; i < 2; i++)
			{
				QuestNode_GetSitePartDefsByTagsAndFaction.tmpTags.Clear();
				foreach (QuestNode_GetSitePartDefsByTagsAndFaction.SitePartOption sitePartOption in this.sitePartsTags.GetValue(slate))
				{
					if (Rand.Chance(sitePartOption.chance) && (i != 1 || sitePartOption.chance >= 1f))
					{
						QuestNode_GetSitePartDefsByTagsAndFaction.tmpTags.Add(sitePartOption.tag);
					}
				}
				IEnumerable<string> source = from x in QuestNode_GetSitePartDefsByTagsAndFaction.tmpTags
				where x != null
				select x;
				Func<string, IEnumerable<SitePartDef>> selector;
				if ((selector = <>9__1) == null)
				{
					selector = (<>9__1 = delegate(string x)
					{
						IEnumerable<SitePartDef> enumerable = SiteMakerHelper.SitePartDefsWithTag(x);
						IEnumerable<SitePartDef> source2 = enumerable;
						Func<SitePartDef, bool> predicate;
						if ((predicate = <>9__3) == null)
						{
							predicate = (<>9__3 = ((SitePartDef y) => points >= y.minThreatPoints));
						}
						IEnumerable<SitePartDef> enumerable2 = source2.Where(predicate);
						if (!enumerable2.Any<SitePartDef>())
						{
							return enumerable;
						}
						return enumerable2;
					});
				}
				IEnumerable<IEnumerable<SitePartDef>> sitePartsCandidates = source.Select(selector);
				Faction factionToUse = faction;
				bool disallowNonHostileFactions = true;
				Predicate<Faction> extraFactionValidator;
				if ((extraFactionValidator = <>9__2) == null)
				{
					extraFactionValidator = (<>9__2 = ((Faction x) => (asker == null || asker.Faction == null || asker.Faction != x) && (mustBeHostileToFactionOfResolved == null || mustBeHostileToFactionOfResolved.Faction == null || (x != mustBeHostileToFactionOfResolved.Faction && x.HostileTo(mustBeHostileToFactionOfResolved.Faction)))));
				}
				List<SitePartDef> list;
				Faction var;
				if (SiteMakerHelper.TryFindSiteParams_MultipleSiteParts(sitePartsCandidates, out list, out var, factionToUse, disallowNonHostileFactions, extraFactionValidator))
				{
					slate.Set<List<SitePartDef>>(this.storeAs.GetValue(slate), list, false);
					slate.Set<int>("sitePartCount", list.Count, false);
					if (QuestGen.Working)
					{
						Dictionary<string, string> dictionary = new Dictionary<string, string>();
						for (int j = 0; j < list.Count; j++)
						{
							dictionary[list[j].defName + "_exists"] = "True";
						}
						QuestGen.AddQuestDescriptionConstants(dictionary);
					}
					if (!this.storeFactionAs.GetValue(slate).NullOrEmpty())
					{
						slate.Set<Faction>(this.storeFactionAs.GetValue(slate), var, false);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x04008909 RID: 35081
		public SlateRef<IEnumerable<QuestNode_GetSitePartDefsByTagsAndFaction.SitePartOption>> sitePartsTags;

		// Token: 0x0400890A RID: 35082
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400890B RID: 35083
		[NoTranslate]
		public SlateRef<string> storeFactionAs;

		// Token: 0x0400890C RID: 35084
		public SlateRef<Thing> mustBeHostileToFactionOf;

		// Token: 0x0400890D RID: 35085
		private static List<string> tmpTags = new List<string>();

		// Token: 0x02003D33 RID: 15667
		public class SitePartOption
		{
			// Token: 0x0400F195 RID: 61845
			[NoTranslate]
			public string tag;

			// Token: 0x0400F196 RID: 61846
			public float chance = 1f;
		}
	}
}
