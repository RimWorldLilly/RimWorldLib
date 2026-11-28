using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002349 RID: 9033
	public class WorldGenStep_AncientRoads : WorldGenStep
	{
		// Token: 0x17002748 RID: 10056
		// (get) Token: 0x0600DFAB RID: 57259 RVA: 0x00532CF4 File Offset: 0x00530EF4
		public override int SeedPart
		{
			get
			{
				return 773428712;
			}
		}

		// Token: 0x0600DFAC RID: 57260 RVA: 0x00532CFB File Offset: 0x00530EFB
		public override void GenerateFresh(string seed, PlanetLayer layer)
		{
			this.GenerateAncientRoads(layer);
		}

		// Token: 0x0600DFAD RID: 57261 RVA: 0x00532D04 File Offset: 0x00530F04
		private void GenerateAncientRoads(PlanetLayer layer)
		{
			Find.WorldPathGrid.RecalculateLayerPerceivedPathCosts(layer, new int?(0));
			List<List<PlanetTile>> list = this.GenerateProspectiveRoads(layer);
			list.Sort((List<PlanetTile> lhs, List<PlanetTile> rhs) => -lhs.Count.CompareTo(rhs.Count));
			HashSet<PlanetTile> used = new HashSet<PlanetTile>();
			Predicate<PlanetTile> <>9__1;
			for (int i = 0; i < list.Count; i++)
			{
				List<PlanetTile> list2 = list[i];
				Predicate<PlanetTile> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((PlanetTile elem) => used.Contains(elem)));
				}
				if (!list2.Any(predicate))
				{
					if (list[i].Count < 4)
					{
						break;
					}
					foreach (PlanetTile item in list[i])
					{
						used.Add(item);
					}
					for (int j = 0; j < list[i].Count - 1; j++)
					{
						float num = Find.WorldGrid.ApproxDistanceInTiles(list[i][j], list[i][j + 1]) * this.maximumSegmentCurviness;
						float costCutoff = num * 12000f;
						using (WorldPath worldPath = layer.Pather.FindPath(list[i][j], list[i][j + 1], null, (float cost) => cost > costCutoff))
						{
							if (worldPath != null && worldPath != WorldPath.NotFound)
							{
								List<PlanetTile> nodesReversed = worldPath.NodesReversed;
								if ((float)nodesReversed.Count <= Find.WorldGrid.ApproxDistanceInTiles(list[i][j], list[i][j + 1]) * this.maximumSegmentCurviness)
								{
									for (int k = 0; k < nodesReversed.Count - 1; k++)
									{
										if (Find.WorldGrid.GetRoadDef(nodesReversed[k], nodesReversed[k + 1], false) != null)
										{
											Find.WorldGrid.OverlayRoad(nodesReversed[k], nodesReversed[k + 1], RoadDefOf.AncientAsphaltHighway);
										}
										else
										{
											Find.WorldGrid.OverlayRoad(nodesReversed[k], nodesReversed[k + 1], RoadDefOf.AncientAsphaltRoad);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600DFAE RID: 57262 RVA: 0x00532FA0 File Offset: 0x005311A0
		private List<List<PlanetTile>> GenerateProspectiveRoads(PlanetLayer layer)
		{
			List<PlanetTile> list = Find.World.genData.ancientSites[layer];
			List<List<PlanetTile>> list2 = new List<List<PlanetTile>>();
			for (int i = 0; i < list.Count; i++)
			{
				for (int j = 0; j < list.Count; j++)
				{
					List<PlanetTile> list3 = new List<PlanetTile>();
					list3.Add(list[i]);
					List<PlanetTile> list4 = list;
					float ang = Find.World.grid.GetHeadingFromTo(list[i], list[j]);
					PlanetTile current = list[i];
					Func<PlanetTile, bool> <>9__0;
					Func<PlanetTile, float> <>9__1;
					for (;;)
					{
						IEnumerable<PlanetTile> source = list4;
						Func<PlanetTile, bool> predicate;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = ((PlanetTile idx) => idx != current && Math.Abs(Find.World.grid.GetHeadingFromTo(current, idx) - ang) < this.maximumSiteCurve));
						}
						list4 = source.Where(predicate).ToList<PlanetTile>();
						if (list4.Count == 0)
						{
							break;
						}
						IEnumerable<PlanetTile> source2 = list4;
						Func<PlanetTile, float> keySelector;
						if ((keySelector = <>9__1) == null)
						{
							keySelector = (<>9__1 = ((PlanetTile idx) => Find.World.grid.ApproxDistanceInTiles(current, idx)));
						}
						PlanetTile planetTile = source2.MinBy(keySelector);
						ang = Find.World.grid.GetHeadingFromTo(current, planetTile);
						current = planetTile;
						list3.Add(current);
					}
					list2.Add(list3);
				}
			}
			return list2;
		}

		// Token: 0x04008F8E RID: 36750
		public float maximumSiteCurve;

		// Token: 0x04008F8F RID: 36751
		public float maximumSegmentCurviness;
	}
}
