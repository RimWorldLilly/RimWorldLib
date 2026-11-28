using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013A1 RID: 5025
	public class WaterBodyTracker : IExposable
	{
		// Token: 0x17001535 RID: 5429
		// (get) Token: 0x06007C8F RID: 31887 RVA: 0x002C803E File Offset: 0x002C623E
		public List<WaterBody> Bodies
		{
			get
			{
				return this.bodies;
			}
		}

		// Token: 0x17001536 RID: 5430
		// (get) Token: 0x06007C90 RID: 31888 RVA: 0x002C8046 File Offset: 0x002C6246
		public bool AnyBodyContainsFish
		{
			get
			{
				return this.anyBodyContainsFish;
			}
		}

		// Token: 0x06007C91 RID: 31889 RVA: 0x002C804E File Offset: 0x002C624E
		public WaterBodyTracker(Map map)
		{
			this.map = map;
		}

		// Token: 0x06007C92 RID: 31890 RVA: 0x002C8068 File Offset: 0x002C6268
		public void ConstructBodies()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.bodiesByCell.Clear();
			if (this.bodies == null)
			{
				this.bodies = new List<WaterBody>();
				for (int i = 0; i < this.map.cellIndices.NumGridCells; i++)
				{
					IntVec3 c = this.map.cellIndices.IndexToCell(i);
					WaterBodyType waterBodyType = c.GetWaterBodyType(this.map);
					WaterBody waterBody;
					if (waterBodyType != WaterBodyType.None && !this.TryGetWaterBodyAt(c, out waterBody))
					{
						this.TryCreateBodyOrAddToExisting(c, waterBodyType);
					}
				}
				for (int j = 0; j < this.bodies.Count; j++)
				{
					this.bodies[j].Initialize();
					if (this.bodies[j].HasFish)
					{
						this.anyBodyContainsFish = true;
					}
				}
				return;
			}
			for (int k = 0; k < this.bodies.Count; k++)
			{
				WaterBody waterBody2 = this.bodies[k];
				waterBody2.CellCount = 0;
				waterBody2.cells.Clear();
				this.FloodFillBody(waterBody2.rootCell, this.bodies[k]);
				waterBody2.RecacheState();
				if (waterBody2.HasFish)
				{
					this.anyBodyContainsFish = true;
				}
			}
			for (int l = 0; l < this.map.cellIndices.NumGridCells; l++)
			{
				IntVec3 c2 = this.map.cellIndices.IndexToCell(l);
				WaterBodyType waterBodyType2 = c2.GetWaterBodyType(this.map);
				WaterBody waterBody;
				if (waterBodyType2 != WaterBodyType.None && !this.TryGetWaterBodyAt(c2, out waterBody))
				{
					this.TryCreateBodyOrAddToExisting(c2, waterBodyType2);
				}
			}
		}

		// Token: 0x06007C93 RID: 31891 RVA: 0x002C8200 File Offset: 0x002C6400
		public void Tick()
		{
			if (this.map.TileInfo.MaxFishPopulation <= 0f)
			{
				return;
			}
			if (this.map.IsHashIntervalTick(2500))
			{
				GameCondition gameCondition;
				float num = this.map.gameConditionManager.FishPopulationOffsetFactorPerDay(this.map, out gameCondition);
				for (int i = 0; i < this.bodies.Count; i++)
				{
					if (this.bodies[i] != null)
					{
						this.bodies[i].Population += this.bodies[i].MaxPopulation * num / 24f;
					}
				}
			}
		}

		// Token: 0x06007C94 RID: 31892 RVA: 0x002C82A8 File Offset: 0x002C64A8
		public void Notify_Fished(IntVec3 c, float amount)
		{
			WaterBody waterBody;
			if (this.TryGetWaterBodyAt(c, out waterBody))
			{
				waterBody.Population -= amount;
			}
		}

		// Token: 0x06007C95 RID: 31893 RVA: 0x002C82D0 File Offset: 0x002C64D0
		public bool AnyFishPopulationAt(IntVec3 c)
		{
			WaterBody waterBody;
			return this.TryGetWaterBodyAt(c, out waterBody) && waterBody.HasFish;
		}

		// Token: 0x06007C96 RID: 31894 RVA: 0x002C82F0 File Offset: 0x002C64F0
		public float FishPopulationAt(IntVec3 c)
		{
			WaterBody waterBody;
			if (!this.TryGetWaterBodyAt(c, out waterBody))
			{
				return 0f;
			}
			return waterBody.Population;
		}

		// Token: 0x06007C97 RID: 31895 RVA: 0x002C8314 File Offset: 0x002C6514
		public float MaxPopulationAt(IntVec3 c)
		{
			WaterBody waterBody;
			if (!this.TryGetWaterBodyAt(c, out waterBody))
			{
				return 0f;
			}
			return waterBody.MaxPopulation;
		}

		// Token: 0x06007C98 RID: 31896 RVA: 0x002C8338 File Offset: 0x002C6538
		public float PopulationPercentAt(IntVec3 c)
		{
			float num = this.MaxPopulationAt(c);
			if (num <= 0f)
			{
				return 0f;
			}
			return this.FishPopulationAt(c) / num;
		}

		// Token: 0x06007C99 RID: 31897 RVA: 0x002C8364 File Offset: 0x002C6564
		public WaterBody WaterBodyAt(IntVec3 c)
		{
			WaterBody result;
			if (!this.TryGetWaterBodyAt(c, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06007C9A RID: 31898 RVA: 0x002C837F File Offset: 0x002C657F
		public bool TryGetWaterBodyAt(IntVec3 c, out WaterBody body)
		{
			if (!c.InBounds(this.map))
			{
				body = null;
				return false;
			}
			return this.bodiesByCell.TryGetValue(c, out body) && body != null;
		}

		// Token: 0x06007C9B RID: 31899 RVA: 0x002C83AC File Offset: 0x002C65AC
		public void Notify_TerrainChanged(IntVec3 cell, TerrainDef oldTerr, TerrainDef newTerr)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			WaterBodyType? waterBodyType = (newTerr != null) ? new WaterBodyType?(newTerr.waterBodyType) : null;
			WaterBodyType? waterBodyType2 = (oldTerr != null) ? new WaterBodyType?(oldTerr.waterBodyType) : null;
			if (waterBodyType.GetValueOrDefault() == waterBodyType2.GetValueOrDefault() & waterBodyType != null == (waterBodyType2 != null))
			{
				return;
			}
			WaterBodyType waterBodyType3 = cell.GetWaterBodyType(this.map);
			if (waterBodyType3 != WaterBodyType.None)
			{
				this.TryCreateBodyOrAddToExisting(cell, waterBodyType3);
			}
			else
			{
				this.RemoveCellFromExistingBody(cell);
			}
			WaterBody waterBody = cell.GetWaterBody(this.map);
			if (waterBody == null)
			{
				return;
			}
			waterBody.Notify_TerrainChanged(cell, oldTerr, newTerr);
		}

		// Token: 0x06007C9C RID: 31900 RVA: 0x002C8454 File Offset: 0x002C6654
		public void Notify_PollutionChanged(IntVec3 cell, bool isPolluted)
		{
			if (this.WaterBodyAt(cell) == null)
			{
				return;
			}
			WaterBody waterBody = cell.GetWaterBody(this.map);
			if (waterBody == null)
			{
				return;
			}
			waterBody.Notify_PollutionChanged(cell, isPolluted);
		}

		// Token: 0x06007C9D RID: 31901 RVA: 0x002C8478 File Offset: 0x002C6678
		private void TryCreateBodyOrAddToExisting(IntVec3 c, WaterBodyType waterBodyType)
		{
			WaterBody waterBody;
			if (this.TryGetWaterBodyAt(c, out waterBody) && waterBody.waterBodyType == waterBodyType)
			{
				return;
			}
			WaterBody waterBody2 = null;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c2 = c + GenAdj.CardinalDirections[i];
				WaterBody waterBody3;
				if (c2.InBounds(this.map) && c2.GetWaterBodyType(this.map) == waterBodyType && this.TryGetWaterBodyAt(c2, out waterBody3) && (waterBody2 == null || waterBody3.HasFish))
				{
					waterBody2 = waterBody3;
				}
			}
			if (waterBody2 != null)
			{
				this.FloodFillBody(c, waterBody2);
				return;
			}
			WaterBody waterBody4 = new WaterBody(this.map, c);
			this.bodies.Add(waterBody4);
			waterBody4.Initialize();
			this.bodiesByCell.SetOrAdd(c, waterBody4);
			waterBody4.cells.Add(c);
			if (waterBody4.MaxPopulation > 0f)
			{
				this.anyBodyContainsFish = true;
			}
		}

		// Token: 0x06007C9E RID: 31902 RVA: 0x002C8550 File Offset: 0x002C6750
		private void RemoveCellFromExistingBody(IntVec3 c)
		{
			Zone_Fishing zone_Fishing = c.GetZone(this.map) as Zone_Fishing;
			if (zone_Fishing != null)
			{
				zone_Fishing.RemoveCell(c);
			}
			if (c.GetWaterBodyType(this.map) != WaterBodyType.None)
			{
				return;
			}
			WaterBody body;
			if (this.TryGetWaterBodyAt(c, out body))
			{
				this.bodiesByCell.Remove(c);
				body.cells.Remove(c);
				WaterBody body2 = body;
				int cellCount = body2.CellCount;
				body2.CellCount = cellCount - 1;
				if (body.CellCount == 0)
				{
					this.bodies.Remove(body);
					return;
				}
				if (body.rootCell == c)
				{
					body.rootCell = this.bodiesByCell.Keys.First((IntVec3 x) => this.bodiesByCell[x] == body);
				}
			}
		}

		// Token: 0x06007C9F RID: 31903 RVA: 0x002C8638 File Offset: 0x002C6838
		private void FloodFillBody(IntVec3 c, WaterBody body)
		{
			this.map.floodFiller.FloodFill(c, delegate(IntVec3 x)
			{
				WaterBody waterBody;
				return (!this.TryGetWaterBodyAt(x, out waterBody) || waterBody != body) && x.GetWaterBodyType(this.map) == body.waterBodyType;
			}, delegate(IntVec3 x)
			{
				WaterBody waterBody;
				if (this.TryGetWaterBodyAt(x, out waterBody) && waterBody != body)
				{
					this.bodies.Remove(waterBody);
				}
				WaterBody body2 = body;
				int cellCount = body2.CellCount;
				body2.CellCount = cellCount + 1;
				this.bodiesByCell.SetOrAdd(x, body);
				body.cells.Add(x);
			}, int.MaxValue, false, null);
		}

		// Token: 0x06007CA0 RID: 31904 RVA: 0x002C868C File Offset: 0x002C688C
		public void ExposeData()
		{
			Scribe_Collections.Look<WaterBody>(ref this.bodies, "waterBodies", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.lastRareCatchTick, "lastRareCatchTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastNegativeCatchTick, "lastNegativeCatchTick", 0, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.bodies != null)
			{
				foreach (WaterBody waterBody in this.bodies)
				{
					waterBody.map = this.map;
				}
			}
		}

		// Token: 0x06007CA1 RID: 31905 RVA: 0x002C872C File Offset: 0x002C692C
		public void DebugDraw()
		{
			if (DebugViewSettings.drawWaterBodies)
			{
				foreach (WaterBody waterBody in this.bodies)
				{
					Rand.PushState(waterBody.GetHashCode());
					Color col = new Color(Rand.Range(0f, 1f), Rand.Range(0f, 1f), Rand.Range(0f, 1f), 0.5f);
					foreach (IntVec3 c in waterBody.cells)
					{
						CellRenderer.RenderCell(c, SolidColorMaterials.SimpleSolidColorMaterial(col, false));
					}
					Rand.PopState();
				}
			}
		}

		// Token: 0x04004E6B RID: 20075
		private readonly Map map;

		// Token: 0x04004E6C RID: 20076
		private List<WaterBody> bodies;

		// Token: 0x04004E6D RID: 20077
		public int lastRareCatchTick;

		// Token: 0x04004E6E RID: 20078
		public int lastNegativeCatchTick;

		// Token: 0x04004E6F RID: 20079
		[Unsaved(false)]
		private bool anyBodyContainsFish;

		// Token: 0x04004E70 RID: 20080
		[Unsaved(false)]
		private Dictionary<IntVec3, WaterBody> bodiesByCell = new Dictionary<IntVec3, WaterBody>();

		// Token: 0x04004E71 RID: 20081
		private const int TickInterval = 2500;
	}
}
