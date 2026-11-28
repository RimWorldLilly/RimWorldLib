using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001148 RID: 4424
	public class GameCondition_LavaFlow : GameCondition
	{
		// Token: 0x17001309 RID: 4873
		// (get) Token: 0x06006F11 RID: 28433 RVA: 0x0026E417 File Offset: 0x0026C617
		private int FlowDurationTicks
		{
			get
			{
				return this.maxFlow * 5;
			}
		}

		// Token: 0x06006F12 RID: 28434 RVA: 0x0026E421 File Offset: 0x0026C621
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.openCells, "openCells", LookMode.Value);
			Scribe_Values.Look<int>(ref this.maxFlow, "maxFlow", 0, false);
		}

		// Token: 0x06006F13 RID: 28435 RVA: 0x0026E44C File Offset: 0x0026C64C
		public override void Init()
		{
			base.Init();
			this.maxFlow = GameCondition_LavaFlow.MaxFlowRange.RandomInRange;
			foreach (IntVec3 intVec in this.GetInitialCells(base.SingleMap))
			{
				base.SingleMap.terrainGrid.SetTempTerrain(intVec, TerrainDefOf.LavaShallow);
				base.SingleMap.tempTerrain.QueueRemoveTerrain(intVec, Find.TickManager.TicksGame + this.FlowDurationTicks + 60000);
				this.openCells.Add(intVec);
			}
			if (this.openCells.Count == 0)
			{
				this.End();
				return;
			}
			Find.LetterStack.ReceiveLetter("LetterLabelLavaFlow".Translate(), "LetterLavaFlow".Translate(), LetterDefOf.ThreatSmall, new LookTargets(this.openCells.First<IntVec3>(), base.SingleMap), null, null, null, null, 0, true);
		}

		// Token: 0x06006F14 RID: 28436 RVA: 0x0026E550 File Offset: 0x0026C750
		private IEnumerable<IntVec3> GetInitialCells(Map map)
		{
			GameCondition_LavaFlow.<GetInitialCells>d__10 <GetInitialCells>d__ = new GameCondition_LavaFlow.<GetInitialCells>d__10(-2);
			<GetInitialCells>d__.<>3__map = map;
			return <GetInitialCells>d__;
		}

		// Token: 0x06006F15 RID: 28437 RVA: 0x0026E560 File Offset: 0x0026C760
		public override void GameConditionTick()
		{
			if (!base.SingleMap.IsHashIntervalTick(5))
			{
				return;
			}
			this.SpreadLava();
			if (Find.TickManager.TicksGame > this.startTick + this.FlowDurationTicks || this.openCells.Count == 0)
			{
				this.End();
			}
		}

		// Token: 0x06006F16 RID: 28438 RVA: 0x0026E5B0 File Offset: 0x0026C7B0
		private void SpreadLava()
		{
			IntVec3 intVec;
			if (!this.openCells.TryRandomElementByWeight((IntVec3 c) => (float)(Mathf.Max(this.AdjacentLavaCells(c), 1) * 4), out intVec))
			{
				intVec = this.openCells.RandomElement<IntVec3>();
			}
			if (this.tempAdjCells == null)
			{
				this.tempAdjCells = GenAdj.CardinalDirections.ToList<IntVec3>();
			}
			this.tempAdjCells.Shuffle<IntVec3>();
			bool flag = true;
			foreach (IntVec3 b in this.tempAdjCells)
			{
				IntVec3 intVec2 = intVec + b;
				if (this.CanLavaPotentiallySpreadInto(intVec2))
				{
					flag = false;
				}
				if (this.CanLavaSpreadInto(intVec2))
				{
					base.SingleMap.terrainGrid.SetTempTerrain(intVec2, TerrainDefOf.LavaShallow);
					base.SingleMap.tempTerrain.QueueRemoveTerrain(intVec2, Find.TickManager.TicksGame + this.FlowDurationTicks + 60000);
					this.openCells.Add(intVec2);
					break;
				}
			}
			if (flag)
			{
				this.openCells.Remove(intVec);
			}
		}

		// Token: 0x06006F17 RID: 28439 RVA: 0x0026E6C8 File Offset: 0x0026C8C8
		private bool CanLavaPotentiallySpreadInto(IntVec3 c)
		{
			return c.InBounds(base.SingleMap) && !this.openCells.Contains(c) && base.SingleMap.terrainGrid.FoundationAt(c) == null && (base.SingleMap.terrainGrid.UnderTerrainAt(c) ?? base.SingleMap.terrainGrid.TopTerrainAt(c)) == TerrainDefOf.VolcanicRock && base.SingleMap.terrainGrid.TerrainAt(c) != TerrainDefOf.LavaShallow;
		}

		// Token: 0x06006F18 RID: 28440 RVA: 0x0026E754 File Offset: 0x0026C954
		private bool CanLavaSpreadInto(IntVec3 c)
		{
			if (!this.CanLavaPotentiallySpreadInto(c))
			{
				return false;
			}
			Building edifice = c.GetEdifice(base.SingleMap);
			return edifice == null || edifice.IsClearableFreeBuilding;
		}

		// Token: 0x06006F19 RID: 28441 RVA: 0x0026E788 File Offset: 0x0026C988
		private int AdjacentLavaCells(IntVec3 c)
		{
			int num = 0;
			foreach (IntVec3 b in GenAdj.CardinalDirections)
			{
				IntVec3 c2 = c + b;
				if (c2.InBounds(base.SingleMap) && c2.GetTerrain(base.SingleMap) == TerrainDefOf.LavaShallow)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x04004709 RID: 18185
		private HashSet<IntVec3> openCells = new HashSet<IntVec3>();

		// Token: 0x0400470A RID: 18186
		private int maxFlow;

		// Token: 0x0400470B RID: 18187
		private const int FlowRateTicks = 5;

		// Token: 0x0400470C RID: 18188
		private const int CoolDurationTicks = 60000;

		// Token: 0x0400470D RID: 18189
		private static readonly IntRange StartPositionCountRange = new IntRange(3, 6);

		// Token: 0x0400470E RID: 18190
		private static readonly IntRange MaxFlowRange = new IntRange(20000, 32000);

		// Token: 0x0400470F RID: 18191
		private List<IntVec3> tempAdjCells;
	}
}
