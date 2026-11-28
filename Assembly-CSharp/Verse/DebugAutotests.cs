using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LudeonTK;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020005BF RID: 1471
	public static class DebugAutotests
	{
		// Token: 0x06002D25 RID: 11557 RVA: 0x00112D4C File Offset: 0x00110F4C
		[DebugAction("Autotests", "Make colony (full)", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MakeColonyFull()
		{
			Autotests_ColonyMaker.MakeColony_Full();
		}

		// Token: 0x06002D26 RID: 11558 RVA: 0x00112D53 File Offset: 0x00110F53
		[DebugAction("Autotests", "Make colony (animals)", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MakeColonyAnimals()
		{
			Autotests_ColonyMaker.MakeColony_Animals();
		}

		// Token: 0x06002D27 RID: 11559 RVA: 0x00112D5A File Offset: 0x00110F5A
		[DebugAction("Autotests", "Make colony (ancient junk)", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MakeColonyAncientJunk()
		{
			Autotests_ColonyMaker.MakeColony_AncientJunk();
		}

		// Token: 0x06002D28 RID: 11560 RVA: 0x00112D64 File Offset: 0x00110F64
		[DebugAction("Autotests", "Test force downed x100", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestForceDownedx100()
		{
			for (int i = 0; i < 100; i++)
			{
				PawnKindDef random = DefDatabase<PawnKindDef>.GetRandom();
				Pawn pawn = PawnGenerator.GeneratePawn(random, FactionUtility.DefaultFactionFrom(random.defaultFactionDef), null);
				GenSpawn.Spawn(pawn, CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(Find.CurrentMap), Find.CurrentMap, 1000), Find.CurrentMap, WipeMode.Vanish);
				HealthUtility.DamageUntilDowned(pawn, true, null, null, null);
				if (pawn.Dead)
				{
					string str = "Pawn died while force downing: ";
					Pawn pawn2 = pawn;
					Log.Error(str + ((pawn2 != null) ? pawn2.ToString() : null) + " at " + pawn.Position.ToString());
					return;
				}
			}
		}

		// Token: 0x06002D29 RID: 11561 RVA: 0x00112E28 File Offset: 0x00111028
		[DebugAction("Autotests", "Test force kill x100", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestForceKillx100()
		{
			for (int i = 0; i < 100; i++)
			{
				PawnKindDef random = DefDatabase<PawnKindDef>.GetRandom();
				Pawn pawn = PawnGenerator.GeneratePawn(random, FactionUtility.DefaultFactionFrom(random.defaultFactionDef), null);
				GenSpawn.Spawn(pawn, CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(Find.CurrentMap), Find.CurrentMap, 1000), Find.CurrentMap, WipeMode.Vanish);
				HealthUtility.DamageUntilDead(pawn, null, null, null);
				if (!pawn.Dead)
				{
					string str = "Pawn died not die: ";
					Pawn pawn2 = pawn;
					Log.Error(str + ((pawn2 != null) ? pawn2.ToString() : null) + " at " + pawn.Position.ToString());
					return;
				}
			}
		}

		// Token: 0x06002D2A RID: 11562 RVA: 0x00112EEC File Offset: 0x001110EC
		[DebugAction("Autotests", "Test generate pawn x1000", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestGeneratePawnx1000()
		{
			float[] array = new float[]
			{
				10f,
				20f,
				50f,
				100f,
				200f,
				500f,
				1000f,
				2000f,
				5000f,
				1E+20f
			};
			int[] array2 = new int[array.Length];
			for (int i = 0; i < 1000; i++)
			{
				PawnKindDef random = DefDatabase<PawnKindDef>.GetRandom();
				PerfLogger.Reset();
				Pawn pawn = PawnGenerator.GeneratePawn(random, FactionUtility.DefaultFactionFrom(random.defaultFactionDef), null);
				float ms = PerfLogger.Duration() * 1000f;
				array2[array.FirstIndexOf((float time) => ms <= time)]++;
				if (pawn.Dead)
				{
					Log.Error("Pawn is dead");
				}
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Pawn creation time histogram:");
			for (int j = 0; j < array2.Length; j++)
			{
				stringBuilder.AppendLine(string.Format("<{0}ms: {1}", array[j], array2[j]));
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06002D2B RID: 11563 RVA: 0x00112FFC File Offset: 0x001111FC
		[DebugAction("Autotests", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GeneratePawnsOfAllShapes()
		{
			Rot4[] array = new Rot4[]
			{
				Rot4.North,
				Rot4.East,
				Rot4.South,
				Rot4.West
			};
			IntVec3 intVec = UI.MouseCell();
			foreach (BodyTypeDef bodyTypeDef in DefDatabase<BodyTypeDef>.AllDefs)
			{
				IntVec3 intVec2 = intVec;
				foreach (Rot4 rot in array)
				{
					Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(Faction.OfPlayer.def.basicMemberKind, Faction.OfPlayer, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false)
					{
						ForceBodyType = bodyTypeDef
					});
					string text = bodyTypeDef.defName + "-" + rot.ToStringWord();
					pawn.Name = new NameTriple(text, text, text);
					GenSpawn.Spawn(pawn, intVec2, Find.CurrentMap, WipeMode.Vanish);
					pawn.apparel.DestroyAll(DestroyMode.Vanish);
					pawn.drafter.Drafted = true;
					pawn.stances.SetStance(new Stance_Warmup(100000, intVec2 + rot.FacingCell, null));
					intVec2 += IntVec3.South * 2;
				}
				intVec += IntVec3.East * 2;
			}
		}

		// Token: 0x06002D2C RID: 11564 RVA: 0x0011320C File Offset: 0x0011140C
		[DebugAction("Autotests", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void CheckRegionListers()
		{
			Autotests_RegionListers.CheckBugs(Find.CurrentMap);
		}

		// Token: 0x06002D2D RID: 11565 RVA: 0x00113218 File Offset: 0x00111418
		[DebugAction("Autotests", "Test time-to-down", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestTimeToDown()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<PawnKindDef> enumerator = (from kd in DefDatabase<PawnKindDef>.AllDefs
			orderby kd.defName
			select kd).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PawnKindDef kindDef = enumerator.Current;
					list.Add(new DebugMenuOption(kindDef.label, DebugMenuOptionMode.Action, delegate()
					{
						if (kindDef == PawnKindDefOf.Colonist)
						{
							Log.Message("Current colonist TTD reference point: 22.3 seconds, stddev 8.35 seconds");
						}
						List<float> results = new List<float>();
						List<PawnKindDef> list2 = new List<PawnKindDef>();
						List<PawnKindDef> list3 = new List<PawnKindDef>();
						list2.Add(kindDef);
						list3.Add(kindDef);
						ArenaUtility.BeginArenaFightSet(1000, list2, list3, delegate(ArenaUtility.ArenaResult result)
						{
							if (result.winner != ArenaUtility.ArenaResult.Winner.Other)
							{
								results.Add(result.tickDuration.TicksToSeconds());
							}
						}, delegate
						{
							string format = "Finished {0} tests; time-to-down {1}, stddev {2}\n\nraw: {3}";
							object[] array = new object[4];
							array[0] = results.Count;
							array[1] = results.Average();
							array[2] = GenMath.Stddev(results);
							array[3] = (from res in results
							select res.ToString()).ToLineList(null, false);
							Log.Message(string.Format(format, array));
						});
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06002D2E RID: 11566 RVA: 0x001132C8 File Offset: 0x001114C8
		[DebugAction("Autotests", "Battle Royale All PawnKinds", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void BattleRoyaleAllPawnKinds()
		{
			ArenaUtility.PerformBattleRoyale(DefDatabase<PawnKindDef>.AllDefs);
		}

		// Token: 0x06002D2F RID: 11567 RVA: 0x001132D4 File Offset: 0x001114D4
		[DebugAction("Autotests", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void BattleRoyaleHumanlikes()
		{
			ArenaUtility.PerformBattleRoyale(from k in DefDatabase<PawnKindDef>.AllDefs
			where k.RaceProps.Humanlike
			select k);
		}

		// Token: 0x06002D30 RID: 11568 RVA: 0x00113304 File Offset: 0x00111504
		[DebugAction("Autotests", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void BattleRoyaleByDamagetype()
		{
			PawnKindDef[] array = new PawnKindDef[]
			{
				PawnKindDefOf.Colonist,
				PawnKindDefOf.Muffalo
			};
			IEnumerable<ToolCapacityDef> enumerable = from tc in DefDatabase<ToolCapacityDef>.AllDefsListForReading
			where tc != ToolCapacityDefOf.KickMaterialInEyes
			select tc;
			Func<PawnKindDef, ToolCapacityDef, string> func = (PawnKindDef pkd, ToolCapacityDef dd) => string.Format("{0}_{1}", pkd.label, dd.defName);
			if (DebugAutotests.pawnKindsForDamageTypeBattleRoyale == null)
			{
				DebugAutotests.pawnKindsForDamageTypeBattleRoyale = new List<PawnKindDef>();
				foreach (PawnKindDef pawnKindDef in array)
				{
					using (IEnumerator<ToolCapacityDef> enumerator = enumerable.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ToolCapacityDef toolType = enumerator.Current;
							string text = func(pawnKindDef, toolType);
							ThingDef thingDef = Gen.MemberwiseClone<ThingDef>(pawnKindDef.race);
							thingDef.defName = text;
							thingDef.label = text;
							thingDef.tools = new List<Tool>(pawnKindDef.race.tools.Select(delegate(Tool tool)
							{
								Tool tool2 = Gen.MemberwiseClone<Tool>(tool);
								tool2.capacities = new List<ToolCapacityDef>();
								tool2.capacities.Add(toolType);
								return tool2;
							}));
							PawnKindDef pawnKindDef2 = Gen.MemberwiseClone<PawnKindDef>(pawnKindDef);
							pawnKindDef2.defName = text;
							pawnKindDef2.label = text;
							pawnKindDef2.race = thingDef;
							DebugAutotests.pawnKindsForDamageTypeBattleRoyale.Add(pawnKindDef2);
						}
					}
				}
			}
			ArenaUtility.PerformBattleRoyale(DebugAutotests.pawnKindsForDamageTypeBattleRoyale);
		}

		// Token: 0x04001E32 RID: 7730
		private static List<PawnKindDef> pawnKindsForDamageTypeBattleRoyale;
	}
}
