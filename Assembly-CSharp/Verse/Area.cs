using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200023C RID: 572
	public abstract class Area : IExposable, ILoadReferenceable, ICellBoolGiver, IRenameable
	{
		// Token: 0x17000337 RID: 823
		// (get) Token: 0x060010C5 RID: 4293 RVA: 0x0006079B File Offset: 0x0005E99B
		// (set) Token: 0x060010C6 RID: 4294 RVA: 0x000607A3 File Offset: 0x0005E9A3
		public virtual string RenamableLabel { get; set; }

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x060010C7 RID: 4295 RVA: 0x000607AC File Offset: 0x0005E9AC
		// (set) Token: 0x060010C8 RID: 4296 RVA: 0x000607B4 File Offset: 0x0005E9B4
		public virtual string BaseLabel { get; set; }

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x060010C9 RID: 4297 RVA: 0x000607BD File Offset: 0x0005E9BD
		public virtual string InspectLabel
		{
			get
			{
				return this.RenamableLabel;
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x060010CA RID: 4298 RVA: 0x000607C5 File Offset: 0x0005E9C5
		public Map Map
		{
			get
			{
				return this.areaManager.map;
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x060010CB RID: 4299 RVA: 0x000607D2 File Offset: 0x0005E9D2
		public BoolGrid InnerGrid
		{
			get
			{
				return this.innerGrid;
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x060010CC RID: 4300 RVA: 0x000607DA File Offset: 0x0005E9DA
		public int TrueCount
		{
			get
			{
				return this.innerGrid.TrueCount;
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x060010CD RID: 4301
		public abstract string Label { get; }

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x060010CE RID: 4302
		public abstract Color Color { get; }

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x060010CF RID: 4303
		public abstract int ListPriority { get; }

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x060010D0 RID: 4304 RVA: 0x000607E7 File Offset: 0x0005E9E7
		public Texture2D ColorTexture
		{
			get
			{
				if (this.colorTextureInt == null)
				{
					this.colorTextureInt = SolidColorMaterials.NewSolidColorTexture(this.Color);
				}
				return this.colorTextureInt;
			}
		}

		// Token: 0x17000341 RID: 833
		public bool this[int index]
		{
			get
			{
				return this.innerGrid[index];
			}
			set
			{
				this.Set(this.Map.cellIndices.IndexToCell(index), value);
			}
		}

		// Token: 0x17000342 RID: 834
		public bool this[IntVec3 c]
		{
			get
			{
				return this.innerGrid[this.Map.cellIndices.CellToIndex(c)];
			}
			set
			{
				this.Set(c, value);
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x060010D5 RID: 4309 RVA: 0x00060860 File Offset: 0x0005EA60
		private CellBoolDrawer Drawer
		{
			get
			{
				if (this.drawer == null)
				{
					this.drawer = new CellBoolDrawer(this, this.Map.Size.x, this.Map.Size.z, 3650, 0.33f);
				}
				return this.drawer;
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x060010D6 RID: 4310 RVA: 0x000608B1 File Offset: 0x0005EAB1
		public IEnumerable<IntVec3> ActiveCells
		{
			get
			{
				return this.innerGrid.ActiveCells;
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x060010D7 RID: 4311 RVA: 0x000608BE File Offset: 0x0005EABE
		public virtual bool Mutable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x000608C1 File Offset: 0x0005EAC1
		public Area()
		{
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x000608D0 File Offset: 0x0005EAD0
		public Area(AreaManager areaManager)
		{
			this.areaManager = areaManager;
			this.innerGrid = new BoolGrid(areaManager.map);
			this.ID = Find.UniqueIDsManager.GetNextAreaID();
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x00060907 File Offset: 0x0005EB07
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Deep.Look<BoolGrid>(ref this.innerGrid, "innerGrid", Array.Empty<object>());
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x00060930 File Offset: 0x0005EB30
		public bool GetCellBool(int index)
		{
			return this.innerGrid[index];
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x0006093E File Offset: 0x0005EB3E
		public Color GetCellExtraColor(int index)
		{
			return Color.white;
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x00060945 File Offset: 0x0005EB45
		public virtual bool AssignableAsAllowed()
		{
			return false;
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x00060948 File Offset: 0x0005EB48
		protected virtual void Set(IntVec3 c, bool val)
		{
			int index = this.Map.cellIndices.CellToIndex(c);
			if (this.innerGrid[index] == val)
			{
				return;
			}
			this.innerGrid[index] = val;
			this.MarkDirty(c);
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x0006098C File Offset: 0x0005EB8C
		private void MarkDirty(IntVec3 c)
		{
			this.Drawer.SetDirty();
			this.Map.pathFinder.MapData.Notify_AreaDelta(this, c);
			Region region = c.GetRegion(this.Map, RegionType.Set_All);
			if (region != null)
			{
				region.Notify_AreaChanged(this);
			}
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x000609D4 File Offset: 0x0005EBD4
		public void Delete()
		{
			this.areaManager.Remove(this);
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x000609E2 File Offset: 0x0005EBE2
		public void MarkForDraw()
		{
			if (this.Map == Find.CurrentMap && !Find.ScreenshotModeHandler.Active)
			{
				this.Drawer.MarkForDraw();
			}
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x00060A08 File Offset: 0x0005EC08
		public void AreaUpdate()
		{
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x00060A15 File Offset: 0x0005EC15
		public void Invert()
		{
			this.innerGrid.Invert();
			this.Drawer.SetDirty();
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x00060A2D File Offset: 0x0005EC2D
		public void Clear()
		{
			this.innerGrid.Clear();
			this.Drawer.SetDirty();
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x00060A45 File Offset: 0x0005EC45
		protected void InvalidateColorTexture()
		{
			Object.Destroy(this.colorTextureInt);
			this.colorTextureInt = null;
			CellBoolDrawer cellBoolDrawer = this.drawer;
			if (cellBoolDrawer == null)
			{
				return;
			}
			cellBoolDrawer.Notify_ColorChanged();
		}

		// Token: 0x060010E6 RID: 4326
		public abstract string GetUniqueLoadID();

		// Token: 0x04000F7C RID: 3964
		public AreaManager areaManager;

		// Token: 0x04000F7D RID: 3965
		public int ID = -1;

		// Token: 0x04000F7E RID: 3966
		private BoolGrid innerGrid;

		// Token: 0x04000F7F RID: 3967
		private CellBoolDrawer drawer;

		// Token: 0x04000F80 RID: 3968
		private Texture2D colorTextureInt;
	}
}
