using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001324 RID: 4900
	public class TileMutatorWorker_Crevasse : TileMutatorWorker
	{
		// Token: 0x06007987 RID: 31111 RVA: 0x002B37F0 File Offset: 0x002B19F0
		public TileMutatorWorker_Crevasse(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007988 RID: 31112 RVA: 0x002B37FC File Offset: 0x002B19FC
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			float num = Rand.Range(0f, 180f);
			this.crevasse = new DistFromAxis((float)map.Size.x * 0.15f);
			this.crevasse = new Rotate(0.0, (double)num, 0.0, this.crevasse);
			this.crevasse = new Translate((double)((float)(-(float)map.Size.x) / 2f), 0.0, (double)((float)(-(float)map.Size.z) / 2f), this.crevasse);
			this.crevasse = MapNoiseUtility.AddDisplacementNoise(this.crevasse, 0.015f, 20f, 4, -1);
			NoiseDebugUI.StoreNoiseRender(this.crevasse, "crevasse");
		}

		// Token: 0x06007989 RID: 31113 RVA: 0x002B38DC File Offset: 0x002B1ADC
		public override void GeneratePostElevationFertility(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (this.crevasse.GetValue(intVec) <= 0.5f)
				{
					elevation[intVec] = 0f;
				}
			}
		}

		// Token: 0x0600798A RID: 31114 RVA: 0x002B3948 File Offset: 0x002B1B48
		public override void GeneratePostTerrain(Map map)
		{
			foreach (IntVec3 intVec in map.AllCells)
			{
				float value = this.crevasse.GetValue(intVec);
				if (value > 0.5f)
				{
					if (value > 0.798f)
					{
						map.roofGrid.SetRoof(intVec, RoofDefOf.RoofRockThick);
					}
					else if (value > 0.728f)
					{
						map.roofGrid.SetRoof(intVec, RoofDefOf.RoofRockThin);
					}
					if (intVec.GetEdifice(map) == null)
					{
						GenSpawn.Spawn(ThingDefOf.SolidIce, intVec, map, WipeMode.Vanish);
						map.terrainGrid.SetTerrain(intVec, TerrainDefOf.Ice);
					}
				}
			}
		}

		// Token: 0x04004C78 RID: 19576
		private ModuleBase crevasse;

		// Token: 0x04004C79 RID: 19577
		private const float CrevasseSpan = 0.15f;

		// Token: 0x04004C7A RID: 19578
		private const float CrevasseThreshold = 0.5f;

		// Token: 0x04004C7B RID: 19579
		private const float CrevasseNoiseFreq = 0.015f;

		// Token: 0x04004C7C RID: 19580
		private const float CrevasseNoiseStrength = 20f;
	}
}
