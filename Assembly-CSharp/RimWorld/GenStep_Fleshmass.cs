using System;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020012A1 RID: 4769
	public class GenStep_Fleshmass : GenStep
	{
		// Token: 0x1700140C RID: 5132
		// (get) Token: 0x06007647 RID: 30279 RVA: 0x0029A254 File Offset: 0x00298454
		public override int SeedPart
		{
			get
			{
				return 9872136;
			}
		}

		// Token: 0x06007648 RID: 30280 RVA: 0x0029A25C File Offset: 0x0029845C
		public override void Generate(Map map, GenStepParams parms)
		{
			Perlin perlin = new Perlin((double)this.fleshFrequency, 2.0, 0.5, this.noiseOctaves, Rand.Int, QualityMode.Medium);
			Perlin perlin2 = new Perlin((double)this.bloodFrequency, 2.0, 0.5, 6, Rand.Int, QualityMode.Medium);
			MapGenFloatGrid caves = MapGenerator.Caves;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (!map.generatorDef.isUnderground || caves[intVec] > 0f)
				{
					Building edifice = intVec.GetEdifice(map);
					if ((edifice == null || (this.fleshmassCanReplaceBuildings && !edifice.def.building.isNaturalRock)) && intVec.GetAffordances(map).Contains(ThingDefOf.Fleshmass.terrainAffordanceNeeded))
					{
						float num = (float)perlin.GetValue((double)intVec.x, 0.0, (double)intVec.z);
						if (this.fleshmassFalloffRadius > 0f)
						{
							float num2 = intVec.DistanceTo(map.Center);
							float num3 = 1f - Mathf.Clamp01((num2 - this.fleshmassFalloffRadius) / ((float)map.Size.x / 2f - this.fleshmassFalloffRadius));
							num *= num3;
						}
						if (num > this.fleshThreshold)
						{
							GenSpawn.Spawn(ThingDefOf.Fleshmass, intVec, map, WipeMode.Vanish).SetFaction(Faction.OfEntities, null);
						}
						if (num > this.fleshTerrainThreshold)
						{
							map.terrainGrid.SetTerrain(intVec, TerrainDefOf.Flesh);
						}
						float num4 = (float)perlin2.GetValue((double)intVec.x, 0.0, (double)intVec.z);
						ThingDef filthDef = Rand.Bool ? ThingDefOf.Filth_TwistedFlesh : ThingDefOf.Filth_Blood;
						if (num4 > this.bloodThreshold)
						{
							FilthMaker.TryMakeFilth(intVec, map, filthDef, 1, FilthSourceFlags.None, true);
						}
					}
				}
			}
		}

		// Token: 0x04004A24 RID: 18980
		private float fleshFrequency = 0.3f;

		// Token: 0x04004A25 RID: 18981
		private float fleshThreshold = 0.2f;

		// Token: 0x04004A26 RID: 18982
		private float fleshTerrainThreshold = 0.1f;

		// Token: 0x04004A27 RID: 18983
		private bool fleshmassCanReplaceBuildings;

		// Token: 0x04004A28 RID: 18984
		private float fleshmassFalloffRadius = -1f;

		// Token: 0x04004A29 RID: 18985
		private int noiseOctaves = 6;

		// Token: 0x04004A2A RID: 18986
		private float bloodFrequency = 0.3f;

		// Token: 0x04004A2B RID: 18987
		private float bloodThreshold = 0.2f;
	}
}
