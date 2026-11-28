using System;
using System.Collections.Generic;
using RimWorld.SketchGen;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021CA RID: 8650
	public class QuestNode_GetMonumentSketch : QuestNode
	{
		// Token: 0x0600D754 RID: 55124 RVA: 0x004FE758 File Offset: 0x004FC958
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D755 RID: 55125 RVA: 0x004FE75B File Offset: 0x004FC95B
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600D756 RID: 55126 RVA: 0x004FE76C File Offset: 0x004FC96C
		private bool DoWork(Slate slate)
		{
			float num = slate.Get<float>("points", 0f, false);
			float value = this.pointsPerArea.GetValue(slate);
			float num2 = Mathf.Min(num / value, 2500f);
			float randomInRange = QuestNode_GetMonumentSketch.RandomAspectRatioRange.RandomInRange;
			float f = Mathf.Sqrt(randomInRange * num2);
			float f2 = Mathf.Sqrt(num2 / randomInRange);
			int num3 = GenMath.RoundRandom(f);
			int num4 = GenMath.RoundRandom(f2);
			if (Rand.Bool)
			{
				int num5 = num3;
				num3 = num4;
				num4 = num5;
			}
			int? value2 = this.maxSize.GetValue(slate);
			if (value2 != null)
			{
				num3 = Mathf.Min(num3, value2.Value);
				num4 = Mathf.Min(num4, value2.Value);
			}
			num3 = Mathf.Max(num3, 3);
			num4 = Mathf.Max(num4, 3);
			IntVec2 value3 = new IntVec2(num3, num4);
			SketchResolveParams sketchResolveParams = default(SketchResolveParams);
			sketchResolveParams.sketch = new Sketch();
			sketchResolveParams.monumentSize = new IntVec2?(value3);
			sketchResolveParams.useOnlyStonesAvailableOnMap = this.useOnlyResourcesAvailableOnMap.GetValue(slate);
			sketchResolveParams.onlyBuildableByPlayer = new bool?(true);
			if (this.useOnlyResourcesAvailableOnMap.GetValue(slate) != null)
			{
				sketchResolveParams.allowWood = new bool?(this.useOnlyResourcesAvailableOnMap.GetValue(slate).Biome.TreeDensity >= BiomeDefOf.BorealForest.TreeDensity);
			}
			sketchResolveParams.allowedMonumentThings = new ThingFilter();
			sketchResolveParams.allowedMonumentThings.SetAllowAll(null, true);
			sketchResolveParams.allowedMonumentThings.SetAllow(ThingDefOf.Urn, false);
			Sketch sketch = SketchGen.Generate(SketchResolverDefOf.Monument, sketchResolveParams);
			if (this.clearStuff.GetValue(slate).GetValueOrDefault(true))
			{
				List<SketchThing> things = sketch.Things;
				for (int i = 0; i < things.Count; i++)
				{
					things[i].stuff = null;
				}
				List<SketchTerrain> terrain = sketch.Terrain;
				for (int j = 0; j < terrain.Count; j++)
				{
					terrain[j].treatSimilarAsSame = true;
				}
			}
			slate.Set<Sketch>(this.storeAs.GetValue(slate), sketch, false);
			return true;
		}

		// Token: 0x040088A3 RID: 34979
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040088A4 RID: 34980
		public SlateRef<Map> useOnlyResourcesAvailableOnMap;

		// Token: 0x040088A5 RID: 34981
		public SlateRef<int?> maxSize;

		// Token: 0x040088A6 RID: 34982
		public SlateRef<float> pointsPerArea;

		// Token: 0x040088A7 RID: 34983
		public SlateRef<bool?> clearStuff;

		// Token: 0x040088A8 RID: 34984
		private static readonly FloatRange RandomAspectRatioRange = new FloatRange(1f, 3f);

		// Token: 0x040088A9 RID: 34985
		private const int MinEdgeLength = 3;

		// Token: 0x040088AA RID: 34986
		private const int MaxArea = 2500;
	}
}
