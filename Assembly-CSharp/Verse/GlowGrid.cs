using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;
using Verse.Glow;

namespace Verse
{
	// Token: 0x02000276 RID: 630
	[BurstCompile(CompileSynchronously = true)]
	public sealed class GlowGrid : IDisposable
	{
		// Token: 0x06001247 RID: 4679 RVA: 0x00068AF8 File Offset: 0x00066CF8
		public GlowGrid(Map map)
		{
			this.map = map;
			this.indices = map.cellIndices;
			this.lights = new NativeList<GlowLight>(4096, Allocator.Persistent);
			this.lightBlockers = new NativeBitArray(this.indices.NumGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.dirtyCells = new UnsafeBitArray(this.indices.NumGridCells, Allocator.Persistent);
			this.accumulatedGlow = new NativeArray<Color32>(this.indices.NumGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.accumulatedGlowNoCavePlants = new NativeArray<Color32>(this.indices.NumGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.glowPool = new GlowGrid.GlowPool();
			GlowGrid.FloodFillerPool.EnsureAllocated();
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x00068BD7 File Offset: 0x00066DD7
		private Color32 GetAccumulatedGlowAt(IntVec3 c, bool ignoreCavePlants = false)
		{
			return this.GetAccumulatedGlowAt(this.map.cellIndices.CellToIndex(c), ignoreCavePlants);
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x00068BF4 File Offset: 0x00066DF4
		private Color32 GetAccumulatedGlowAt(int index, bool ignoreCavePlants = false)
		{
			if (!this.accumulatedGlow.IsCreated)
			{
				return default(Color32);
			}
			if (this.map.terrainGrid.TerrainAt(index).exposesToVacuum)
			{
				return default(Color32);
			}
			return (ignoreCavePlants ? this.accumulatedGlowNoCavePlants : this.accumulatedGlow)[index];
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x00068C54 File Offset: 0x00066E54
		public Color32 VisualGlowAt(int index)
		{
			return this.GetAccumulatedGlowAt(index, false);
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x00068C5E File Offset: 0x00066E5E
		public Color32 VisualGlowAt(IntVec3 c)
		{
			return this.GetAccumulatedGlowAt(c, false);
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x00068C68 File Offset: 0x00066E68
		public float GroundGlowAt(IntVec3 c, bool ignoreCavePlants = false, bool ignoreSky = false)
		{
			float num = 0f;
			if (!ignoreSky && !this.map.roofGrid.Roofed(c))
			{
				num = this.map.skyManager.CurSkyGlow;
				if (num >= 1f)
				{
					return num;
				}
			}
			Color32 accumulatedGlowAt = this.GetAccumulatedGlowAt(c, ignoreCavePlants);
			if (accumulatedGlowAt.a == 1)
			{
				return 1f;
			}
			float b = (float)Mathf.Max(Mathf.Max((int)accumulatedGlowAt.r, (int)accumulatedGlowAt.g), (int)accumulatedGlowAt.b) / 255f * 3.6f;
			b = Mathf.Min(0.5f, b);
			return Mathf.Max(num, b);
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x00068D01 File Offset: 0x00066F01
		public PsychGlow PsychGlowAt(IntVec3 c)
		{
			return GlowGrid.PsychGlowAtGlow(this.GroundGlowAt(c, false, false));
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x00068D11 File Offset: 0x00066F11
		public static PsychGlow PsychGlowAtGlow(float glow)
		{
			if (glow > 0.9f)
			{
				return PsychGlow.Overlit;
			}
			if (glow > 0.3f)
			{
				return PsychGlow.Lit;
			}
			return PsychGlow.Dark;
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x00068D28 File Offset: 0x00066F28
		public void RegisterGlower(CompGlower newGlow)
		{
			if (!this.litGlowers.Add(newGlow))
			{
				return;
			}
			GlowLight glowLight = new GlowLight(newGlow, this.glowPool.Take());
			this.lights.Add(glowLight);
			this.anyDirtyLight = true;
			glowLight = this.lights[this.lights.Length - 1];
			foreach (IntVec3 cell in glowLight.AffectedRect.ClipInsideMap(this.map))
			{
				this.DirtyCell(cell);
			}
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x00068DD8 File Offset: 0x00066FD8
		public void DeRegisterGlower(CompGlower oldGlow)
		{
			if (!this.litGlowers.Remove(oldGlow))
			{
				return;
			}
			if (!this.lights.IsCreated)
			{
				return;
			}
			int glowerIndex = this.GetGlowerIndex(oldGlow);
			GlowLight glowLight = this.lights[glowerIndex];
			foreach (IntVec3 cell in glowLight.AffectedRect.ClipInsideMap(this.map))
			{
				this.DirtyCell(cell);
			}
			this.glowPool.ReturnSet(glowLight.localGlowPoolIndex);
			this.lights.RemoveAt(glowerIndex);
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x00068E90 File Offset: 0x00067090
		[BurstCompile]
		private int GetGlowerIndex(CompGlower glower)
		{
			for (int i = 0; i < this.lights.Length; i++)
			{
				GlowLight glowLight = this.lights[i];
				if (!glowLight.isTerrain && !glowLight.isCustom && glowLight.id == glower.parent.thingIDNumber)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x00068EE8 File Offset: 0x000670E8
		public void RegisterTerrain(IntVec3 cell)
		{
			if (!this.litTerrain.Add(cell))
			{
				return;
			}
			GlowLight glowLight = new GlowLight(cell, this.map, this.glowPool.Take());
			this.lights.Add(glowLight);
			this.anyDirtyLight = true;
			glowLight = this.lights[this.lights.Length - 1];
			foreach (IntVec3 cell2 in glowLight.AffectedRect.ClipInsideMap(this.map))
			{
				this.DirtyCell(cell2);
			}
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x00068F9C File Offset: 0x0006719C
		public void DeregisterTerrain(IntVec3 cell)
		{
			if (!this.litTerrain.Remove(cell))
			{
				return;
			}
			if (!this.lights.IsCreated)
			{
				return;
			}
			for (int i = 0; i < this.lights.Length; i++)
			{
				GlowLight glowLight = this.lights[i];
				if (glowLight.isTerrain && !(glowLight.position != cell))
				{
					foreach (IntVec3 cell2 in glowLight.AffectedRect.ClipInsideMap(this.map))
					{
						this.DirtyCell(cell2);
					}
					this.glowPool.ReturnSet(glowLight.localGlowPoolIndex);
					this.lights.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x00069080 File Offset: 0x00067280
		public int RegisterCustomGlower(IntVec3 position, float glowRadius, ColorInt glowColor, float overlightRadius = 0f, bool isCavePlant = false)
		{
			glowRadius = Mathf.Clamp(glowRadius, 0f, 40f);
			int num = this.nextCustomGlowerHandle;
			this.nextCustomGlowerHandle = num - 1;
			int num2 = num;
			this.litCustom.Add(num2);
			GlowLight glowLight = new GlowLight(num2, position, glowRadius, glowColor, overlightRadius, isCavePlant, this.glowPool.Take());
			this.lights.Add(glowLight);
			this.anyDirtyLight = true;
			glowLight = this.lights[this.lights.Length - 1];
			foreach (IntVec3 cell in glowLight.AffectedRect.ClipInsideMap(this.map))
			{
				this.DirtyCell(cell);
			}
			return num2;
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x0006915C File Offset: 0x0006735C
		public void DeregisterCustomGlower(int handle)
		{
			if (!this.litCustom.Remove(handle))
			{
				return;
			}
			if (!this.lights.IsCreated)
			{
				return;
			}
			for (int i = 0; i < this.lights.Length; i++)
			{
				GlowLight glowLight = this.lights[i];
				if (glowLight.isCustom && glowLight.id == handle)
				{
					foreach (IntVec3 cell in glowLight.AffectedRect.ClipInsideMap(this.map))
					{
						this.DirtyCell(cell);
					}
					this.glowPool.ReturnSet(glowLight.localGlowPoolIndex);
					this.lights.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x00069238 File Offset: 0x00067438
		public void GlowGridUpdate_First()
		{
			if (!this.accumulatedGlow.IsCreated || !this.accumulatedGlowNoCavePlants.IsCreated)
			{
				return;
			}
			try
			{
				if (this.anyDirtyLight)
				{
					using (new ProfilerBlock("ComputeGlowGridsJob()"))
					{
						new ComputeGlowGridsJob
						{
							lights = this.lights,
							indices = this.indices,
							lightBlockers = this.lightBlockers,
							statePool = GlowGrid.FloodFillerPool.pool,
							glowPool = this.glowPool.pool,
							mapSize = this.map.Size
						}.Schedule(this.lights.Length, UnityData.GetIdealBatchCount(this.lights.Length), default(JobHandle)).Complete();
						this.anyDirtyLight = false;
					}
				}
				if (this.anyDirtyCell)
				{
					using (new ProfilerBlock("CombineColorsJob()"))
					{
						new GlowGrid.CombineColorsJob
						{
							lights = this.lights,
							dirtyCells = this.dirtyCells,
							indices = this.indices,
							glow = this.accumulatedGlow,
							glowNoCavePlants = this.accumulatedGlowNoCavePlants,
							glowPool = this.glowPool.pool,
							allDirty = !this.hasRunInitially
						}.Schedule(this.indices.NumGridCells, UnityData.GetIdealBatchCount(this.indices.NumGridCells), default(JobHandle)).Complete();
						this.anyDirtyCell = false;
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
				throw;
			}
			if (!this.hasRunInitially)
			{
				this.map.mapDrawer.WholeMapChanged(MapMeshFlagDefOf.GroundGlow);
			}
			this.hasRunInitially = true;
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x0006946C File Offset: 0x0006766C
		public void DirtyCell(IntVec3 cell)
		{
			if (!this.dirtyCells.IsCreated || !this.hasRunInitially)
			{
				return;
			}
			this.dirtyCells.Set(this.indices.CellToIndex(cell), true);
			this.map.mapDrawer.MapMeshDirty(cell, MapMeshFlagDefOf.Roofs);
			this.map.mapDrawer.MapMeshDirty(cell, MapMeshFlagDefOf.GroundGlow);
			this.anyDirtyCell = true;
			this.map.events.Notify_GlowChanged(cell);
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x000694F5 File Offset: 0x000676F5
		public void LightBlockerAdded(IntVec3 cell)
		{
			this.lightBlockers.Set(this.indices.CellToIndex(cell), true);
			this.DirtyCell(cell);
			this.DirtyLightsAround(cell);
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x0006951D File Offset: 0x0006771D
		public void LightBlockerRemoved(IntVec3 cell)
		{
			if (!this.lightBlockers.IsCreated)
			{
				return;
			}
			this.lightBlockers.Set(this.indices.CellToIndex(cell), false);
			this.DirtyCell(cell);
			this.DirtyLightsAround(cell);
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x00069554 File Offset: 0x00067754
		[BurstCompile]
		private void DirtyLightsAround(IntVec3 cell)
		{
			if (!this.hasRunInitially)
			{
				return;
			}
			for (int i = 0; i < this.lights.Length; i++)
			{
				GlowLight glowLight = this.lights[i];
				if (!glowLight.dirty && glowLight.AffectedRect.Contains(cell))
				{
					glowLight.dirty = true;
					this.lights[i] = glowLight;
					foreach (IntVec3 intVec in glowLight.AffectedRect)
					{
						if (intVec.InBounds(this.map) && this.indices.Contains(intVec))
						{
							this.DirtyCell(intVec);
						}
					}
					this.anyDirtyLight = true;
				}
			}
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x00069638 File Offset: 0x00067838
		public void DevPrintLightIdsAffectingCell(IntVec3 cell)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = stringBuilder;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Lights affecting ");
			defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(cell);
			defaultInterpolatedStringHandler.AppendLiteral("\n");
			stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
			for (int i = 0; i < this.lights.Length; i++)
			{
				if (this.lights[i].AffectedRect.Contains(cell))
				{
					StringBuilder stringBuilder3 = stringBuilder;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(8, 2);
					defaultInterpolatedStringHandler2.AppendLiteral("   [");
					defaultInterpolatedStringHandler2.AppendFormatted<int>(i);
					defaultInterpolatedStringHandler2.AppendLiteral("] - ");
					defaultInterpolatedStringHandler2.AppendFormatted(this.lights[i].ToString());
					stringBuilder3.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x00069720 File Offset: 0x00067920
		public void DevDirtyRect(CellRect rect)
		{
			foreach (IntVec3 c in rect.Cells)
			{
				this.dirtyCells.Set(this.indices.CellToIndex(c), true);
			}
			this.anyDirtyCell = true;
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x00069788 File Offset: 0x00067988
		public void Dispose()
		{
			this.glowPool.Dispose();
			this.lights.Dispose();
			this.dirtyCells.Dispose();
			this.lightBlockers.Dispose();
			this.accumulatedGlow.Dispose();
			this.accumulatedGlowNoCavePlants.Dispose();
		}

		// Token: 0x04001083 RID: 4227
		private readonly Map map;

		// Token: 0x04001084 RID: 4228
		private readonly CellIndices indices;

		// Token: 0x04001085 RID: 4229
		private readonly GlowGrid.GlowPool glowPool;

		// Token: 0x04001086 RID: 4230
		private NativeList<GlowLight> lights;

		// Token: 0x04001087 RID: 4231
		private NativeBitArray lightBlockers;

		// Token: 0x04001088 RID: 4232
		private UnsafeBitArray dirtyCells;

		// Token: 0x04001089 RID: 4233
		private NativeArray<Color32> accumulatedGlow;

		// Token: 0x0400108A RID: 4234
		private NativeArray<Color32> accumulatedGlowNoCavePlants;

		// Token: 0x0400108B RID: 4235
		private readonly HashSet<CompGlower> litGlowers = new HashSet<CompGlower>();

		// Token: 0x0400108C RID: 4236
		private readonly HashSet<IntVec3> litTerrain = new HashSet<IntVec3>();

		// Token: 0x0400108D RID: 4237
		private readonly HashSet<int> litCustom = new HashSet<int>();

		// Token: 0x0400108E RID: 4238
		private int nextCustomGlowerHandle = -1;

		// Token: 0x0400108F RID: 4239
		private bool anyDirtyCell = true;

		// Token: 0x04001090 RID: 4240
		private bool anyDirtyLight;

		// Token: 0x04001091 RID: 4241
		private bool hasRunInitially;

		// Token: 0x04001092 RID: 4242
		private const int AlphaOfOverlit = 1;

		// Token: 0x04001093 RID: 4243
		private const float GameGlowLitThreshold = 0.3f;

		// Token: 0x04001094 RID: 4244
		private const float GameGlowOverlitThreshold = 0.9f;

		// Token: 0x04001095 RID: 4245
		private const float GroundGameGlowFactor = 3.6f;

		// Token: 0x04001096 RID: 4246
		private const float MaxGameGlowFromNonOverlitGroundLights = 0.5f;

		// Token: 0x04001097 RID: 4247
		public const int MaxLightRadius = 40;

		// Token: 0x04001098 RID: 4248
		public const int MaxLightDiameter = 81;

		// Token: 0x04001099 RID: 4249
		public const int MaxLightCells = 6561;

		// Token: 0x020025DB RID: 9691
		private static class FloodFillerPool
		{
			// Token: 0x0600F110 RID: 61712 RVA: 0x0058A298 File Offset: 0x00588498
			public static void EnsureAllocated()
			{
				if (GlowGrid.FloodFillerPool.allocated)
				{
					return;
				}
				GlowGrid.FloodFillerPool.allocated = true;
				UnityData.DisposeStatic += GlowGrid.FloodFillerPool.Dispose;
				GlowGrid.FloodFillerPool.pool = new NativeArray<GlowUniqueState>(UnityData.MaxJobWorkerThreadCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				for (int i = 0; i < GlowGrid.FloodFillerPool.pool.Length; i++)
				{
					GlowGrid.FloodFillerPool.pool[i] = GlowUniqueState.AllocateNew();
				}
			}

			// Token: 0x0600F111 RID: 61713 RVA: 0x0058A2FC File Offset: 0x005884FC
			private static void Dispose()
			{
				if (!GlowGrid.FloodFillerPool.allocated)
				{
					return;
				}
				for (int i = 0; i < GlowGrid.FloodFillerPool.pool.Length; i++)
				{
					GlowGrid.FloodFillerPool.pool[i].Dispose();
				}
				GlowGrid.FloodFillerPool.pool.Dispose();
				GlowGrid.FloodFillerPool.allocated = false;
			}

			// Token: 0x04009B3F RID: 39743
			public static NativeArray<GlowUniqueState> pool;

			// Token: 0x04009B40 RID: 39744
			private static bool allocated;
		}

		// Token: 0x020025DC RID: 9692
		private class GlowPool : IDisposable
		{
			// Token: 0x0600F112 RID: 61714 RVA: 0x0058A34C File Offset: 0x0058854C
			public int Take()
			{
				for (int i = 0; i < this.pool.Length; i++)
				{
					LocalGlowArea localGlowArea = this.pool[i];
					if (!localGlowArea.inUse)
					{
						localGlowArea.Take();
						this.pool[i] = localGlowArea;
						return i;
					}
				}
				LocalGlowArea localGlowArea2 = LocalGlowArea.AllocateNew();
				localGlowArea2.Take();
				this.pool.Add(localGlowArea2);
				return this.pool.Length - 1;
			}

			// Token: 0x0600F113 RID: 61715 RVA: 0x0058A3C4 File Offset: 0x005885C4
			public void ReturnSet(int index)
			{
				LocalGlowArea value = this.pool[index];
				value.Return();
				this.pool[index] = value;
			}

			// Token: 0x0600F114 RID: 61716 RVA: 0x0058A3F4 File Offset: 0x005885F4
			public void Dispose()
			{
				for (int i = 0; i < this.pool.Length; i++)
				{
					this.pool[i].Dispose();
				}
				this.pool.Dispose();
			}

			// Token: 0x04009B41 RID: 39745
			public NativeList<LocalGlowArea> pool = new NativeList<LocalGlowArea>(4096, Allocator.Persistent);
		}

		// Token: 0x020025DD RID: 9693
		[BurstCompile]
		private struct CombineColorsJob : IJobParallelFor
		{
			// Token: 0x0600F116 RID: 61718 RVA: 0x0058A454 File Offset: 0x00588654
			[BurstCompile]
			public void Execute(int i)
			{
				if (!this.dirtyCells.IsSet(i) && !this.allDirty)
				{
					return;
				}
				this.dirtyCells.Set(i, false);
				IntVec3 c = this.indices.IndexToCell(i);
				Color32 value = default(Color32);
				Color32 value2 = default(Color32);
				for (int j = 0; j < this.lights.Length; j++)
				{
					GlowLight glowLight = this.lights[j];
					if (glowLight.AffectedRect.Contains(c))
					{
						int index = glowLight.WorldToLocalIndex(c);
						Color32 color = this.glowPool[glowLight.localGlowPoolIndex].colors[index];
						this.AddColors(ref value, color, glowLight.overlightRadius);
						if (!glowLight.isCavePlant)
						{
							this.AddColors(ref value2, color, glowLight.overlightRadius);
						}
					}
				}
				this.glow[i] = value;
				this.glowNoCavePlants[i] = value2;
			}

			// Token: 0x0600F117 RID: 61719 RVA: 0x0058A550 File Offset: 0x00588750
			[BurstCompile]
			private void AddColors(ref Color32 existingSum, in Color32 toAdd, float overlightRadius)
			{
				float num = (float)toAdd.a;
				ColorInt colorInt = toAdd.AsColorInt();
				colorInt.ClampToNonNegative();
				colorInt.a = 0;
				if (colorInt.r <= 0 && colorInt.g <= 0 && colorInt.b <= 0)
				{
					return;
				}
				ColorInt colA = existingSum.AsColorInt();
				colA += colorInt;
				if (num < overlightRadius)
				{
					colA.a = 1;
				}
				Color32 color;
				colA.ProjectToColor32Fast(out color);
				existingSum = color;
			}

			// Token: 0x04009B42 RID: 39746
			[ReadOnly]
			public NativeList<GlowLight> lights;

			// Token: 0x04009B43 RID: 39747
			public UnsafeBitArray dirtyCells;

			// Token: 0x04009B44 RID: 39748
			public CellIndices indices;

			// Token: 0x04009B45 RID: 39749
			public bool allDirty;

			// Token: 0x04009B46 RID: 39750
			[NativeDisableParallelForRestriction]
			public NativeArray<Color32> glow;

			// Token: 0x04009B47 RID: 39751
			[NativeDisableParallelForRestriction]
			public NativeArray<Color32> glowNoCavePlants;

			// Token: 0x04009B48 RID: 39752
			[NativeDisableParallelForRestriction]
			public NativeList<LocalGlowArea> glowPool;
		}
	}
}
