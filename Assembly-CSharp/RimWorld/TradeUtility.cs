using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001B9A RID: 7066
	public static class TradeUtility
	{
		// Token: 0x0600B55F RID: 46431 RVA: 0x003FDC70 File Offset: 0x003FBE70
		public static bool EverPlayerSellable(ThingDef def)
		{
			return def.tradeability.PlayerCanSell() && def.GetStatValueAbstract(StatDefOf.MarketValue, null) > 0f && (def.category == ThingCategory.Item || def.category == ThingCategory.Pawn || def.category == ThingCategory.Building) && (def.category != ThingCategory.Building || def.Minifiable);
		}

		// Token: 0x0600B560 RID: 46432 RVA: 0x003FDCD4 File Offset: 0x003FBED4
		public static bool PlayerSellableNow(Thing t, ITrader trader)
		{
			t = t.GetInnerIfMinified();
			if (!TradeUtility.EverPlayerSellable(t.def))
			{
				return false;
			}
			if (t.IsNotFresh())
			{
				return false;
			}
			Apparel apparel = t as Apparel;
			if (apparel != null && apparel.WornByCorpse)
			{
				return false;
			}
			if (CompBiocodable.IsBiocoded(t))
			{
				return false;
			}
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				if ((pawn.GetExtraHostFaction(null) != null && pawn.GetExtraHostFaction(null) == trader.Faction) || (pawn.IsQuestLodger() && pawn.GetExtraHomeFaction(null) == trader.Faction) || (pawn.HomeFaction != null && pawn.HomeFaction == trader.Faction && !pawn.IsSlave && !pawn.IsPrisoner))
				{
					return false;
				}
				if (pawn.RaceProps.Animal && pawn.IsFormingCaravan())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600B561 RID: 46433 RVA: 0x003FDD9C File Offset: 0x003FBF9C
		public static void SpawnDropPod(IntVec3 dropSpot, Map map, Thing t)
		{
			DropPodUtility.MakeDropPodAt(dropSpot, map, new ActiveTransporterInfo
			{
				SingleContainedThing = t,
				leaveSlag = false
			}, null);
		}

		// Token: 0x0600B562 RID: 46434 RVA: 0x003FDDC6 File Offset: 0x003FBFC6
		public static IEnumerable<Thing> AllLaunchableThingsForTrade(Map map, ITrader trader = null)
		{
			TradeUtility.<AllLaunchableThingsForTrade>d__7 <AllLaunchableThingsForTrade>d__ = new TradeUtility.<AllLaunchableThingsForTrade>d__7(-2);
			<AllLaunchableThingsForTrade>d__.<>3__map = map;
			<AllLaunchableThingsForTrade>d__.<>3__trader = trader;
			return <AllLaunchableThingsForTrade>d__;
		}

		// Token: 0x0600B563 RID: 46435 RVA: 0x003FDDDD File Offset: 0x003FBFDD
		public static IEnumerable<Pawn> AllSellableColonyPawns(Map map, bool checkAcceptableTemperatureOfAnimals = true)
		{
			TradeUtility.<AllSellableColonyPawns>d__8 <AllSellableColonyPawns>d__ = new TradeUtility.<AllSellableColonyPawns>d__8(-2);
			<AllSellableColonyPawns>d__.<>3__map = map;
			<AllSellableColonyPawns>d__.<>3__checkAcceptableTemperatureOfAnimals = checkAcceptableTemperatureOfAnimals;
			return <AllSellableColonyPawns>d__;
		}

		// Token: 0x0600B564 RID: 46436 RVA: 0x003FDDF4 File Offset: 0x003FBFF4
		public static Thing ThingFromStockToMergeWith(ITrader trader, Thing thing)
		{
			if (thing is Pawn)
			{
				return null;
			}
			foreach (Thing thing2 in trader.Goods)
			{
				if (TransferableUtility.TransferAsOne(thing2, thing, TransferAsOneMode.Normal) && thing2.CanStackWith(thing) && thing2.def.stackLimit != 1)
				{
					return thing2;
				}
			}
			return null;
		}

		// Token: 0x0600B565 RID: 46437 RVA: 0x003FDE6C File Offset: 0x003FC06C
		public static void LaunchThingsOfType(ThingDef resDef, int debt, Map map, TradeShip trader)
		{
			while (debt > 0)
			{
				Thing thing = null;
				foreach (Building_OrbitalTradeBeacon building_OrbitalTradeBeacon in Building_OrbitalTradeBeacon.AllPowered(map))
				{
					foreach (IntVec3 c in building_OrbitalTradeBeacon.TradeableCells)
					{
						foreach (Thing thing2 in map.thingGrid.ThingsAt(c))
						{
							if (thing2.def == resDef)
							{
								thing = thing2;
								goto IL_9D;
							}
						}
					}
				}
				IL_9D:
				if (thing == null)
				{
					Log.Error("Could not find any " + ((resDef != null) ? resDef.ToString() : null) + " to transfer to trader.");
					return;
				}
				int num = Math.Min(debt, thing.stackCount);
				if (trader != null)
				{
					trader.GiveSoldThingToTrader(thing, num, TradeSession.playerNegotiator);
				}
				else
				{
					thing.SplitOff(num).Destroy(DestroyMode.Vanish);
				}
				debt -= num;
			}
		}

		// Token: 0x0600B566 RID: 46438 RVA: 0x003FDF9C File Offset: 0x003FC19C
		public static void LaunchSilver(Map map, int fee)
		{
			TradeUtility.LaunchThingsOfType(ThingDefOf.Silver, fee, map, null);
		}

		// Token: 0x0600B567 RID: 46439 RVA: 0x003FDFAC File Offset: 0x003FC1AC
		public static Map PlayerHomeMapWithMostLaunchableSilver()
		{
			return (from x in Find.Maps
			where x.IsPlayerHome
			select x).MaxBy((Map x) => (from t in TradeUtility.AllLaunchableThingsForTrade(x, null)
			where t.def == ThingDefOf.Silver
			select t).Sum((Thing t) => t.stackCount));
		}

		// Token: 0x0600B568 RID: 46440 RVA: 0x003FE008 File Offset: 0x003FC208
		public static bool ColonyHasEnoughSilver(Map map, int fee)
		{
			return (from t in TradeUtility.AllLaunchableThingsForTrade(map, null)
			where t.def == ThingDefOf.Silver
			select t).Sum((Thing t) => t.stackCount) >= fee;
		}

		// Token: 0x0600B569 RID: 46441 RVA: 0x003FE06C File Offset: 0x003FC26C
		public static void CheckInteractWithTradersTeachOpportunity(Pawn pawn)
		{
			if (pawn.Dead)
			{
				return;
			}
			Lord lord = pawn.GetLord();
			if (lord != null && lord.CurLordToil is LordToil_DefendTraderCaravan)
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.InteractingWithTraders, pawn, OpportunityType.Important);
			}
		}

		// Token: 0x0600B56A RID: 46442 RVA: 0x003FE0A8 File Offset: 0x003FC2A8
		public static float GetPricePlayerSell(Thing thing, float priceFactorSell_TraderPriceType, float priceFactorSell_HumanPawn, float priceGain_PlayerNegotiator, float priceGain_FactionBase, float priceGain_DrugBonus, float priceGain_AnimalProduceBonus, TradeCurrency currency = TradeCurrency.Silver)
		{
			if (currency == TradeCurrency.Favor)
			{
				return thing.RoyalFavorValue;
			}
			float statValue = thing.GetStatValue(StatDefOf.SellPriceFactor, true, -1);
			float num = thing.MarketValue * 0.6f * priceFactorSell_TraderPriceType * statValue * priceFactorSell_HumanPawn * (1f - Find.Storyteller.difficulty.tradePriceFactorLoss);
			num *= 1f + priceGain_PlayerNegotiator + priceGain_DrugBonus + priceGain_FactionBase + priceGain_AnimalProduceBonus;
			num = Mathf.Max(num, 0.01f);
			if (num > 99.5f)
			{
				num = Mathf.Round(num);
			}
			return num;
		}

		// Token: 0x0600B56B RID: 46443 RVA: 0x003FE128 File Offset: 0x003FC328
		public static float GetPricePlayerBuy(Thing thing, float priceFactorBuy_TraderPriceType, float priceFactorBuy_JoinAs, float priceGain_PlayerNegotiator, float priceGain_FactionBase)
		{
			float num = thing.MarketValue * 1.4f * priceFactorBuy_TraderPriceType * priceFactorBuy_JoinAs * (1f + Find.Storyteller.difficulty.tradePriceFactorLoss);
			num *= 1f - priceGain_PlayerNegotiator - priceGain_FactionBase;
			num = Mathf.Max(num, 0.5f);
			if (num > 99.5f)
			{
				num = Mathf.Round(num);
			}
			return num;
		}

		// Token: 0x0600B56C RID: 46444 RVA: 0x003FE188 File Offset: 0x003FC388
		public static float TotalMarketValue(IEnumerable<Thing> things)
		{
			if (things == null)
			{
				return 0f;
			}
			float num = 0f;
			foreach (Thing thing in things)
			{
				num += thing.MarketValue * (float)thing.stackCount;
			}
			return num;
		}

		// Token: 0x0600B56D RID: 46445 RVA: 0x003FE1EC File Offset: 0x003FC3EC
		public static void CheckGiveTraderQuest(Pawn trader)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			if (!Rand.Chance(Find.Storyteller.QuestGiverTraderChanceNow))
			{
				return;
			}
			trader.mindState.hasQuest = true;
			Find.Storyteller.Notify_QuestGiverTraderSpawned();
		}

		// Token: 0x0600B56E RID: 46446 RVA: 0x003FE220 File Offset: 0x003FC420
		public static void ReceiveQuestFromTrader(Pawn trader, Pawn negotiator)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			Slate slate = new Slate();
			slate.Set<float>("points", StorytellerUtility.DefaultThreatPointsNow(negotiator.Map), false);
			slate.Set<TaggedString>("discoveryMethod", "QuestDiscoveredFromTrader".Translate(trader.Named("TRADER"), negotiator.Named("NEGOTIATOR")), false);
			List<QuestScriptDef> list = QuestUtility.GetGiverQuests(QuestGiverTag.Traders).ToList<QuestScriptDef>();
			if (list.NullOrEmpty<QuestScriptDef>())
			{
				return;
			}
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(list.RandomElementByWeight((QuestScriptDef q) => q.rootSelectionWeight), slate);
			Messages.Message("MessageTraderGaveQuest".Translate(quest.name, trader.Named("TRADER"), negotiator.Named("NEGOTIATOR")), MessageTypeDefOf.PositiveEvent, true);
			if (!quest.hidden && quest.root.sendAvailableLetter)
			{
				QuestUtility.SendLetterQuestAvailable(quest, slate.Get<string>("discoveryMethod", null, false));
			}
			trader.mindState.hasQuest = false;
		}

		// Token: 0x04006844 RID: 26692
		public const float MinimumBuyPrice = 0.5f;

		// Token: 0x04006845 RID: 26693
		public const float MinimumSellPrice = 0.01f;

		// Token: 0x04006846 RID: 26694
		public const float PriceFactorBuy_Global = 1.4f;

		// Token: 0x04006847 RID: 26695
		public const float PriceFactorSell_Global = 0.6f;
	}
}
