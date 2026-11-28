using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002A7 RID: 679
	public sealed class MapDrawer
	{
		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x060014CC RID: 5324 RVA: 0x00078268 File Offset: 0x00076468
		private IntVec2 SectionCount
		{
			get
			{
				return new IntVec2
				{
					x = Mathf.CeilToInt((float)this.map.Size.x / 17f),
					z = Mathf.CeilToInt((float)this.map.Size.z / 17f)
				};
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x060014CD RID: 5325 RVA: 0x000782C4 File Offset: 0x000764C4
		private CellRect VisibleSections
		{
			get
			{
				CellRect viewRect = this.ViewRect;
				IntVec2 intVec = this.SectionCoordsAt(viewRect.Min);
				IntVec2 intVec2 = this.SectionCoordsAt(viewRect.Max);
				if (intVec2.x < intVec.x || intVec2.z < intVec.z)
				{
					return CellRect.Empty;
				}
				return CellRect.FromLimits(intVec.x, intVec.z, intVec2.x, intVec2.z);
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x060014CE RID: 5326 RVA: 0x00078334 File Offset: 0x00076534
		private CellRect ViewRect
		{
			get
			{
				return Find.CameraDriver.CurrentViewRect.ExpandedBy(1).ClipInsideMap(this.map);
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x060014CF RID: 5327 RVA: 0x00078362 File Offset: 0x00076562
		public List<MapDrawLayer> GlobalLayers
		{
			get
			{
				return this.global;
			}
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x0007836A File Offset: 0x0007656A
		public MapDrawer(Map map)
		{
			this.map = map;
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x0007837C File Offset: 0x0007657C
		public T GetGlobalLayer<T>() where T : MapDrawLayer
		{
			for (int i = 0; i < this.global.Count; i++)
			{
				T t = this.global[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x000783CC File Offset: 0x000765CC
		public void MapMeshDirty(IntVec3 loc, ulong dirtyFlags)
		{
			bool regenAdjacentCells = (dirtyFlags & (MapMeshFlagDefOf.FogOfWar | MapMeshFlagDefOf.Buildings | MapMeshFlagDefOf.Roofs)) > 0UL;
			this.MapMeshDirty(loc, dirtyFlags, regenAdjacentCells, false);
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x0007840C File Offset: 0x0007660C
		public void MapMeshDirty(IntVec3 loc, ulong dirtyFlags, bool regenAdjacentCells, bool regenAdjacentSections)
		{
			if (Current.ProgramState != ProgramState.Playing || this.sections == null)
			{
				return;
			}
			this.SectionAt(loc).dirtyFlags |= dirtyFlags;
			this.globalDirtyFlags |= dirtyFlags;
			if (regenAdjacentCells)
			{
				for (int i = 0; i < 8; i++)
				{
					IntVec3 intVec = loc + GenAdj.AdjacentCells[i];
					if (intVec.InBounds(this.map))
					{
						this.SectionAt(intVec).dirtyFlags |= dirtyFlags;
					}
				}
			}
			if (regenAdjacentSections)
			{
				IntVec2 a = this.SectionCoordsAt(loc);
				for (int j = 0; j < 8; j++)
				{
					IntVec3 intVec2 = GenAdj.AdjacentCells[j];
					IntVec2 intVec3 = a + new IntVec2(intVec2.x, intVec2.z);
					IntVec2 sectionCount = this.SectionCount;
					if (intVec3.x >= 0 && intVec3.z >= 0 && intVec3.x <= sectionCount.x - 1 && intVec3.z <= sectionCount.z - 1)
					{
						this.sections[intVec3.x, intVec3.z].dirtyFlags |= dirtyFlags;
					}
				}
			}
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x00078540 File Offset: 0x00076740
		public void MapMeshDrawerUpdate_First()
		{
			if (this.globalDirtyFlags != 0UL)
			{
				for (int i = 0; i < this.global.Count; i++)
				{
					MapDrawLayer mapDrawLayer = this.global[i];
					mapDrawLayer.Dirty = (mapDrawLayer.Dirty || (this.globalDirtyFlags & mapDrawLayer.relevantChangeTypes) > 0UL);
					if (mapDrawLayer.Dirty)
					{
						mapDrawLayer.Regenerate();
					}
				}
			}
			CellRect viewRect = this.ViewRect;
			bool flag = false;
			Section[,] array = this.sections;
			int upperBound = array.GetUpperBound(0);
			int upperBound2 = array.GetUpperBound(1);
			for (int j = array.GetLowerBound(0); j <= upperBound; j++)
			{
				for (int k = array.GetLowerBound(1); k <= upperBound2; k++)
				{
					if (array[j, k].TryUpdate(viewRect))
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				return;
			}
			for (int l = 0; l < this.SectionCount.x; l++)
			{
				for (int m = 0; m < this.SectionCount.z; m++)
				{
					if (this.sections[l, m].TryUpdate(viewRect))
					{
						return;
					}
				}
			}
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x00078664 File Offset: 0x00076864
		public void DrawMapMesh()
		{
			CellRect view = this.ViewRect;
			if (WorldComponent_GravshipController.GravshipRenderInProgess)
			{
				view = view.Encapsulate(WorldComponent_GravshipController.GravshipRenderBounds);
			}
			for (int i = 0; i < this.global.Count; i++)
			{
				MapDrawLayer mapDrawLayer = this.global[i];
				if (mapDrawLayer.Visible)
				{
					if (mapDrawLayer.Dirty)
					{
						mapDrawLayer.Regenerate();
						mapDrawLayer.RefreshSubMeshBounds();
					}
					mapDrawLayer.DrawLayer();
				}
			}
			for (int j = 0; j < this.SectionCount.x; j++)
			{
				for (int k = 0; k < this.SectionCount.z; k++)
				{
					Section section = this.sections[j, k];
					if (view.Overlaps(section.Bounds))
					{
						section.DrawSection();
					}
					else
					{
						section.DrawDynamicSections(view);
					}
				}
			}
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x00078732 File Offset: 0x00076932
		private IntVec2 SectionCoordsAt(IntVec3 loc)
		{
			return new IntVec2(Mathf.FloorToInt((float)(loc.x / 17)), Mathf.FloorToInt((float)(loc.z / 17)));
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x00078758 File Offset: 0x00076958
		public Section SectionAt(IntVec3 loc)
		{
			IntVec2 intVec = this.SectionCoordsAt(loc);
			return this.sections[intVec.x, intVec.z];
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x00078784 File Offset: 0x00076984
		public void RegenerateLayerNow(Type type)
		{
			this.EnsureGlobalLayersInitialized();
			if (this.sections == null)
			{
				this.sections = new Section[this.SectionCount.x, this.SectionCount.z];
			}
			for (int i = 0; i < this.global.Count; i++)
			{
				MapDrawLayer mapDrawLayer = this.global[i];
				if (mapDrawLayer.GetType() == type && mapDrawLayer.Visible)
				{
					this.global[i].Regenerate();
					mapDrawLayer.RefreshSubMeshBounds();
				}
			}
			for (int j = 0; j < this.SectionCount.x; j++)
			{
				for (int k = 0; k < this.SectionCount.z; k++)
				{
					Section[,] array = this.sections;
					int num = j;
					int num2 = k;
					if (array[num, num2] == null)
					{
						array[num, num2] = new Section(new IntVec3(j, 0, k), this.map);
					}
					this.sections[j, k].RegenerateSingleLayer(this.sections[j, k].GetLayer(type));
				}
			}
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x000788A0 File Offset: 0x00076AA0
		public void RegenerateEverythingNow()
		{
			this.EnsureGlobalLayersInitialized();
			if (this.sections == null)
			{
				this.sections = new Section[this.SectionCount.x, this.SectionCount.z];
			}
			for (int i = 0; i < this.global.Count; i++)
			{
				MapDrawLayer mapDrawLayer = this.global[i];
				if (mapDrawLayer.Visible)
				{
					mapDrawLayer.Regenerate();
					mapDrawLayer.RefreshSubMeshBounds();
				}
			}
			for (int j = 0; j < this.SectionCount.x; j++)
			{
				for (int k = 0; k < this.SectionCount.z; k++)
				{
					Section[,] array = this.sections;
					int num = j;
					int num2 = k;
					if (array[num, num2] == null)
					{
						array[num, num2] = new Section(new IntVec3(j, 0, k), this.map);
					}
					this.sections[j, k].RegenerateAllLayers();
				}
			}
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x0007898C File Offset: 0x00076B8C
		private void EnsureGlobalLayersInitialized()
		{
			if (this.global != null)
			{
				return;
			}
			this.global = new List<MapDrawLayer>();
			foreach (Type type in typeof(MapDrawLayer).AllSubclassesNonAbstract())
			{
				if (!typeof(SectionLayer).IsAssignableFrom(type))
				{
					this.global.Add((MapDrawLayer)Activator.CreateInstance(type, new object[]
					{
						this.map
					}));
				}
			}
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x00078A2C File Offset: 0x00076C2C
		public void WholeMapChanged(ulong change)
		{
			this.globalDirtyFlags |= change;
			for (int i = 0; i < this.SectionCount.x; i++)
			{
				for (int j = 0; j < this.SectionCount.z; j++)
				{
					this.sections[i, j].dirtyFlags |= change;
				}
			}
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x00078A90 File Offset: 0x00076C90
		public void Dispose()
		{
			for (int i = 0; i < this.SectionCount.x; i++)
			{
				for (int j = 0; j < this.SectionCount.z; j++)
				{
					this.sections[i, j].Dispose();
				}
			}
			this.sections = null;
			foreach (MapDrawLayer mapDrawLayer in this.global)
			{
				mapDrawLayer.Dispose();
			}
		}

		// Token: 0x04001261 RID: 4705
		private readonly Map map;

		// Token: 0x04001262 RID: 4706
		private Section[,] sections;

		// Token: 0x04001263 RID: 4707
		private List<MapDrawLayer> global;

		// Token: 0x04001264 RID: 4708
		private ulong globalDirtyFlags;
	}
}
