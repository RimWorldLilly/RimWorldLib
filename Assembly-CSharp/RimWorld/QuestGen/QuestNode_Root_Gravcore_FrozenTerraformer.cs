using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002250 RID: 8784
	public class QuestNode_Root_Gravcore_FrozenTerraformer : QuestNode_Root_Gravcore
	{
		// Token: 0x0600D930 RID: 55600 RVA: 0x0050756C File Offset: 0x0050576C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			PlanetTile tile;
			if (!this.TryFindSiteTile(out tile))
			{
				Log.Error("Could not find valid site tile for frozen terraformer quest.");
				return;
			}
			string text = QuestGenUtility.HardcodedSignalWithQuestID("site.MapGenerated");
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.MapRemoved");
			Site site = QuestGen_Sites.GenerateSite(new SitePartDefWithParams[]
			{
				new SitePartDefWithParams(SitePartDefOf.FrozenTerraformer, new SitePartParams
				{
					points = slate.Get<float>("points", 0f, false),
					threatPoints = slate.Get<float>("points", 0f, false)
				})
			}, tile, null, false, null, WorldObjectDefOf.ClaimableSite);
			slate.Set<Site>("site", site, false);
			quest.SpawnWorldObject(site, null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_DefinedThingDef(ThingDefOf.Gravcore));
			choice.rewards.Add(new Reward_DefinedThingDef(ThingDefOf.GravlitePanel));
			quest.RewardChoice(null, null).choices.Add(choice);
			Quest quest2 = quest;
			LetterDef neutralEvent = LetterDefOf.NeutralEvent;
			string inSignal2 = text;
			string chosenPawnSignal = null;
			Faction relatedFaction = null;
			MapParent useColonistsOnMap = null;
			bool useColonistsFromCaravanArg = false;
			QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly;
			string label = "FrozenTerraformerArrivedLetter".Translate();
			string text2 = "FrozenTerraformerArrivedLetterText".Translate();
			quest2.Letter(neutralEvent, inSignal2, chosenPawnSignal, relatedFaction, useColonistsOnMap, useColonistsFromCaravanArg, signalListenMode, Gen.YieldSingle<Map>(site.Map), false, text2, null, label, null, null);
			quest.End(QuestEndOutcome.Success, 0, null, text, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.End(QuestEndOutcome.Unknown, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, false, false);
		}
	}
}
