using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200032F RID: 815
	public sealed class ZoneManager : IExposable
	{
		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06001905 RID: 6405 RVA: 0x000958AC File Offset: 0x00093AAC
		public List<Zone> AllZones
		{
			get
			{
				return this.allZones;
			}
		}

		// Token: 0x06001906 RID: 6406 RVA: 0x000958B4 File Offset: 0x00093AB4
		public ZoneManager(Map map)
		{
			this.map = map;
			this.zoneGrid = new Zone[map.cellIndices.NumGridCells];
		}

		// Token: 0x06001907 RID: 6407 RVA: 0x000958E4 File Offset: 0x00093AE4
		public void ExposeData()
		{
			Scribe_Collections.Look<Zone>(ref this.allZones, "allZones", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.UpdateZoneManagerLinks();
				this.RebuildZoneGrid();
			}
		}

		// Token: 0x06001908 RID: 6408 RVA: 0x00095910 File Offset: 0x00093B10
		private void UpdateZoneManagerLinks()
		{
			for (int i = 0; i < this.allZones.Count; i++)
			{
				this.allZones[i].zoneManager = this;
			}
		}

		// Token: 0x06001909 RID: 6409 RVA: 0x00095948 File Offset: 0x00093B48
		private void RebuildZoneGrid()
		{
			CellIndices cellIndices = this.map.cellIndices;
			this.zoneGrid = new Zone[cellIndices.NumGridCells];
			foreach (Zone zone in this.allZones)
			{
				foreach (IntVec3 c in zone)
				{
					this.zoneGrid[cellIndices.CellToIndex(c)] = zone;
				}
			}
		}

		// Token: 0x0600190A RID: 6410 RVA: 0x000959F4 File Offset: 0x00093BF4
		public void RegisterZone(Zone newZone)
		{
			this.allZones.Add(newZone);
			newZone.PostRegister();
		}

		// Token: 0x0600190B RID: 6411 RVA: 0x00095A08 File Offset: 0x00093C08
		public void DeregisterZone(Zone oldZone)
		{
			this.allZones.Remove(oldZone);
			oldZone.PostDeregister();
			if (Find.Selector.SelectedZone == oldZone)
			{
				Find.Selector.ClearSelection();
			}
		}

		// Token: 0x0600190C RID: 6412 RVA: 0x00095A34 File Offset: 0x00093C34
		internal void AddZoneGridCell(Zone zone, IntVec3 c)
		{
			this.zoneGrid[this.map.cellIndices.CellToIndex(c)] = zone;
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x00095A4F File Offset: 0x00093C4F
		internal void ClearZoneGridCell(IntVec3 c)
		{
			this.zoneGrid[this.map.cellIndices.CellToIndex(c)] = null;
		}

		// Token: 0x0600190E RID: 6414 RVA: 0x00095A6A File Offset: 0x00093C6A
		public Zone ZoneAt(IntVec3 c)
		{
			return this.zoneGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x0600190F RID: 6415 RVA: 0x00095A84 File Offset: 0x00093C84
		public string NewZoneName(string nameBase)
		{
			for (int i = 1; i <= 1000; i++)
			{
				string cand = nameBase + " " + i.ToString();
				if (!this.allZones.Any((Zone z) => z.label == cand))
				{
					return cand;
				}
			}
			Log.Error("Ran out of zone names.");
			return "Zone X";
		}

		// Token: 0x06001910 RID: 6416 RVA: 0x00095AF0 File Offset: 0x00093CF0
		internal void Notify_NoZoneOverlapThingSpawned(Thing thing)
		{
			CellRect cellRect = thing.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					Zone zone = this.ZoneAt(c);
					if (zone != null)
					{
						zone.RemoveCell(c);
						zone.CheckContiguous();
					}
				}
			}
		}

		// Token: 0x040014B0 RID: 5296
		public readonly Map map;

		// Token: 0x040014B1 RID: 5297
		private List<Zone> allZones = new List<Zone>();

		// Token: 0x040014B2 RID: 5298
		private Zone[] zoneGrid;
	}
}
