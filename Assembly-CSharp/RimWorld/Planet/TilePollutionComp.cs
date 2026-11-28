using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002337 RID: 9015
	public class TilePollutionComp : WorldComponent
	{
		// Token: 0x1700271E RID: 10014
		// (get) Token: 0x0600DEBD RID: 57021 RVA: 0x0052DCB8 File Offset: 0x0052BEB8
		public float TotalWorldPollution
		{
			get
			{
				if (this.totalWorldPollution < 0f)
				{
					for (int i = 0; i < this.world.grid.TilesCount; i++)
					{
						this.totalWorldPollution += this.world.grid[i].pollution;
					}
				}
				return this.totalWorldPollution;
			}
		}

		// Token: 0x0600DEBE RID: 57022 RVA: 0x0052DD16 File Offset: 0x0052BF16
		public TilePollutionComp(World world) : base(world)
		{
		}

		// Token: 0x0600DEBF RID: 57023 RVA: 0x0052DD2C File Offset: 0x0052BF2C
		public override void WorldComponentTick()
		{
			int tilesCount = this.world.grid.TilesCount;
			int num = Mathf.CeilToInt(1.6666667E-05f * (float)tilesCount);
			for (int i = 0; i < num; i++)
			{
				PlanetTile tile = new PlanetTile(this.cycleIndex);
				if (!this.world.grid.InBounds(tile))
				{
					this.totalWorldPollution = this.currentWorldPollution;
					this.cycleIndex = 0;
					this.currentWorldPollution = 0f;
					break;
				}
				this.currentWorldPollution += this.world.grid[tile].pollution;
				this.cycleIndex++;
			}
			if (Find.TickManager.TicksGame % 60000 == 0)
			{
				this.DoPollutionReduction();
			}
		}

		// Token: 0x0600DEC0 RID: 57024 RVA: 0x0052DDF0 File Offset: 0x0052BFF0
		private void DoPollutionReduction()
		{
			float num = this.TotalWorldPollution * 0.0001f;
			List<int> tiles = Find.World.tilesInRandomOrder.Tiles;
			int num2 = Rand.Range(0, tiles.Count);
			for (int i = 0; i < tiles.Count; i++)
			{
				int tileId = tiles[(i + num2) % tiles.Count];
				PlanetTile planetTile = new PlanetTile(tileId);
				if (this.world.grid[planetTile].pollution > 0f)
				{
					float num3 = Mathf.Min(num, this.world.grid[planetTile].pollution);
					this.world.grid[planetTile].pollution -= num3;
					num -= num3;
					Find.World.renderer.Notify_TilePollutionChanged(planetTile);
					if (num <= 0f)
					{
						break;
					}
				}
			}
		}

		// Token: 0x0600DEC1 RID: 57025 RVA: 0x0052DED4 File Offset: 0x0052C0D4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.cycleIndex, "cycleIndex", 0, false);
			Scribe_Values.Look<float>(ref this.totalWorldPollution, "totalWorldPollution", 0f, false);
			Scribe_Values.Look<float>(ref this.currentWorldPollution, "currentWorldPollution", 0f, false);
		}

		// Token: 0x04008F07 RID: 36615
		private const float WorldFractionCheckPerTick = 1.6666667E-05f;

		// Token: 0x04008F08 RID: 36616
		private const int WorldPollutionReductionInterval = 60000;

		// Token: 0x04008F09 RID: 36617
		private const float WorldPollutionReductionFactor = 0.0001f;

		// Token: 0x04008F0A RID: 36618
		private int cycleIndex;

		// Token: 0x04008F0B RID: 36619
		private float totalWorldPollution = -1f;

		// Token: 0x04008F0C RID: 36620
		private float currentWorldPollution;
	}
}
