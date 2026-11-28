using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020012EF RID: 4847
	public class GenStep_FrozenTerraformer : GenStep_BaseRuins
	{
		// Token: 0x17001492 RID: 5266
		// (get) Token: 0x06007848 RID: 30792 RVA: 0x002A854A File Offset: 0x002A674A
		public override int SeedPart
		{
			get
			{
				return 9871234;
			}
		}

		// Token: 0x17001493 RID: 5267
		// (get) Token: 0x06007849 RID: 30793 RVA: 0x002A8551 File Offset: 0x002A6751
		protected override LayoutDef LayoutDef
		{
			get
			{
				return LayoutDefOf.AncientRuinsFrozenTerraformer;
			}
		}

		// Token: 0x17001494 RID: 5268
		// (get) Token: 0x0600784A RID: 30794 RVA: 0x002A8558 File Offset: 0x002A6758
		protected override Faction Faction
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600784B RID: 30795 RVA: 0x002A855C File Offset: 0x002A675C
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckOdyssey("Frozen Terraformer"))
			{
				return;
			}
			this.iceNoise = new Perlin(0.009999999776482582, 2.0, 0.5, 6, Rand.Int, QualityMode.Medium);
			ModuleBase moduleBase = new DistFromPoint((float)map.Size.x * 0.3f * 2f);
			moduleBase = new ScaleBias(-1.0, 1.0, moduleBase);
			moduleBase = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), moduleBase);
			this.iceNoise = new Blend(this.iceNoise, moduleBase, new Const(0.699999988079071));
			MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (intVec.GetEdifice(map) == null)
				{
					float value = this.iceNoise.GetValue(intVec);
					if (value > 0.4f)
					{
						map.terrainGrid.SetTerrain(intVec, TerrainDefOf.Ice);
					}
					if (value > 0.5f)
					{
						GenSpawn.Spawn(ThingDefOf.SolidIce, intVec, map, WipeMode.Vanish);
					}
				}
			}
			CellRect cellRect = CellRect.CenteredOn(map.Center, 20);
			this.GenerateAndSpawn(cellRect, map, parms, this.LayoutDef);
			MapGenerator.SetVar<CellRect>("SpawnRect", cellRect);
		}

		// Token: 0x0600784C RID: 30796 RVA: 0x002A86D4 File Offset: 0x002A68D4
		public override void PostMapInitialized(Map map, GenStepParams parms)
		{
			BaseGenUtility.ScatterSentryDronesInMap(GenStep_FrozenTerraformer.SentryCountFromPointsCurve, map, Faction.OfAncientsHostile, parms);
		}

		// Token: 0x04004B61 RID: 19297
		private ModuleBase iceNoise;

		// Token: 0x04004B62 RID: 19298
		private const float NoiseFrequency = 0.01f;

		// Token: 0x04004B63 RID: 19299
		private const float FalloffStrength = 0.7f;

		// Token: 0x04004B64 RID: 19300
		private const float IceRadius = 0.3f;

		// Token: 0x04004B65 RID: 19301
		private const float IceThreshold = 0.5f;

		// Token: 0x04004B66 RID: 19302
		private const float IceTerrainThreshold = 0.4f;

		// Token: 0x04004B67 RID: 19303
		private const int BuildingSize = 40;

		// Token: 0x04004B68 RID: 19304
		private static readonly SimpleCurve SentryCountFromPointsCurve = new SimpleCurve(new CurvePoint[]
		{
			new CurvePoint(0f, 0f),
			new CurvePoint(100f, 2f),
			new CurvePoint(1000f, 8f),
			new CurvePoint(5000f, 16f)
		});
	}
}
