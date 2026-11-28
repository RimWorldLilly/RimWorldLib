using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200029C RID: 668
	[StaticConstructorOnStartup]
	public class LavaFXComponent : MapComponent
	{
		// Token: 0x06001477 RID: 5239 RVA: 0x000754DC File Offset: 0x000736DC
		public LavaFXComponent(Map map) : base(map)
		{
			map.events.TerrainChanged += this.Notify_OnTerrainChanged;
			this.deepLavaSpawner = new CellFXSpawner(0.015f, new Action<Vector3>(this.EmitDeepLavaFX));
			this.shallowLavaSpawner = new CellFXSpawner(0.015f, new Action<Vector3>(this.EmitShallowLavaFX));
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x0007553F File Offset: 0x0007373F
		private void Notify_OnTerrainChanged(IntVec3 cell)
		{
			if (!this.TryAddLavaCell(cell))
			{
				this.deepLavaSpawner.Cells.Remove(cell);
				this.shallowLavaSpawner.Cells.Remove(cell);
			}
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x0007556E File Offset: 0x0007376E
		public override void FinalizeInit()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.RefreshLavaCells();
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x0007557E File Offset: 0x0007377E
		public override void MapComponentTick()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.deepLavaSpawner.Tick();
			this.shallowLavaSpawner.Tick();
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x0007559E File Offset: 0x0007379E
		public void EmitDeepLavaFX(Vector3 pos)
		{
			FleckMaker.ThrowMicroSparks(pos, this.map);
			this.ThrowLavaSmoke(pos, this.map, 1.5f);
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x000755BE File Offset: 0x000737BE
		public void EmitShallowLavaFX(Vector3 pos)
		{
			FleckMaker.ThrowMicroSparks(pos, this.map);
			FleckMaker.ThrowSmoke(pos, this.map, 1.5f);
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x000755E0 File Offset: 0x000737E0
		private void RefreshLavaCells()
		{
			this.deepLavaSpawner.Cells.Clear();
			this.shallowLavaSpawner.Cells.Clear();
			for (int i = 0; i < this.map.Size.x; i++)
			{
				for (int j = 0; j < this.map.Size.z; j++)
				{
					this.TryAddLavaCell(new IntVec3(i, 0, j));
				}
			}
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x00075654 File Offset: 0x00073854
		private bool TryAddLavaCell(IntVec3 cell)
		{
			if (this.map.terrainGrid.TerrainAt(cell) == TerrainDefOf.LavaDeep)
			{
				this.deepLavaSpawner.Cells.Add(cell);
				return true;
			}
			if (this.map.terrainGrid.TempTerrainAt(cell) == TerrainDefOf.LavaShallow)
			{
				this.shallowLavaSpawner.Cells.Add(cell);
				return true;
			}
			return false;
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x000756B8 File Offset: 0x000738B8
		private void ThrowLavaSmoke(Vector3 loc, Map map, float size)
		{
			if (!loc.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.LavaSmoke, Rand.Range(1.5f, 2.5f) * size);
			dataStatic.rotationRate = Rand.Range(-30f, 30f);
			dataStatic.velocityAngle = (float)Rand.Range(30, 40);
			dataStatic.velocitySpeed = Rand.Range(0.5f, 0.7f);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x0400121B RID: 4635
		private CellFXSpawner deepLavaSpawner;

		// Token: 0x0400121C RID: 4636
		private CellFXSpawner shallowLavaSpawner;

		// Token: 0x0400121D RID: 4637
		private const float DeepLavaFXPerTilePerSecond = 0.015f;

		// Token: 0x0400121E RID: 4638
		private const float ShallowLavaFXPerTilePerSecond = 0.015f;
	}
}
