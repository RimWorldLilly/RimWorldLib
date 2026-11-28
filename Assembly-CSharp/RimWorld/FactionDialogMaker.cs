using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015FF RID: 5631
	public static class FactionDialogMaker
	{
		// Token: 0x06008C1C RID: 35868 RVA: 0x0032DA68 File Offset: 0x0032BC68
		public static DiaNode FactionDialogFor(Pawn negotiator, Faction faction)
		{
			FactionDialogMaker.<>c__DisplayClass0_0 CS$<>8__locals1;
			CS$<>8__locals1.negotiator = negotiator;
			Map map = CS$<>8__locals1.negotiator.Map;
			Pawn pawn;
			string value;
			if (faction.leader != null)
			{
				pawn = faction.leader;
				value = faction.leader.Name.ToStringFull.Colorize(ColoredText.NameColor);
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Faction ");
				defaultInterpolatedStringHandler.AppendFormatted<Faction>(faction);
				defaultInterpolatedStringHandler.AppendLiteral(" has no leader.");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				pawn = CS$<>8__locals1.negotiator;
				value = faction.Name;
			}
			if (faction.PlayerRelationKind == FactionRelationKind.Hostile)
			{
				string key;
				if (!faction.def.permanentEnemy && "FactionGreetingHostileAppreciative".CanTranslate())
				{
					key = ((!string.IsNullOrEmpty(faction.def.dialogFactionGreetingHostileAppreciative)) ? faction.def.dialogFactionGreetingHostileAppreciative : "FactionGreetingHostileAppreciative");
				}
				else
				{
					key = ((!string.IsNullOrEmpty(faction.def.dialogFactionGreetingHostile)) ? faction.def.dialogFactionGreetingHostile : "FactionGreetingHostile");
				}
				CS$<>8__locals1.root = new DiaNode(key.Translate(value).AdjustedFor(pawn, "PAWN", true));
			}
			else if (faction.PlayerRelationKind == FactionRelationKind.Neutral)
			{
				string key2 = "FactionGreetingWary";
				if (!string.IsNullOrEmpty(faction.def.dialogFactionGreetingWary))
				{
					key2 = faction.def.dialogFactionGreetingWary;
				}
				CS$<>8__locals1.root = new DiaNode(key2.Translate(value, CS$<>8__locals1.negotiator.LabelShort, CS$<>8__locals1.negotiator.Named("NEGOTIATOR"), pawn.Named("LEADER")).AdjustedFor(pawn, "PAWN", true));
			}
			else
			{
				string key3 = "FactionGreetingWarm";
				if (!string.IsNullOrEmpty(faction.def.dialogFactionGreetingWarm))
				{
					key3 = faction.def.dialogFactionGreetingWarm;
				}
				CS$<>8__locals1.root = new DiaNode(key3.Translate(value, CS$<>8__locals1.negotiator.LabelShort, CS$<>8__locals1.negotiator.Named("NEGOTIATOR"), pawn.Named("LEADER")).AdjustedFor(pawn, "PAWN", true));
			}
			if (map != null && map.IsPlayerHome)
			{
				if (faction.def.canRequestTraders)
				{
					FactionDialogMaker.<FactionDialogFor>g__AddAndDecorateOption|0_0(FactionDialogMaker.RequestTraderOption(map, faction, CS$<>8__locals1.negotiator), true, ref CS$<>8__locals1);
				}
				if (faction.def.canRequestOrbitalTrader)
				{
					FactionDialogMaker.<FactionDialogFor>g__AddAndDecorateOption|0_0(FactionDialogMaker.RequestOrbitalTraderOption(map, faction, CS$<>8__locals1.negotiator), true, ref CS$<>8__locals1);
				}
				if (faction.def.canRequestMilitaryAid)
				{
					FactionDialogMaker.<FactionDialogFor>g__AddAndDecorateOption|0_0(FactionDialogMaker.RequestMilitaryAidOption(map, faction, CS$<>8__locals1.negotiator), true, ref CS$<>8__locals1);
				}
				Pawn_RoyaltyTracker royalty = CS$<>8__locals1.negotiator.royalty;
				if (royalty != null && royalty.HasAnyTitleIn(faction))
				{
					foreach (RoyalTitle royalTitle in royalty.AllTitlesInEffectForReading)
					{
						if (royalTitle.def.permits != null)
						{
							foreach (RoyalTitlePermitDef royalTitlePermitDef in royalTitle.def.permits)
							{
								IEnumerable<DiaOption> factionCommDialogOptions = royalTitlePermitDef.Worker.GetFactionCommDialogOptions(map, CS$<>8__locals1.negotiator, faction);
								if (factionCommDialogOptions != null)
								{
									foreach (DiaOption opt in factionCommDialogOptions)
									{
										FactionDialogMaker.<FactionDialogFor>g__AddAndDecorateOption|0_0(opt, true, ref CS$<>8__locals1);
									}
								}
							}
						}
					}
					if (royalty.GetCurrentTitle(faction).canBeInherited && !CS$<>8__locals1.negotiator.IsQuestLodger())
					{
						FactionDialogMaker.<FactionDialogFor>g__AddAndDecorateOption|0_0(FactionDialogMaker.RequestRoyalHeirChangeOption(map, faction, pawn, CS$<>8__locals1.negotiator), false, ref CS$<>8__locals1);
					}
				}
				if (DefDatabase<ResearchProjectDef>.AllDefsListForReading.Any((ResearchProjectDef rp) => rp.HasTag(ResearchProjectTagDefOf.ShipRelated) && rp.IsFinished))
				{
					FactionDialogMaker.<FactionDialogFor>g__AddAndDecorateOption|0_0(FactionDialogMaker.RequestAICoreQuest(map, faction, CS$<>8__locals1.negotiator), true, ref CS$<>8__locals1);
				}
			}
			if (Prefs.DevMode)
			{
				foreach (DiaOption opt2 in FactionDialogMaker.DebugOptions(faction, CS$<>8__locals1.negotiator))
				{
					FactionDialogMaker.<FactionDialogFor>g__AddAndDecorateOption|0_0(opt2, false, ref CS$<>8__locals1);
				}
			}
			FactionDialogMaker.<FactionDialogFor>g__AddAndDecorateOption|0_0(new DiaOption("(" + "Disconnect".Translate() + ")")
			{
				resolveTree = true
			}, false, ref CS$<>8__locals1);
			return CS$<>8__locals1.root;
		}

		// Token: 0x06008C1D RID: 35869 RVA: 0x0032DF1C File Offset: 0x0032C11C
		private static IEnumerable<DiaOption> DebugOptions(Faction faction, Pawn negotiator)
		{
			FactionDialogMaker.<DebugOptions>d__1 <DebugOptions>d__ = new FactionDialogMaker.<DebugOptions>d__1(-2);
			<DebugOptions>d__.<>3__faction = faction;
			<DebugOptions>d__.<>3__negotiator = negotiator;
			return <DebugOptions>d__;
		}

		// Token: 0x06008C1E RID: 35870 RVA: 0x0032DF34 File Offset: 0x0032C134
		private static int AmountSendableSilver(Map map)
		{
			return (from t in TradeUtility.AllLaunchableThingsForTrade(map, null)
			where t.def == ThingDefOf.Silver
			select t).Sum((Thing t) => t.stackCount);
		}

		// Token: 0x06008C1F RID: 35871 RVA: 0x0032DF90 File Offset: 0x0032C190
		private static DiaOption RequestAICoreQuest(Map map, Faction faction, Pawn negotiator)
		{
			TaggedString taggedString = "RequestAICoreInformation".Translate(ThingDefOf.AIPersonaCore.label, 1500.ToString());
			if (faction.PlayerGoodwill < 40)
			{
				DiaOption diaOption = new DiaOption(taggedString);
				diaOption.Disable("NeedGoodwill".Translate(40.ToString("F0")));
				return diaOption;
			}
			bool flag = PlayerItemAccessibilityUtility.ItemStashHas(ThingDefOf.AIPersonaCore);
			Slate slate = new Slate();
			slate.Set<float>("points", StorytellerUtility.DefaultThreatPointsNow(Find.World), false);
			slate.Set<Pawn>("asker", faction.leader, false);
			slate.Set<ThingDef>("itemStashSingleThing", ThingDefOf.AIPersonaCore, false);
			bool flag2 = QuestScriptDefOf.OpportunitySite_ItemStash.CanRun(slate, Find.World);
			if (flag || !flag2)
			{
				DiaOption diaOption2 = new DiaOption(taggedString);
				diaOption2.Disable("NoKnownAICore".Translate(1500));
				return diaOption2;
			}
			if (FactionDialogMaker.AmountSendableSilver(map) < 1500)
			{
				DiaOption diaOption3 = new DiaOption(taggedString);
				diaOption3.Disable("NeedSilverLaunchable".Translate(1500));
				return diaOption3;
			}
			return new DiaOption(taggedString)
			{
				action = delegate()
				{
					Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(QuestScriptDefOf.OpportunitySite_ItemStash, slate);
					if (!quest.hidden && quest.root.sendAvailableLetter)
					{
						QuestUtility.SendLetterQuestAvailable(quest, null);
					}
					TradeUtility.LaunchThingsOfType(ThingDefOf.Silver, 1500, map, null);
					Current.Game.GetComponent<GameComponent_OnetimeNotification>().sendAICoreRequestReminder = false;
				},
				link = new DiaNode("RequestAICoreInformationResult".Translate(faction.leader).CapitalizeFirst())
				{
					options = 
					{
						FactionDialogMaker.OKToRoot(faction, negotiator)
					}
				}
			};
		}

		// Token: 0x06008C20 RID: 35872 RVA: 0x0032E154 File Offset: 0x0032C354
		private static DiaOption RequestOrbitalTraderOption(Map map, Faction faction, Pawn negotiator)
		{
			TaggedString taggedString = "RequestOrbitalTrader".Translate(-Faction.OfPlayer.CalculateAdjustedGoodwillChange(faction, -30));
			if (faction.PlayerRelationKind != FactionRelationKind.Ally)
			{
				DiaOption diaOption = new DiaOption(taggedString);
				diaOption.Disable("MustBeAlly".Translate());
				return diaOption;
			}
			int num = faction.lastOrbitalTraderRequestTick + 900000 - Find.TickManager.TicksGame;
			if (num > 0)
			{
				DiaOption diaOption2 = new DiaOption(taggedString);
				diaOption2.Disable("WaitTime".Translate(num.ToStringTicksToPeriod(true, false, true, true, false)));
				return diaOption2;
			}
			DiaOption diaOption3 = new DiaOption(taggedString);
			DiaNode diaNode = new DiaNode("OrbitalTraderSent".Translate(faction.leader).CapitalizeFirst());
			diaNode.options.Add(FactionDialogMaker.OKToRoot(faction, negotiator));
			DiaNode diaNode2 = new DiaNode("ChooseOrbitalTraderKind".Translate(faction.leader));
			foreach (TraderKindDef localTk2 in from x in faction.def.orbitalTraderKinds
			where x.requestable
			select x)
			{
				TraderKindDef localTk = localTk2;
				DiaOption diaOption4 = new DiaOption(localTk.LabelCap);
				if (localTk.TitleRequiredToTrade != null && (negotiator.royalty == null || localTk.TitleRequiredToTrade.seniority > negotiator.GetCurrentTitleSeniorityIn(faction)))
				{
					DiaNode diaNode3 = new DiaNode("OrbitalTradeRequestDeniedDueTitle".Translate(negotiator.Named("NEGOTIATOR"), localTk.TitleRequiredToTrade.GetLabelCapFor(negotiator).Named("TITLE"), faction.Named("FACTION")));
					DiaOption diaOption5 = new DiaOption("GoBack".Translate());
					diaNode3.options.Add(diaOption5);
					diaOption4.link = diaNode3;
					diaOption5.link = diaNode2;
				}
				else
				{
					diaOption4.action = delegate()
					{
						IncidentParms parms = new IncidentParms
						{
							target = map,
							faction = faction,
							traderKind = localTk,
							forced = true
						};
						PassingShipManager passingShipManager = map.passingShipManager;
						for (int i = passingShipManager.passingShips.Count - 1; i >= 0; i--)
						{
							passingShipManager.passingShips[i].Depart();
						}
						Find.Storyteller.incidentQueue.Add(IncidentDefOf.OrbitalTraderArrival, Find.TickManager.TicksGame + Mathf.RoundToInt(FactionDialogMaker.OrbitalTraderDelayHours.RandomInRange * 2500f), parms, (int)FactionDialogMaker.OrbitalTraderDelayHours.min);
						faction.lastOrbitalTraderRequestTick = Find.TickManager.TicksGame;
						Faction.OfPlayer.TryAffectGoodwillWith(faction, -30, false, true, HistoryEventDefOf.RequestedOrbitalTrader, null);
					};
					diaOption4.link = diaNode;
				}
				diaNode2.options.Add(diaOption4);
			}
			DiaOption diaOption6 = new DiaOption("GoBack".Translate());
			diaOption6.linkLateBind = FactionDialogMaker.ResetToRoot(faction, negotiator);
			diaNode2.options.Add(diaOption6);
			diaOption3.link = diaNode2;
			return diaOption3;
		}

		// Token: 0x06008C21 RID: 35873 RVA: 0x0032E46C File Offset: 0x0032C66C
		private static DiaOption RequestTraderOption(Map map, Faction faction, Pawn negotiator)
		{
			TaggedString taggedString = "RequestTrader".Translate(-Faction.OfPlayer.CalculateAdjustedGoodwillChange(faction, -15));
			if (faction.PlayerRelationKind != FactionRelationKind.Ally)
			{
				DiaOption diaOption = new DiaOption(taggedString);
				diaOption.Disable("MustBeAlly".Translate());
				return diaOption;
			}
			if (!faction.def.allowedArrivalTemperatureRange.ExpandedBy(-4f).Includes(map.mapTemperature.SeasonalTemp))
			{
				DiaOption diaOption2 = new DiaOption(taggedString);
				diaOption2.Disable("BadTemperature".Translate());
				return diaOption2;
			}
			int num = faction.lastTraderRequestTick + 240000 - Find.TickManager.TicksGame;
			if (num > 0)
			{
				DiaOption diaOption3 = new DiaOption(taggedString);
				diaOption3.Disable("WaitTime".Translate(num.ToStringTicksToPeriod(true, false, true, true, false)));
				return diaOption3;
			}
			DiaOption diaOption4 = new DiaOption(taggedString);
			DiaNode diaNode = new DiaNode("TraderSent".Translate(faction.leader).CapitalizeFirst());
			diaNode.options.Add(FactionDialogMaker.OKToRoot(faction, negotiator));
			DiaNode diaNode2 = new DiaNode("ChooseTraderKind".Translate(faction.leader));
			foreach (TraderKindDef localTk2 in from x in faction.def.caravanTraderKinds
			where x.requestable
			select x)
			{
				TraderKindDef localTk = localTk2;
				DiaOption diaOption5 = new DiaOption(localTk.LabelCap);
				if (localTk.TitleRequiredToTrade != null && (negotiator.royalty == null || localTk.TitleRequiredToTrade.seniority > negotiator.GetCurrentTitleSeniorityIn(faction)))
				{
					DiaNode diaNode3 = new DiaNode("TradeCaravanRequestDeniedDueTitle".Translate(negotiator.Named("NEGOTIATOR"), localTk.TitleRequiredToTrade.GetLabelCapFor(negotiator).Named("TITLE"), faction.Named("FACTION")));
					DiaOption diaOption6 = new DiaOption("GoBack".Translate());
					diaNode3.options.Add(diaOption6);
					diaOption5.link = diaNode3;
					diaOption6.link = diaNode2;
				}
				else
				{
					diaOption5.action = delegate()
					{
						IncidentParms parms = new IncidentParms
						{
							target = map,
							faction = faction,
							traderKind = localTk,
							forced = true
						};
						Find.Storyteller.incidentQueue.Add(IncidentDefOf.TraderCaravanArrival, Find.TickManager.TicksGame + 120000, parms, 240000);
						faction.lastTraderRequestTick = Find.TickManager.TicksGame;
						Faction.OfPlayer.TryAffectGoodwillWith(faction, -15, false, true, HistoryEventDefOf.RequestedTrader, null);
					};
					diaOption5.link = diaNode;
				}
				diaNode2.options.Add(diaOption5);
			}
			DiaOption diaOption7 = new DiaOption("GoBack".Translate());
			diaOption7.linkLateBind = FactionDialogMaker.ResetToRoot(faction, negotiator);
			diaNode2.options.Add(diaOption7);
			diaOption4.link = diaNode2;
			return diaOption4;
		}

		// Token: 0x06008C22 RID: 35874 RVA: 0x0032E7D8 File Offset: 0x0032C9D8
		private static DiaOption RequestMilitaryAidOption(Map map, Faction faction, Pawn negotiator)
		{
			string text = "RequestMilitaryAid".Translate(-Faction.OfPlayer.CalculateAdjustedGoodwillChange(faction, -25));
			if (faction.PlayerRelationKind != FactionRelationKind.Ally)
			{
				DiaOption diaOption = new DiaOption(text);
				diaOption.Disable("MustBeAlly".Translate());
				return diaOption;
			}
			if (!faction.def.allowedArrivalTemperatureRange.ExpandedBy(-4f).Includes(map.mapTemperature.SeasonalTemp))
			{
				DiaOption diaOption2 = new DiaOption(text);
				diaOption2.Disable("BadTemperature".Translate());
				return diaOption2;
			}
			int num = faction.lastMilitaryAidRequestTick + 60000 - Find.TickManager.TicksGame;
			if (num > 0)
			{
				DiaOption diaOption3 = new DiaOption(text);
				diaOption3.Disable("WaitTime".Translate(num.ToStringTicksToPeriod(true, false, true, true, false)));
				return diaOption3;
			}
			if (NeutralGroupIncidentUtility.AnyBlockingHostileLord(map, faction))
			{
				DiaOption diaOption4 = new DiaOption(text);
				diaOption4.Disable("HostileVisitorsPresent".Translate());
				return diaOption4;
			}
			DiaOption diaOption5 = new DiaOption(text);
			if (faction.def.techLevel < TechLevel.Industrial)
			{
				diaOption5.link = FactionDialogMaker.CantMakeItInTime(faction, negotiator);
			}
			else
			{
				IEnumerable<Faction> source = (from t in map.attackTargetsCache.TargetsHostileToColony
				where GenHostility.IsActiveThreatToPlayer(t, false)
				select t into x
				select ((Thing)x).Faction into x
				where x != null && !x.HostileTo(faction)
				select x).Distinct<Faction>();
				if (source.Any<Faction>())
				{
					DiaNode diaNode = new DiaNode("MilitaryAidConfirmMutualEnemy".Translate(faction.Name, (from fa in source
					select fa.Name).ToCommaList(true, false)));
					DiaOption diaOption6 = new DiaOption("CallConfirm".Translate());
					diaOption6.action = delegate()
					{
						FactionDialogMaker.CallForAid(map, faction);
					};
					diaOption6.link = FactionDialogMaker.FightersSent(faction, negotiator);
					DiaOption diaOption7 = new DiaOption("CallCancel".Translate());
					diaOption7.linkLateBind = FactionDialogMaker.ResetToRoot(faction, negotiator);
					diaNode.options.Add(diaOption6);
					diaNode.options.Add(diaOption7);
					diaOption5.link = diaNode;
				}
				else
				{
					diaOption5.action = delegate()
					{
						FactionDialogMaker.CallForAid(map, faction);
					};
					diaOption5.link = FactionDialogMaker.FightersSent(faction, negotiator);
				}
			}
			return diaOption5;
		}

		// Token: 0x06008C23 RID: 35875 RVA: 0x0032EAC8 File Offset: 0x0032CCC8
		private static DiaOption RequestRoyalHeirChangeOption(Map map, Faction faction, Pawn factionRepresentative, Pawn negotiator)
		{
			RoyalTitleDef currentTitle = negotiator.royalty.GetCurrentTitle(faction);
			Pawn heir = negotiator.royalty.GetHeir(faction);
			DiaOption diaOption = new DiaOption((heir != null) ? "RequestChangeRoyalHeir".Translate(negotiator.Named("HOLDER"), currentTitle.GetLabelCapFor(negotiator).Named("TITLE"), heir.Named("HEIR")) : "RequestSetRoyalHeir".Translate(negotiator.Named("HOLDER"), currentTitle.GetLabelCapFor(negotiator).Named("TITLE")));
			Predicate<QuestPart> <>9__1;
			bool flag = Find.QuestManager.QuestsListForReading.Any(delegate(Quest q)
			{
				if (q.root == QuestScriptDefOf.ChangeRoyalHeir && q.State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = q.PartsListForReading;
					Predicate<QuestPart> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = delegate(QuestPart p)
						{
							QuestPart_ChangeHeir questPart_ChangeHeir = p as QuestPart_ChangeHeir;
							return questPart_ChangeHeir != null && !questPart_ChangeHeir.done && questPart_ChangeHeir.holder == negotiator;
						});
					}
					return partsListForReading.Any(predicate);
				}
				return false;
			});
			diaOption.link = FactionDialogMaker.RoyalHeirChangeCandidates(faction, factionRepresentative, negotiator);
			if (flag)
			{
				diaOption.Disable("RequestChangeRoyalHeirAlreadyInProgress".Translate(negotiator.Named("PAWN")));
			}
			return diaOption;
		}

		// Token: 0x06008C24 RID: 35876 RVA: 0x0032EBDC File Offset: 0x0032CDDC
		public static DiaNode RoyalHeirChangeCandidates(Faction faction, Pawn factionRepresentative, Pawn negotiator)
		{
			DiaNode diaNode = new DiaNode("ChooseHeir".Translate(negotiator.Named("HOLDER")));
			RoyalTitleDef title = negotiator.royalty.GetCurrentTitle(faction);
			Pawn heir2 = negotiator.royalty.GetHeir(faction);
			foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsAndPrisonersSpawned)
			{
				DiaOption diaOption = new DiaOption(pawn.Name.ToStringFull);
				if (pawn != negotiator && pawn != heir2)
				{
					if (pawn.royalty != null)
					{
						RoyalTitleDef currentTitle = pawn.royalty.GetCurrentTitle(faction);
						if (currentTitle != null && currentTitle.seniority >= title.seniority)
						{
							continue;
						}
					}
					if (!pawn.IsQuestLodger())
					{
						Pawn heir = pawn;
						Action confirmedAct = delegate()
						{
							QuestScriptDef changeRoyalHeir = QuestScriptDefOf.ChangeRoyalHeir;
							Slate slate = new Slate();
							slate.Set<int>("points", title.changeHeirQuestPoints, false);
							slate.Set<Pawn>("asker", factionRepresentative, false);
							slate.Set<Pawn>("titleHolder", negotiator, false);
							slate.Set<Pawn>("titleHeir", heir, false);
							slate.Set<Pawn>("titlePreviousHeir", negotiator.royalty.GetHeir(faction), false);
							Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(changeRoyalHeir, slate);
							if (!quest.hidden && quest.root.sendAvailableLetter)
							{
								QuestUtility.SendLetterQuestAvailable(quest, null);
							}
						};
						diaOption.link = FactionDialogMaker.RoyalHeirChangeConfirm(faction, negotiator, heir2, confirmedAct);
						diaNode.options.Add(diaOption);
					}
				}
			}
			DiaOption diaOption2 = new DiaOption("GoBack".Translate());
			diaOption2.linkLateBind = FactionDialogMaker.ResetToRoot(faction, negotiator);
			diaNode.options.Add(diaOption2);
			return diaNode;
		}

		// Token: 0x06008C25 RID: 35877 RVA: 0x0032EDAC File Offset: 0x0032CFAC
		public static DiaNode RoyalHeirChangeConfirm(Faction faction, Pawn negotiator, Pawn currentHeir, Action confirmedAct)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("MakeHeirConfirm".Translate(faction, negotiator.Named("HOLDER")));
			if (currentHeir != null)
			{
				stringBuilder.Append(" " + "MakeHeirPreviousHeirWarning".Translate(negotiator.Named("HOLDER"), currentHeir.Named("HEIR")));
			}
			stringBuilder.Append(" " + "AreYouSure".Translate());
			DiaNode diaNode = new DiaNode(stringBuilder.ToString());
			DiaOption diaOption = new DiaOption("Confirm".Translate());
			diaOption.action = confirmedAct;
			diaOption.linkLateBind = FactionDialogMaker.ResetToRoot(faction, negotiator);
			diaNode.options.Add(diaOption);
			DiaOption diaOption2 = new DiaOption("GoBack".Translate());
			diaOption2.linkLateBind = FactionDialogMaker.ResetToRoot(faction, negotiator);
			diaNode.options.Add(diaOption2);
			return diaNode;
		}

		// Token: 0x06008C26 RID: 35878 RVA: 0x0032EEB4 File Offset: 0x0032D0B4
		public static DiaNode CantMakeItInTime(Faction faction, Pawn negotiator)
		{
			return new DiaNode("CantSendMilitaryAidInTime".Translate(faction.leader).CapitalizeFirst())
			{
				options = 
				{
					FactionDialogMaker.OKToRoot(faction, negotiator)
				}
			};
		}

		// Token: 0x06008C27 RID: 35879 RVA: 0x0032EEF8 File Offset: 0x0032D0F8
		public static DiaNode FightersSent(Faction faction, Pawn negotiator)
		{
			string key = "MilitaryAidSent";
			if (faction.def.dialogMilitaryAidSent != null && faction.def.dialogMilitaryAidSent != "")
			{
				key = faction.def.dialogMilitaryAidSent;
			}
			return new DiaNode(key.Translate(faction.leader).CapitalizeFirst())
			{
				options = 
				{
					FactionDialogMaker.OKToRoot(faction, negotiator)
				}
			};
		}

		// Token: 0x06008C28 RID: 35880 RVA: 0x0032EF6C File Offset: 0x0032D16C
		private static void CallForAid(Map map, Faction faction)
		{
			Faction.OfPlayer.TryAffectGoodwillWith(faction, -25, false, true, HistoryEventDefOf.RequestedMilitaryAid, null);
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = map;
			incidentParms.faction = faction;
			incidentParms.raidArrivalModeForQuickMilitaryAid = true;
			incidentParms.points = DiplomacyTuning.RequestedMilitaryAidPointsRange.RandomInRange;
			faction.lastMilitaryAidRequestTick = Find.TickManager.TicksGame;
			IncidentDefOf.RaidFriendly.Worker.TryExecute(incidentParms);
		}

		// Token: 0x06008C29 RID: 35881 RVA: 0x0032EFE6 File Offset: 0x0032D1E6
		private static DiaOption OKToRoot(Faction faction, Pawn negotiator)
		{
			return new DiaOption("OK".Translate())
			{
				linkLateBind = FactionDialogMaker.ResetToRoot(faction, negotiator)
			};
		}

		// Token: 0x06008C2A RID: 35882 RVA: 0x0032F009 File Offset: 0x0032D209
		public static Func<DiaNode> ResetToRoot(Faction faction, Pawn negotiator)
		{
			return () => FactionDialogMaker.FactionDialogFor(negotiator, faction);
		}

		// Token: 0x06008C2C RID: 35884 RVA: 0x0032F040 File Offset: 0x0032D240
		[CompilerGenerated]
		internal static void <FactionDialogFor>g__AddAndDecorateOption|0_0(DiaOption opt, bool needsSocial, ref FactionDialogMaker.<>c__DisplayClass0_0 A_2)
		{
			if (opt == null)
			{
				return;
			}
			if (needsSocial && A_2.negotiator.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
			{
				opt.Disable("WorkTypeDisablesOption".Translate(SkillDefOf.Social.label));
			}
			A_2.root.options.Add(opt);
		}

		// Token: 0x04005617 RID: 22039
		private static readonly FloatRange OrbitalTraderDelayHours = new FloatRange(1f, 2f);
	}
}
