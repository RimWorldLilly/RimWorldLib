using System;
using System.Collections.Generic;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020012BA RID: 4794
	public class GenStep_RockChunks : GenStep
	{
		// Token: 0x17001439 RID: 5177
		// (get) Token: 0x060076FB RID: 30459 RVA: 0x0029F10E File Offset: 0x0029D30E
		public override int SeedPart
		{
			get
			{
				return 1898758716;
			}
		}

		// Token: 0x060076FC RID: 30460 RVA: 0x0029F118 File Offset: 0x0029D318
		public override void Generate(Map map, GenStepParams parms)
		{
			if (map.TileInfo.WaterCovered)
			{
				return;
			}
			this.freqFactorNoise = new Perlin(0.014999999664723873, 2.0, 0.5, 6, Rand.Range(0, 999999), QualityMode.Medium);
			this.freqFactorNoise = new ScaleBias(1.0, 1.0, this.freqFactorNoise);
			NoiseDebugUI.StoreNoiseRender(this.freqFactorNoise, "rock_chunks_freq_factor");
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			float num = 0.006f;
			foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
			{
				num *= tileMutatorDef.chunkDensityFactor;
			}
			foreach (IntVec3 intVec in map.AllCells)
			{
				float num2 = num * this.freqFactorNoise.GetValue(intVec);
				if ((elevation[intVec] < 0.55f || map.generatorDef.isUnderground) && Rand.Value < num2)
				{
					this.GrowLowRockFormationFrom(intVec, map);
				}
			}
			this.freqFactorNoise = null;
		}

		// Token: 0x060076FD RID: 30461 RVA: 0x0029F270 File Offset: 0x0029D470
		private void GrowLowRockFormationFrom(IntVec3 root, Map map)
		{
			ThingDef filth_RubbleRock = ThingDefOf.Filth_RubbleRock;
			List<ThingDef> forceRockTypes = map.Biome.forceRockTypes;
			ThingDef thingDef;
			if (forceRockTypes != null)
			{
				thingDef = forceRockTypes.RandomElement<ThingDef>();
			}
			else
			{
				thingDef = Find.World.NaturalRockTypesIn(map.Tile).RandomElement<ThingDef>();
			}
			ThingDef mineableThing = thingDef.building.mineableThing;
			if (mineableThing == null)
			{
				return;
			}
			Rot4 random = Rot4.Random;
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			IntVec3 intVec = root;
			for (;;)
			{
				Rot4 random2 = Rot4.Random;
				if (!(random2 == random))
				{
					intVec += random2.FacingCell;
					if (!intVec.InBounds(map) || intVec.GetEdifice(map) != null || intVec.GetFirstItem(map) != null)
					{
						break;
					}
					if (!map.generatorDef.isUnderground && elevation[intVec] > 0.55f)
					{
						return;
					}
					if (!intVec.GetAffordances(map).Contains(TerrainAffordanceDefOf.Heavy))
					{
						return;
					}
					if (intVec.GetDoor(map) != null)
					{
						return;
					}
					GenSpawn.Spawn(mineableThing, intVec, map, WipeMode.Vanish);
					foreach (IntVec3 b in GenAdj.AdjacentCellsAndInside)
					{
						if (Rand.Value < 0.5f)
						{
							IntVec3 c = intVec + b;
							if (c.InBounds(map))
							{
								bool flag = false;
								List<Thing> thingList = c.GetThingList(map);
								for (int j = 0; j < thingList.Count; j++)
								{
									Thing thing = thingList[j];
									if (thing.def.category != ThingCategory.Plant && thing.def.category != ThingCategory.Item && thing.def.category != ThingCategory.Pawn)
									{
										flag = true;
										break;
									}
								}
								if (!flag)
								{
									FilthMaker.TryMakeFilth(c, map, filth_RubbleRock, 1, FilthSourceFlags.None, true);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x04004AAB RID: 19115
		private ModuleBase freqFactorNoise;

		// Token: 0x04004AAC RID: 19116
		private const float ThreshLooseRock = 0.55f;

		// Token: 0x04004AAD RID: 19117
		private const float PlaceProbabilityPerCell = 0.006f;

		// Token: 0x04004AAE RID: 19118
		private const float RubbleProbability = 0.5f;
	}
}
