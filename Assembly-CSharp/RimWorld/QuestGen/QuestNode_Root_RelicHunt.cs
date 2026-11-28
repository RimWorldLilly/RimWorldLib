using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200228F RID: 8847
	public class QuestNode_Root_RelicHunt : QuestNode
	{
		// Token: 0x0600DA76 RID: 55926 RVA: 0x00512EA4 File Offset: 0x005110A4
		protected override void RunInt()
		{
			if (!ModLister.CheckIdeology("Relic hunt rescue"))
			{
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null, true);
			float num = slate.Get<float>("points", 0f, false);
			Ideo primaryIdeo = Faction.OfPlayer.ideos.PrimaryIdeo;
			Precept_Relic precept_Relic;
			this.TryGetRandomPlayerRelic(out precept_Relic);
			PlanetTile tile;
			this.TryFindSiteTile(out tile, false);
			string text = QuestGen.GenerateNewSignal("SubquestsCompleted", true);
			string awakenSecurityThreatsSignal = QuestGen.GenerateNewSignal("AwakenSecurityThreats", true);
			QuestGen.GenerateNewSignal("PostMapAdded", true);
			string text2 = QuestGen.GenerateNewSignal("RelicLostFromMap", true);
			bool allowViolentQuests = Find.Storyteller.difficulty.allowViolentQuests;
			slate.Set<bool>("classicMode", Find.IdeoManager.classicMode, false);
			slate.Set<int>("relicGlobalCount", primaryIdeo.PreceptsListForReading.Count((Precept x) => x is Precept_Relic), false);
			QuestPart_SubquestGenerator_RelicHunt questPart_SubquestGenerator_RelicHunt = new QuestPart_SubquestGenerator_RelicHunt();
			questPart_SubquestGenerator_RelicHunt.inSignalEnable = QuestGen.slate.Get<string>("inSignal", null, false);
			questPart_SubquestGenerator_RelicHunt.interval = new IntRange(300000, 600000);
			questPart_SubquestGenerator_RelicHunt.relic = precept_Relic;
			questPart_SubquestGenerator_RelicHunt.relicSlateName = "relic";
			questPart_SubquestGenerator_RelicHunt.useMapParentThreatPoints = ((map != null) ? map.Parent : null);
			questPart_SubquestGenerator_RelicHunt.expiryInfoPartKey = "RelicInfoFound";
			questPart_SubquestGenerator_RelicHunt.maxSuccessfulSubquests = 5;
			questPart_SubquestGenerator_RelicHunt.subquestDefs.AddRange(this.GetAllSubquests(QuestGen.Root));
			questPart_SubquestGenerator_RelicHunt.outSignalsCompleted.Add(text);
			quest.AddPart(questPart_SubquestGenerator_RelicHunt);
			QuestGenUtility.RunAdjustPointsForDistantFight();
			num = slate.Get<float>("points", 0f, false);
			Thing thing = precept_Relic.GenerateRelic();
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("relicThing.StartedExtractingFromContainer");
			quest.SignalPass(null, inSignal, awakenSecurityThreatsSignal);
			Reward_Items reward_Items = new Reward_Items();
			reward_Items.items.Add(thing);
			QuestPart_Choice questPart_Choice = quest.RewardChoice(null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(reward_Items);
			questPart_Choice.choices.Add(choice);
			if (Faction.OfPlayer.ideos.FluidIdeo != null)
			{
				choice.rewards.Add(new Reward_DevelopmentPoints(quest));
			}
			float num2 = allowViolentQuests ? num : 0f;
			SitePartParams sitePartParams = new SitePartParams
			{
				points = num2,
				relicThing = thing,
				triggerSecuritySignal = awakenSecurityThreatsSignal,
				relicLostSignal = text2
			};
			if (num2 > 0f)
			{
				sitePartParams.exteriorThreatPoints = QuestNode_Root_RelicHunt.ExteriorThreatPointsOverPoints.Evaluate(num2);
				sitePartParams.interiorThreatPoints = QuestNode_Root_RelicHunt.InteriorThreatPointsOverPoints.Evaluate(num2);
			}
			Site site = QuestGen_Sites.GenerateSite(Gen.YieldSingle<SitePartDefWithParams>(new SitePartDefWithParams(SitePartDefOf.AncientAltar, sitePartParams)), tile, Faction.OfAncientsHostile, false, null, null);
			quest.SpawnWorldObject(site, null, text);
			TaggedString t = "LetterTextRelicFoundLocation".Translate(precept_Relic.Label);
			if (allowViolentQuests)
			{
				t += "\n\n" + "LetterTextRelicFoundSecurityThreats".Translate(180000.ToStringTicksToPeriodVague(true, true)).Resolve();
			}
			Quest quest2 = quest;
			LetterDef relicHuntInstallationFound = LetterDefOf.RelicHuntInstallationFound;
			string inSignal2 = text;
			string chosenPawnSignal = null;
			Faction relatedFaction = null;
			MapParent useColonistsOnMap = null;
			bool useColonistsFromCaravanArg = false;
			QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly;
			string label = "LetterLabelRelicFound".Translate() + ": " + precept_Relic.Label;
			string text3 = t.Resolve();
			quest2.Letter(relicHuntInstallationFound, inSignal2, chosenPawnSignal, relatedFaction, useColonistsOnMap, useColonistsFromCaravanArg, signalListenMode, Gen.YieldSingle<Site>(site), false, text3, null, label, null, null);
			quest.DescriptionPart("[questDescriptionPartBeforeDiscovery]", quest.AddedSignal, text, QuestPart.SignalListenMode.OngoingOrNotYetAccepted, null);
			if (allowViolentQuests)
			{
				quest.DescriptionPart("RelicHuntFoundRelicSite".Translate(), text, null, QuestPart.SignalListenMode.OngoingOnly, null);
				QuestPart_Delay questPart_Delay = new QuestPart_Delay();
				questPart_Delay.delayTicks = 180000;
				questPart_Delay.alertLabel = "AncientAltarThreatsWaking".Translate();
				questPart_Delay.alertExplanation = "AncientAltarThreatsWakingDesc".Translate().Resolve();
				questPart_Delay.ticksLeftAlertCritical = 2500;
				questPart_Delay.inSignalEnable = text;
				questPart_Delay.alertCulprits.Add(thing);
				questPart_Delay.alertCulprits.Add(site);
				questPart_Delay.isBad = true;
				questPart_Delay.outSignalsCompleted.Add(awakenSecurityThreatsSignal);
				quest.AddPart(questPart_Delay);
				string text4 = QuestGen.GenerateNewSignal("ReTriggerSecurityThreats", true);
				QuestPart_PassWhileActive questPart_PassWhileActive = new QuestPart_PassWhileActive();
				questPart_PassWhileActive.inSignalEnable = awakenSecurityThreatsSignal;
				questPart_PassWhileActive.inSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.MapGenerated");
				questPart_PassWhileActive.outSignal = text4;
				quest.AddPart(questPart_PassWhileActive);
				quest.SignalPass(delegate
				{
					quest.SignalPass(null, null, awakenSecurityThreatsSignal);
					quest.Message("MessageAncientAltarThreatsAlerted".Translate(), MessageTypeDefOf.NegativeEvent, false, null, null, null);
				}, text4, null);
				quest.AnyHostileThreatToPlayer(site, true, delegate
				{
					quest.Message("MessageAncientAltarThreatsWokenUp".Translate(), MessageTypeDefOf.NegativeEvent, false, null, null, null);
				}, null, awakenSecurityThreatsSignal, null, null, QuestPart.SignalListenMode.OngoingOnly);
			}
			else
			{
				quest.DescriptionPart("RelicHuntFoundRelicSitePeaceful".Translate(), text, null, QuestPart.SignalListenMode.OngoingOnly, null);
			}
			quest.End(QuestEndOutcome.Fail, 0, null, QuestGenUtility.HardcodedSignalWithQuestID("relicThing.Destroyed"), QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.End(QuestEndOutcome.Success, 0, null, text2, QuestPart.SignalListenMode.OngoingOnly, true, false);
			slate.Set<Ideo>("ideo", primaryIdeo, false);
			slate.Set<Precept_Relic>("relic", precept_Relic, false);
			slate.Set<Thing>("relicThing", thing, false);
			slate.Set<Site>("site", site, false);
		}

		// Token: 0x0600DA77 RID: 55927 RVA: 0x00513440 File Offset: 0x00511640
		private bool TryFindSiteTile(out PlanetTile tile, bool exitOnFirstTileFound = false)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 2, 10, false, null, 0.5f, true, TileFinderMode.Near, exitOnFirstTileFound, false, null, null);
		}

		// Token: 0x0600DA78 RID: 55928 RVA: 0x00513463 File Offset: 0x00511663
		private bool TryGetRandomPlayerRelic(out Precept_Relic relic)
		{
			return (from p in Faction.OfPlayer.ideos.PrimaryIdeo.GetAllPreceptsOfType<Precept_Relic>()
			where p.CanGenerateRelic
			select p).TryRandomElement(out relic);
		}

		// Token: 0x0600DA79 RID: 55929 RVA: 0x005134A4 File Offset: 0x005116A4
		private IEnumerable<QuestScriptDef> GetAllSubquests(QuestScriptDef parent)
		{
			return from q in DefDatabase<QuestScriptDef>.AllDefs
			where q.epicParent == parent
			select q;
		}

		// Token: 0x0600DA7A RID: 55930 RVA: 0x005134D4 File Offset: 0x005116D4
		protected override bool TestRunInt(Slate slate)
		{
			Precept_Relic precept_Relic;
			PlanetTile planetTile;
			return this.TryGetRandomPlayerRelic(out precept_Relic) && this.TryFindSiteTile(out planetTile, true) && this.GetAllSubquests(QuestGen.Root).Any<QuestScriptDef>();
		}

		// Token: 0x04008B90 RID: 35728
		private const int RelicsInfoRequiredCount = 5;

		// Token: 0x04008B91 RID: 35729
		private const int MinIntervalTicks = 300000;

		// Token: 0x04008B92 RID: 35730
		private const int MaxIntervalTicks = 600000;

		// Token: 0x04008B93 RID: 35731
		private const int MinDistanceFromColony = 2;

		// Token: 0x04008B94 RID: 35732
		private const int MaxDistanceFromColony = 10;

		// Token: 0x04008B95 RID: 35733
		private const int SecurityWakeupDelayTicks = 180000;

		// Token: 0x04008B96 RID: 35734
		private const int SecurityWakupDelayCriticalTicks = 2500;

		// Token: 0x04008B97 RID: 35735
		private static readonly SimpleCurve ExteriorThreatPointsOverPoints = new SimpleCurve
		{
			{
				new CurvePoint(0f, 500f),
				true
			},
			{
				new CurvePoint(500f, 500f),
				true
			},
			{
				new CurvePoint(10000f, 10000f),
				true
			}
		};

		// Token: 0x04008B98 RID: 35736
		private static readonly SimpleCurve InteriorThreatPointsOverPoints = new SimpleCurve
		{
			{
				new CurvePoint(0f, 300f),
				true
			},
			{
				new CurvePoint(300f, 300f),
				true
			},
			{
				new CurvePoint(10000f, 5000f),
				true
			}
		};
	}
}
