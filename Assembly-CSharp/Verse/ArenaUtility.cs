using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020006B7 RID: 1719
	public static class ArenaUtility
	{
		// Token: 0x060036A8 RID: 13992 RVA: 0x0015EB1B File Offset: 0x0015CD1B
		public static bool ValidateArenaCapability()
		{
			if (Find.World.info.planetCoverage < 0.299f)
			{
				Log.Error("Planet coverage must be 30%+ to ensure a representative mix of biomes.");
				return false;
			}
			return true;
		}

		// Token: 0x060036A9 RID: 13993 RVA: 0x0015EB40 File Offset: 0x0015CD40
		public static void BeginArenaFight(List<PawnKindDef> lhs, List<PawnKindDef> rhs, Action<ArenaUtility.ArenaResult> callback)
		{
			MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Debug_Arena);
			mapParent.Tile = TileFinder.RandomSettlementTileFor(Faction.OfPlayer, true, (PlanetTile tile) => lhs.Concat(rhs).Any((PawnKindDef pawnkind) => Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(tile, pawnkind.race)));
			mapParent.SetFaction(Faction.OfPlayer);
			Find.WorldObjects.Add(mapParent);
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(mapParent.Tile, new IntVec3(50, 1, 50), null, null, false);
			IntVec3 spot;
			IntVec3 spot2;
			MultipleCaravansCellFinder.FindStartingCellsFor2Groups(orGenerateMap, out spot, out spot2);
			List<Pawn> lhs2 = ArenaUtility.SpawnPawnSet(orGenerateMap, lhs, spot, Faction.OfAncients);
			List<Pawn> rhs2 = ArenaUtility.SpawnPawnSet(orGenerateMap, rhs, spot2, Faction.OfAncientsHostile);
			DebugArena component = mapParent.GetComponent<DebugArena>();
			component.lhs = lhs2;
			component.rhs = rhs2;
			component.callback = callback;
		}

		// Token: 0x060036AA RID: 13994 RVA: 0x0015EC0C File Offset: 0x0015CE0C
		public static List<Pawn> SpawnPawnSet(Map map, List<PawnKindDef> kinds, IntVec3 spot, Faction faction)
		{
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < kinds.Count; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(kinds[i], faction, null);
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(spot, map, 12, null);
				GenSpawn.Spawn(pawn, loc, map, Rot4.Random, WipeMode.Vanish, false, false);
				list.Add(pawn);
			}
			LordMaker.MakeNewLord(faction, new LordJob_DefendPoint(map.Center, null, null, false, true), map, list);
			return list;
		}

		// Token: 0x060036AB RID: 13995 RVA: 0x0015EC98 File Offset: 0x0015CE98
		private static bool ArenaFightQueue(List<PawnKindDef> lhs, List<PawnKindDef> rhs, Action<ArenaUtility.ArenaResult> callback, ArenaUtility.ArenaSetState state)
		{
			if (!ArenaUtility.ValidateArenaCapability())
			{
				return false;
			}
			if (state.live < 15)
			{
				ArenaUtility.BeginArenaFight(lhs, rhs, delegate(ArenaUtility.ArenaResult result)
				{
					state.live--;
					callback(result);
				});
				state.live++;
				return true;
			}
			return false;
		}

		// Token: 0x060036AC RID: 13996 RVA: 0x0015ECFC File Offset: 0x0015CEFC
		public static void BeginArenaFightSet(int count, List<PawnKindDef> lhs, List<PawnKindDef> rhs, Action<ArenaUtility.ArenaResult> callback, Action report)
		{
			if (!ArenaUtility.ValidateArenaCapability())
			{
				return;
			}
			int remaining = count;
			ArenaUtility.ArenaSetState state = new ArenaUtility.ArenaSetState();
			Action<ArenaUtility.ArenaResult> <>9__1;
			Func<bool> <>9__0;
			for (int i = 0; i < count; i++)
			{
				GameComponent_DebugTools component = Current.Game.GetComponent<GameComponent_DebugTools>();
				Func<bool> callback2;
				if ((callback2 = <>9__0) == null)
				{
					callback2 = (<>9__0 = delegate()
					{
						List<PawnKindDef> lhs2 = lhs;
						List<PawnKindDef> rhs2 = rhs;
						Action<ArenaUtility.ArenaResult> callback3;
						if ((callback3 = <>9__1) == null)
						{
							callback3 = (<>9__1 = delegate(ArenaUtility.ArenaResult result)
							{
								callback(result);
								int remaining;
								remaining--;
								remaining = remaining;
								if (remaining % 10 == 0)
								{
									report();
								}
							});
						}
						return ArenaUtility.ArenaFightQueue(lhs2, rhs2, callback3, state);
					});
				}
				component.AddPerFrameCallback(callback2);
			}
		}

		// Token: 0x060036AD RID: 13997 RVA: 0x0015ED80 File Offset: 0x0015CF80
		public static void PerformBattleRoyale(IEnumerable<PawnKindDef> kindsEnumerable)
		{
			if (!ArenaUtility.ValidateArenaCapability())
			{
				return;
			}
			List<PawnKindDef> kinds = kindsEnumerable.ToList<PawnKindDef>();
			Dictionary<PawnKindDef, float> ratings = new Dictionary<PawnKindDef, float>();
			foreach (PawnKindDef pawnKindDef in kinds)
			{
				ratings[pawnKindDef] = EloUtility.CalculateRating(pawnKindDef.combatPower, 1500f, 60f);
			}
			int currentFights = 0;
			int completeFights = 0;
			Current.Game.GetComponent<GameComponent_DebugTools>().AddPerFrameCallback(delegate
			{
				int currentFights;
				if (currentFights >= 15)
				{
					return false;
				}
				PawnKindDef lhsDef = kinds.RandomElement<PawnKindDef>();
				PawnKindDef rhsDef = kinds.RandomElement<PawnKindDef>();
				float num = EloUtility.CalculateExpectation(ratings[lhsDef], ratings[rhsDef]);
				float num2 = 1f - num;
				float num3 = num;
				float num4 = Mathf.Min(num2, num3);
				num2 /= num4;
				num3 /= num4;
				float num5 = Mathf.Max(num2, num3);
				if (num5 > 40f)
				{
					return false;
				}
				float num6 = 40f / num5;
				float num7 = (float)Math.Exp((double)Rand.Range(0f, (float)Math.Log((double)num6)));
				num2 *= num7;
				num3 *= num7;
				List<PawnKindDef> lhs = Enumerable.Repeat<PawnKindDef>(lhsDef, GenMath.RoundRandom(num2)).ToList<PawnKindDef>();
				List<PawnKindDef> rhs = Enumerable.Repeat<PawnKindDef>(rhsDef, GenMath.RoundRandom(num3)).ToList<PawnKindDef>();
				currentFights++;
				currentFights = currentFights;
				ArenaUtility.BeginArenaFight(lhs, rhs, delegate(ArenaUtility.ArenaResult result)
				{
					int num8 = currentFights - 1;
					currentFights = num8;
					num8 = completeFights + 1;
					completeFights = num8;
					if (result.winner != ArenaUtility.ArenaResult.Winner.Other)
					{
						float value = ratings[lhsDef];
						float value2 = ratings[rhsDef];
						float kfactor = 8f * Mathf.Pow(0.5f, Time.realtimeSinceStartup / 900f);
						EloUtility.Update(ref value, ref value2, 0.5f, result.winner == ArenaUtility.ArenaResult.Winner.Lhs, kfactor);
						ratings[lhsDef] = value;
						ratings[rhsDef] = value2;
						Log.Message(string.Format("Scores after {0} trials:\n\n{1}", completeFights, (from v in ratings
						orderby v.Value
						select string.Format("  {0}: {1}->{2} (rating {2})", new object[]
						{
							v.Key.label,
							v.Key.combatPower,
							EloUtility.CalculateLinearScore(v.Value, 1500f, 60f).ToString("F0"),
							v.Value.ToString("F0")
						})).ToLineList(null, false)));
					}
				});
				return false;
			});
		}

		// Token: 0x04002347 RID: 9031
		private const int liveSimultaneous = 15;

		// Token: 0x02002A1E RID: 10782
		public struct ArenaResult
		{
			// Token: 0x0400AC89 RID: 44169
			public ArenaUtility.ArenaResult.Winner winner;

			// Token: 0x0400AC8A RID: 44170
			public int tickDuration;

			// Token: 0x02003F52 RID: 16210
			public enum Winner
			{
				// Token: 0x0400F9A0 RID: 63904
				Other,
				// Token: 0x0400F9A1 RID: 63905
				Lhs,
				// Token: 0x0400F9A2 RID: 63906
				Rhs
			}
		}

		// Token: 0x02002A1F RID: 10783
		private class ArenaSetState
		{
			// Token: 0x0400AC8B RID: 44171
			public int live;
		}
	}
}
