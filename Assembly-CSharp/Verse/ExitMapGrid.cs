using System;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200027F RID: 639
	public sealed class ExitMapGrid : ICellBoolGiver
	{
		// Token: 0x1700038F RID: 911
		// (get) Token: 0x060012B2 RID: 4786 RVA: 0x0006A7C1 File Offset: 0x000689C1
		public bool MapUsesExitGrid
		{
			get
			{
				if (Find.TickManager.TicksGame == this.lastCheckedMapTick)
				{
					return this.mapUsesExitGrid;
				}
				this.lastCheckedMapTick = Find.TickManager.TicksGame;
				this.mapUsesExitGrid = this.MapUsesExitGridNow;
				return this.mapUsesExitGrid;
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x060012B3 RID: 4787 RVA: 0x0006A800 File Offset: 0x00068A00
		private bool MapUsesExitGridNow
		{
			get
			{
				if (this.map.IsPlayerHome)
				{
					return false;
				}
				if (this.map.IsPocketMap)
				{
					return false;
				}
				if (this.map.Biome.inVacuum)
				{
					return false;
				}
				CaravansBattlefield caravansBattlefield = this.map.Parent as CaravansBattlefield;
				if (caravansBattlefield != null && caravansBattlefield.def.blockExitGridUntilBattleIsWon && !caravansBattlefield.WonBattle)
				{
					return false;
				}
				MapParent parent = this.map.Parent;
				FormCaravanComp formCaravanComp = (parent != null) ? parent.GetComponent<FormCaravanComp>() : null;
				return formCaravanComp == null || !formCaravanComp.CanFormOrReformCaravanNow;
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x060012B4 RID: 4788 RVA: 0x0006A890 File Offset: 0x00068A90
		public CellBoolDrawer Drawer
		{
			get
			{
				if (!this.MapUsesExitGrid)
				{
					return null;
				}
				if (this.dirty)
				{
					this.Rebuild();
				}
				if (this.drawerInt == null)
				{
					this.drawerInt = new CellBoolDrawer(this, this.map.Size.x, this.map.Size.z, 3690, 0.33f);
				}
				return this.drawerInt;
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x060012B5 RID: 4789 RVA: 0x0006A8F9 File Offset: 0x00068AF9
		public BoolGrid Grid
		{
			get
			{
				if (!this.MapUsesExitGrid)
				{
					return null;
				}
				if (this.dirty)
				{
					this.Rebuild();
				}
				return this.exitMapGrid;
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x060012B6 RID: 4790 RVA: 0x0006A919 File Offset: 0x00068B19
		public Color Color
		{
			get
			{
				return new Color(0.35f, 1f, 0.35f, 0.12f);
			}
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x0006A934 File Offset: 0x00068B34
		public ExitMapGrid(Map map)
		{
			this.map = map;
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x0006A955 File Offset: 0x00068B55
		public bool GetCellBool(int index)
		{
			return this.Grid[index] && !this.map.fogGrid.IsFogged(index);
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x0006A97B File Offset: 0x00068B7B
		public Color GetCellExtraColor(int index)
		{
			return Color.white;
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x0006A982 File Offset: 0x00068B82
		public bool IsExitCell(IntVec3 c)
		{
			return this.MapUsesExitGrid && c.InBounds(this.map) && this.Grid[c];
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x0006A9AA File Offset: 0x00068BAA
		public void ExitMapGridUpdate()
		{
			if (!this.MapUsesExitGrid || Find.ScreenshotModeHandler.Active)
			{
				return;
			}
			this.Drawer.MarkForDraw();
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x0006A9D7 File Offset: 0x00068BD7
		public void Notify_LOSBlockerSpawned()
		{
			this.dirty = true;
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x0006A9E0 File Offset: 0x00068BE0
		public void Notify_LOSBlockerDespawned()
		{
			this.dirty = true;
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x0006A9EC File Offset: 0x00068BEC
		private void Rebuild()
		{
			this.dirty = false;
			if (this.exitMapGrid == null)
			{
				this.exitMapGrid = new BoolGrid(this.map);
			}
			else
			{
				this.exitMapGrid.Clear();
			}
			CellRect cellRect = CellRect.WholeMap(this.map);
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					if (i > 1 && i < cellRect.maxZ - 2 + 1 && j > 1 && j < cellRect.maxX - 2 + 1)
					{
						j = cellRect.maxX - 2 + 1;
					}
					IntVec3 intVec = new IntVec3(j, 0, i);
					if (this.IsGoodExitCell(intVec))
					{
						this.exitMapGrid[intVec] = true;
					}
				}
			}
			if (this.drawerInt != null)
			{
				this.drawerInt.SetDirty();
			}
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x0006AAC0 File Offset: 0x00068CC0
		private bool IsGoodExitCell(IntVec3 cell)
		{
			if (!cell.CanBeSeenOver(this.map))
			{
				return false;
			}
			int num = GenRadial.NumCellsInRadius(2f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = cell + GenRadial.RadialPattern[i];
				if (intVec.InBounds(this.map) && intVec.OnEdge(this.map) && intVec.CanBeSeenOverFast(this.map) && GenSight.LineOfSight(cell, intVec, this.map))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040010B3 RID: 4275
		private Map map;

		// Token: 0x040010B4 RID: 4276
		private bool dirty = true;

		// Token: 0x040010B5 RID: 4277
		private BoolGrid exitMapGrid;

		// Token: 0x040010B6 RID: 4278
		private CellBoolDrawer drawerInt;

		// Token: 0x040010B7 RID: 4279
		private const int MaxDistToEdge = 2;

		// Token: 0x040010B8 RID: 4280
		[Unsaved(false)]
		private bool mapUsesExitGrid;

		// Token: 0x040010B9 RID: 4281
		[Unsaved(false)]
		private int lastCheckedMapTick = -99999;
	}
}
