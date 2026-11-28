using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001338 RID: 4920
	public class TileMutatorWorker_LavaFlow : TileMutatorWorker
	{
		// Token: 0x060079E1 RID: 31201 RVA: 0x002B5D56 File Offset: 0x002B3F56
		public TileMutatorWorker_LavaFlow(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x060079E2 RID: 31202 RVA: 0x002B5D60 File Offset: 0x002B3F60
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.rockNoise = new RidgedMultifractal(0.029999999329447746, 2.0, 3, Rand.Int, QualityMode.High);
			this.rockNoise = new Clamp(0.0, 1.0, this.rockNoise);
			this.rockNoise = new Invert(this.rockNoise);
			this.rockNoise = new ScaleBias(1.0, 1.0, this.rockNoise);
			NoiseDebugUI.StoreNoiseRender(this.rockNoise, "lava flow");
		}

		// Token: 0x060079E3 RID: 31203 RVA: 0x002B5E00 File Offset: 0x002B4000
		public override void GeneratePostElevationFertility(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (this.rockNoise.GetValue(intVec) <= 0.85f)
				{
					elevation[intVec] = 0f;
				}
			}
		}

		// Token: 0x060079E4 RID: 31204 RVA: 0x002B5E6C File Offset: 0x002B406C
		public override void GeneratePostTerrain(Map map)
		{
			TerrainPatchMaker terrainPatchMaker = map.Biome.terrainPatchMakers.FirstOrDefault((TerrainPatchMaker x) => x.isPond);
			foreach (IntVec3 intVec in map.AllCells)
			{
				TerrainDef terrain = intVec.GetTerrain(map);
				if (!terrain.IsWater)
				{
					if (terrain == TerrainDefOf.VolcanicRock)
					{
						map.terrainGrid.SetTerrain(intVec, TerrainDefOf.Soil);
					}
					TerrainDef terrainDef = (terrainPatchMaker != null) ? terrainPatchMaker.TerrainAt(intVec, map, intVec.GetFertility(map)) : null;
					if (terrainDef != null)
					{
						map.terrainGrid.SetTerrain(intVec, terrainDef);
						if (terrainDef == TerrainDefOf.VolcanicRock)
						{
							map.terrainGrid.SetTerrain(intVec, TerrainDefOf.CooledLava);
						}
					}
					else if (this.rockNoise.GetValue(intVec) <= 0.85f && terrain != TerrainDefOf.LavaDeep)
					{
						map.terrainGrid.SetTerrain(intVec, TerrainDefOf.VolcanicRock);
						map.terrainGrid.SetTerrain(intVec, TerrainDefOf.CooledLava);
					}
				}
			}
			if (terrainPatchMaker != null)
			{
				terrainPatchMaker.Cleanup();
			}
		}

		// Token: 0x060079E5 RID: 31205 RVA: 0x002B5FA0 File Offset: 0x002B41A0
		public override void Tick(Map map)
		{
			IncidentParms parms = StorytellerUtility.DefaultParmsNow(IncidentDefOf.LavaFlow.category, map);
			if (map.GameConditionManager.ConditionIsActive(GameConditionDefOf.LavaFlow))
			{
				return;
			}
			if (Rand.MTBEventOccurs(6f, 60000f, 1f) && IncidentDefOf.LavaFlow.Worker.CanFireNow(parms))
			{
				Find.Storyteller.incidentQueue.Add(IncidentDefOf.LavaFlow, Find.TickManager.TicksGame, parms, 0);
			}
		}

		// Token: 0x04004CE3 RID: 19683
		private const int IncidentMTBDays = 6;

		// Token: 0x04004CE4 RID: 19684
		private const float RidgedFreq = 0.03f;

		// Token: 0x04004CE5 RID: 19685
		private const float RidgedLac = 2f;

		// Token: 0x04004CE6 RID: 19686
		private const int RidgedOctaves = 3;

		// Token: 0x04004CE7 RID: 19687
		private const float RockThreshold = 0.85f;

		// Token: 0x04004CE8 RID: 19688
		private ModuleBase rockNoise;
	}
}
