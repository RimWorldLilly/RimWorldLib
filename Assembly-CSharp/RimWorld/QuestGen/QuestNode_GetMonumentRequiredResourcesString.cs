using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021C8 RID: 8648
	public class QuestNode_GetMonumentRequiredResourcesString : QuestNode
	{
		// Token: 0x0600D749 RID: 55113 RVA: 0x004FDF14 File Offset: 0x004FC114
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x0600D74A RID: 55114 RVA: 0x004FDF1E File Offset: 0x004FC11E
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600D74B RID: 55115 RVA: 0x004FDF2C File Offset: 0x004FC12C
		private void DoWork(Slate slate)
		{
			MonumentMarker value = this.monumentMarker.GetValue(slate);
			if (value == null)
			{
				if (!this.storeMarketValueAs.GetValue(slate).NullOrEmpty())
				{
					slate.Set<float>(this.storeMarketValueAs.GetValue(slate), 0f, false);
				}
				return;
			}
			Dictionary<ThingDef, int> dictionary = new Dictionary<ThingDef, int>();
			List<Pair<List<StuffCategoryDef>, int>> list4 = new List<Pair<List<StuffCategoryDef>, int>>();
			List<List<Pair<ThingDef, int>>> list2 = new List<List<Pair<ThingDef, int>>>();
			int num = value.sketch.Entities.Where(delegate(SketchEntity x)
			{
				SketchTerrain sketchTerrain = x as SketchTerrain;
				return sketchTerrain != null && sketchTerrain.treatSimilarAsSame;
			}).Count<SketchEntity>();
			foreach (SketchEntity sketchEntity in value.sketch.Entities)
			{
				SketchBuildable sketchBuildable = sketchEntity as SketchBuildable;
				if (sketchBuildable != null)
				{
					if (sketchBuildable.Buildable.MadeFromStuff && sketchBuildable.Stuff == null)
					{
						int num2 = this.FindStuffsIndexFor(sketchBuildable.Buildable, list4);
						if (num2 < 0)
						{
							list4.Add(new Pair<List<StuffCategoryDef>, int>(sketchBuildable.Buildable.stuffCategories, sketchBuildable.Buildable.CostStuffCount));
						}
						else
						{
							list4[num2] = new Pair<List<StuffCategoryDef>, int>(list4[num2].First, list4[num2].Second + sketchBuildable.Buildable.CostStuffCount);
						}
						if (sketchBuildable.Buildable.CostList != null)
						{
							for (int i = 0; i < sketchBuildable.Buildable.CostList.Count; i++)
							{
								ThingDefCountClass thingDefCountClass = sketchBuildable.Buildable.CostList[i];
								int num3;
								if (!dictionary.TryGetValue(thingDefCountClass.thingDef, out num3))
								{
									num3 = 0;
								}
								dictionary[thingDefCountClass.thingDef] = num3 + thingDefCountClass.count;
							}
						}
					}
					else
					{
						SketchTerrain st = sketchBuildable as SketchTerrain;
						if (st != null && st.treatSimilarAsSame)
						{
							using (IEnumerator<TerrainDef> enumerator2 = (from x in DefDatabase<TerrainDef>.AllDefs
							where st.IsSameOrSimilar(x)
							select x).GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									TerrainDef terrainDef = enumerator2.Current;
									if (!terrainDef.CostList.NullOrEmpty<ThingDefCountClass>())
									{
										List<Pair<ThingDef, int>> list = new List<Pair<ThingDef, int>>();
										foreach (ThingDefCountClass thingDefCountClass2 in terrainDef.CostList)
										{
											list.Add(new Pair<ThingDef, int>(thingDefCountClass2.thingDef, thingDefCountClass2.count * num));
										}
										if (!list2.Any((List<Pair<ThingDef, int>> x) => x.SetsEqual(list)))
										{
											list2.Add(list);
										}
									}
								}
								continue;
							}
						}
						List<ThingDefCountClass> list3 = sketchBuildable.Buildable.CostListAdjusted(sketchBuildable.Stuff, true);
						for (int j = 0; j < list3.Count; j++)
						{
							int num4;
							if (!dictionary.TryGetValue(list3[j].thingDef, out num4))
							{
								num4 = 0;
							}
							dictionary[list3[j].thingDef] = num4 + list3[j].count;
						}
					}
				}
			}
			float num5 = 0f;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pair<List<StuffCategoryDef>, int> pair in list4)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("  - " + "AnyOf".Translate() + ": " + (from x in pair.First
				select x.label).ToCommaList(false, false) + " x" + pair.Second.ToString());
				num5 += this.GetCheapestStuffMarketValue(pair.First, pair.Second);
			}
			foreach (KeyValuePair<ThingDef, int> keyValuePair in dictionary)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("  - " + GenLabel.ThingLabel(keyValuePair.Key, null, keyValuePair.Value).CapitalizeFirst());
				num5 += keyValuePair.Key.BaseMarketValue * (float)keyValuePair.Value;
			}
			if (list2.Any<List<Pair<ThingDef, int>>>())
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("  - " + "AnyOf".Translate() + ":");
				foreach (List<Pair<ThingDef, int>> source in list2)
				{
					stringBuilder.AppendLine();
					stringBuilder.Append("    - " + (from x in source
					select x.First.label + " x" + x.Second.ToString()).ToCommaList(false, false));
				}
				num5 += this.GetCheapestThingMarketValue(list2);
			}
			slate.Set<string>(this.storeAs.GetValue(slate), stringBuilder.ToString(), false);
			if (!this.storeMarketValueAs.GetValue(slate).NullOrEmpty())
			{
				slate.Set<float>(this.storeMarketValueAs.GetValue(slate), num5, false);
			}
		}

		// Token: 0x0600D74C RID: 55116 RVA: 0x004FE598 File Offset: 0x004FC798
		private int FindStuffsIndexFor(BuildableDef buildable, List<Pair<List<StuffCategoryDef>, int>> anyOf)
		{
			for (int i = 0; i < anyOf.Count; i++)
			{
				if (anyOf[i].First.SetsEqual(buildable.stuffCategories))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600D74D RID: 55117 RVA: 0x004FE5D8 File Offset: 0x004FC7D8
		private float GetCheapestStuffMarketValue(List<StuffCategoryDef> categories, int count)
		{
			if (!categories.Any<StuffCategoryDef>())
			{
				return 0f;
			}
			float num = float.MaxValue;
			for (int i = 0; i < categories.Count; i++)
			{
				foreach (ThingDef thingDef in GenStuff.AllowedStuffs(categories, TechLevel.Undefined))
				{
					int num2 = Mathf.Max(Mathf.RoundToInt((float)count / thingDef.VolumePerUnit), 1);
					float num3 = thingDef.BaseMarketValue * (float)num2;
					if (num3 < num)
					{
						num = num3;
					}
				}
			}
			return num;
		}

		// Token: 0x0600D74E RID: 55118 RVA: 0x004FE670 File Offset: 0x004FC870
		private float GetCheapestThingMarketValue(List<List<Pair<ThingDef, int>>> costs)
		{
			if (!costs.Any<List<Pair<ThingDef, int>>>())
			{
				return 0f;
			}
			float num = float.MaxValue;
			for (int i = 0; i < costs.Count; i++)
			{
				float num2 = 0f;
				for (int j = 0; j < costs[i].Count; j++)
				{
					num2 += costs[i][j].First.BaseMarketValue * (float)costs[i][j].Second;
				}
				if (num2 < num)
				{
					num = num2;
				}
			}
			return num;
		}

		// Token: 0x0400889E RID: 34974
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400889F RID: 34975
		[NoTranslate]
		public SlateRef<string> storeMarketValueAs;

		// Token: 0x040088A0 RID: 34976
		public SlateRef<MonumentMarker> monumentMarker;
	}
}
