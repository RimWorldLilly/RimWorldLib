using System;
using System.Runtime.CompilerServices;
using RimWorld;
using Unity.Burst;
using UnityEngine;

namespace Verse.Glow
{
	// Token: 0x020007B7 RID: 1975
	[BurstCompile]
	public struct GlowLight
	{
		// Token: 0x06003F8E RID: 16270 RVA: 0x001970CC File Offset: 0x001952CC
		public GlowLight(CompGlower glower, int glowPoolIndex)
		{
			this.overlightRadius = glower.Props.overlightRadius;
			this.glowRadius = glower.GlowRadius;
			this.glowColor = glower.GlowColor;
			bool flag;
			if (!glower.Props.overrideIsCavePlant)
			{
				PlantProperties plant = glower.parent.def.plant;
				flag = (plant != null && plant.cavePlant);
			}
			else
			{
				flag = true;
			}
			this.isCavePlant = flag;
			this.id = glower.parent.thingIDNumber;
			this.localGlowPoolIndex = glowPoolIndex;
			this.dirty = true;
			this.isTerrain = false;
			this.isCustom = false;
			this.position = glower.parent.Position;
			this.diameter = Mathf.CeilToInt(glower.GlowRadius * 2f + 1f);
			this.radius = Mathf.CeilToInt(glower.GlowRadius);
			this.localGlowGridStartPos = this.position - new IntVec3(this.radius, 0, this.radius);
		}

		// Token: 0x06003F8F RID: 16271 RVA: 0x001971C4 File Offset: 0x001953C4
		public GlowLight(IntVec3 cell, Map map, int glowPoolIndex)
		{
			this.id = map.cellIndices.CellToIndex(cell);
			TerrainDef terrainDef = map.terrainGrid.TerrainAt(this.id);
			this.overlightRadius = 0f;
			this.glowRadius = terrainDef.glowRadius;
			this.glowColor = terrainDef.glowColor;
			this.isCavePlant = false;
			this.isTerrain = true;
			this.isCustom = false;
			this.localGlowPoolIndex = glowPoolIndex;
			this.dirty = true;
			this.position = cell;
			this.diameter = Mathf.CeilToInt(this.glowRadius * 2f + 1f);
			this.radius = Mathf.CeilToInt(this.glowRadius);
			this.localGlowGridStartPos = this.position - new IntVec3(this.radius, 0, this.radius);
		}

		// Token: 0x06003F90 RID: 16272 RVA: 0x00197294 File Offset: 0x00195494
		public GlowLight(int customId, IntVec3 cell, float glowRadius, ColorInt glowColor, float overlightRadius, bool isCavePlant, int glowPoolIndex)
		{
			this.id = customId;
			this.overlightRadius = overlightRadius;
			this.glowRadius = glowRadius;
			this.glowColor = glowColor;
			this.isCavePlant = isCavePlant;
			this.isTerrain = false;
			this.isCustom = true;
			this.localGlowPoolIndex = glowPoolIndex;
			this.dirty = true;
			this.position = cell;
			this.diameter = Mathf.CeilToInt(glowRadius * 2f + 1f);
			this.radius = Mathf.CeilToInt(glowRadius);
			this.localGlowGridStartPos = this.position - new IntVec3(this.radius, 0, this.radius);
		}

		// Token: 0x17000C3B RID: 3131
		// (get) Token: 0x06003F91 RID: 16273 RVA: 0x00197332 File Offset: 0x00195532
		public CellRect AffectedRect
		{
			get
			{
				return new CellRect(this.localGlowGridStartPos.x, this.localGlowGridStartPos.z, this.diameter, this.diameter);
			}
		}

		// Token: 0x06003F92 RID: 16274 RVA: 0x0019735C File Offset: 0x0019555C
		[BurstCompile]
		public int WorldToLocalIndex(in IntVec3 world)
		{
			IntVec3 intVec = world - this.position;
			return this.DeltaToLocalIndex(intVec);
		}

		// Token: 0x06003F93 RID: 16275 RVA: 0x00197383 File Offset: 0x00195583
		[BurstCompile]
		public int DeltaToLocalIndex(in IntVec3 delta)
		{
			return CellIndicesUtility.CellToIndex(new IntVec3(this.radius, 0, this.radius) + delta, this.diameter);
		}

		// Token: 0x06003F94 RID: 16276 RVA: 0x001973AD File Offset: 0x001955AD
		[BurstCompile]
		public IntVec3 IndexToLocalDelta(int index)
		{
			return CellIndicesUtility.IndexToCell(index, this.diameter) - new IntVec3(this.radius, 0, this.radius);
		}

		// Token: 0x06003F95 RID: 16277 RVA: 0x001973D4 File Offset: 0x001955D4
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(98, 9);
			defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(this.position);
			defaultInterpolatedStringHandler.AppendLiteral(" with radius ");
			defaultInterpolatedStringHandler.AppendFormatted<float>(this.glowRadius);
			defaultInterpolatedStringHandler.AppendLiteral(" [ID: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.id);
			defaultInterpolatedStringHandler.AppendLiteral(", Rect: ");
			defaultInterpolatedStringHandler.AppendFormatted<CellRect>(this.AffectedRect);
			defaultInterpolatedStringHandler.AppendLiteral("], rgba: ");
			defaultInterpolatedStringHandler.AppendFormatted<ColorInt>(this.glowColor);
			defaultInterpolatedStringHandler.AppendLiteral(", isCavePlant: ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.isCavePlant);
			defaultInterpolatedStringHandler.AppendLiteral(", isTerrain: ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.isTerrain);
			defaultInterpolatedStringHandler.AppendLiteral(", isCustom: ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.isCustom);
			defaultInterpolatedStringHandler.AppendLiteral(", localGlowPoolIndex: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.localGlowPoolIndex);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04002A14 RID: 10772
		public readonly float overlightRadius;

		// Token: 0x04002A15 RID: 10773
		public readonly float glowRadius;

		// Token: 0x04002A16 RID: 10774
		public readonly ColorInt glowColor;

		// Token: 0x04002A17 RID: 10775
		public readonly bool isCavePlant;

		// Token: 0x04002A18 RID: 10776
		public readonly bool isTerrain;

		// Token: 0x04002A19 RID: 10777
		public readonly bool isCustom;

		// Token: 0x04002A1A RID: 10778
		public readonly IntVec3 position;

		// Token: 0x04002A1B RID: 10779
		public readonly IntVec3 localGlowGridStartPos;

		// Token: 0x04002A1C RID: 10780
		public readonly int radius;

		// Token: 0x04002A1D RID: 10781
		public readonly int diameter;

		// Token: 0x04002A1E RID: 10782
		public readonly int id;

		// Token: 0x04002A1F RID: 10783
		public int localGlowPoolIndex;

		// Token: 0x04002A20 RID: 10784
		public bool dirty;
	}
}
