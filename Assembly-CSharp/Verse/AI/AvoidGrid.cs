using System;
using System.Collections.Generic;
using LudeonTK;
using RimWorld;
using Unity.Collections;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x0200090F RID: 2319
	public class AvoidGrid : IDisposable
	{
		// Token: 0x17000D14 RID: 3348
		// (get) Token: 0x0600468F RID: 18063 RVA: 0x001BBD82 File Offset: 0x001B9F82
		public NativeArray<byte>.ReadOnly Grid
		{
			get
			{
				if (this.gridDirty)
				{
					this.Regenerate();
				}
				return this.grid.AsReadOnly();
			}
		}

		// Token: 0x17000D15 RID: 3349
		public byte this[IntVec3 c]
		{
			get
			{
				return this.grid[this.map.cellIndices.CellToIndex(c)];
			}
			private set
			{
				this.grid[this.map.cellIndices.CellToIndex(c)] = value;
			}
		}

		// Token: 0x06004692 RID: 18066 RVA: 0x001BBDDC File Offset: 0x001B9FDC
		public AvoidGrid(Map map)
		{
			this.map = map;
			this.grid = new NativeArray<byte>(map.cellIndices.NumGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			map.events.BuildingSpawned += this.Notify_BuildingSpawned;
			map.events.BuildingDespawned += this.Notify_BuildingDespawned;
		}

		// Token: 0x06004693 RID: 18067 RVA: 0x001BBE43 File Offset: 0x001BA043
		public void Dispose()
		{
			this.grid.Dispose();
		}

		// Token: 0x06004694 RID: 18068 RVA: 0x001BBE50 File Offset: 0x001BA050
		public void Regenerate()
		{
			this.gridDirty = false;
			this.grid.Clear<byte>();
			List<Building> allBuildingsColonist = this.map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				if (allBuildingsColonist[i].def.building.ai_combatDangerous)
				{
					Building_TurretGun building_TurretGun = allBuildingsColonist[i] as Building_TurretGun;
					if (building_TurretGun != null)
					{
						this.PrintAvoidGridAroundTurret(building_TurretGun);
					}
				}
			}
			this.ExpandAvoidGridIntoEdifices();
		}

		// Token: 0x06004695 RID: 18069 RVA: 0x001BBEC6 File Offset: 0x001BA0C6
		private void Notify_BuildingSpawned(Building building)
		{
			if (building.def.building.ai_combatDangerous || !building.CanBeSeenOver())
			{
				this.gridDirty = true;
			}
		}

		// Token: 0x06004696 RID: 18070 RVA: 0x001BBEE9 File Offset: 0x001BA0E9
		private void Notify_BuildingDespawned(Building building)
		{
			if (building.def.building.ai_combatDangerous || !building.CanBeSeenOver())
			{
				this.gridDirty = true;
			}
		}

		// Token: 0x06004697 RID: 18071 RVA: 0x001BBF0C File Offset: 0x001BA10C
		public void DebugDrawOnMap()
		{
			if (DebugViewSettings.drawAvoidGrid && Find.CurrentMap == this.map)
			{
				this.DebugDraw();
			}
		}

		// Token: 0x06004698 RID: 18072 RVA: 0x001BBF28 File Offset: 0x001BA128
		private void PrintAvoidGridAroundTurret(Building_TurretGun tur)
		{
			float range = tur.GunCompEq.PrimaryVerb.verbProps.range;
			float num = tur.GunCompEq.PrimaryVerb.verbProps.EffectiveMinRange(true);
			int num2 = GenRadial.NumCellsInRadius(range + 4f);
			for (int i = (num < 1f) ? 0 : GenRadial.NumCellsInRadius(num); i < num2; i++)
			{
				IntVec3 intVec = tur.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(tur.Map) && intVec.WalkableByNormal(tur.Map) && GenSight.LineOfSight(intVec, tur.Position, tur.Map, true, null, 0, 0))
				{
					this.IncrementAvoidGrid(intVec, 45);
				}
			}
		}

		// Token: 0x06004699 RID: 18073 RVA: 0x001BBFE0 File Offset: 0x001BA1E0
		private void IncrementAvoidGrid(IntVec3 c, int num)
		{
			byte b = this[c];
			b = (byte)Mathf.Min(255, (int)b + num);
			this[c] = b;
		}

		// Token: 0x0600469A RID: 18074 RVA: 0x001BC00C File Offset: 0x001BA20C
		private void ExpandAvoidGridIntoEdifices()
		{
			int numGridCells = this.map.cellIndices.NumGridCells;
			for (int i = 0; i < numGridCells; i++)
			{
				if (this.grid[i] != 0 && this.map.edificeGrid[i] == null)
				{
					for (int j = 0; j < 8; j++)
					{
						IntVec3 c = this.map.cellIndices.IndexToCell(i) + GenAdj.AdjacentCells[j];
						if (c.InBounds(this.map) && c.GetEdifice(this.map) != null)
						{
							this[c] = (byte)Mathf.Min(255, Mathf.Max((int)this[c], (int)this.grid[i]));
						}
					}
				}
			}
		}

		// Token: 0x0600469B RID: 18075 RVA: 0x001BC0D4 File Offset: 0x001BA2D4
		private void DebugDraw()
		{
			for (int i = 0; i < this.grid.Length; i++)
			{
				byte b = this.grid[i];
				if (b > 0)
				{
					CellRenderer.RenderCell(this.map.cellIndices.IndexToCell(i), (float)b / 255f * 0.5f);
				}
			}
		}

		// Token: 0x04002DE8 RID: 11752
		public readonly Map map;

		// Token: 0x04002DE9 RID: 11753
		private NativeArray<byte> grid;

		// Token: 0x04002DEA RID: 11754
		private bool gridDirty = true;
	}
}
