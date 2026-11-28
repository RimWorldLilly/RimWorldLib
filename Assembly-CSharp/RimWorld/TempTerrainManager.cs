using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200139E RID: 5022
	public class TempTerrainManager : IExposable
	{
		// Token: 0x06007C7C RID: 31868 RVA: 0x002C7AC4 File Offset: 0x002C5CC4
		public TempTerrainManager(Map map)
		{
			this.map = map;
			if (ModsConfig.OdysseyActive)
			{
				this.freezeManager = new FreezeManager(map);
			}
		}

		// Token: 0x06007C7D RID: 31869 RVA: 0x002C7AF4 File Offset: 0x002C5CF4
		public void Tick()
		{
			int num = Mathf.CeilToInt((float)this.map.Area * 0.0006f);
			int area = this.map.Area;
			using (new ProfilerBlock("Temporary Terrain Steady Effects"))
			{
				for (int i = 0; i < num; i++)
				{
					if (this.cycleIndex >= area)
					{
						this.cycleIndex = 0;
					}
					IntVec3 c = this.map.cellsInRandomOrder.Get(this.cycleIndex);
					this.DoCellSteadyEffects(c);
					this.cycleIndex++;
				}
			}
			using (new ProfilerBlock("Temporary Terrain Removal"))
			{
				while (this.terrainToRemove.Count > 0 && this.terrainToRemove.Peek().tick <= Find.TickManager.TicksGame)
				{
					IntVec3 cell = this.terrainToRemove.Dequeue().cell;
					this.map.terrainGrid.RemoveTempTerrain(cell, false, false);
				}
			}
		}

		// Token: 0x06007C7E RID: 31870 RVA: 0x002C7C18 File Offset: 0x002C5E18
		private void DoCellSteadyEffects(IntVec3 c)
		{
			if (ModsConfig.OdysseyActive)
			{
				this.freezeManager.DoCellSteadyEffects(c);
			}
		}

		// Token: 0x06007C7F RID: 31871 RVA: 0x002C7C30 File Offset: 0x002C5E30
		public void QueueRemoveTerrain(IntVec3 c, int tick)
		{
			this.terrainToRemove.Enqueue(new TerrainRemoveTick
			{
				cell = c,
				tick = tick
			}, tick);
		}

		// Token: 0x06007C80 RID: 31872 RVA: 0x002C7C64 File Offset: 0x002C5E64
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.cycleIndex, "cycleIndex", 0, false);
			Scribe_Deep.Look<FreezeManager>(ref this.freezeManager, "freezeManager", Array.Empty<object>());
			List<IntVec3> list = new List<IntVec3>();
			List<int> list2 = new List<int>();
			while (this.terrainToRemove.Count > 0)
			{
				TerrainRemoveTick terrainRemoveTick = this.terrainToRemove.Dequeue();
				list.Add(terrainRemoveTick.cell);
				list2.Add(terrainRemoveTick.tick);
			}
			Scribe_Collections.Look<IntVec3>(ref list, "terrainToRemoveCells", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<int>(ref list2, "terrainToRemoveTicks", LookMode.Value, Array.Empty<object>());
			for (int i = 0; i < list.Count; i++)
			{
				this.terrainToRemove.Enqueue(new TerrainRemoveTick
				{
					cell = list[i],
					tick = list2[i]
				}, list2[i]);
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.freezeManager == null)
			{
				this.freezeManager = new FreezeManager(this.map);
			}
		}

		// Token: 0x04004E5D RID: 20061
		private const float MapFractionCheckPerTick = 0.0006f;

		// Token: 0x04004E5E RID: 20062
		private Map map;

		// Token: 0x04004E5F RID: 20063
		private FreezeManager freezeManager;

		// Token: 0x04004E60 RID: 20064
		private int cycleIndex;

		// Token: 0x04004E61 RID: 20065
		private PriorityQueue<TerrainRemoveTick, int> terrainToRemove = new PriorityQueue<TerrainRemoveTick, int>();
	}
}
