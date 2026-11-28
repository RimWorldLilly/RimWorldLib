using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AEF RID: 6895
	public abstract class Flood : Thing
	{
		// Token: 0x17001FBD RID: 8125
		// (get) Token: 0x0600B015 RID: 45077 RVA: 0x003E0B9B File Offset: 0x003DED9B
		private int ExpandIntervalTicks
		{
			get
			{
				return this.MaxFloodDurationTicks / this.estimatedFloodedTiles;
			}
		}

		// Token: 0x17001FBE RID: 8126
		// (get) Token: 0x0600B016 RID: 45078 RVA: 0x003E0BAA File Offset: 0x003DEDAA
		protected virtual int StartDelayTicks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001FBF RID: 8127
		// (get) Token: 0x0600B017 RID: 45079 RVA: 0x003E0BAD File Offset: 0x003DEDAD
		protected int FloodingTicks
		{
			get
			{
				return this.MaxFloodDurationTicks + this.StartDelayTicks;
			}
		}

		// Token: 0x17001FC0 RID: 8128
		// (get) Token: 0x0600B018 RID: 45080 RVA: 0x003E0BBC File Offset: 0x003DEDBC
		protected virtual int MaxFloodDurationTicks
		{
			get
			{
				return 120000;
			}
		}

		// Token: 0x0600B019 RID: 45081 RVA: 0x003E0BC4 File Offset: 0x003DEDC4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.estimatedFloodedTiles, "estimatedFloodedTiles", 0, false);
			Scribe_Values.Look<int>(ref this.floodedTileCount, "floodedTileCount", 0, false);
			Dictionary<IntVec3, int> dictionary = new Dictionary<IntVec3, int>();
			for (int i = 0; i < this.openCellWeights.Length; i++)
			{
				HashSet<IntVec3>[] array = this.openCellWeights;
				int num = i;
				if (array[num] == null)
				{
					array[num] = new HashSet<IntVec3>();
				}
				foreach (IntVec3 key in this.openCellWeights[i])
				{
					dictionary[key] = i;
				}
			}
			Scribe_Collections.Look<IntVec3, int>(ref dictionary, "openCellWeights", LookMode.Value, LookMode.Value, ref this.tmpOpenCells, ref this.tmpFloodedWeights, true, false, false);
			this.openCellWeights = new HashSet<IntVec3>[Flood.FloodWidthRange.max];
			for (int j = 0; j < this.openCellWeights.Length; j++)
			{
				this.openCellWeights[j] = new HashSet<IntVec3>();
			}
			foreach (KeyValuePair<IntVec3, int> keyValuePair in dictionary)
			{
				int num;
				IntVec3 intVec;
				keyValuePair.Deconstruct(out intVec, out num);
				IntVec3 item = intVec;
				int num2 = num;
				this.openCellWeights[num2].Add(item);
			}
		}

		// Token: 0x0600B01A RID: 45082 RVA: 0x003E0D28 File Offset: 0x003DEF28
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!ModLister.CheckOdyssey("Flood"))
			{
				return;
			}
			map.events.TerrainChanged += this.Notify_OnTerrainChanged;
			map.events.BuildingSpawned += this.Notify_BuildingChanged;
			map.events.BuildingDespawned += this.Notify_BuildingChanged;
			if (respawningAfterLoad)
			{
				return;
			}
			for (int i = 0; i < this.openCellWeights.Length; i++)
			{
				this.openCellWeights[i] = new HashSet<IntVec3>();
			}
			foreach (ValueTuple<IntVec3, int> valueTuple in this.GetInitialCells(map))
			{
				IntVec3 item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				this.openCellWeights[item2 - 1].Add(item);
			}
			if (!this.AnyCellOpen())
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			this.estimatedFloodedTiles = this.openCellWeights.Sum((HashSet<IntVec3> c) => c.Count) * Flood.FloodWidthRange.max;
		}

		// Token: 0x0600B01B RID: 45083 RVA: 0x003E0E58 File Offset: 0x003DF058
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Map.events.TerrainChanged -= this.Notify_OnTerrainChanged;
			base.Map.events.BuildingSpawned -= this.Notify_BuildingChanged;
			base.Map.events.BuildingDespawned -= this.Notify_BuildingChanged;
			base.DeSpawn(mode);
		}

		// Token: 0x0600B01C RID: 45084 RVA: 0x003E0EC0 File Offset: 0x003DF0C0
		private void Notify_BuildingChanged(Building building)
		{
			if (this.CanFloodSpreadInto(building.Position))
			{
				this.noPossibleCell = false;
			}
		}

		// Token: 0x0600B01D RID: 45085 RVA: 0x003E0ED7 File Offset: 0x003DF0D7
		private void Notify_OnTerrainChanged(IntVec3 cell)
		{
			if (this.CanFloodSpreadInto(cell))
			{
				this.noPossibleCell = false;
			}
		}

		// Token: 0x0600B01E RID: 45086
		protected abstract IEnumerable<ValueTuple<IntVec3, int>> GetInitialCells(Map map);

		// Token: 0x0600B01F RID: 45087 RVA: 0x003E0EE9 File Offset: 0x003DF0E9
		protected override void Tick()
		{
			if (!this.noPossibleCell && this.IsHashIntervalTick(this.ExpandIntervalTicks) && Find.TickManager.TicksGame > this.spawnedTick + this.StartDelayTicks)
			{
				this.SpreadFlood();
			}
		}

		// Token: 0x0600B020 RID: 45088 RVA: 0x003E0F20 File Offset: 0x003DF120
		private void SpreadFlood()
		{
			if (this.tempAdjCells == null)
			{
				this.tempAdjCells = GenAdj.CardinalDirections.ToList<IntVec3>();
			}
			this.tempAdjCells.Shuffle<IntVec3>();
			bool flag = false;
			while (!flag)
			{
				ValueTuple<IntVec3, int> nextCell = this.GetNextCell();
				IntVec3 item = nextCell.Item1;
				int item2 = nextCell.Item2;
				if (!item.IsValid)
				{
					this.noPossibleCell = true;
					return;
				}
				bool flag2 = true;
				TerrainDef terrainDef = base.Map.terrainGrid.TempTerrainAt(item);
				if (terrainDef == null || terrainDef.floodTerrain == null)
				{
					terrainDef = base.Map.terrainGrid.BaseTerrainAt(item);
				}
				for (int i = 0; i < 4; i++)
				{
					IntVec3 intVec = item + GenAdj.CardinalDirections[i];
					if (this.CanFloodPotentiallySpreadInto(intVec))
					{
						flag2 = false;
					}
					if (this.CanFloodSpreadInto(intVec))
					{
						int num = item2 - 1;
						if (num > 0)
						{
							this.openCellWeights[num - 1].Add(intVec);
						}
						this.SpreadFlood(intVec, terrainDef);
						this.floodedTileCount++;
						flag = true;
						break;
					}
				}
				if (flag2)
				{
					this.openCellWeights[item2 - 1].Remove(item);
				}
			}
		}

		// Token: 0x0600B021 RID: 45089
		protected abstract void SpreadFlood(IntVec3 cell, TerrainDef sourceTerrain);

		// Token: 0x0600B022 RID: 45090 RVA: 0x003E103D File Offset: 0x003DF23D
		private bool CanFloodPotentiallySpreadInto(IntVec3 cell)
		{
			return cell.InBounds(base.Map) && !this.CellOpen(cell) && !base.Map.terrainGrid.TerrainAt(cell).IsWater;
		}

		// Token: 0x0600B023 RID: 45091 RVA: 0x003E1075 File Offset: 0x003DF275
		private bool CanFloodSpreadInto(IntVec3 cell)
		{
			return this.CanFloodPotentiallySpreadInto(cell) && base.Map.terrainGrid.FoundationAt(cell) == null && cell.GetEdifice(base.Map) == null;
		}

		// Token: 0x0600B024 RID: 45092 RVA: 0x003E10A8 File Offset: 0x003DF2A8
		private ValueTuple<IntVec3, int> GetNextCell()
		{
			ValueTuple<IntVec3, int> result;
			using (ProfilerBlock.Scope("GetNextCell"))
			{
				for (int i = this.openCellWeights.Length - 1; i >= 0; i--)
				{
					if (this.openCellWeights[i].Count != 0)
					{
						for (int j = 0; j < 10; j++)
						{
							IntVec3 intVec;
							if (this.openCellWeights[i].TryRandomElement(out intVec) && this.CellCanSpreadFlood(intVec))
							{
								return new ValueTuple<IntVec3, int>(intVec, i + 1);
							}
							if (!this.CellCanPotentiallySpreadFlood(intVec))
							{
								this.openCellWeights[i].Remove(intVec);
							}
						}
						for (int k = this.openCellWeights[i].Count - 1; k >= 0; k--)
						{
							IntVec3 intVec2;
							if (this.openCellWeights[i].TryRandomElement(out intVec2) && this.CellCanSpreadFlood(intVec2))
							{
								return new ValueTuple<IntVec3, int>(intVec2, i + 1);
							}
							if (!this.CellCanPotentiallySpreadFlood(intVec2))
							{
								this.openCellWeights[i].Remove(intVec2);
							}
						}
					}
				}
				result = new ValueTuple<IntVec3, int>(IntVec3.Invalid, -1);
			}
			return result;
		}

		// Token: 0x0600B025 RID: 45093 RVA: 0x003E11CC File Offset: 0x003DF3CC
		private bool CellCanSpreadFlood(IntVec3 cell)
		{
			for (int i = 0; i < GenAdj.CardinalDirections.Length; i++)
			{
				IntVec3 b = GenAdj.CardinalDirections[i];
				if (this.CanFloodSpreadInto(cell + b))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B026 RID: 45094 RVA: 0x003E120C File Offset: 0x003DF40C
		private bool CellCanPotentiallySpreadFlood(IntVec3 cell)
		{
			for (int i = 0; i < GenAdj.CardinalDirections.Length; i++)
			{
				IntVec3 b = GenAdj.CardinalDirections[i];
				if (this.CanFloodPotentiallySpreadInto(cell + b))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B027 RID: 45095 RVA: 0x003E124C File Offset: 0x003DF44C
		private bool CellOpen(IntVec3 cell)
		{
			for (int i = 0; i < this.openCellWeights.Length; i++)
			{
				if (this.openCellWeights[i].Contains(cell))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B028 RID: 45096 RVA: 0x003E1280 File Offset: 0x003DF480
		private bool AnyCellOpen()
		{
			for (int i = 0; i < this.openCellWeights.Length; i++)
			{
				if (this.openCellWeights[i].Count > 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040065A4 RID: 26020
		protected static readonly IntRange FloodWidthRange = new IntRange(10, 12);

		// Token: 0x040065A5 RID: 26021
		private HashSet<IntVec3>[] openCellWeights = new HashSet<IntVec3>[Flood.FloodWidthRange.max];

		// Token: 0x040065A6 RID: 26022
		protected int estimatedFloodedTiles;

		// Token: 0x040065A7 RID: 26023
		protected int floodedTileCount;

		// Token: 0x040065A8 RID: 26024
		private bool noPossibleCell;

		// Token: 0x040065A9 RID: 26025
		private List<IntVec3> tmpOpenCells = new List<IntVec3>();

		// Token: 0x040065AA RID: 26026
		private List<int> tmpFloodedWeights = new List<int>();

		// Token: 0x040065AB RID: 26027
		private List<IntVec3> tempAdjCells;
	}
}
