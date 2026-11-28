using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001336 RID: 4918
	public class TileMutatorWorker_Lakeshore : TileMutatorWorker_Coast
	{
		// Token: 0x170014C9 RID: 5321
		// (get) Token: 0x060079D5 RID: 31189 RVA: 0x002B5A7E File Offset: 0x002B3C7E
		protected override float MaxForDeepWater
		{
			get
			{
				return 0.2f;
			}
		}

		// Token: 0x170014CA RID: 5322
		// (get) Token: 0x060079D6 RID: 31190 RVA: 0x002B5A85 File Offset: 0x002B3C85
		protected override float MaxForShallowWater
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x170014CB RID: 5323
		// (get) Token: 0x060079D7 RID: 31191 RVA: 0x002B5A8C File Offset: 0x002B3C8C
		protected override float MaxForSand
		{
			get
			{
				return 0.55f;
			}
		}

		// Token: 0x170014CC RID: 5324
		// (get) Token: 0x060079D8 RID: 31192 RVA: 0x002B5A93 File Offset: 0x002B3C93
		protected override FloatRange CoastOffset
		{
			get
			{
				return new FloatRange(0.25f, 0.35f);
			}
		}

		// Token: 0x060079D9 RID: 31193 RVA: 0x002B5AA4 File Offset: 0x002B3CA4
		public TileMutatorWorker_Lakeshore(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x060079DA RID: 31194 RVA: 0x002B5AAD File Offset: 0x002B3CAD
		protected override TerrainDef DeepWaterTerrainAt(IntVec3 cell, Map map)
		{
			return MapGenUtility.DeepFreshWaterTerrainAt(cell, map);
		}

		// Token: 0x060079DB RID: 31195 RVA: 0x002B5AB6 File Offset: 0x002B3CB6
		protected override TerrainDef ShallowWaterTerrainAt(IntVec3 cell, Map map)
		{
			return MapGenUtility.ShallowFreshWaterTerrainAt(cell, map);
		}

		// Token: 0x060079DC RID: 31196 RVA: 0x002B5ABF File Offset: 0x002B3CBF
		protected override TerrainDef BeachTerrainAt(IntVec3 cell, Map map)
		{
			return MapGenUtility.LakeshoreTerrainAt(cell, map);
		}

		// Token: 0x060079DD RID: 31197 RVA: 0x002B5AC8 File Offset: 0x002B3CC8
		protected override float GetCoastAngle(PlanetTile tile)
		{
			return Find.World.CoastAngleAt(tile, BiomeDefOf.Lake).Value;
		}
	}
}
