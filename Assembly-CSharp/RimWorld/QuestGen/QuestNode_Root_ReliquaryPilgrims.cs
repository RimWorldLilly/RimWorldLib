using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02002290 RID: 8848
	public class QuestNode_Root_ReliquaryPilgrims : QuestNode
	{
		// Token: 0x0600DA7D RID: 55933 RVA: 0x005135B8 File Offset: 0x005117B8
		protected override void RunInt()
		{
			if (!ModLister.CheckIdeology("Reliquary pilgrims"))
			{
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = this.GetMap();
			int randomInRange = QuestNode_Root_ReliquaryPilgrims.PilgrimCount.RandomInRange;
			if (map == null)
			{
				return;
			}
			FactionDef factionDef;
			PawnKindDef kindDef;
			this.GetFactionAndPawnKind(out factionDef, out kindDef);
			Precept_Relic precept_Relic;
			Building building;
			Thing var;
			QuestNode_Root_ReliquaryPilgrims.TryFindReliquaryWithRelic(map, out precept_Relic, out building, out var);
			List<FactionRelation> list = new List<FactionRelation>();
			foreach (Faction faction2 in Find.FactionManager.AllFactionsListForReading)
			{
				if (!faction2.def.PermanentlyHostileTo(factionDef))
				{
					list.Add(new FactionRelation
					{
						other = faction2,
						kind = FactionRelationKind.Neutral
					});
				}
			}
			Faction faction = FactionGenerator.NewGeneratedFactionWithRelations(factionDef, list, true);
			faction.temporary = true;
			Find.FactionManager.Add(faction);
			List<Pawn> list2 = new List<Pawn>();
			int num = 0;
			bool var2 = false;
			if (Find.Storyteller.difficulty.ChildrenAllowed)
			{
				List<ValueTuple<int, float>> list3 = new List<ValueTuple<int, float>>();
				list3.Add(new ValueTuple<int, float>(0, 0.25f));
				list3.Add(new ValueTuple<int, float>(Rand.Range(1, Mathf.Max(1, randomInRange / 2)), 0.5f));
				list3.Add(new ValueTuple<int, float>(randomInRange, 0.25f));
				ValueTuple<int, float> valueTuple;
				list3.TryRandomElementByWeight((ValueTuple<int, float> p) => p.Item2, out valueTuple);
				num = valueTuple.Item1;
				var2 = (randomInRange == num);
			}
			slate.Set<int>("childCount", num, false);
			slate.Set<bool>("allChildren", var2, false);
			for (int i = 0; i < randomInRange; i++)
			{
				DevelopmentalStage developmentalStages = (i >= randomInRange - num) ? DevelopmentalStage.Child : DevelopmentalStage.Adult;
				Pawn pawn = quest.GeneratePawn_NewTemp(kindDef, faction, new PlanetTile?(map.Tile), true, null, 0f, true, null, 0f, 0f, false, true, developmentalStages, false);
				pawn.ideo.SetIdeo(precept_Relic.ideo);
				list2.Add(pawn);
			}
			quest.SetFactionHidden(faction, false, null);
			quest.PawnsArrive(list2, null, map.Parent, null, false, null, "[pilgrimsArrivedLetterLabel]", "[pilgrimsArrivedLetterText]", null, null, false, false, true);
			string text = QuestGen.GenerateNewSignal("VenerationCompleted", true);
			string text2 = QuestGenUtility.HardcodedSignalWithQuestID("relicThing.Despawned");
			QuestPart_Venerate questPart_Venerate = new QuestPart_Venerate();
			questPart_Venerate.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
			questPart_Venerate.inSignalForceExit = text2;
			questPart_Venerate.pawns.AddRange(list2);
			questPart_Venerate.target = building;
			questPart_Venerate.venerateDurationTicks = QuestNode_Root_ReliquaryPilgrims.VisitDurationTicksRange.RandomInRange;
			questPart_Venerate.faction = faction;
			questPart_Venerate.mapParent = map.Parent;
			questPart_Venerate.outSignalVenerationCompleted = text;
			quest.AddPart(questPart_Venerate);
			Quest quest2 = quest;
			string message = "[pilgrimsLeavingMessage]";
			MessageTypeDef neutralEvent = MessageTypeDefOf.NeutralEvent;
			bool getLookTargetsFromSignal = false;
			RulePack rules = null;
			string inSignal = text;
			quest2.Message(message, neutralEvent, getLookTargetsFromSignal, rules, list2, inSignal);
			string text3 = QuestGen.GenerateNewSignal("AllLeftMap", true);
			QuestPart_PassAll questPart_PassAll = new QuestPart_PassAll();
			questPart_PassAll.outSignal = text3;
			quest.AddPart(questPart_PassAll);
			slate.Set<List<Pawn>>("pawns", list2, false);
			slate.Set<Faction>("faction", faction, false);
			foreach (Pawn pawn2 in list2)
			{
				string text4 = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(pawn2.ThingID);
				QuestUtility.AddQuestTag(pawn2, text4);
				string item = QuestGenUtility.HardcodedSignalWithQuestID(text4 + ".LeftMap");
				questPart_PassAll.inSignals.Add(item);
			}
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.totalMarketValueRange = new FloatRange?(QuestNode_Root_ReliquaryPilgrims.RewardMarketValueRange);
			parms.qualityGenerator = new QualityGenerator?(QualityGenerator.Reward);
			parms.makingFaction = faction;
			parms.countRange = new IntRange?(new IntRange(1, 1));
			List<Thing> list4 = ThingSetMakerDefOf.Reward_ReliquaryPilgrims.root.Generate(parms);
			QuestPart_DelayedRewardDropPods delayedReward = new QuestPart_DelayedRewardDropPods();
			delayedReward.inSignal = text3;
			delayedReward.faction = faction;
			delayedReward.giver = list2[0];
			delayedReward.rewards.AddRange(list4);
			delayedReward.delayTicks = QuestNode_Root_ReliquaryPilgrims.RewardDelayRangeTicks.RandomInRange;
			delayedReward.chance = QuestNode_Root_ReliquaryPilgrims.RewardChance;
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				delayedReward.customLetterText = x;
			}, QuestGenUtility.MergeRules(null, "[delayedRewardLetterText]", "root"));
			quest.Message("[pilgrimsLeftMessage]", MessageTypeDefOf.NeutralEvent, false, null, null, text3);
			quest.AddPart(delayedReward);
			string item2 = QuestGenUtility.HardcodedSignalWithQuestID("pawns.Arrested");
			string item3 = QuestGenUtility.HardcodedSignalWithQuestID("pawns.Killed");
			quest.AnySignal(new List<string>
			{
				item2,
				item3
			}, delegate
			{
				quest.RecordHistoryEvent(HistoryEventDefOf.CharityRefused_Pilgrims_Betrayed, null, QuestPart.SignalListenMode.Always);
				QuestPart_FactionRelationChange questPart_FactionRelationChange = new QuestPart_FactionRelationChange();
				questPart_FactionRelationChange.faction = faction;
				questPart_FactionRelationChange.relationKind = FactionRelationKind.Hostile;
				questPart_FactionRelationChange.canSendHostilityLetter = false;
				questPart_FactionRelationChange.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
				quest.AddPart(questPart_FactionRelationChange);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
			string text5 = QuestGen.GenerateNewSignal("RelicInvalidated", true);
			QuestPart_PassAnyActivable questPart_PassAnyActivable = new QuestPart_PassAnyActivable();
			questPart_PassAnyActivable.inSignalEnable = QuestGen.slate.Get<string>("inSignal", null, false);
			questPart_PassAnyActivable.inSignals.Add(text2);
			questPart_PassAnyActivable.inSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID("reliquary.Destroyed"));
			questPart_PassAnyActivable.outSignalsCompleted.Add(text5);
			questPart_PassAnyActivable.inSignalDisable = text;
			quest.AddPart(questPart_PassAnyActivable);
			quest.RecordHistoryEvent(HistoryEventDefOf.CharityRefused_Pilgrims, text2, QuestPart.SignalListenMode.OngoingOnly);
			QuestPart_Choice questPart_Choice = quest.RewardChoice(null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_PossibleFutureReward());
			if (Faction.OfPlayer.ideos.FluidIdeo != null)
			{
				choice.rewards.Add(new Reward_DevelopmentPoints(quest));
			}
			questPart_Choice.choices.Add(choice);
			quest.End(QuestEndOutcome.Fail, 0, null, text5, QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.End(QuestEndOutcome.Fail, 0, null, QuestGenUtility.HardcodedSignalWithQuestID("map.MapRemoved"), QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.End(QuestEndOutcome.Fail, 0, null, QuestGenUtility.HardcodedSignalWithQuestID("faction.BecameHostileToPlayer"), QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.End(QuestEndOutcome.Success, 0, null, text3, QuestPart.SignalListenMode.OngoingOnly, false, false);
			slate.Set<Map>("map", map, false);
			slate.Set<Precept_Relic>("relic", precept_Relic, false);
			slate.Set<Thing>("relicThing", var, false);
			slate.Set<Building>("reliquary", building, false);
			slate.Set<int>("pilgrimCount", randomInRange, false);
			slate.Set<string>("rewards", GenLabel.ThingsLabel(list4, "  - "), false);
			slate.Set<float>("rewardsMarketValue", TradeUtility.TotalMarketValue(list4), false);
			slate.Set<string>("venerateDate", GenDate.DateFullStringAt((long)GenDate.TickGameToAbs(quest.acceptanceTick), Find.WorldGrid.LongLatOf(map.Tile)), false);
			slate.Set<bool>("pilgrimFaction", faction.def == FactionDefOf.Pilgrims, false);
		}

		// Token: 0x0600DA7E RID: 55934 RVA: 0x00513D00 File Offset: 0x00511F00
		private static bool TryFindReliquaryWithRelic(Map map, out Precept_Relic relic, out Building reliquary, out Thing relicThing)
		{
			foreach (Thing thing in map.listerThings.ThingsOfDef(ThingDefOf.Reliquary).InRandomOrder(null))
			{
				CompThingContainer compThingContainer = thing.TryGetComp<CompThingContainer>();
				if (compThingContainer != null)
				{
					foreach (Thing thing2 in ((IEnumerable<Thing>)compThingContainer.GetDirectlyHeldThings()))
					{
						Precept_Relic precept_Relic = thing2.StyleSourcePrecept as Precept_Relic;
						if (precept_Relic != null)
						{
							reliquary = (Building)thing;
							relic = precept_Relic;
							relicThing = thing2;
							return true;
						}
					}
				}
			}
			reliquary = null;
			relic = null;
			relicThing = null;
			return false;
		}

		// Token: 0x0600DA7F RID: 55935 RVA: 0x00513DCC File Offset: 0x00511FCC
		private void GetFactionAndPawnKind(out FactionDef factionDef, out PawnKindDef pawnKind)
		{
			if (Rand.Bool)
			{
				factionDef = FactionDefOf.Pilgrims;
				pawnKind = PawnKindDefOf.PovertyPilgrim;
				return;
			}
			factionDef = FactionDefOf.OutlanderCivil;
			pawnKind = PawnKindDefOf.WellEquippedTraveler;
		}

		// Token: 0x0600DA80 RID: 55936 RVA: 0x00513DF2 File Offset: 0x00511FF2
		protected override bool TestRunInt(Slate slate)
		{
			return this.GetMap() != null;
		}

		// Token: 0x0600DA81 RID: 55937 RVA: 0x00513E00 File Offset: 0x00512000
		private Map GetMap()
		{
			List<Map> source = Find.Maps.Where(new Func<Map, bool>(QuestNode_Root_ReliquaryPilgrims.<GetMap>g__Validator|10_0)).ToList<Map>();
			Map result;
			if (!(from x in source
			where x.mapPawns.FreeColonists.Count >= 1
			select x).TryRandomElement(out result))
			{
				(from x in source
				where x.mapPawns.FreeColonists.Any<Pawn>()
				select x).TryRandomElement(out result);
			}
			return result;
		}

		// Token: 0x0600DA84 RID: 55940 RVA: 0x00513EF0 File Offset: 0x005120F0
		[CompilerGenerated]
		internal static bool <GetMap>g__Validator|10_0(Map m)
		{
			Precept_Relic precept_Relic;
			Building building;
			Thing thing;
			return m.IsPlayerHome && !m.Tile.LayerDef.isSpace && QuestNode_Root_ReliquaryPilgrims.TryFindReliquaryWithRelic(m, out precept_Relic, out building, out thing);
		}

		// Token: 0x04008B99 RID: 35737
		private static IntRange VisitDurationTicksRange = new IntRange(5000, 10000);

		// Token: 0x04008B9A RID: 35738
		private static IntRange PilgrimCount = new IntRange(1, 4);

		// Token: 0x04008B9B RID: 35739
		private static float RewardChance = 0.5f;

		// Token: 0x04008B9C RID: 35740
		private static FloatRange RewardMarketValueRange = new FloatRange(1000f, 2000f);

		// Token: 0x04008B9D RID: 35741
		private static IntRange RewardDelayRangeTicks = new IntRange(300000, 600000);

		// Token: 0x04008B9E RID: 35742
		private const string RootSymbol = "root";
	}
}
