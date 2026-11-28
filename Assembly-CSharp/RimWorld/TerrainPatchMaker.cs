using System;
using System.Collections.Generic;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020012D0 RID: 4816
	public class TerrainPatchMaker
	{
		// Token: 0x06007777 RID: 30583 RVA: 0x002A2520 File Offset: 0x002A0720
		private void Init(Map map)
		{
			this.noise = new Perlin((double)this.perlinFrequency, (double)this.perlinLacunarity, (double)this.perlinPersistence, this.perlinOctaves, Rand.Range(0, int.MaxValue), QualityMode.Medium);
			NoiseDebugUI.RenderSize = new IntVec2(map.Size.x, map.Size.z);
			NoiseDebugUI.StoreNoiseRender(this.noise, "TerrainPatchMaker " + this.thresholds[0].terrain.defName);
			this.currentlyInitializedForMap = map;
		}

		// Token: 0x06007778 RID: 30584 RVA: 0x002A25B1 File Offset: 0x002A07B1
		public void Cleanup()
		{
			this.noise = null;
			this.currentlyInitializedForMap = null;
		}

		// Token: 0x06007779 RID: 30585 RVA: 0x002A25C4 File Offset: 0x002A07C4
		public TerrainDef TerrainAt(IntVec3 c, Map map, float fertility)
		{
			if (fertility < this.minFertility || fertility > this.maxFertility)
			{
				return null;
			}
			if (this.noise != null && map != this.currentlyInitializedForMap)
			{
				this.Cleanup();
			}
			if (this.noise == null)
			{
				this.Init(map);
			}
			if (this.minSize > 0)
			{
				int count = 0;
				map.floodFiller.FloodFill(c, (IntVec3 x) => TerrainThreshold.TerrainAtValue(this.thresholds, this.noise.GetValue(x)) != null, delegate(IntVec3 x)
				{
					int count = count;
					count++;
					return count >= this.minSize;
				}, int.MaxValue, false, null);
				if (count < this.minSize)
				{
					return null;
				}
			}
			return TerrainThreshold.TerrainAtValue(this.thresholds, this.noise.GetValue(c));
		}

		// Token: 0x04004AF3 RID: 19187
		private Map currentlyInitializedForMap;

		// Token: 0x04004AF4 RID: 19188
		public List<TerrainThreshold> thresholds = new List<TerrainThreshold>();

		// Token: 0x04004AF5 RID: 19189
		public float perlinFrequency = 0.01f;

		// Token: 0x04004AF6 RID: 19190
		public float perlinLacunarity = 2f;

		// Token: 0x04004AF7 RID: 19191
		public float perlinPersistence = 0.5f;

		// Token: 0x04004AF8 RID: 19192
		public int perlinOctaves = 6;

		// Token: 0x04004AF9 RID: 19193
		public float minFertility = -999f;

		// Token: 0x04004AFA RID: 19194
		public float maxFertility = 999f;

		// Token: 0x04004AFB RID: 19195
		public int minSize;

		// Token: 0x04004AFC RID: 19196
		public bool isPond;

		// Token: 0x04004AFD RID: 19197
		[Unsaved(false)]
		private ModuleBase noise;
	}
}
