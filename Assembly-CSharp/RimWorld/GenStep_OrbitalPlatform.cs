using System;
using System.Collections.Generic;
using System.Xml;
using RimWorld.BaseGen;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012B0 RID: 4784
	public class GenStep_OrbitalPlatform : GenStep
	{
		// Token: 0x17001426 RID: 5158
		// (get) Token: 0x06007696 RID: 30358 RVA: 0x0029C114 File Offset: 0x0029A314
		protected virtual LayoutDef LayoutDef
		{
			get
			{
				return this.layoutDef;
			}
		}

		// Token: 0x17001427 RID: 5159
		// (get) Token: 0x06007697 RID: 30359 RVA: 0x0029C11C File Offset: 0x0029A31C
		protected virtual float SpawnTemp
		{
			get
			{
				return this.temperature.GetValueOrDefault(-75f);
			}
		}

		// Token: 0x17001428 RID: 5160
		// (get) Token: 0x06007698 RID: 30360 RVA: 0x0029C12E File Offset: 0x0029A32E
		public override int SeedPart
		{
			get
			{
				return 8256151;
			}
		}

		// Token: 0x06007699 RID: 30361 RVA: 0x0029C135 File Offset: 0x0029A335
		protected virtual Faction GetFaction(Map map)
		{
			if (this.useSiteFaction && map.Parent.Faction != null)
			{
				return map.Parent.Faction;
			}
			if (this.factionDef != null)
			{
				return Find.FactionManager.FirstFactionOfDef(this.factionDef);
			}
			return null;
		}

		// Token: 0x0600769A RID: 30362 RVA: 0x0029C174 File Offset: 0x0029A374
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckOdyssey("Orbital Platform"))
			{
				return;
			}
			float? threatPoints = null;
			if (parms.sitePart != null)
			{
				threatPoints = new float?(parms.sitePart.parms.points);
			}
			if (threatPoints == null)
			{
				Site site = map.Parent as Site;
				if (site != null)
				{
					threatPoints = new float?(site.ActualThreatPoints);
				}
			}
			Faction faction = this.GetFaction(map);
			CellRect rect = this.GeneratePlatform(map, faction, threatPoints);
			if (Rand.Chance(0.33f))
			{
				this.DoRing(map, rect);
			}
			else if (Rand.Chance(0.5f))
			{
				this.DoLargePlatforms(map, rect);
			}
			else
			{
				this.DoSmallPlatforms(map, rect);
			}
			this.SpawnCannons(map, rect.ExpandedBy(6));
			map.FogOfWarColor = new Color?(this.fogOfWarColor.ToColor);
			map.OrbitalDebris = this.orbitalDebrisDef;
			this.SpawnExteriorPrefabs(map, rect.ExpandedBy(6), faction);
		}

		// Token: 0x0600769B RID: 30363 RVA: 0x0029C264 File Offset: 0x0029A464
		private void DoLargePlatforms(Map map, CellRect rect)
		{
			int randomInRange = GenStep_OrbitalPlatform.LargeDockRange.RandomInRange;
			List<Rot4> list = new List<Rot4>
			{
				Rot4.North,
				Rot4.East,
				Rot4.South,
				Rot4.West
			};
			int num = 0;
			while (num < randomInRange && list.Any<Rot4>())
			{
				Rot4 rot = list.RandomElement<Rot4>();
				list.Remove(rot);
				SpaceGenUtility.GenerateConnectedPlatform(map, this.platformTerrain, rect, GenStep_OrbitalPlatform.LargeLandingAreaWidthRange, GenStep_OrbitalPlatform.LargeLandingAreaHeightRange, rot, 14, 0.2f, 2, null, null, null, null);
				num++;
			}
		}

		// Token: 0x0600769C RID: 30364 RVA: 0x0029C320 File Offset: 0x0029A520
		private void DoSmallPlatforms(Map map, CellRect rect)
		{
			ValueTuple<CellRect, CellRect, CellRect, CellRect> valueTuple = rect.Subdivide(1);
			int randomInRange = GenStep_OrbitalPlatform.SmallPlatformRange.RandomInRange;
			List<ValueTuple<CellRect, Rot4>> list = new List<ValueTuple<CellRect, Rot4>>
			{
				new ValueTuple<CellRect, Rot4>(valueTuple.Item1, Rot4.South),
				new ValueTuple<CellRect, Rot4>(valueTuple.Item1, Rot4.West),
				new ValueTuple<CellRect, Rot4>(valueTuple.Item3, Rot4.South),
				new ValueTuple<CellRect, Rot4>(valueTuple.Item3, Rot4.East),
				new ValueTuple<CellRect, Rot4>(valueTuple.Item2, Rot4.North),
				new ValueTuple<CellRect, Rot4>(valueTuple.Item2, Rot4.West),
				new ValueTuple<CellRect, Rot4>(valueTuple.Item4, Rot4.North),
				new ValueTuple<CellRect, Rot4>(valueTuple.Item4, Rot4.East)
			};
			int num = 0;
			while (num < randomInRange && list.Any<ValueTuple<CellRect, Rot4>>())
			{
				ValueTuple<CellRect, Rot4> valueTuple2 = list.RandomElement<ValueTuple<CellRect, Rot4>>();
				list.Remove(valueTuple2);
				ValueTuple<CellRect, Rot4> valueTuple3 = valueTuple2;
				CellRect item = valueTuple3.Item1;
				Rot4 item2 = valueTuple3.Item2;
				SpaceGenUtility.GenerateConnectedPlatform(map, this.platformTerrain, item, GenStep_OrbitalPlatform.SmallPlatformSizeRange, GenStep_OrbitalPlatform.SmallPlatformSizeRange, item2, GenStep_OrbitalPlatform.SmallPlatformDistanceRange.RandomInRange, 0.2f, 2, null, null, null, null);
				num++;
			}
		}

		// Token: 0x0600769D RID: 30365 RVA: 0x0029C490 File Offset: 0x0029A690
		private void DoRing(Map map, CellRect rect)
		{
			float num = Mathf.Sqrt((float)(rect.Width * rect.Width + rect.Height * rect.Height)) - (float)rect.Width - 12f;
			SpaceGenUtility.GenerateRing(map, rect, this.platformTerrain, Mathf.RoundToInt(num / 2f), 0, 13.9f, 0.5f, 0f);
		}

		// Token: 0x0600769E RID: 30366 RVA: 0x0029C4FC File Offset: 0x0029A6FC
		private void SpawnCannons(Map map, CellRect rect)
		{
			if (this.cannonDef == null)
			{
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				Rot4 rot = new Rot4(i);
				IntVec3 corner = rect.GetCorner(rot, true);
				int num = Mathf.Max(this.cannonDef.size.x, this.cannonDef.size.z) + 4;
				CellRect item = corner.RectAbout(num, num);
				MapGenUtility.Line_NewTemp(this.platformTerrain, map, corner, rect.CenterCell, 6f, true, TerrainDefOf.Space);
				foreach (IntVec3 c in item.Cells)
				{
					if (c.InHorDistOf(corner, (float)num / 2f - 0.6f))
					{
						map.terrainGrid.SetTerrain(c, TerrainDefOf.AncientTile);
					}
					else if (c.InHorDistOf(corner, (float)num / 2f + 0.5f))
					{
						map.terrainGrid.SetTerrain(c, this.platformTerrain);
					}
				}
				GenSpawn.Spawn(ThingMaker.MakeThing(this.cannonDef, null), corner, map, new Rot4(i % 4), WipeMode.Vanish, false, false);
				MapGenerator.UsedRects.Add(item);
			}
		}

		// Token: 0x0600769F RID: 30367 RVA: 0x0029C64C File Offset: 0x0029A84C
		private void SpawnExteriorPrefabs(Map map, CellRect rect, Faction faction)
		{
			GenStep_OrbitalPlatform.<>c__DisplayClass33_0 CS$<>8__locals1 = new GenStep_OrbitalPlatform.<>c__DisplayClass33_0();
			CS$<>8__locals1.rect = rect;
			CS$<>8__locals1.map = map;
			using (List<GenStep_OrbitalPlatform.PrefabRange>.Enumerator enumerator = this.exteriorPrefabs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GenStep_OrbitalPlatform.<>c__DisplayClass33_1 CS$<>8__locals2 = new GenStep_OrbitalPlatform.<>c__DisplayClass33_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals2.set = enumerator.Current;
					int randomInRange = CS$<>8__locals2.set.countRange.RandomInRange;
					for (int i = 0; i < randomInRange; i++)
					{
						IntVec3 intVec;
						if (CS$<>8__locals2.CS$<>8__locals1.rect.TryFindRandomCell(out intVec, new Predicate<IntVec3>(CS$<>8__locals2.<SpawnExteriorPrefabs>g__Validator|0)))
						{
							Rot4 opposite = CS$<>8__locals2.CS$<>8__locals1.rect.GetClosestEdge(intVec).Opposite;
							PrefabUtility.SpawnPrefab(CS$<>8__locals2.set.prefab, CS$<>8__locals2.CS$<>8__locals1.map, intVec, opposite, faction, null, null, null, false);
						}
					}
				}
			}
		}

		// Token: 0x060076A0 RID: 30368 RVA: 0x0029C748 File Offset: 0x0029A948
		private CellRect GeneratePlatform(Map map, Faction faction, float? threatPoints)
		{
			IntVec2 size = new IntVec2(GenStep_OrbitalPlatform.SizeRange.RandomInRange, GenStep_OrbitalPlatform.SizeRange.RandomInRange);
			Rot4 random = Rot4.Random;
			CellRect cellRect = map.Center.RectAbout(size, random).ClipInsideMap(map);
			StructureGenParams parms = new StructureGenParams
			{
				size = cellRect.Size
			};
			LayoutWorker worker = this.LayoutDef.Worker;
			LayoutStructureSketch layoutStructureSketch = worker.GenerateStructureSketch(parms);
			map.layoutStructureSketches.Add(layoutStructureSketch);
			worker.Spawn(layoutStructureSketch, map, cellRect.Min, threatPoints, null, true, false, faction);
			MapGenerator.SetVar<CellRect>("SpawnRect", cellRect);
			MapGenerator.UsedRects.Add(cellRect);
			return cellRect;
		}

		// Token: 0x060076A1 RID: 30369 RVA: 0x0029C7F4 File Offset: 0x0029A9F4
		public override void PostMapInitialized(Map map, GenStepParams parms)
		{
			MapGenUtility.SetMapRoomTemperature(map, this.layoutDef, this.SpawnTemp);
			if (this.spawnSentryDrones)
			{
				BaseGenUtility.ScatterSentryDronesInMap(GenStep_OrbitalPlatform.SentryCountFromPointsCurve, map, this.GetFaction(map), parms);
			}
		}

		// Token: 0x04004A50 RID: 19024
		private FactionDef factionDef;

		// Token: 0x04004A51 RID: 19025
		private LayoutDef layoutDef;

		// Token: 0x04004A52 RID: 19026
		private bool useSiteFaction;

		// Token: 0x04004A53 RID: 19027
		private float? temperature;

		// Token: 0x04004A54 RID: 19028
		private bool spawnSentryDrones;

		// Token: 0x04004A55 RID: 19029
		private ThingDef cannonDef;

		// Token: 0x04004A56 RID: 19030
		private TerrainDef platformTerrain;

		// Token: 0x04004A57 RID: 19031
		private OrbitalDebrisDef orbitalDebrisDef;

		// Token: 0x04004A58 RID: 19032
		private ColorInt fogOfWarColor = new ColorInt(43, 46, 47);

		// Token: 0x04004A59 RID: 19033
		private List<GenStep_OrbitalPlatform.PrefabRange> exteriorPrefabs = new List<GenStep_OrbitalPlatform.PrefabRange>();

		// Token: 0x04004A5A RID: 19034
		private static readonly IntRange LargeDockRange = new IntRange(1, 2);

		// Token: 0x04004A5B RID: 19035
		private static readonly IntRange SmallPlatformRange = new IntRange(4, 6);

		// Token: 0x04004A5C RID: 19036
		private static readonly IntRange SmallPlatformSizeRange = new IntRange(16, 20);

		// Token: 0x04004A5D RID: 19037
		private static readonly IntRange SmallPlatformDistanceRange = new IntRange(10, 18);

		// Token: 0x04004A5E RID: 19038
		private static readonly IntRange SizeRange = new IntRange(70, 80);

		// Token: 0x04004A5F RID: 19039
		private static readonly IntRange LargeLandingAreaWidthRange = new IntRange(30, 40);

		// Token: 0x04004A60 RID: 19040
		private static readonly IntRange LargeLandingAreaHeightRange = new IntRange(50, 60);

		// Token: 0x04004A61 RID: 19041
		public static readonly IntRange LandingPadBorderLumpLengthRange = new IntRange(6, 10);

		// Token: 0x04004A62 RID: 19042
		public static readonly IntRange LandingPadBorderLumpOffsetRange = new IntRange(-1, 1);

		// Token: 0x04004A63 RID: 19043
		private static readonly SimpleCurve SentryCountFromPointsCurve = new SimpleCurve(new CurvePoint[]
		{
			new CurvePoint(0f, 0f),
			new CurvePoint(100f, 2f),
			new CurvePoint(1000f, 8f),
			new CurvePoint(5000f, 16f)
		});

		// Token: 0x020031A3 RID: 12707
		private class PrefabRange
		{
			// Token: 0x06012966 RID: 76134 RVA: 0x0063A60B File Offset: 0x0063880B
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				XmlHelper.ParseElements(this, xmlRoot, "prefab", "countRange");
			}

			// Token: 0x0400C88B RID: 51339
			public PrefabDef prefab;

			// Token: 0x0400C88C RID: 51340
			public IntRange countRange;
		}
	}
}
