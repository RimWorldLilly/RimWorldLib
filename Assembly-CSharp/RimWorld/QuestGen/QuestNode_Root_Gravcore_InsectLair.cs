using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002251 RID: 8785
	public class QuestNode_Root_Gravcore_InsectLair : QuestNode_Root_Gravcore
	{
		// Token: 0x0600D932 RID: 55602 RVA: 0x005076D4 File Offset: 0x005058D4
		protected override bool TestRunInt(Slate slate)
		{
			return Faction.OfInsects != null && base.TestRunInt(slate);
		}

		// Token: 0x0600D933 RID: 55603 RVA: 0x005076E8 File Offset: 0x005058E8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			PlanetTile tile;
			if (!this.TryFindSiteTile(out tile))
			{
				Log.Error("Could not find valid site tile for insect lair quest.");
				return;
			}
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.MapGenerated");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("site.MapRemoved");
			Site site = QuestGen_Sites.GenerateSite(new SitePartDefWithParams[]
			{
				new SitePartDefWithParams(SitePartDefOf.InsectLair, new SitePartParams
				{
					points = slate.Get<float>("points", 0f, false),
					threatPoints = slate.Get<float>("points", 0f, false)
				})
			}, tile, Faction.OfInsects, false, null, WorldObjectDefOf.ClaimableSite);
			slate.Set<Site>("site", site, false);
			quest.SpawnWorldObject(site, null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_DefinedThingDef(ThingDefOf.Gravcore));
			choice.rewards.Add(new Reward_DefinedThingDef(ThingDefOf.GravlitePanel));
			quest.RewardChoice(null, null).choices.Add(choice);
			quest.End(QuestEndOutcome.Success, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.End(QuestEndOutcome.Unknown, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, false, false);
		}
	}
}
