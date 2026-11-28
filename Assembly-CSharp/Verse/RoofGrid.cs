using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200030E RID: 782
	public sealed class RoofGrid : IExposable, ICellBoolGiver
	{
		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06001802 RID: 6146 RVA: 0x0009064C File Offset: 0x0008E84C
		public CellBoolDrawer Drawer
		{
			get
			{
				if (this.drawerInt == null)
				{
					this.drawerInt = new CellBoolDrawer(this, this.map.Size.x, this.map.Size.z, 3620, 0.33f);
				}
				return this.drawerInt;
			}
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06001803 RID: 6147 RVA: 0x0009069D File Offset: 0x0008E89D
		public Color Color
		{
			get
			{
				return new Color(0.3f, 1f, 0.4f);
			}
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x000906B3 File Offset: 0x0008E8B3
		public RoofGrid(Map map)
		{
			this.map = map;
			this.roofGrid = new RoofDef[map.cellIndices.NumGridCells];
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x000906D8 File Offset: 0x0008E8D8
		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, delegate(IntVec3 c)
			{
				if (this.roofGrid[this.map.cellIndices.CellToIndex(c)] != null)
				{
					return this.roofGrid[this.map.cellIndices.CellToIndex(c)].shortHash;
				}
				return 0;
			}, delegate(IntVec3 c, ushort val)
			{
				this.SetRoof(c, DefDatabase<RoofDef>.GetByShortHash(val));
			}, "roofs");
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x00090702 File Offset: 0x0008E902
		public bool GetCellBool(int index)
		{
			return this.roofGrid[index] != null && !this.map.fogGrid.IsFogged(index);
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x00090724 File Offset: 0x0008E924
		public Color GetCellExtraColor(int index)
		{
			if (RoofDefOf.RoofRockThick != null && this.roofGrid[index] == RoofDefOf.RoofRockThick)
			{
				return Color.gray;
			}
			return Color.white;
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x00090747 File Offset: 0x0008E947
		public bool Roofed(int index)
		{
			return this.roofGrid[index] != null;
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x00090754 File Offset: 0x0008E954
		public bool Roofed(int x, int z)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(x, z)] != null;
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x00090772 File Offset: 0x0008E972
		public bool Roofed(IntVec3 c)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(c)] != null;
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x0009078F File Offset: 0x0008E98F
		public RoofDef RoofAt(int index)
		{
			return this.roofGrid[index];
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x00090799 File Offset: 0x0008E999
		public RoofDef RoofAt(IntVec3 c)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x000907B3 File Offset: 0x0008E9B3
		public RoofDef RoofAt(int x, int z)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(x, z)];
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x000907D0 File Offset: 0x0008E9D0
		public void SetRoof(IntVec3 c, RoofDef def)
		{
			if (this.roofGrid[this.map.cellIndices.CellToIndex(c)] == def)
			{
				return;
			}
			this.roofGrid[this.map.cellIndices.CellToIndex(c)] = def;
			Region validRegionAt_NoRebuild = this.map.regionGrid.GetValidRegionAt_NoRebuild(c);
			if (validRegionAt_NoRebuild != null)
			{
				validRegionAt_NoRebuild.District.Notify_RoofChanged();
			}
			if (this.drawerInt != null)
			{
				this.drawerInt.SetDirty();
			}
			this.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Roofs);
			this.map.glowGrid.DirtyCell(c);
			this.map.events.Notify_RoofChanged(c);
		}

		// Token: 0x0600180F RID: 6159 RVA: 0x00090882 File Offset: 0x0008EA82
		public void RemoveRoofUnsafe(int index)
		{
			this.roofGrid[index] = null;
		}

		// Token: 0x06001810 RID: 6160 RVA: 0x0009088D File Offset: 0x0008EA8D
		public void RoofGridUpdate()
		{
			if (Find.PlaySettings.showRoofOverlay && !Find.ScreenshotModeHandler.Active)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x0400142F RID: 5167
		private Map map;

		// Token: 0x04001430 RID: 5168
		private RoofDef[] roofGrid;

		// Token: 0x04001431 RID: 5169
		private CellBoolDrawer drawerInt;
	}
}
