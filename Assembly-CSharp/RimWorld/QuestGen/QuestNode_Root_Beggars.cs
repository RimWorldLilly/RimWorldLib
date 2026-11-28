using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using LudeonTK;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002269 RID: 8809
	public class QuestNode_Root_Beggars : QuestNode
	{
		// Token: 0x17002615 RID: 9749
		// (get) Token: 0x0600D99E RID: 55710 RVA: 0x0050A3DA File Offset: 0x005085DA
		private static IEnumerable<ThingDef> AllowedThings
		{
			get
			{
				return new QuestNode_Root_Beggars.<get_AllowedThings>d__8(-2);
			}
		}

		// Token: 0x0600D99F RID: 55711 RVA: 0x0050A3E4 File Offset: 0x005085E4
		protected override void RunInt()
		{
			if (!ModLister.CheckIdeology("Beggars"))
			{
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null, false);
			float points = slate.Get<float>("points", 0f, false);
			slate.Set<int>("visitDurationTicks", 60000, false);
			float totalRequestValue = QuestNode_Root_Beggars.GetTotalRequestValue(points);
			ThingDef thingDef;
			int num;
			if (QuestNode_Root_Beggars.TryFindRandomRequestedThing(map, totalRequestValue, out thingDef, out num, QuestNode_Root_Beggars.AllowedThings))
			{
				slate.Set<ThingDef>("requestedThing", thingDef, false);
				slate.Set<string>("requestedThingDefName", thingDef.defName, false);
				slate.Set<int>("requestedThingCount", num, false);
			}
			int num2 = QuestNode_Root_Beggars.LodgerCountFromPopulation(slate, map);
			List<FactionRelation> list = new List<FactionRelation>();
			foreach (Faction faction3 in Find.FactionManager.AllFactionsListForReading)
			{
				if (!faction3.def.PermanentlyHostileTo(FactionDefOf.Beggars))
				{
					list.Add(new FactionRelation
					{
						other = faction3,
						kind = FactionRelationKind.Neutral
					});
				}
			}
			Faction faction = FactionGenerator.NewGeneratedFactionWithRelations(FactionDefOf.Beggars, list, true);
			faction.temporary = true;
			Find.FactionManager.Add(faction);
			slate.Set<Faction>("faction", faction, false);
			slate.Set<Map>("map", map, false);
			int num3 = 0;
			bool var = false;
			if (Find.Storyteller.difficulty.ChildrenAllowed)
			{
				List<ValueTuple<int, float>> list2 = new List<ValueTuple<int, float>>();
				list2.Add(new ValueTuple<int, float>(0, 0.4f));
				list2.Add(new ValueTuple<int, float>(Rand.Range(1, Mathf.Max(1, num2 / 2)), 0.4f));
				list2.Add(new ValueTuple<int, float>(num2, 0.2f));
				ValueTuple<int, float> valueTuple;
				list2.TryRandomElementByWeight((ValueTuple<int, float> p) => p.Item2, out valueTuple);
				num3 = valueTuple.Item1;
				var = (num2 == num3);
			}
			slate.Set<int>("childCount", num3, false);
			slate.Set<bool>("allChildren", var, false);
			List<Pawn> pawns = new List<Pawn>();
			for (int i = 0; i < num2; i++)
			{
				DevelopmentalStage developmentalStage = (i >= num2 - num3) ? DevelopmentalStage.Child : DevelopmentalStage.Adult;
				List<Pawn> pawns2 = pawns;
				Quest quest5 = quest;
				PawnKindDef beggar = PawnKindDefOf.Beggar;
				Faction faction2 = faction;
				PawnGenerationContext context = PawnGenerationContext.NonPlayer;
				PlanetTile? tile = new PlanetTile?(map.Tile);
				bool forceGenerateNewPawn = true;
				bool allowDead = false;
				bool allowDowned = false;
				bool canGeneratePawnRelations = true;
				bool mustBeCapableOfViolence = false;
				float colonistRelationChanceFactor = 1f;
				bool forceAddFreeWarmLayerIfNeeded = false;
				bool allowGay = true;
				bool allowPregnant = true;
				bool allowFood = true;
				bool allowAddictions = true;
				bool inhabitant = false;
				bool certainlyBeenInCryptosleep = false;
				bool forceRedressWorldPawnIfFormerColonist = false;
				bool worldPawnFactionDoesntMatter = false;
				float biocodeWeaponChance = 0f;
				float biocodeApparelChance = 0f;
				Pawn extraPawnForExtraRelationChance = null;
				float relationWithExtraPawnChanceFactor = 1f;
				Predicate<Pawn> validatorPreGear = null;
				Predicate<Pawn> validatorPostGear = null;
				IEnumerable<TraitDef> forcedTraits = null;
				IEnumerable<TraitDef> prohibitedTraits = null;
				DevelopmentalStage developmentalStages = developmentalStage;
				pawns2.Add(quest5.GeneratePawn(new PawnGenerationRequest(beggar, faction2, context, tile, forceGenerateNewPawn, allowDead, allowDowned, canGeneratePawnRelations, mustBeCapableOfViolence, colonistRelationChanceFactor, forceAddFreeWarmLayerIfNeeded, allowGay, allowPregnant, allowFood, allowAddictions, inhabitant, certainlyBeenInCryptosleep, forceRedressWorldPawnIfFormerColonist, worldPawnFactionDoesntMatter, biocodeWeaponChance, biocodeApparelChance, extraPawnForExtraRelationChance, relationWithExtraPawnChanceFactor, validatorPreGear, validatorPostGear, forcedTraits, prohibitedTraits, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, developmentalStages, null, null, null, false, false, false, -1, 0, false), false));
			}
			for (int j = 0; j < pawns.Count; j++)
			{
				Pawn pawn = pawns[j];
				if (pawn.inventory != null)
				{
					for (int k = pawn.inventory.innerContainer.Count - 1; k >= 0; k--)
					{
						if (pawn.inventory.innerContainer[k].def == thingDef)
						{
							pawn.inventory.innerContainer[k].Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
			slate.Set<List<Pawn>>("beggars", pawns, false);
			slate.Set<int>("beggarCount", num2, false);
			quest.SetFactionHidden(faction, false, null);
			quest.PawnsArrive(pawns, null, map.Parent, null, false, null, null, null, null, null, false, false, false);
			string text = QuestGenUtility.HardcodedTargetQuestTagWithQuestID("Receiver");
			string itemsReceivedSignal = QuestGen.GenerateNewSignal("ItemsReceived", true);
			Pawn pawn2 = pawns[0];
			QuestUtility.AddQuestTag(ref pawn2.questTags, text);
			QuestPart_BegForItems questPart_BegForItems = new QuestPart_BegForItems();
			questPart_BegForItems.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
			questPart_BegForItems.outSignalItemsReceived = itemsReceivedSignal;
			questPart_BegForItems.pawns.AddRange(pawns);
			questPart_BegForItems.target = pawn2;
			questPart_BegForItems.faction = faction;
			questPart_BegForItems.mapParent = map.Parent;
			questPart_BegForItems.thingDef = thingDef;
			questPart_BegForItems.amount = num;
			quest.AddPart(questPart_BegForItems);
			quest.GiveRewards(new RewardsGeneratorParams
			{
				rewardValue = totalRequestValue,
				allowDevelopmentPoints = true,
				thingRewardDisallowed = true
			}, null, null, null, null, null, null, null, null, false, null, false, false, null);
			if (ModsConfig.OdysseyActive)
			{
				string inSignal = QuestGenUtility.HardcodedSignalWithQuestID(text + ".ReceivedItems");
				List<QuestScriptDef> list3 = QuestUtility.GetGiverQuests(QuestGiverTag.Beggars).ToList<QuestScriptDef>();
				if (Rand.Chance(0.35f) && !list3.NullOrEmpty<QuestScriptDef>())
				{
					QuestPart_AddGiverQuest questPart_AddGiverQuest = new QuestPart_AddGiverQuest();
					questPart_AddGiverQuest.inSignal = inSignal;
					questPart_AddGiverQuest.questScript = list3.RandomElementByWeight((QuestScriptDef q) => q.rootSelectionWeight);
					questPart_AddGiverQuest.discoveryMethodTranslationKey = "QuestDiscoveredFromBeggar";
					questPart_AddGiverQuest.sendAvailableLetter = true;
					questPart_AddGiverQuest.points = points;
					quest.AddPart(questPart_AddGiverQuest);
				}
			}
			string pawnLabelSingleOrPlural = (num2 > 1) ? faction.def.pawnsPlural : faction.def.pawnSingular;
			Action <>9__7;
			quest.Delay(60000, delegate
			{
				quest.Leave(pawns, null, false, false, null, false);
				quest.RecordHistoryEvent(HistoryEventDefOf.CharityRefused_Beggars, null, QuestPart.SignalListenMode.OngoingOnly);
				Quest quest2 = quest;
				Action action;
				if ((action = <>9__7) == null)
				{
					action = (<>9__7 = delegate()
					{
						Quest quest3 = quest;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
						defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("MessageCharityEventRefused".Translate());
						defaultInterpolatedStringHandler.AppendLiteral(": ");
						defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("MessageBeggarsLeavingWithNoItems".Translate(pawnLabelSingleOrPlural));
						quest3.Message(defaultInterpolatedStringHandler.ToStringAndClear(), MessageTypeDefOf.NegativeEvent, false, null, pawns, null);
					});
				}
				quest2.AnyColonistWithCharityPrecept(action, null, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
			}, null, itemsReceivedSignal, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
			string text2 = QuestGenUtility.HardcodedSignalWithQuestID("beggars.Arrested");
			string text3 = QuestGenUtility.HardcodedSignalWithQuestID("beggars.Killed");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("beggars.LeftMap");
			TaggedString leavingWithItemsMessage = (num2 > 1) ? "MessageBeggarsLeavingWithItemsPlural".Translate(pawnLabelSingleOrPlural) : "MessageBeggarsLeavingWithItemsSingular".Translate(pawnLabelSingleOrPlural);
			quest.AnyColonistWithCharityPrecept(delegate
			{
				quest.Message("MessageCharityEventFulfilled".Translate() + ": " + leavingWithItemsMessage, MessageTypeDefOf.PositiveEvent, false, null, pawns, null);
				quest.RecordHistoryEvent(HistoryEventDefOf.CharityFulfilled_Beggars, null, QuestPart.SignalListenMode.OngoingOnly);
			}, delegate
			{
				quest.Message(leavingWithItemsMessage, MessageTypeDefOf.PositiveEvent, false, null, pawns, null);
			}, itemsReceivedSignal, null, null, QuestPart.SignalListenMode.OngoingOnly);
			Action <>9__9;
			Action <>9__8;
			quest.AnySignal(new string[]
			{
				text3,
				text2
			}, delegate
			{
				Quest quest2 = quest;
				Action action;
				if ((action = <>9__8) == null)
				{
					action = (<>9__8 = delegate()
					{
						Quest quest3 = quest;
						Action action2;
						if ((action2 = <>9__9) == null)
						{
							action2 = (<>9__9 = delegate()
							{
								Quest quest4 = quest;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
								defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("MessageCharityEventRefused".Translate());
								defaultInterpolatedStringHandler.AppendLiteral(": ");
								defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("MessageBeggarsLeavingWithNoItems".Translate(pawnLabelSingleOrPlural));
								quest4.Message(defaultInterpolatedStringHandler.ToStringAndClear(), MessageTypeDefOf.NegativeEvent, false, null, pawns, null);
							});
						}
						quest3.AnyColonistWithCharityPrecept(action2, null, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
					});
				}
				quest2.SignalPassActivable(action, null, null, null, null, itemsReceivedSignal, false);
				quest.Letter(LetterDefOf.NegativeEvent, null, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, pawns, false, "[letterTextBeggarsBetrayed]", null, "[letterLabelBeggarsBetrayed]", null, null);
				QuestPart_FactionRelationChange questPart_FactionRelationChange = new QuestPart_FactionRelationChange();
				questPart_FactionRelationChange.faction = faction;
				questPart_FactionRelationChange.relationKind = FactionRelationKind.Hostile;
				questPart_FactionRelationChange.canSendHostilityLetter = false;
				questPart_FactionRelationChange.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
				quest.AddPart(questPart_FactionRelationChange);
				quest.RecordHistoryEvent(HistoryEventDefOf.CharityRefused_Beggars_Betrayed, null, QuestPart.SignalListenMode.OngoingOnly);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
			quest.End(QuestEndOutcome.Fail, 0, null, QuestGenUtility.HardcodedSignalWithQuestID("map.MapRemoved"), QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.End(QuestEndOutcome.Fail, 0, null, QuestGenUtility.HardcodedSignalWithQuestID("faction.BecameHostileToPlayer"), QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.AllPawnsDespawned(pawns, delegate
			{
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, inSignal2, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x0600D9A0 RID: 55712 RVA: 0x0050AB0C File Offset: 0x00508D0C
		public static int LodgerCountFromPopulation(Slate slate, Map map)
		{
			int num = slate.Exists("population", false) ? slate.Get<int>("population", 0, false) : map.mapPawns.FreeAdultColonistsSpawnedCount;
			float randomInRange;
			if (num <= QuestNode_Root_Beggars.PopulationCountToCurve.TrueMin)
			{
				randomInRange = QuestNode_Root_Beggars.LodgerCountBasedOnColonyPopulationFactorRange_Low.RandomInRange;
			}
			else if (num >= QuestNode_Root_Beggars.PopulationCountToCurve.TrueMax)
			{
				randomInRange = QuestNode_Root_Beggars.LodgerCountBaseOnColonyPopulationFactorRange_High.RandomInRange;
			}
			else
			{
				float t = Mathf.InverseLerp((float)QuestNode_Root_Beggars.PopulationCountToCurve.TrueMin, (float)QuestNode_Root_Beggars.PopulationCountToCurve.TrueMax, (float)num);
				FloatRange floatRange = new FloatRange(Mathf.Lerp(QuestNode_Root_Beggars.LodgerCountBasedOnColonyPopulationFactorRange_Low.TrueMin, QuestNode_Root_Beggars.LodgerCountBaseOnColonyPopulationFactorRange_High.TrueMin, t), Mathf.Lerp(QuestNode_Root_Beggars.LodgerCountBasedOnColonyPopulationFactorRange_Low.TrueMax, QuestNode_Root_Beggars.LodgerCountBaseOnColonyPopulationFactorRange_High.TrueMax, t));
				randomInRange = floatRange.RandomInRange;
			}
			return Mathf.Max(Mathf.RoundToInt((float)num * randomInRange), 1);
		}

		// Token: 0x0600D9A1 RID: 55713 RVA: 0x0050AC10 File Offset: 0x00508E10
		private static float GetTotalRequestValue(float points)
		{
			return Mathf.Min(points * 0.85f, 700f);
		}

		// Token: 0x0600D9A2 RID: 55714 RVA: 0x0050AC24 File Offset: 0x00508E24
		private static bool TryFindRandomRequestedThing(Map map, float value, out ThingDef thingDef, out int count, IEnumerable<ThingDef> allowedThings)
		{
			QuestNode_Root_Beggars.requestCountDict.Clear();
			Func<ThingDef, bool> globalValidator = delegate(ThingDef td)
			{
				if (!td.PlayerAcquirable)
				{
					return false;
				}
				int num = ThingUtility.RoundedResourceStackCount(Mathf.Max(1, Mathf.RoundToInt(value / td.BaseMarketValue)));
				QuestNode_Root_Beggars.requestCountDict.Add(td, num);
				return PlayerItemAccessibilityUtility.Accessible(td, num, map);
			};
			if ((from def in allowedThings
			where globalValidator(def)
			select def).TryRandomElement(out thingDef))
			{
				count = QuestNode_Root_Beggars.requestCountDict[thingDef];
				return true;
			}
			count = 0;
			return false;
		}

		// Token: 0x0600D9A3 RID: 55715 RVA: 0x0050AC90 File Offset: 0x00508E90
		protected override bool TestRunInt(Slate slate)
		{
			Map map = QuestGen_Get.GetMap(false, null, false);
			if (map == null)
			{
				return false;
			}
			if (!FactionDefOf.Beggars.allowedArrivalTemperatureRange.Includes(map.mapTemperature.OutdoorTemp))
			{
				return false;
			}
			float points = slate.Get<float>("points", 0f, false);
			ThingDef thingDef;
			int num;
			return QuestNode_Root_Beggars.TryFindRandomRequestedThing(map, QuestNode_Root_Beggars.GetTotalRequestValue(points), out thingDef, out num, QuestNode_Root_Beggars.AllowedThings);
		}

		// Token: 0x0600D9A4 RID: 55716 RVA: 0x0050ACFC File Offset: 0x00508EFC
		[DebugOutput]
		public static void BeggarQuestItems()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Possible items with quantities based on points using the current map:");
			for (int i = 0; i < 10000; i += 100)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 1);
				defaultInterpolatedStringHandler.AppendLiteral("========== points ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(i);
				defaultInterpolatedStringHandler.AppendLiteral(" ==========");
				stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
				foreach (ThingDef thingDef in QuestNode_Root_Beggars.AllowedThings)
				{
					ThingDef thingDef2;
					int value;
					if (QuestNode_Root_Beggars.TryFindRandomRequestedThing(Find.CurrentMap, QuestNode_Root_Beggars.GetTotalRequestValue((float)i), out thingDef2, out value, Gen.YieldSingle<ThingDef>(thingDef)))
					{
						StringBuilder stringBuilder3 = stringBuilder;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(34, 2);
						defaultInterpolatedStringHandler2.AppendLiteral("<color=green>[POSSIBLE]</color> ");
						defaultInterpolatedStringHandler2.AppendFormatted(thingDef2.label);
						defaultInterpolatedStringHandler2.AppendLiteral(" x");
						defaultInterpolatedStringHandler2.AppendFormatted<int>(value);
						stringBuilder3.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
					}
					else
					{
						stringBuilder.AppendLine("<color=red>[NOT POSSIBLE]</color> " + thingDef.label);
					}
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04008B10 RID: 35600
		private static readonly FloatRange LodgerCountBasedOnColonyPopulationFactorRange_Low = new FloatRange(0.3f, 1f);

		// Token: 0x04008B11 RID: 35601
		private static readonly FloatRange LodgerCountBaseOnColonyPopulationFactorRange_High = new FloatRange(0.25f, 0.75f);

		// Token: 0x04008B12 RID: 35602
		private static readonly IntRange PopulationCountToCurve = new IntRange(5, 15);

		// Token: 0x04008B13 RID: 35603
		private const int VisitDuration = 60000;

		// Token: 0x04008B14 RID: 35604
		private const float BeggarRequestValueFactor = 0.85f;

		// Token: 0x04008B15 RID: 35605
		private const float MaxRequestValue = 700f;

		// Token: 0x04008B16 RID: 35606
		private const float GiveQuestChance = 0.35f;

		// Token: 0x04008B17 RID: 35607
		private static Dictionary<ThingDef, int> requestCountDict = new Dictionary<ThingDef, int>();
	}
}
