using System;
using LudeonTK;
using Verse;

namespace RimWorld
{
	// Token: 0x0200133E RID: 4926
	public class TileMutatorWorker_Oasis : TileMutatorWorker_Lake
	{
		// Token: 0x170014D0 RID: 5328
		// (get) Token: 0x060079FE RID: 31230 RVA: 0x002B6CBC File Offset: 0x002B4EBC
		protected override float LakeRadius
		{
			get
			{
				return TileMutatorWorker_Oasis.LakeRadiusInt;
			}
		}

		// Token: 0x170014D1 RID: 5329
		// (get) Token: 0x060079FF RID: 31231 RVA: 0x002B6CC3 File Offset: 0x002B4EC3
		protected override float LakeFalloffDecay
		{
			get
			{
				return TileMutatorWorker_Oasis.LakeFalloffDecayInt;
			}
		}

		// Token: 0x06007A00 RID: 31232 RVA: 0x002B6CCA File Offset: 0x002B4ECA
		public TileMutatorWorker_Oasis(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007A01 RID: 31233 RVA: 0x002B6CD3 File Offset: 0x002B4ED3
		protected override IntVec3 GetLakeCenter(Map map)
		{
			return CellFinder.RandomNotEdgeCell(map.Size.x / 2, map);
		}

		// Token: 0x06007A02 RID: 31234 RVA: 0x002B6CE8 File Offset: 0x002B4EE8
		protected override void ProcessCell(IntVec3 cell, Map map)
		{
			float valAt = this.GetValAt(cell, map);
			TerrainDef terrainDef = map.terrainGrid.TerrainAt(cell);
			if (valAt > 0.75f)
			{
				map.terrainGrid.SetTerrain(cell, MapGenUtility.DeepFreshWaterTerrainAt(cell, map));
				return;
			}
			if (valAt > 0.57f)
			{
				map.terrainGrid.SetTerrain(cell, MapGenUtility.ShallowFreshWaterTerrainAt(cell, map));
				return;
			}
			if (valAt > 0.45f && !terrainDef.IsWater)
			{
				map.terrainGrid.SetTerrain(cell, TerrainDefOf.SoilRich);
				return;
			}
			if (valAt > 0.3f && !terrainDef.IsWater)
			{
				map.terrainGrid.SetTerrain(cell, TerrainDefOf.Soil);
			}
		}

		// Token: 0x04004CFA RID: 19706
		private new const float DeepWaterThreshold = 0.75f;

		// Token: 0x04004CFB RID: 19707
		private new const float WaterThreshold = 0.57f;

		// Token: 0x04004CFC RID: 19708
		private const float RichSoilThreshold = 0.45f;

		// Token: 0x04004CFD RID: 19709
		private const float SoilThreshold = 0.3f;

		// Token: 0x04004CFE RID: 19710
		[TweakValue("Oasis", 0f, 1f)]
		private static float LakeRadiusInt = 0.4f;

		// Token: 0x04004CFF RID: 19711
		[TweakValue("Oasis", 0f, 5f)]
		private static float LakeFalloffDecayInt = 2f;
	}
}
