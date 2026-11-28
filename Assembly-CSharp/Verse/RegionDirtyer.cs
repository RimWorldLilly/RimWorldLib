using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002F8 RID: 760
	public class RegionDirtyer
	{
		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06001734 RID: 5940 RVA: 0x0008BC2B File Offset: 0x00089E2B
		public bool AnyDirty
		{
			get
			{
				return this.dirtyCells.Count > 0;
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06001735 RID: 5941 RVA: 0x0008BC3B File Offset: 0x00089E3B
		public HashSet<IntVec3> DirtyCells
		{
			get
			{
				return this.dirtyCells;
			}
		}

		// Token: 0x06001736 RID: 5942 RVA: 0x0008BC43 File Offset: 0x00089E43
		public RegionDirtyer(Map map)
		{
			this.map = map;
		}

		// Token: 0x06001737 RID: 5943 RVA: 0x0008BC68 File Offset: 0x00089E68
		internal void Notify_WalkabilityChanged(IntVec3 c, bool newWalkability)
		{
			this.regionsToDirty.Clear();
			for (int i = 0; i < 9; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCellsAndInside[i];
				if (c2.InBounds(this.map))
				{
					Region regionAt_NoRebuild_InvalidAllowed = this.map.regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(c2);
					if (regionAt_NoRebuild_InvalidAllowed != null && regionAt_NoRebuild_InvalidAllowed.valid)
					{
						this.map.TemperatureVacuumCache.TryCacheRegionTempInfo(c2, regionAt_NoRebuild_InvalidAllowed);
						this.regionsToDirty.Add(regionAt_NoRebuild_InvalidAllowed);
					}
				}
			}
			for (int j = 0; j < this.regionsToDirty.Count; j++)
			{
				this.SetRegionDirty(this.regionsToDirty[j], true);
			}
			this.regionsToDirty.Clear();
			if (newWalkability)
			{
				this.dirtyCells.Add(c);
			}
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x0008BD2C File Offset: 0x00089F2C
		internal void Notify_ThingAffectingRegionsSpawned(Thing b)
		{
			this.DirtyRegionForThing(b);
		}

		// Token: 0x06001739 RID: 5945 RVA: 0x0008BD35 File Offset: 0x00089F35
		internal void Notify_ThingAffectingRegionsDespawned(Thing b)
		{
			this.DirtyRegionForThing(b);
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x0008BD40 File Offset: 0x00089F40
		internal void DirtyRegionForThing(Thing b)
		{
			this.regionsToDirty.Clear();
			foreach (IntVec3 c in b.OccupiedRect().ExpandedBy(1).ClipInsideMap(b.Map))
			{
				Region validRegionAt_NoRebuild = b.Map.regionGrid.GetValidRegionAt_NoRebuild(c);
				if (validRegionAt_NoRebuild != null)
				{
					b.Map.TemperatureVacuumCache.TryCacheRegionTempInfo(c, validRegionAt_NoRebuild);
					this.regionsToDirty.Add(validRegionAt_NoRebuild);
				}
			}
			for (int i = 0; i < this.regionsToDirty.Count; i++)
			{
				this.SetRegionDirty(this.regionsToDirty[i], true);
			}
			this.regionsToDirty.Clear();
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x0008BE24 File Offset: 0x0008A024
		internal void SetAllClean()
		{
			foreach (IntVec3 c in this.dirtyCells)
			{
				this.map.TemperatureVacuumCache.ResetCachedCellInfo(c);
			}
			this.dirtyCells.Clear();
		}

		// Token: 0x0600173C RID: 5948 RVA: 0x0008BE8C File Offset: 0x0008A08C
		private void SetRegionDirty(Region reg, bool addCellsToDirtyCells = true)
		{
			if (!reg.valid)
			{
				return;
			}
			reg.valid = false;
			reg.District = null;
			for (int i = 0; i < reg.links.Count; i++)
			{
				reg.links[i].Deregister(reg);
			}
			reg.links.Clear();
			reg.ListerThings.Clear();
			this.map.GetComponent<VacuumComponent>().SetDrawerDirty();
			if (addCellsToDirtyCells)
			{
				foreach (IntVec3 intVec in reg.Cells)
				{
					this.dirtyCells.Add(intVec);
					if (DebugViewSettings.drawRegionDirties)
					{
						this.map.debugDrawer.FlashCell(intVec, 0f, null, 50);
					}
				}
			}
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x0008BF68 File Offset: 0x0008A168
		internal void SetAllDirty()
		{
			this.dirtyCells.Clear();
			foreach (IntVec3 item in this.map)
			{
				this.dirtyCells.Add(item);
			}
			foreach (Region reg in this.map.regionGrid.AllRegions_NoRebuild_InvalidAllowed)
			{
				this.SetRegionDirty(reg, false);
			}
		}

		// Token: 0x040013C8 RID: 5064
		private Map map;

		// Token: 0x040013C9 RID: 5065
		private HashSet<IntVec3> dirtyCells = new HashSet<IntVec3>();

		// Token: 0x040013CA RID: 5066
		private List<Region> regionsToDirty = new List<Region>();
	}
}
