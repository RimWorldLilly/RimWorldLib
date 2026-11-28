using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LudeonTK;
using RimWorld;
using RimWorld.QuestGen;

namespace Verse
{
	// Token: 0x020005BC RID: 1468
	public static class DebugActionsRoyalty
	{
		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06002D16 RID: 11542 RVA: 0x001123EB File Offset: 0x001105EB
		private static IEnumerable<Faction> FactionsWithRoyalTitles
		{
			get
			{
				return from f in Find.FactionManager.AllFactions
				where f.def.RoyalTitlesAwardableInSeniorityOrderForReading.Count > 0
				select f;
			}
		}

		// Token: 0x06002D17 RID: 11543 RVA: 0x0011241B File Offset: 0x0011061B
		private static bool CheckAnyFactionWithRoyalTitles()
		{
			if (!DebugActionsRoyalty.FactionsWithRoyalTitles.Any<Faction>())
			{
				Messages.Message("No factions with royal titles found.", MessageTypeDefOf.RejectInput, false);
				return false;
			}
			return true;
		}

		// Token: 0x06002D18 RID: 11544 RVA: 0x0011243C File Offset: 0x0011063C
		[DebugAction("General", "Award 4 honor", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresRoyalty = true)]
		private static void Award4RoyalFavor()
		{
			if (!DebugActionsRoyalty.CheckAnyFactionWithRoyalTitles())
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Faction localFaction2 in DebugActionsRoyalty.FactionsWithRoyalTitles)
			{
				Faction localFaction = localFaction2;
				list.Add(new DebugMenuOption(localFaction.Name, DebugMenuOptionMode.Tool, delegate()
				{
					Pawn firstPawn = UI.MouseCell().GetFirstPawn(Find.CurrentMap);
					if (firstPawn != null)
					{
						firstPawn.royalty.GainFavor(localFaction, 4);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002D19 RID: 11545 RVA: 0x001124D0 File Offset: 0x001106D0
		[DebugAction("General", "Award 10 honor", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresRoyalty = true)]
		private static void Award10RoyalFavor()
		{
			if (!DebugActionsRoyalty.CheckAnyFactionWithRoyalTitles())
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Faction localFaction2 in DebugActionsRoyalty.FactionsWithRoyalTitles)
			{
				Faction localFaction = localFaction2;
				list.Add(new DebugMenuOption(localFaction.Name, DebugMenuOptionMode.Tool, delegate()
				{
					Pawn firstPawn = UI.MouseCell().GetFirstPawn(Find.CurrentMap);
					if (firstPawn != null)
					{
						firstPawn.royalty.GainFavor(localFaction, 10);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002D1A RID: 11546 RVA: 0x00112564 File Offset: 0x00110764
		[DebugAction("General", "Remove 4 honor", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresRoyalty = true)]
		private static void Remove4RoyalFavor()
		{
			if (!DebugActionsRoyalty.CheckAnyFactionWithRoyalTitles())
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Faction localFaction2 in DebugActionsRoyalty.FactionsWithRoyalTitles)
			{
				Faction localFaction = localFaction2;
				list.Add(new DebugMenuOption(localFaction.Name, DebugMenuOptionMode.Tool, delegate()
				{
					Pawn firstPawn = UI.MouseCell().GetFirstPawn(Find.CurrentMap);
					if (firstPawn != null)
					{
						firstPawn.royalty.TryRemoveFavor(localFaction, 4);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002D1B RID: 11547 RVA: 0x001125F8 File Offset: 0x001107F8
		[DebugAction("General", "Reduce royal title", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresRoyalty = true)]
		private static void ReduceRoyalTitle()
		{
			if (!DebugActionsRoyalty.CheckAnyFactionWithRoyalTitles())
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Faction localFaction2 in DebugActionsRoyalty.FactionsWithRoyalTitles)
			{
				Faction localFaction = localFaction2;
				list.Add(new DebugMenuOption(localFaction.Name, DebugMenuOptionMode.Tool, delegate()
				{
					Pawn firstPawn = UI.MouseCell().GetFirstPawn(Find.CurrentMap);
					if (firstPawn != null)
					{
						firstPawn.royalty.ReduceTitle(localFaction);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002D1C RID: 11548 RVA: 0x0011268C File Offset: 0x0011088C
		[DebugAction("General", "Set royal title", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresRoyalty = true)]
		private static void SetTitleForced()
		{
			if (!DebugActionsRoyalty.CheckAnyFactionWithRoyalTitles())
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Faction localFaction2 in DebugActionsRoyalty.FactionsWithRoyalTitles)
			{
				Faction localFaction = localFaction2;
				Action <>9__1;
				list.Add(new DebugMenuOption(localFaction.Name, DebugMenuOptionMode.Action, delegate()
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					List<DebugMenuOption> list3 = list2;
					string label = "(none)";
					DebugMenuOptionMode mode = DebugMenuOptionMode.Tool;
					Action method;
					if ((method = <>9__1) == null)
					{
						method = (<>9__1 = delegate()
						{
							Pawn firstPawn = UI.MouseCell().GetFirstPawn(Find.CurrentMap);
							if (firstPawn != null && firstPawn.royalty != null && firstPawn.royalty.HasAnyTitleIn(localFaction))
							{
								firstPawn.royalty.SetTitle(localFaction, null, true, false, true);
								DebugActionsUtility.DustPuffFrom(firstPawn);
							}
						});
					}
					list3.Add(new DebugMenuOption(label, mode, method));
					foreach (RoyalTitleDef localTitleDef2 in DefDatabase<RoyalTitleDef>.AllDefsListForReading)
					{
						RoyalTitleDef localTitleDef = localTitleDef2;
						list2.Add(new DebugMenuOption(localTitleDef.defName, DebugMenuOptionMode.Tool, delegate()
						{
							Pawn firstPawn = UI.MouseCell().GetFirstPawn(Find.CurrentMap);
							if (firstPawn != null)
							{
								firstPawn.royalty.SetTitle(localFaction, localTitleDef, true, false, true);
							}
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002D1D RID: 11549 RVA: 0x00112720 File Offset: 0x00110920
		[DebugOutput]
		private static void RoyalTitles()
		{
			IEnumerable<RoyalTitleDef> allDefsListForReading = DefDatabase<RoyalTitleDef>.AllDefsListForReading;
			TableDataGetter<RoyalTitleDef>[] array = new TableDataGetter<RoyalTitleDef>[10];
			array[0] = new TableDataGetter<RoyalTitleDef>("defName", (RoyalTitleDef title) => title.defName);
			array[1] = new TableDataGetter<RoyalTitleDef>("seniority", (RoyalTitleDef title) => title.seniority);
			array[2] = new TableDataGetter<RoyalTitleDef>("favorCost", (RoyalTitleDef title) => title.favorCost);
			array[3] = new TableDataGetter<RoyalTitleDef>("Awardable", (RoyalTitleDef title) => title.Awardable);
			array[4] = new TableDataGetter<RoyalTitleDef>("minimumExpectationLock", delegate(RoyalTitleDef title)
			{
				if (title.minExpectation != null)
				{
					return title.minExpectation.defName;
				}
				return "NULL";
			});
			array[5] = new TableDataGetter<RoyalTitleDef>("requiredMinimumApparelQuality", delegate(RoyalTitleDef title)
			{
				if (title.requiredMinimumApparelQuality != QualityCategory.Awful)
				{
					return title.requiredMinimumApparelQuality.ToString();
				}
				return "None";
			});
			array[6] = new TableDataGetter<RoyalTitleDef>("requireApparel", delegate(RoyalTitleDef title)
			{
				if (title.requiredApparel != null)
				{
					return string.Join(",\r\n", (from a in title.requiredApparel
					select a.ToString()).ToArray<string>());
				}
				return "NULL";
			});
			array[7] = new TableDataGetter<RoyalTitleDef>("awardThought", delegate(RoyalTitleDef title)
			{
				if (title.awardThought != null)
				{
					return title.awardThought.defName;
				}
				return "NULL";
			});
			array[8] = new TableDataGetter<RoyalTitleDef>("lostThought", delegate(RoyalTitleDef title)
			{
				if (title.lostThought != null)
				{
					return title.lostThought.defName;
				}
				return "NULL";
			});
			array[9] = new TableDataGetter<RoyalTitleDef>("factions", (RoyalTitleDef title) => string.Join(",", (from f in DefDatabase<FactionDef>.AllDefsListForReading
			where f.RoyalTitlesAwardableInSeniorityOrderForReading.Contains(title)
			select f.defName).ToArray<string>()));
			DebugTables.MakeTablesDialog<RoyalTitleDef>(allDefsListForReading, array);
		}

		// Token: 0x06002D1E RID: 11550 RVA: 0x001128F8 File Offset: 0x00110AF8
		[DebugOutput(name = "Honor Availability (slow)")]
		private static void RoyalFavorAvailability()
		{
			StorytellerCompProperties_OnOffCycle storytellerCompProperties_OnOffCycle = (StorytellerCompProperties_OnOffCycle)StorytellerDefOf.Cassandra.comps.Find(delegate(StorytellerCompProperties x)
			{
				StorytellerCompProperties_OnOffCycle storytellerCompProperties_OnOffCycle2 = x as StorytellerCompProperties_OnOffCycle;
				if (storytellerCompProperties_OnOffCycle2 == null)
				{
					return false;
				}
				if (storytellerCompProperties_OnOffCycle2.IncidentCategory != IncidentCategoryDefOf.GiveQuest)
				{
					return false;
				}
				if (storytellerCompProperties_OnOffCycle2.enableIfAnyModActive != null)
				{
					if (storytellerCompProperties_OnOffCycle2.enableIfAnyModActive.Any((string m) => m.ToLower() == "ludeon.rimworld.royalty"))
					{
						return true;
					}
				}
				return false;
			});
			float onDays = storytellerCompProperties_OnOffCycle.onDays;
			float average = storytellerCompProperties_OnOffCycle.numIncidentsRange.Average;
			float num = average / onDays;
			SimpleCurve simpleCurve = new SimpleCurve
			{
				{
					new CurvePoint(0f, 35f),
					true
				},
				{
					new CurvePoint(15f, 150f),
					true
				},
				{
					new CurvePoint(150f, 5000f),
					true
				}
			};
			int num2 = 0;
			List<RoyalTitleDef> royalTitlesAwardableInSeniorityOrderForReading = FactionDefOf.Empire.RoyalTitlesAwardableInSeniorityOrderForReading;
			for (int i = 0; i < royalTitlesAwardableInSeniorityOrderForReading.Count; i++)
			{
				num2 += royalTitlesAwardableInSeniorityOrderForReading[i].favorCost;
				if (royalTitlesAwardableInSeniorityOrderForReading[i] == RoyalTitleDefOf.Count)
				{
					break;
				}
			}
			float num3 = 0f;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = -1;
			int num9 = -1;
			int num10 = -1;
			int ticksGame = Find.TickManager.TicksGame;
			StoryState storyState = new StoryState(Find.World);
			for (int j = 0; j < 200; j++)
			{
				Find.TickManager.DebugSetTicksGame(j * 60000);
				num3 += num * storytellerCompProperties_OnOffCycle.acceptFractionByDaysPassedCurve.Evaluate((float)j);
				while (num3 >= 1f)
				{
					num3 -= 1f;
					num4++;
					float points = simpleCurve.Evaluate((float)j);
					Slate slate = new Slate();
					slate.Set<float>("points", points, false);
					QuestScriptDef questScriptDef = DefDatabase<QuestScriptDef>.AllDefsListForReading.Where(delegate(QuestScriptDef x)
					{
						if (x.IsRootRandomSelected)
						{
							Slate slate = slate;
							IIncidentTarget currentMap = Find.CurrentMap;
							return x.CanRun(slate, currentMap ?? Find.World);
						}
						return false;
					}).RandomElementByWeight((QuestScriptDef x) => NaturalRandomQuestChooser.GetNaturalRandomSelectionWeight(x, points, storyState));
					Quest quest = QuestGen.Generate(questScriptDef, slate);
					if (quest.InvolvedFactions.Contains(Faction.OfEmpire))
					{
						num7++;
					}
					QuestPart_GiveRoyalFavor questPart_GiveRoyalFavor = (QuestPart_GiveRoyalFavor)quest.PartsListForReading.Find((QuestPart x) => x is QuestPart_GiveRoyalFavor);
					if (questPart_GiveRoyalFavor != null)
					{
						num5 += questPart_GiveRoyalFavor.amount;
						num6++;
						if (num5 >= num2 && num8 < 0)
						{
							num8 = j;
						}
						if (num9 < 0 || questPart_GiveRoyalFavor.amount < num9)
						{
							num9 = questPart_GiveRoyalFavor.amount;
						}
						if (num10 < 0 || questPart_GiveRoyalFavor.amount > num10)
						{
							num10 = questPart_GiveRoyalFavor.amount;
						}
					}
					storyState.RecordRandomQuestFired(questScriptDef);
					quest.CleanupQuestParts();
				}
			}
			Find.TickManager.DebugSetTicksGame(ticksGame);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(string.Concat(new string[]
			{
				"Results for: Days=",
				200.ToString(),
				", intervalDays=",
				onDays.ToString(),
				", questsPerInterval=",
				average.ToString(),
				":"
			}));
			stringBuilder.AppendLine("Quests: " + num4.ToString());
			stringBuilder.AppendLine("Quests with honor: " + num6.ToString());
			stringBuilder.AppendLine("Quests from Empire: " + num7.ToString());
			stringBuilder.AppendLine("Min honor reward: " + num9.ToString());
			stringBuilder.AppendLine("Max honor reward: " + num10.ToString());
			stringBuilder.AppendLine("Total honor: " + num5.ToString());
			stringBuilder.AppendLine("Honor required for Count: " + num2.ToString());
			stringBuilder.AppendLine("Count title possible on day: " + num8.ToString());
			Log.Message(stringBuilder.ToString());
		}
	}
}
