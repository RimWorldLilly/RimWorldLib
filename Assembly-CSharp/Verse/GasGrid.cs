using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000282 RID: 642
	public class GasGrid : IExposable
	{
		// Token: 0x17000394 RID: 916
		// (get) Token: 0x060012C0 RID: 4800 RVA: 0x0006AB42 File Offset: 0x00068D42
		public bool CalculateGasEffects
		{
			get
			{
				return this.anyGasEverAdded;
			}
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x0006AB4C File Offset: 0x00068D4C
		public GasGrid(Map map)
		{
			this.map = map;
			this.gasDensity = new uint[map.cellIndices.NumGridCells];
			this.cardinalDirections = new List<IntVec3>();
			this.cardinalDirections.AddRange(GenAdj.CardinalDirections);
			this.cycleIndexDiffusion = Rand.Range(0, map.Area / 2);
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x0006ABAC File Offset: 0x00068DAC
		public void RecalculateEverHadGas()
		{
			this.anyGasEverAdded = false;
			for (int i = 0; i < this.gasDensity.Length; i++)
			{
				if (this.gasDensity[i] > 0U)
				{
					this.anyGasEverAdded = true;
					return;
				}
			}
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x0006ABE8 File Offset: 0x00068DE8
		public void Tick()
		{
			if (!this.CalculateGasEffects)
			{
				return;
			}
			int area = this.map.Area;
			int num = Mathf.CeilToInt((float)area * 0.015625f);
			this.cellsInRandomOrder = this.map.cellsInRandomOrder.GetAll();
			using (new ProfilerBlock("Dissipation"))
			{
				for (int i = 0; i < num; i++)
				{
					if (this.cycleIndexDissipation >= area)
					{
						this.cycleIndexDissipation = 0;
					}
					using (new ProfilerBlock("TryDissipateGases"))
					{
						this.TryDissipateGases(CellIndicesUtility.CellToIndex(this.cellsInRandomOrder[this.cycleIndexDissipation], this.map.Size.x));
					}
					this.cycleIndexDissipation++;
				}
			}
			using (new ProfilerBlock("Diffusion"))
			{
				num = Mathf.CeilToInt((float)area * 0.03125f);
				for (int j = 0; j < num; j++)
				{
					if (this.cycleIndexDiffusion >= area)
					{
						this.cycleIndexDiffusion = 0;
					}
					using (new ProfilerBlock("TryDiffuseGases"))
					{
						this.TryDiffuseGases(this.cellsInRandomOrder[this.cycleIndexDiffusion]);
					}
					this.cycleIndexDiffusion++;
				}
			}
			if (this.map.IsHashIntervalTick(600))
			{
				this.RecalculateEverHadGas();
			}
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x0006AD94 File Offset: 0x00068F94
		public bool AnyGasAt(IntVec3 cell)
		{
			return this.AnyGasAt(CellIndicesUtility.CellToIndex(cell, this.map.Size.x));
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x0006ADB2 File Offset: 0x00068FB2
		private bool AnyGasAt(int idx)
		{
			return this.gasDensity[idx] > 0U;
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x0006ADBF File Offset: 0x00068FBF
		public byte DensityAt(IntVec3 cell, GasType gasType)
		{
			return this.DensityAt(CellIndicesUtility.CellToIndex(cell, this.map.info.Size.x), gasType);
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x0006ADE3 File Offset: 0x00068FE3
		private byte DensityAt(int index, GasType gasType)
		{
			return (byte)(this.gasDensity[index] >> (int)gasType & 255U);
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x0006ADF9 File Offset: 0x00068FF9
		public float DensityPercentAt(IntVec3 cell, GasType gasType)
		{
			return (float)this.DensityAt(cell, gasType) / 255f;
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x0006AE0C File Offset: 0x0006900C
		public void AddGas(IntVec3 cell, GasType gasType, int amount, bool canOverflow = true)
		{
			if (amount <= 0 || !this.GasCanMoveTo(cell))
			{
				return;
			}
			this.anyGasEverAdded = true;
			int index = CellIndicesUtility.CellToIndex(cell, this.map.Size.x);
			byte b = this.DensityAt(index, GasType.BlindSmoke);
			byte b2 = this.DensityAt(index, GasType.ToxGas);
			byte b3 = this.DensityAt(index, GasType.RotStink);
			byte b4 = this.DensityAt(index, GasType.DeadlifeDust);
			int num = 0;
			if (gasType <= GasType.ToxGas)
			{
				if (gasType == GasType.BlindSmoke)
				{
					b = this.AdjustedDensity((int)b + amount, out num);
					goto IL_CE;
				}
				if (gasType == GasType.ToxGas)
				{
					if (!ModLister.CheckBiotech("Tox gas"))
					{
						return;
					}
					b2 = this.AdjustedDensity((int)b2 + amount, out num);
					goto IL_CE;
				}
			}
			else
			{
				if (gasType == GasType.RotStink)
				{
					b3 = this.AdjustedDensity((int)b3 + amount, out num);
					goto IL_CE;
				}
				if (gasType == GasType.DeadlifeDust)
				{
					if (!ModLister.CheckAnomaly("Deadlife dust"))
					{
						return;
					}
					b4 = this.AdjustedDensity((int)b4 + amount, out num);
					goto IL_CE;
				}
			}
			Log.Error("Trying to add unknown gas type.");
			return;
			IL_CE:
			this.SetDirect(index, b, b2, b3, b4);
			this.map.mapDrawer.MapMeshDirty(cell, MapMeshFlagDefOf.Gas);
			if (canOverflow && num > 0)
			{
				this.Overflow(cell, gasType, num);
			}
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x0006AF21 File Offset: 0x00069121
		private byte AdjustedDensity(int newDensity, out int overflow)
		{
			if (newDensity > 255)
			{
				overflow = newDensity - 255;
				return byte.MaxValue;
			}
			overflow = 0;
			if (newDensity < 0)
			{
				return 0;
			}
			return (byte)newDensity;
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x0006AF48 File Offset: 0x00069148
		public Vector4 DensitiesAt(IntVec3 cell)
		{
			int index = CellIndicesUtility.CellToIndex(cell, this.map.Size.x);
			float x = (float)this.DensityAt(index, GasType.BlindSmoke);
			float y = (float)this.DensityAt(index, GasType.ToxGas);
			float z = (float)this.DensityAt(index, GasType.RotStink);
			float w = (float)this.DensityAt(index, GasType.DeadlifeDust);
			return new Vector4(x, y, z, w);
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x0006AFA0 File Offset: 0x000691A0
		public void Notify_ThingSpawned(Thing thing)
		{
			if (thing.Spawned && thing.def.Fillage == FillCategory.Full)
			{
				foreach (IntVec3 intVec in thing.OccupiedRect())
				{
					if (this.AnyGasAt(intVec))
					{
						this.gasDensity[CellIndicesUtility.CellToIndex(intVec, this.map.Size.x)] = 0U;
						this.map.mapDrawer.MapMeshDirty(intVec, MapMeshFlagDefOf.Gas);
					}
				}
			}
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x0006B04C File Offset: 0x0006924C
		public uint GetDirect(IntVec3 c)
		{
			return this.gasDensity[CellIndicesUtility.CellToIndex(c, this.map.Size.x)];
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x0006B06B File Offset: 0x0006926B
		public uint GetDirect(int index)
		{
			return this.gasDensity[index];
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x0006B075 File Offset: 0x00069275
		public void SetDirect(IntVec3 c, byte smoke, byte toxic, byte rotStink, byte deadlife)
		{
			this.SetDirect(CellIndicesUtility.CellToIndex(c, this.map.Size.x), smoke, toxic, rotStink, deadlife);
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x0006B099 File Offset: 0x00069299
		public void SetDirect(int index, byte smoke, byte toxic, byte rotStink, byte deadlife)
		{
			if (!ModsConfig.BiotechActive)
			{
				toxic = 0;
			}
			if (!ModsConfig.AnomalyActive)
			{
				deadlife = 0;
			}
			this.gasDensity[index] = (uint)((int)rotStink << 16 | (int)toxic << 8 | (int)smoke | (int)deadlife << 24);
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x0006B0C8 File Offset: 0x000692C8
		public void SetDirect(IntVec3 c, uint packedDensities)
		{
			this.SetDirect(CellIndicesUtility.CellToIndex(c, this.map.Size.x), packedDensities);
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x0006B0E8 File Offset: 0x000692E8
		public void SetDirect(int index, uint packedDensities)
		{
			uint num = uint.MaxValue;
			if (!ModsConfig.BiotechActive)
			{
				num &= 4294902015U;
			}
			if (!ModsConfig.AnomalyActive)
			{
				num &= 16777215U;
			}
			this.gasDensity[index] = (packedDensities & num);
			this.anyGasEverAdded = true;
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x0006B128 File Offset: 0x00069328
		private void Overflow(IntVec3 cell, GasType gasType, int amount)
		{
			if (amount <= 0)
			{
				return;
			}
			int remainingAmount = amount;
			this.map.floodFiller.FloodFill(cell, new Predicate<IntVec3>(this.GasCanMoveTo), delegate(IntVec3 c)
			{
				int num = Mathf.Min(remainingAmount, (int)(byte.MaxValue - this.DensityAt(c, gasType)));
				if (num > 0)
				{
					this.AddGas(c, gasType, num, false);
					remainingAmount -= num;
				}
				return remainingAmount <= 0;
			}, GenRadial.NumCellsInRadius(40f), true, null);
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x0006B18C File Offset: 0x0006938C
		public void EstimateGasDiffusion(IntVec3 cell, GasType gasType, int amount, Action<IntVec3> processor)
		{
			processor(cell);
			int num = Mathf.Min(amount, (int)(48 - this.DensityAt(cell, gasType)));
			amount -= num;
			this.map.floodFiller.FloodFill(cell, new Predicate<IntVec3>(this.GasCanMoveTo), delegate(IntVec3 c)
			{
				int num2 = Mathf.Min(amount, (int)(48 - this.DensityAt(c, gasType)));
				if (num2 > 0)
				{
					processor(c);
					amount -= num2;
				}
				return amount <= 0;
			}, GenRadial.NumCellsInRadius(40f), true, null);
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x0006B228 File Offset: 0x00069428
		private void TryDissipateGases(int index)
		{
			if (!this.AnyGasAt(index))
			{
				return;
			}
			IntVec3 intVec = CellIndicesUtility.IndexToCell(index, this.map.Size.x);
			float vacuum = intVec.GetVacuum(this.map);
			float num = intVec.Roofed(this.map) ? 0.5f : 1f;
			num += vacuum * 25f;
			bool flag = false;
			int num2 = (int)this.DensityAt(index, GasType.BlindSmoke);
			if (num2 > 0)
			{
				num2 = Math.Max(num2 - Mathf.RoundToInt(4f * num), 0);
				if (num2 == 0)
				{
					flag = true;
				}
			}
			int num3 = (int)this.DensityAt(index, GasType.ToxGas);
			if (num3 > 0)
			{
				num3 = Math.Max(num3 - Mathf.RoundToInt(3f * num), 0);
				if (num3 == 0)
				{
					flag = true;
				}
			}
			int num4 = (int)this.DensityAt(index, GasType.RotStink);
			if (num4 > 0)
			{
				num4 = Math.Max(num4 - Mathf.RoundToInt(4f * num), 0);
				if (num4 == 0)
				{
					flag = true;
				}
			}
			int num5 = (int)this.DensityAt(index, GasType.DeadlifeDust);
			if (num5 > 0)
			{
				num5 = Math.Max(num5 - Mathf.RoundToInt(3f * num), 0);
				if (num5 == 0)
				{
					flag = true;
				}
			}
			this.SetDirect(index, (byte)num2, (byte)num3, (byte)num4, (byte)num5);
			if (flag)
			{
				this.map.mapDrawer.MapMeshDirty(intVec, MapMeshFlagDefOf.Gas);
			}
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x0006B370 File Offset: 0x00069570
		private void TryDiffuseGases(IntVec3 cell)
		{
			int index = CellIndicesUtility.CellToIndex(cell, this.map.info.Size.x);
			int num = (int)this.DensityAt(index, GasType.ToxGas);
			int num2 = (int)this.DensityAt(index, GasType.RotStink);
			int num3 = (int)this.DensityAt(index, GasType.DeadlifeDust);
			if (num + num2 + num3 < 17)
			{
				return;
			}
			bool flag = false;
			this.cardinalDirections.Shuffle<IntVec3>();
			for (int i = 0; i < this.cardinalDirections.Count; i++)
			{
				IntVec3 intVec = cell + this.cardinalDirections[i];
				if (this.GasCanMoveTo(intVec))
				{
					int index2 = CellIndicesUtility.CellToIndex(intVec, this.map.Size.x);
					int num4 = (int)this.DensityAt(index2, GasType.ToxGas);
					int num5 = (int)this.DensityAt(index2, GasType.RotStink);
					int num6 = (int)this.DensityAt(index2, GasType.DeadlifeDust);
					if (false | this.TryDiffuseIndividualGas(ref num, ref num4) | this.TryDiffuseIndividualGas(ref num2, ref num5) | this.TryDiffuseIndividualGas(ref num3, ref num6))
					{
						this.SetDirect(index2, this.DensityAt(index2, GasType.BlindSmoke), (byte)num4, (byte)num5, (byte)num6);
						this.map.mapDrawer.MapMeshDirty(intVec, MapMeshFlagDefOf.Gas);
						flag = true;
						if (num + num2 < 17)
						{
							break;
						}
					}
				}
			}
			if (flag)
			{
				this.SetDirect(index, this.DensityAt(index, GasType.BlindSmoke), (byte)num, (byte)num2, (byte)num3);
				this.map.mapDrawer.MapMeshDirty(cell, MapMeshFlagDefOf.Gas);
			}
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x0006B4E4 File Offset: 0x000696E4
		private bool TryDiffuseIndividualGas(ref int gasA, ref int gasB)
		{
			if (gasA < 17)
			{
				return false;
			}
			int num = Mathf.Abs(gasA - gasB) / 2;
			if (gasA > gasB && num >= 17)
			{
				gasA -= num;
				gasB += num;
				return true;
			}
			return false;
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x0006B520 File Offset: 0x00069720
		public bool GasCanMoveTo(IntVec3 cell)
		{
			if (!cell.InBounds(this.map))
			{
				return false;
			}
			Building edifice = cell.GetEdifice(this.map);
			if (edifice != null && edifice.def.Fillage == FillCategory.Full)
			{
				Building_Door building_Door = edifice as Building_Door;
				return building_Door != null && building_Door.Open;
			}
			return true;
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x0006B570 File Offset: 0x00069770
		public void EqualizeGasThroughBuilding(Building b, bool twoWay)
		{
			GasGrid.<>c__DisplayClass46_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			if (!this.CalculateGasEffects)
			{
				return;
			}
			for (int i = 0; i < GasGrid.beqCells.Length; i++)
			{
				GasGrid.beqCells[i] = IntVec3.Invalid;
			}
			CS$<>8__locals1.beqCellCount = 0;
			CS$<>8__locals1.totalRotStink = 0;
			CS$<>8__locals1.totalToxGas = 0;
			CS$<>8__locals1.totalDeadlifeDust = 0;
			if (twoWay)
			{
				for (int j = 0; j < 2; j++)
				{
					IntVec3 cell = (j == 0) ? (b.Position + b.Rotation.FacingCell) : (b.Position - b.Rotation.FacingCell);
					this.<EqualizeGasThroughBuilding>g__VisitCell|46_0(cell, ref CS$<>8__locals1);
				}
			}
			else
			{
				for (int k = 0; k < 4; k++)
				{
					IntVec3 cell2 = b.Position + GenAdj.CardinalDirections[k];
					this.<EqualizeGasThroughBuilding>g__VisitCell|46_0(cell2, ref CS$<>8__locals1);
				}
			}
			if (CS$<>8__locals1.beqCellCount <= 1)
			{
				return;
			}
			byte toxic = (byte)Mathf.Min(CS$<>8__locals1.totalToxGas / CS$<>8__locals1.beqCellCount, 255);
			byte rotStink = (byte)Mathf.Min(CS$<>8__locals1.totalRotStink / CS$<>8__locals1.beqCellCount, 255);
			byte deadlife = (byte)Mathf.Min(CS$<>8__locals1.totalDeadlifeDust / CS$<>8__locals1.beqCellCount, 255);
			for (int l = 0; l < CS$<>8__locals1.beqCellCount; l++)
			{
				if (GasGrid.beqCells[l].IsValid)
				{
					this.SetDirect(this.map.cellIndices.CellToIndex(GasGrid.beqCells[l]), this.DensityAt(GasGrid.beqCells[l], GasType.BlindSmoke), toxic, rotStink, deadlife);
					this.map.mapDrawer.MapMeshDirty(GasGrid.beqCells[l], MapMeshFlagDefOf.Gas);
				}
			}
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x0006B741 File Offset: 0x00069941
		public void ClearCellUnsafe(IntVec3 cell)
		{
			this.gasDensity[CellIndicesUtility.CellToIndex(cell, this.map.Size.x)] = 0U;
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x0006B764 File Offset: 0x00069964
		public void Debug_ClearAll()
		{
			for (int i = 0; i < this.gasDensity.Length; i++)
			{
				this.gasDensity[i] = 0U;
			}
			this.anyGasEverAdded = false;
			this.map.mapDrawer.WholeMapChanged(MapMeshFlagDefOf.Gas);
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x0006B7B0 File Offset: 0x000699B0
		public void Debug_FillAll()
		{
			for (int i = 0; i < this.gasDensity.Length; i++)
			{
				if (this.GasCanMoveTo(this.map.cellIndices.IndexToCell(i)))
				{
					this.SetDirect(i, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				}
			}
			this.anyGasEverAdded = true;
			this.map.mapDrawer.WholeMapChanged(MapMeshFlagDefOf.Gas);
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x0006B828 File Offset: 0x00069A28
		public void ExposeData()
		{
			MapExposeUtility.ExposeUint(this.map, (IntVec3 c) => this.gasDensity[this.map.cellIndices.CellToIndex(c)], delegate(IntVec3 c, uint val)
			{
				this.gasDensity[this.map.cellIndices.CellToIndex(c)] = val;
			}, "gasDensity");
			Scribe_Values.Look<int>(ref this.cycleIndexDiffusion, "cycleIndexDiffusion", 0, false);
			Scribe_Values.Look<int>(ref this.cycleIndexDissipation, "cycleIndexDissipation", 0, false);
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x0006B890 File Offset: 0x00069A90
		[CompilerGenerated]
		private void <EqualizeGasThroughBuilding>g__VisitCell|46_0(IntVec3 cell, ref GasGrid.<>c__DisplayClass46_0 A_2)
		{
			if (cell.IsValid && this.GasCanMoveTo(cell))
			{
				if (this.AnyGasAt(cell))
				{
					A_2.totalRotStink += (int)this.DensityAt(cell, GasType.RotStink);
					if (ModsConfig.BiotechActive)
					{
						A_2.totalToxGas += (int)this.DensityAt(cell, GasType.ToxGas);
					}
					if (ModsConfig.AnomalyActive)
					{
						A_2.totalDeadlifeDust += (int)this.DensityAt(cell, GasType.DeadlifeDust);
					}
				}
				GasGrid.beqCells[A_2.beqCellCount] = cell;
				int beqCellCount = A_2.beqCellCount;
				A_2.beqCellCount = beqCellCount + 1;
			}
		}

		// Token: 0x040010C4 RID: 4292
		private uint[] gasDensity;

		// Token: 0x040010C5 RID: 4293
		private Map map;

		// Token: 0x040010C6 RID: 4294
		private int cycleIndexDiffusion;

		// Token: 0x040010C7 RID: 4295
		private int cycleIndexDissipation;

		// Token: 0x040010C8 RID: 4296
		[Unsaved(false)]
		private List<IntVec3> cardinalDirections;

		// Token: 0x040010C9 RID: 4297
		[Unsaved(false)]
		private List<IntVec3> cellsInRandomOrder;

		// Token: 0x040010CA RID: 4298
		[Unsaved(false)]
		private bool anyGasEverAdded;

		// Token: 0x040010CB RID: 4299
		public const int MaxGasPerCell = 255;

		// Token: 0x040010CC RID: 4300
		private const int EstimatedMaxGasPerCell = 48;

		// Token: 0x040010CD RID: 4301
		private const float CellsToDissipatePerTickFactor = 0.015625f;

		// Token: 0x040010CE RID: 4302
		private const float CellsToDiffusePerTickFactor = 0.03125f;

		// Token: 0x040010CF RID: 4303
		private const float VacuumDissipatePerTickFactor = 25f;

		// Token: 0x040010D0 RID: 4304
		private const float MaxOverflowFloodfillRadius = 40f;

		// Token: 0x040010D1 RID: 4305
		private const int DissipationAmount_BlindSmoke = 4;

		// Token: 0x040010D2 RID: 4306
		private const int DissipationAmount_ToxGas = 3;

		// Token: 0x040010D3 RID: 4307
		private const int DissipationAmount_RotStink = 4;

		// Token: 0x040010D4 RID: 4308
		private const int DissipationAmount_DeadlifeDust = 3;

		// Token: 0x040010D5 RID: 4309
		private const int MinDiffusion = 17;

		// Token: 0x040010D6 RID: 4310
		private const int AnyGasCheckIntervalTicks = 600;

		// Token: 0x040010D7 RID: 4311
		private static readonly IntVec3[] beqCells = new IntVec3[4];
	}
}
