using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000286 RID: 646
	public class PollutionGrid : IExposable
	{
		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06001335 RID: 4917 RVA: 0x0006CBAB File Offset: 0x0006ADAB
		public int TotalPollution
		{
			get
			{
				return this.grid.TrueCount;
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06001336 RID: 4918 RVA: 0x0006CBB8 File Offset: 0x0006ADB8
		public List<IntVec3> AllPollutableCells
		{
			get
			{
				this.allPollutableCells.Clear();
				this.allPollutableCells.AddRange(this.map.AllCells.Where(new Func<IntVec3, bool>(this.EverPollutable)));
				return this.allPollutableCells;
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06001337 RID: 4919 RVA: 0x0006CBF2 File Offset: 0x0006ADF2
		public float TotalPollutionPercent
		{
			get
			{
				return (float)this.grid.TrueCount / (float)this.AllPollutableCells.Count;
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06001338 RID: 4920 RVA: 0x0006CC10 File Offset: 0x0006AE10
		public CellBoolDrawer Drawer
		{
			get
			{
				if (this.drawerInt == null)
				{
					this.drawerInt = new CellBoolDrawer(new Func<int, bool>(this.CellBoolDrawerGetBoolInt), new Func<Color>(this.CellBoolDrawerColorInt), new Func<int, Color>(this.CellBoolDrawerGetExtraColorInt), this.map.Size.x, this.map.Size.z, 3650, 0.33f);
				}
				return this.drawerInt;
			}
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x0006CC84 File Offset: 0x0006AE84
		public PollutionGrid(Map map)
		{
			this.grid = new BoolGrid(map);
			this.map = map;
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x0006CCB5 File Offset: 0x0006AEB5
		public bool IsPolluted(IntVec3 cell)
		{
			return ModsConfig.BiotechActive && cell.InBounds(this.map) && this.grid[cell];
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x0006CCDC File Offset: 0x0006AEDC
		public bool EverPollutable(IntVec3 cell)
		{
			if (!cell.InBounds(this.map))
			{
				return false;
			}
			Building edifice = cell.GetEdifice(this.map);
			return (edifice == null || edifice.def.EverPollutable) && cell.GetTerrain(this.map).canBePolluted;
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x0006CD2C File Offset: 0x0006AF2C
		public void SetPolluted(IntVec3 cell, bool isPolluted, bool silent = false)
		{
			if (!ModLister.CheckBiotech("Set pollution"))
			{
				return;
			}
			if (!cell.InBounds(this.map) || this.grid[cell] == isPolluted)
			{
				return;
			}
			this.grid.Set(cell, isPolluted);
			this.dirty = true;
			this.map.mapDrawer.MapMeshDirty(cell, MapMeshFlagDefOf.Terrain, true, false);
			this.map.mapDrawer.MapMeshDirty(cell, MapMeshFlagDefOf.Pollution, false, false);
			this.Drawer.SetDirty();
			this.map.fertilityGrid.Drawer.SetDirty();
			WaterBodyTracker waterBodyTracker = this.map.waterBodyTracker;
			if (waterBodyTracker != null)
			{
				waterBodyTracker.Notify_PollutionChanged(cell, isPolluted);
			}
			if (!silent && isPolluted && MapGenerator.mapBeingGenerated != this.map && !this.pollutedCellsThisTick.Contains(cell))
			{
				this.pollutedCellsThisTick.Add(cell);
			}
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x0006CE19 File Offset: 0x0006B019
		public bool CanPollute(IntVec3 c)
		{
			return this.EverPollutable(c) && !this.IsPolluted(c);
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x0006CE30 File Offset: 0x0006B030
		public bool CanUnpollute(IntVec3 c)
		{
			return this.IsPolluted(c);
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x0006CE3C File Offset: 0x0006B03C
		public void PollutionTick()
		{
			if (this.dirty)
			{
				if (this.map.Tile.Valid)
				{
					Find.WorldGrid[this.map.Tile].pollution = this.TotalPollutionPercent;
					Find.World.renderer.Notify_TilePollutionChanged(this.map.Tile);
				}
				this.dirty = false;
			}
			if (this.map.IsHashIntervalTick(200) && this.TotalPollution > 0)
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.PollutedTerrain, OpportunityType.Important);
			}
			if (this.pollutedCellsThisTick.Count > 0)
			{
				EffecterDef effecterDef = (this.pollutedCellsThisTick.Count > 10) ? EffecterDefOf.CellPollution_Performant : EffecterDefOf.CellPollution;
				for (int i = 0; i < this.pollutedCellsThisTick.Count; i++)
				{
					IntVec3 intVec = this.pollutedCellsThisTick[i];
					Effecter eff = effecterDef.Spawn(intVec, this.map, Vector3.zero, 1f);
					this.map.effecterMaintainer.AddEffecterToMaintain(eff, intVec, 45);
				}
			}
			this.pollutedCellsThisTick.Clear();
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x0006CF53 File Offset: 0x0006B153
		public void PollutionGridUpdate()
		{
			if (Find.PlaySettings.showPollutionOverlay && !Find.ScreenshotModeHandler.Active)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x0006CF84 File Offset: 0x0006B184
		private bool CellBoolDrawerGetBoolInt(int index)
		{
			IntVec3 intVec = CellIndicesUtility.IndexToCell(index, this.map.Size.x);
			return intVec.InBounds(this.map) && !intVec.Filled(this.map) && !intVec.Fogged(this.map) && this.EverPollutable(intVec) && this.IsPolluted(intVec);
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x0006CFE6 File Offset: 0x0006B1E6
		private Color CellBoolDrawerColorInt()
		{
			return Color.white;
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x0006CFF0 File Offset: 0x0006B1F0
		private Color CellBoolDrawerGetExtraColorInt(int index)
		{
			IntVec3 cell = CellIndicesUtility.IndexToCell(index, this.map.Size.x);
			if (!this.IsPolluted(cell))
			{
				return Color.white;
			}
			return Color.red;
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x0006D028 File Offset: 0x0006B228
		public void ExposeData()
		{
			Scribe_Deep.Look<BoolGrid>(ref this.grid, "grid", Array.Empty<object>());
		}

		// Token: 0x040010E5 RID: 4325
		private const int LessonPollutionTicks = 200;

		// Token: 0x040010E6 RID: 4326
		private const int PerformantPollutionEffectThreshold = 10;

		// Token: 0x040010E7 RID: 4327
		public const int DissolutionEffecterDuration = 45;

		// Token: 0x040010E8 RID: 4328
		private BoolGrid grid;

		// Token: 0x040010E9 RID: 4329
		private Map map;

		// Token: 0x040010EA RID: 4330
		private bool dirty;

		// Token: 0x040010EB RID: 4331
		private CellBoolDrawer drawerInt;

		// Token: 0x040010EC RID: 4332
		private List<IntVec3> allPollutableCells = new List<IntVec3>();

		// Token: 0x040010ED RID: 4333
		private List<IntVec3> pollutedCellsThisTick = new List<IntVec3>();
	}
}
