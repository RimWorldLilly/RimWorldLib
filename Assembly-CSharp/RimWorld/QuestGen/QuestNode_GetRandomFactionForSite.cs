using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021D7 RID: 8663
	public class QuestNode_GetRandomFactionForSite : QuestNode
	{
		// Token: 0x0600D78D RID: 55181 RVA: 0x004FFF35 File Offset: 0x004FE135
		protected override bool TestRunInt(Slate slate)
		{
			return this.TrySetVars(slate, true);
		}

		// Token: 0x0600D78E RID: 55182 RVA: 0x004FFF3F File Offset: 0x004FE13F
		protected override void RunInt()
		{
			this.TrySetVars(QuestGen.slate, false);
		}

		// Token: 0x0600D78F RID: 55183 RVA: 0x004FFF50 File Offset: 0x004FE150
		private bool TrySetVars(Slate slate, bool test)
		{
			Pawn asker = slate.Get<Pawn>("asker", null, false);
			Thing mustBeHostileToFactionOfResolved = this.mustBeHostileToFactionOf.GetValue(slate);
			Faction faction;
			if (!SiteMakerHelper.TryFindRandomFactionFor(this.sitePartDefs.GetValue(slate), out faction, true, (Faction x) => (asker == null || asker.Faction != x) && (mustBeHostileToFactionOfResolved == null || mustBeHostileToFactionOfResolved.Faction == null || (x != mustBeHostileToFactionOfResolved.Faction && x.HostileTo(mustBeHostileToFactionOfResolved.Faction)))))
			{
				return false;
			}
			if (!Find.Storyteller.difficulty.allowViolentQuests && faction.HostileTo(Faction.OfPlayer))
			{
				return false;
			}
			slate.Set<Faction>(this.storeAs.GetValue(slate), faction, false);
			if (!test && faction != null && !faction.Hidden)
			{
				QuestPart_InvolvedFactions questPart_InvolvedFactions = new QuestPart_InvolvedFactions();
				questPart_InvolvedFactions.factions.Add(faction);
				QuestGen.quest.AddPart(questPart_InvolvedFactions);
			}
			return true;
		}

		// Token: 0x040088F0 RID: 35056
		public SlateRef<IEnumerable<SitePartDef>> sitePartDefs;

		// Token: 0x040088F1 RID: 35057
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040088F2 RID: 35058
		public SlateRef<Thing> mustBeHostileToFactionOf;
	}
}
