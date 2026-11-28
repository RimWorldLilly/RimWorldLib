using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002274 RID: 8820
	public class QuestNode_Root_Hack_Spacedrone : QuestNode
	{
		// Token: 0x0600D9D8 RID: 55768 RVA: 0x0050D538 File Offset: 0x0050B738
		protected override void RunInt()
		{
			if (!ModLister.CheckIdeology("Spacedrone hack"))
			{
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null, false);
			float num = slate.Get<float>("points", 0f, false);
			Precept_Relic precept_Relic = slate.Get<Precept_Relic>("relic", null, false);
			bool allowViolentQuests = Find.Storyteller.difficulty.allowViolentQuests;
			IntVec3 landingSpot;
			QuestPart_SpawnSpaceDrone.TryFindSpacedronePosition(map, out landingSpot);
			Thing spacedrone = ThingMaker.MakeThing(ThingDefOf.Spacedrone, null);
			string text = QuestGen.GenerateNewSignal("RaidsDelay", true);
			string spacedroneDestroyedSignal = QuestGenUtility.HardcodedSignalWithQuestID("spacedrone.Destroyed");
			string spacedroneHackedSignal = QuestGenUtility.HardcodedSignalWithQuestID("spacedrone.Hacked");
			string triggerRaidSignal = QuestGen.GenerateNewSignal("TriggerRaid", true);
			string text2 = QuestGen.GenerateNewSignal("QuestEndSuccess", true);
			string text3 = QuestGen.GenerateNewSignal("QuestEndFailure", true);
			string text4 = QuestGen.GenerateNewSignal("SpacedroneDelayDestroy", true);
			if (precept_Relic == null)
			{
				precept_Relic = Faction.OfPlayer.ideos.PrimaryIdeo.GetAllPreceptsOfType<Precept_Relic>().RandomElement<Precept_Relic>();
				Log.Warning("Spacedrone Hack quest requires relic from parent quest. None found so picking random player relic");
			}
			quest.Delay(120, delegate
			{
				Quest quest3 = quest;
				LetterDef neutralEvent = LetterDefOf.NeutralEvent;
				string inSignal = null;
				string chosenPawnSignal2 = null;
				Faction relatedFaction2 = null;
				MapParent useColonistsOnMap2 = null;
				bool useColonistsFromCaravanArg2 = false;
				QuestPart.SignalListenMode signalListenMode2 = QuestPart.SignalListenMode.OngoingOnly;
				string label = "LetterLabelSpacedroneIncoming".Translate();
				string text7 = "LetterTextSpacedroneIncoming".Translate();
				quest3.Letter(neutralEvent, inSignal, chosenPawnSignal2, relatedFaction2, useColonistsOnMap2, useColonistsFromCaravanArg2, signalListenMode2, Gen.YieldSingle<Thing>(spacedrone), false, text7, null, label, null, null);
				quest.SpawnSpaceDrone(map, ThingDefOf.SpacedroneIncoming, Gen.YieldSingle<Thing>(spacedrone), null, new IntVec3?(landingSpot), null, false, false, null, null);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, true);
			if (allowViolentQuests)
			{
				Faction faction;
				this.TryFindEnemyFaction(out faction);
				QuestPart_FactionGoodwillLocked questPart_FactionGoodwillLocked = new QuestPart_FactionGoodwillLocked();
				questPart_FactionGoodwillLocked.faction1 = Faction.OfPlayer;
				questPart_FactionGoodwillLocked.faction2 = faction;
				questPart_FactionGoodwillLocked.inSignalEnable = QuestGen.slate.Get<string>("inSignal", null, false);
				quest.AddPart(questPart_FactionGoodwillLocked);
				QuestPart_Delay questPart_Delay = new QuestPart_Delay();
				questPart_Delay.delayTicks = QuestNode_Root_Hack_Spacedrone.RaidDelayTicksRange.RandomInRange;
				questPart_Delay.alertLabel = "QuestPartRaidsDelay".Translate();
				questPart_Delay.alertExplanation = "QuestPartRaidsDelayDesc".Translate();
				questPart_Delay.ticksLeftAlertCritical = 60000;
				questPart_Delay.inSignalEnable = QuestGen.slate.Get<string>("inSignal", null, false);
				questPart_Delay.alertCulprits.Add(spacedrone);
				questPart_Delay.isBad = true;
				questPart_Delay.outSignalsCompleted.Add(text);
				questPart_Delay.waitUntilPlayerHasHomeMap = true;
				quest.AddPart(questPart_Delay);
				quest.Signal(text, delegate
				{
					QuestPart_PassOutInterval questPart_PassOutInterval = new QuestPart_PassOutInterval();
					questPart_PassOutInterval.inSignalEnable = QuestGen.slate.Get<string>("inSignal", null, false);
					questPart_PassOutInterval.outSignals.Add(triggerRaidSignal);
					questPart_PassOutInterval.inSignalsDisable.Add(spacedroneHackedSignal);
					questPart_PassOutInterval.inSignalsDisable.Add(spacedroneDestroyedSignal);
					questPart_PassOutInterval.ticksInterval = QuestNode_Root_Hack_Spacedrone.RaidIntervalTicksRange;
					quest.AddPart(questPart_PassOutInterval);
				}, null, QuestPart.SignalListenMode.OngoingOnly);
				QuestPart_RandomRaid questPart_RandomRaid = new QuestPart_RandomRaid();
				questPart_RandomRaid.inSignal = triggerRaidSignal;
				questPart_RandomRaid.mapParent = map.Parent;
				questPart_RandomRaid.pointsRange = new FloatRange(num * QuestNode_Root_Hack_Spacedrone.MinRaidThreatPointsFactor, num * QuestNode_Root_Hack_Spacedrone.MaxRaidThreatPointsFactor);
				questPart_RandomRaid.faction = faction;
				questPart_RandomRaid.arrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
				questPart_RandomRaid.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
				questPart_RandomRaid.attackTargets = new List<Thing>
				{
					spacedrone
				};
				questPart_RandomRaid.generateFightersOnly = true;
				questPart_RandomRaid.customLetterLabel = "Raid".Translate() + ": " + faction.Name;
				questPart_RandomRaid.customLetterText = "LetterTextRaidSpacedrone".Translate(faction.NameColored, spacedrone.LabelCap).Resolve();
				questPart_RandomRaid.fallbackToPlayerHomeMap = true;
				quest.AddPart(questPart_RandomRaid);
				slate.Set<Faction>("enemyFaction", faction, false);
				slate.Set<bool>("enemyFactionNeolithic", faction.def.techLevel == TechLevel.Neolithic, false);
			}
			Quest quest4 = quest;
			int spacedroneDestroyDelayTicks = QuestNode_Root_Hack_Spacedrone.SpacedroneDestroyDelayTicks;
			Action inner = delegate()
			{
			};
			string inSignalEnable = null;
			string inSignalDisable = null;
			string text5 = "SpacedroneSelfDestructsIn".Translate();
			quest4.Delay(spacedroneDestroyDelayTicks, inner, inSignalEnable, inSignalDisable, text4, false, null, null, false, text5, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
			Quest quest2 = quest;
			LetterDef positiveEvent = LetterDefOf.PositiveEvent;
			string spacedroneHackedSignal2 = spacedroneHackedSignal;
			string chosenPawnSignal = null;
			Faction relatedFaction = null;
			MapParent useColonistsOnMap = null;
			bool useColonistsFromCaravanArg = false;
			QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("Hacked".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted(spacedrone.LabelCap);
			text5 = defaultInterpolatedStringHandler.ToStringAndClear();
			string text6 = "LetterTextSpacedroneHacked".Translate(spacedrone.LabelCap, precept_Relic.ThingDef.LabelCap);
			quest2.Letter(positiveEvent, spacedroneHackedSignal2, chosenPawnSignal, relatedFaction, useColonistsOnMap, useColonistsFromCaravanArg, signalListenMode, Gen.YieldSingle<Thing>(spacedrone), false, text6, null, text5, null, null);
			quest.AnySignal(new string[]
			{
				text4,
				spacedroneHackedSignal
			}, delegate
			{
				quest.StartWick(spacedrone, null);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
			QuestPart_Filter_AllThingsHacked questPart_Filter_AllThingsHacked = new QuestPart_Filter_AllThingsHacked();
			questPart_Filter_AllThingsHacked.things.Add(spacedrone);
			questPart_Filter_AllThingsHacked.inSignal = spacedroneDestroyedSignal;
			questPart_Filter_AllThingsHacked.outSignal = text2;
			questPart_Filter_AllThingsHacked.outSignalElse = text3;
			quest.AddPart(questPart_Filter_AllThingsHacked);
			Reward_RelicInfo reward_RelicInfo = new Reward_RelicInfo();
			reward_RelicInfo.relic = precept_Relic;
			reward_RelicInfo.quest = quest;
			QuestPart_Choice questPart_Choice = quest.RewardChoice(null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(reward_RelicInfo);
			questPart_Choice.choices.Add(choice);
			quest.End(QuestEndOutcome.Fail, 0, null, text3, QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.End(QuestEndOutcome.Success, 0, null, text2, QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.End(QuestEndOutcome.Fail, 0, null, QuestGenUtility.HardcodedSignalWithQuestID("map.MapRemoved"), QuestPart.SignalListenMode.OngoingOnly, false, false);
			slate.Set<Map>("map", map, false);
			slate.Set<Precept_Relic>("relic", precept_Relic, false);
			slate.Set<Thing>("spacedrone", spacedrone, false);
			slate.Set<float>("raidIntervalAvg", QuestNode_Root_Hack_Spacedrone.RaidIntervalTicksRange.Average, false);
			slate.Set<bool>("allowViolence", allowViolentQuests, false);
			slate.Set<int>("destroyDelayTicks", QuestNode_Root_Hack_Spacedrone.SpacedroneDestroyDelayTicks, false);
		}

		// Token: 0x0600D9D9 RID: 55769 RVA: 0x0050DB18 File Offset: 0x0050BD18
		protected override bool TestRunInt(Slate slate)
		{
			Faction faction;
			if (Find.Storyteller.difficulty.allowViolentQuests && !this.TryFindEnemyFaction(out faction))
			{
				return false;
			}
			Map map = QuestGen_Get.GetMap(false, null, false);
			IntVec3 intVec;
			return map != null && QuestPart_SpawnSpaceDrone.TryFindSpacedronePosition(map, out intVec);
		}

		// Token: 0x0600D9DA RID: 55770 RVA: 0x0050DB60 File Offset: 0x0050BD60
		private bool CanUseFaction(Faction f)
		{
			return !f.temporary && !f.defeated && !f.IsPlayer && (f.def.humanlikeFaction || f == Faction.OfMechanoids) && (!f.Hidden || f == Faction.OfMechanoids) && f.HostileTo(Faction.OfPlayer);
		}

		// Token: 0x0600D9DB RID: 55771 RVA: 0x0050DBB7 File Offset: 0x0050BDB7
		private bool TryFindEnemyFaction(out Faction enemyFaction)
		{
			return Find.FactionManager.AllFactions.Where(new Func<Faction, bool>(this.CanUseFaction)).TryRandomElement(out enemyFaction);
		}

		// Token: 0x04008B44 RID: 35652
		private const int QuestStartDelay = 120;

		// Token: 0x04008B45 RID: 35653
		private static IntRange RaidDelayTicksRange = new IntRange(18000, 30000);

		// Token: 0x04008B46 RID: 35654
		private static IntRange RaidIntervalTicksRange = new IntRange(17500, 22500);

		// Token: 0x04008B47 RID: 35655
		private static float MinRaidThreatPointsFactor = 0.3f;

		// Token: 0x04008B48 RID: 35656
		private static float MaxRaidThreatPointsFactor = 0.55f;

		// Token: 0x04008B49 RID: 35657
		private static int SpacedroneDestroyDelayTicks = 1800000;
	}
}
