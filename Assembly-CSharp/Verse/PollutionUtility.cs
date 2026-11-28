using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Noise;

namespace Verse
{
	// Token: 0x020002E7 RID: 743
	public static class PollutionUtility
	{
		// Token: 0x0600169C RID: 5788 RVA: 0x000875DC File Offset: 0x000857DC
		public static void PawnPollutionTickInterval(Pawn pawn, int delta)
		{
			if (!pawn.Spawned)
			{
				return;
			}
			if (pawn.IsHashIntervalTick(60, delta) && pawn.Position.IsPolluted(pawn.Map) && PollutionUtility.StimulatedByPollution(pawn) && !pawn.health.hediffSet.HasHediff(HediffDefOf.PollutionStimulus, false))
			{
				pawn.health.AddHediff(HediffDefOf.PollutionStimulus, null, null, null);
			}
		}

		// Token: 0x0600169D RID: 5789 RVA: 0x0008764C File Offset: 0x0008584C
		public static void Notify_TunnelHiveSpawnedInsect(Pawn pawn)
		{
			if (!pawn.Spawned || !pawn.RaceProps.Insect)
			{
				return;
			}
			if (pawn.Position.IsPolluted(pawn.Map))
			{
				pawn.health.AddHediff(HediffDefOf.PollutionStimulus, null, null, null);
				HealthUtility.AdjustSeverity(pawn, HediffDefOf.PollutionStimulus, HediffDefOf.PollutionStimulus.maxSeverity);
			}
		}

		// Token: 0x0600169E RID: 5790 RVA: 0x000876B3 File Offset: 0x000858B3
		private static bool StimulatedByPollution(Pawn pawn)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			if (pawn.RaceProps.Insect)
			{
				return true;
			}
			Pawn_GeneTracker genes = pawn.genes;
			return ((genes != null) ? genes.GetFirstGeneOfType<Gene_PollutionRush>() : null) != null;
		}

		// Token: 0x0600169F RID: 5791 RVA: 0x000876E4 File Offset: 0x000858E4
		public static bool SettableEntirelyPolluted(IPlantToGrowSettable s)
		{
			using (IEnumerator<IntVec3> enumerator = s.Cells.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.IsPolluted(s.Map))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060016A0 RID: 5792 RVA: 0x00087740 File Offset: 0x00085940
		public static bool CanPlantAt(ThingDef plantDef, IPlantToGrowSettable settable)
		{
			if (plantDef.plant.RequiresNoPollution)
			{
				using (IEnumerator<IntVec3> enumerator = settable.Cells.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.IsPolluted(settable.Map))
						{
							return true;
						}
					}
				}
				return false;
			}
			if (plantDef.plant.RequiresPollution)
			{
				using (IEnumerator<IntVec3> enumerator = settable.Cells.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.IsPolluted(settable.Map))
						{
							return true;
						}
					}
				}
				return false;
			}
			return true;
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x000877FC File Offset: 0x000859FC
		public static bool IsExecutingPollutionIgnoredQuest()
		{
			foreach (Quest quest in Find.QuestManager.QuestsListForReading)
			{
				if (quest.State == QuestState.Ongoing && (quest.root == QuestScriptDefOf.EndGame_ArchonexusVictory_FirstCycle || quest.root == QuestScriptDefOf.EndGame_ArchonexusVictory_SecondCycle))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x00087878 File Offset: 0x00085A78
		public static void GrowPollutionAt(IntVec3 root, Map map, int cellCountToPollute = 4, Action<IntVec3> onPolluteAction = null, bool silent = false, Func<IntVec3, bool> adjacentPolluteValidator = null)
		{
			PollutionUtility.<>c__DisplayClass10_0 CS$<>8__locals1 = new PollutionUtility.<>c__DisplayClass10_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.adjacentPolluteValidator = adjacentPolluteValidator;
			if (cellCountToPollute < 0)
			{
				Log.Error("Non-positive max cells value passed in PolluteClosestMapTerrain");
				return;
			}
			if (CS$<>8__locals1.map.pollutionGrid.TotalPollution >= CS$<>8__locals1.map.cellIndices.NumGridCells)
			{
				return;
			}
			if (root.CanPollute(CS$<>8__locals1.map))
			{
				root.Pollute(CS$<>8__locals1.map, false);
				cellCountToPollute--;
				if (onPolluteAction != null)
				{
					onPolluteAction(root);
				}
			}
			if (cellCountToPollute <= 0)
			{
				return;
			}
			PollutionUtility.tmpPollutableCells = new FastPriorityQueue<IntVec3>(new PollutionUtility.PollutionCellComparer(root, CS$<>8__locals1.map, 0.015f));
			CS$<>8__locals1.map.floodFiller.FloodFill(root, (IntVec3 x) => x.IsPolluted(CS$<>8__locals1.map), delegate(IntVec3 x)
			{
				PollutionUtility.tmpPollutedCells.Add(x);
			}, int.MaxValue, false, null);
			if (PollutionUtility.tmpPollutedCells.Count == 0)
			{
				return;
			}
			PollutionUtility.tmpPollutableCells.Clear();
			for (int i = 0; i < PollutionUtility.tmpPollutedCells.Count; i++)
			{
				foreach (IntVec3 item in CS$<>8__locals1.<GrowPollutionAt>g__GetAdjacentPollutableCells|0(PollutionUtility.tmpPollutedCells[i], CS$<>8__locals1.map))
				{
					if (!PollutionUtility.tmpPollutableCells.Contains(item))
					{
						PollutionUtility.tmpPollutableCells.Push(item);
					}
				}
			}
			PollutionUtility.tmpPollutedCells.Clear();
			while (cellCountToPollute > 0 && PollutionUtility.tmpPollutableCells.Count > 0)
			{
				IntVec3 intVec = PollutionUtility.tmpPollutableCells.Pop();
				CS$<>8__locals1.map.pollutionGrid.SetPolluted(intVec, true, silent);
				if (onPolluteAction != null)
				{
					onPolluteAction(intVec);
				}
				foreach (IntVec3 item2 in CS$<>8__locals1.<GrowPollutionAt>g__GetAdjacentPollutableCells|0(intVec, CS$<>8__locals1.map))
				{
					if (!PollutionUtility.tmpPollutableCells.Contains(item2))
					{
						PollutionUtility.tmpPollutableCells.Push(item2);
					}
				}
				cellCountToPollute--;
			}
		}

		// Token: 0x060016A3 RID: 5795 RVA: 0x00087A90 File Offset: 0x00085C90
		public static PollutionLevel PollutionLevel(this Tile tile)
		{
			if (tile.pollution < 0.25f)
			{
				return Verse.PollutionLevel.None;
			}
			if (tile.pollution < 0.5f)
			{
				return Verse.PollutionLevel.Light;
			}
			if (tile.pollution < 0.75f)
			{
				return Verse.PollutionLevel.Moderate;
			}
			return Verse.PollutionLevel.Extreme;
		}

		// Token: 0x060016A4 RID: 5796 RVA: 0x00087AC0 File Offset: 0x00085CC0
		public static void PolluteMapToPercent(Map map, float mapPollutionPercent, int globSize = 1000)
		{
			PollutionUtility.<>c__DisplayClass13_0 CS$<>8__locals1 = new PollutionUtility.<>c__DisplayClass13_0();
			CS$<>8__locals1.map = map;
			mapPollutionPercent = Mathf.Clamp01(mapPollutionPercent);
			float totalPollutionPercent = CS$<>8__locals1.map.pollutionGrid.TotalPollutionPercent;
			if (Mathf.Approximately(mapPollutionPercent, totalPollutionPercent))
			{
				return;
			}
			List<IntVec3> allPollutableCells = CS$<>8__locals1.map.pollutionGrid.AllPollutableCells;
			if (mapPollutionPercent < totalPollutionPercent)
			{
				PollutionUtility.tmpMapCells.Clear();
				PollutionUtility.tmpMapCells.AddRange(allPollutableCells.InRandomOrder(null));
				float num = totalPollutionPercent - mapPollutionPercent;
				float num2 = Mathf.Max(0f, (float)PollutionUtility.tmpMapCells.Count * num);
				int num3 = 0;
				while ((float)num3 < num2)
				{
					PollutionUtility.tmpMapCells[num3].Unpollute(CS$<>8__locals1.map);
					num3++;
				}
				PollutionUtility.tmpMapCells.Clear();
				return;
			}
			int num4 = Mathf.FloorToInt((float)allPollutableCells.Count * mapPollutionPercent);
			int num5 = Mathf.CeilToInt((float)num4 / (float)globSize);
			for (int i = 0; i < num5; i++)
			{
				int num6 = Mathf.Min(globSize, num4);
				PollutionUtility.GrowPollutionAt(allPollutableCells.RandomElementByWeight(new Func<IntVec3, float>(CS$<>8__locals1.<PolluteMapToPercent>g__GlobCellSelectionWeight|0)), CS$<>8__locals1.map, num6, null, false, null);
				num4 -= num6;
			}
		}

		// Token: 0x04001369 RID: 4969
		private const float PollutionPerlinFrequency = 0.015f;

		// Token: 0x0400136A RID: 4970
		private static FastPriorityQueue<IntVec3> tmpPollutableCells;

		// Token: 0x0400136B RID: 4971
		private static readonly List<IntVec3> tmpPollutedCells = new List<IntVec3>();

		// Token: 0x0400136C RID: 4972
		private static readonly List<IntVec3> tmpMapCells = new List<IntVec3>();

		// Token: 0x02002620 RID: 9760
		internal class PollutionCellComparer : IComparer<IntVec3>
		{
			// Token: 0x0600F21E RID: 61982 RVA: 0x0058CEAC File Offset: 0x0058B0AC
			public PollutionCellComparer(IntVec3 root, Map map, float frequency = 0.015f)
			{
				this.root = root;
				this.map = map;
				this.perlin = new Perlin((double)frequency, 2.0, 0.5, 6, map.uniqueID, QualityMode.Medium);
				this.perlin = new ScaleBias(0.5, 0.5, this.perlin);
			}

			// Token: 0x0600F21F RID: 61983 RVA: 0x0058CF18 File Offset: 0x0058B118
			private float PolluteScore(IntVec3 c)
			{
				float num = 1f;
				num *= 1f / c.DistanceTo(this.root);
				num *= 1f + (float)this.perlin.GetValue((double)c.x, (double)c.y, (double)c.z) * 2f;
				if (MapGenerator.mapBeingGenerated == this.map)
				{
					num *= c.DistanceTo(MapGenerator.PlayerStartSpot) / this.map.Size.LengthHorizontal;
				}
				return num * (1f + (float)this.AdjacentPollutedCount(c) / 8f * 0.25f);
			}

			// Token: 0x0600F220 RID: 61984 RVA: 0x0058CFC0 File Offset: 0x0058B1C0
			private int AdjacentPollutedCount(IntVec3 c)
			{
				int num = 0;
				for (int i = 0; i < GenAdj.AdjacentCells.Length; i++)
				{
					IntVec3 b = GenAdj.AdjacentCells[i];
					IntVec3 c2 = c + b;
					if (c2.InBounds(this.map) && c2.IsPolluted(this.map))
					{
						num++;
					}
				}
				return num;
			}

			// Token: 0x0600F221 RID: 61985 RVA: 0x0058D018 File Offset: 0x0058B218
			public int Compare(IntVec3 a, IntVec3 b)
			{
				float num = this.PolluteScore(a);
				float num2 = this.PolluteScore(b);
				if (num < num2)
				{
					return 1;
				}
				if (num > num2)
				{
					return -1;
				}
				return 0;
			}

			// Token: 0x04009C43 RID: 40003
			private const float NoisyEdgeFactor = 0.25f;

			// Token: 0x04009C44 RID: 40004
			private const float PerlinNoiseFactor = 2f;

			// Token: 0x04009C45 RID: 40005
			private readonly IntVec3 root;

			// Token: 0x04009C46 RID: 40006
			private readonly Map map;

			// Token: 0x04009C47 RID: 40007
			private readonly ModuleBase perlin;
		}
	}
}
