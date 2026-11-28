using System;
using RimWorld.Planet;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001326 RID: 4902
	public class TileMutatorWorker_Fjord : TileMutatorWorker_Coast
	{
		// Token: 0x0600798E RID: 31118 RVA: 0x002B3BD0 File Offset: 0x002B1DD0
		public TileMutatorWorker_Fjord(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600798F RID: 31119 RVA: 0x002B3BD9 File Offset: 0x002B1DD9
		public override string GetLabel(PlanetTile tile)
		{
			return this.def.label;
		}

		// Token: 0x06007990 RID: 31120 RVA: 0x002B3BE8 File Offset: 0x002B1DE8
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			float coastAngle = this.GetCoastAngle(map.Tile);
			this.fjordNoise = new DistFromAxis((float)map.Size.x * 0.25f);
			this.fjordNoise = new Rotate(0.0, (double)(coastAngle + 90f), 0.0, this.fjordNoise);
			this.fjordNoise = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), this.fjordNoise);
			this.fjordNoise = new Max(this.fjordNoise, new Const((double)this.MaxForDeepWater));
			ModuleBase moduleBase = new DistFromCone(2f, (float)map.Size.x * 0.35f);
			moduleBase = new Translate(0.0, 0.0, (double)((float)(-(float)map.Size.x) * -0.5f), moduleBase);
			moduleBase = new Rotate(0.0, (double)(coastAngle + 90f), 0.0, moduleBase);
			moduleBase = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), moduleBase);
			this.fjordNoise = new SmoothMin(this.fjordNoise, moduleBase, 0.5);
			NoiseDebugUI.StoreNoiseRender(this.fjordNoise, "fjord shape");
			this.fjordNoise = MapNoiseUtility.AddDisplacementNoise(this.fjordNoise, 0.015f, 25f, 4, -1);
			NoiseDebugUI.StoreNoiseRender(this.fjordNoise, "fjord");
			this.coastNoise = new SmoothMin(this.fjordNoise, this.coastNoise, 0.5);
		}

		// Token: 0x06007991 RID: 31121 RVA: 0x002B3DB8 File Offset: 0x002B1FB8
		public override void GeneratePostElevationFertility(Map map)
		{
			base.GeneratePostElevationFertility(map);
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				float value = this.fjordNoise.GetValue(intVec);
				if (value < this.MaxForSand)
				{
					elevation[intVec] = 0f;
				}
				else
				{
					MapGenFloatGrid mapGenFloatGrid = elevation;
					IntVec3 c = intVec;
					mapGenFloatGrid[c] += value + -1f;
				}
			}
		}

		// Token: 0x04004C89 RID: 19593
		private const float FjordSpan = 0.25f;

		// Token: 0x04004C8A RID: 19594
		private const float ConeGradient = 2f;

		// Token: 0x04004C8B RID: 19595
		private const float ConeSpan = 0.35f;

		// Token: 0x04004C8C RID: 19596
		private const float ConeOffset = -0.5f;

		// Token: 0x04004C8D RID: 19597
		private const float ElevationOffset = -1f;

		// Token: 0x04004C8E RID: 19598
		private ModuleBase fjordNoise;
	}
}
