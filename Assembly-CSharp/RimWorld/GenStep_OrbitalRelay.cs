using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x020012B1 RID: 4785
	public class GenStep_OrbitalRelay : GenStep
	{
		// Token: 0x17001429 RID: 5161
		// (get) Token: 0x060076A4 RID: 30372 RVA: 0x0029C934 File Offset: 0x0029AB34
		public override int SeedPart
		{
			get
			{
				return 982398321;
			}
		}

		// Token: 0x060076A5 RID: 30373 RVA: 0x0029C93C File Offset: 0x0029AB3C
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckOdyssey("Orbital Relay"))
			{
				return;
			}
			map.regionAndRoomUpdater.Enabled = true;
			CellRect area;
			foreach (ValueTuple<CellRect, Rot4> valueTuple in GenStep_OrbitalRelay.GetSatelliteRects(area = this.GeneratePlatform(map)).TakeRandomDistinct(this.satelliteRange.RandomInRange))
			{
				CellRect item = valueTuple.Item1;
				Rot4 item2 = valueTuple.Item2;
				area = area.Encapsulate(item);
				this.SpawnSatellite(map, item, item2);
			}
			GenStep_OrbitalRelay.ReserveRelayAreas(map);
			CellRect cellRect;
			if (GenStep_OrbitalRelay.TryGetRect(area, map, new IntVec2(12, 12), out cellRect) || GenStep_OrbitalRelay.TryGetRect(area, map, new IntVec2(10, 10), out cellRect))
			{
				MapGenerator.PlayerStartSpot = cellRect.CenterCell;
			}
			int randomInRange = this.scatterRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				this.TryScatterPrefab(area, map);
			}
			map.OrbitalDebris = this.orbitalDebrisDef;
			GenStep_OrbitalRelay.used.Clear();
		}

		// Token: 0x060076A6 RID: 30374 RVA: 0x0029CA50 File Offset: 0x0029AC50
		private static void ReserveRelayAreas(Map map)
		{
			foreach (Thing t in map.listerThings.ThingsOfDef(ThingDefOf.MechRelay))
			{
				MapGenerator.UsedRects.Add(t.OccupiedRect().ExpandedBy(2));
			}
		}

		// Token: 0x060076A7 RID: 30375 RVA: 0x0029CAC0 File Offset: 0x0029ACC0
		private void TryScatterPrefab(CellRect area, Map map)
		{
			PrefabParms prefabParms;
			this.scatterPrefabs.Where(new Func<PrefabParms, bool>(GenStep_OrbitalRelay.RequiredForMinimum)).TryRandomElementByWeight((PrefabParms w) => w.weight, out prefabParms);
			if (prefabParms == null)
			{
				this.scatterPrefabs.Where(new Func<PrefabParms, bool>(GenStep_OrbitalRelay.CanSpawn)).TryRandomElementByWeight((PrefabParms w) => w.weight, out prefabParms);
			}
			if (prefabParms == null)
			{
				return;
			}
			ValueTuple<IntVec3, Rot4> valueTuple;
			if (!GenStep_OrbitalRelay.TryGetPrefabSpawn(area, map, prefabParms.def, out valueTuple))
			{
				return;
			}
			PrefabUtility.SpawnPrefab(prefabParms.def, map, valueTuple.Item1, valueTuple.Item2, null, null, null, null, false);
			if (!GenStep_OrbitalRelay.used.TryAdd(prefabParms.def, 1))
			{
				Dictionary<PrefabDef, int> dictionary = GenStep_OrbitalRelay.used;
				PrefabDef def = prefabParms.def;
				int num = dictionary[def];
				dictionary[def] = num + 1;
			}
			CellRect cellRect = valueTuple.Item1.RectAbout(prefabParms.def.size, valueTuple.Item2);
			MapGenerator.UsedRects.Add(cellRect.ExpandedBy(1));
		}

		// Token: 0x060076A8 RID: 30376 RVA: 0x0029CBE0 File Offset: 0x0029ADE0
		private void SpawnSatellite(Map map, CellRect rect, Rot4 rot)
		{
			PrefabParms prefabParms;
			this.satellitePrefabs.Where(new Func<PrefabParms, bool>(GenStep_OrbitalRelay.RequiredForMinimum)).TryRandomElementByWeight((PrefabParms w) => w.weight, out prefabParms);
			if (prefabParms == null)
			{
				this.satellitePrefabs.Where(new Func<PrefabParms, bool>(GenStep_OrbitalRelay.CanSpawn)).TryRandomElementByWeight((PrefabParms w) => w.weight, out prefabParms);
			}
			if (prefabParms == null)
			{
				return;
			}
			PrefabUtility.SpawnPrefab(prefabParms.def, map, rect.CenterCell, rot, null, null, null, null, false);
			if (!GenStep_OrbitalRelay.used.TryAdd(prefabParms.def, 1))
			{
				Dictionary<PrefabDef, int> dictionary = GenStep_OrbitalRelay.used;
				PrefabDef def = prefabParms.def;
				int num = dictionary[def];
				dictionary[def] = num + 1;
			}
		}

		// Token: 0x060076A9 RID: 30377 RVA: 0x0029CCBC File Offset: 0x0029AEBC
		private static bool RequiredForMinimum(PrefabParms p)
		{
			int num;
			return p.minMaxRange.IsValid && p.minMaxRange.min > 0 && (!GenStep_OrbitalRelay.used.TryGetValue(p.def, out num) || num < p.minMaxRange.min);
		}

		// Token: 0x060076AA RID: 30378 RVA: 0x0029CD0C File Offset: 0x0029AF0C
		private static bool CanSpawn(PrefabParms p)
		{
			int num;
			return p.minMaxRange.IsInvalid || !GenStep_OrbitalRelay.used.TryGetValue(p.def, out num) || num < p.minMaxRange.max;
		}

		// Token: 0x060076AB RID: 30379 RVA: 0x0029CD4C File Offset: 0x0029AF4C
		private static bool TryGetPrefabSpawn(CellRect area, Map map, PrefabDef prefab, [TupleElementNames(new string[]
		{
			"cell",
			"rot"
		})] out ValueTuple<IntVec3, Rot4> result)
		{
			List<CellRect> usedRects = MapGenerator.UsedRects;
			int num = Rand.Range(0, 3);
			foreach (IntVec3 intVec in area.Cells.InRandomOrder(null))
			{
				for (int i = 0; i < 4; i++)
				{
					Rot4 rot = new Rot4(num + i);
					CellRect cellRect = intVec.RectAbout(prefab.size, rot);
					bool flag = true;
					using (CellRect.Enumerator enumerator2 = cellRect.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							IntVec3 inner = enumerator2.Current;
							if (usedRects.Any((CellRect r) => r.Contains(inner)))
							{
								flag = false;
								break;
							}
						}
					}
					if (flag && PrefabUtility.CanSpawnPrefab(prefab, map, intVec, rot, false, 0))
					{
						result = new ValueTuple<IntVec3, Rot4>(intVec, rot);
						return true;
					}
				}
			}
			result = default(ValueTuple<IntVec3, Rot4>);
			return false;
		}

		// Token: 0x060076AC RID: 30380 RVA: 0x0029CE70 File Offset: 0x0029B070
		private static bool TryGetRect(CellRect area, Map map, IntVec2 size, out CellRect rect)
		{
			GenStep_OrbitalRelay.<>c__DisplayClass17_0 CS$<>8__locals1 = new GenStep_OrbitalRelay.<>c__DisplayClass17_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.usedRects = MapGenerator.UsedRects;
			bool flag = area.TryFindRandomInnerRect(size, out rect, new Predicate<CellRect>(CS$<>8__locals1.<TryGetRect>g__Validator|0));
			if (flag)
			{
				CS$<>8__locals1.usedRects.Add(rect);
			}
			return flag;
		}

		// Token: 0x060076AD RID: 30381 RVA: 0x0029CEC0 File Offset: 0x0029B0C0
		private static List<ValueTuple<CellRect, Rot4>> GetSatelliteRects(CellRect center)
		{
			List<ValueTuple<CellRect, Rot4>> list = new List<ValueTuple<CellRect, Rot4>>(8);
			foreach (ValueTuple<int, int, int, int, Rot4, bool> valueTuple in new ValueTuple<int, int, int, int, Rot4, bool>[]
			{
				new ValueTuple<int, int, int, int, Rot4, bool>(center.minX, center.maxZ, center.maxX, center.maxZ + 20, Rot4.North, true),
				new ValueTuple<int, int, int, int, Rot4, bool>(center.minX, center.minZ - 20, center.maxX, center.minZ - 1, Rot4.South, true),
				new ValueTuple<int, int, int, int, Rot4, bool>(center.maxX, center.minZ, center.maxX + 20, center.maxZ, Rot4.East, false),
				new ValueTuple<int, int, int, int, Rot4, bool>(center.minX - 20, center.minZ, center.minX - 1, center.maxZ, Rot4.West, false)
			})
			{
				int item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				int item3 = valueTuple.Item3;
				int item4 = valueTuple.Item4;
				Rot4 item5 = valueTuple.Item5;
				bool item6 = valueTuple.Item6;
				CellRect cellRect = CellRect.FromLimits(item, item2, item3, item4);
				CellRect item7;
				CellRect item8;
				if (!item6)
				{
					ValueTuple<CellRect, CellRect> valueTuple2 = cellRect.SplitVertical(0);
					item7 = valueTuple2.Item1;
					item8 = valueTuple2.Item2;
				}
				else
				{
					ValueTuple<CellRect, CellRect> valueTuple2 = cellRect.SplitHorizontal(0);
					item7 = valueTuple2.Item1;
					item8 = valueTuple2.Item2;
				}
				list.Add(new ValueTuple<CellRect, Rot4>(item7, item5));
				list.Add(new ValueTuple<CellRect, Rot4>(item8, item5));
			}
			return list;
		}

		// Token: 0x060076AE RID: 30382 RVA: 0x0029D044 File Offset: 0x0029B244
		private CellRect GeneratePlatform(Map map)
		{
			PrefabParms prefabParms = this.centerPrefabs.RandomElementByWeight((PrefabParms w) => w.weight);
			CellRect result = map.Center.RectAbout(prefabParms.def.size);
			PrefabUtility.SpawnPrefab(prefabParms.def, map, map.Center, Rot4.North, null, null, null, null, false);
			foreach (Thing thing in map.listerThings.ThingsOfDef(ThingDefOf.MechRelay))
			{
				thing.TryGetComp<CompMechRelay>().inert = true;
			}
			return result;
		}

		// Token: 0x04004A64 RID: 19044
		private OrbitalDebrisDef orbitalDebrisDef;

		// Token: 0x04004A65 RID: 19045
		private List<PrefabParms> centerPrefabs = new List<PrefabParms>();

		// Token: 0x04004A66 RID: 19046
		private List<PrefabParms> satellitePrefabs = new List<PrefabParms>();

		// Token: 0x04004A67 RID: 19047
		private List<PrefabParms> scatterPrefabs = new List<PrefabParms>();

		// Token: 0x04004A68 RID: 19048
		private IntRange satelliteRange = new IntRange(2, 4);

		// Token: 0x04004A69 RID: 19049
		private IntRange scatterRange = new IntRange(2, 3);

		// Token: 0x04004A6A RID: 19050
		private const int SatelliteSize = 20;

		// Token: 0x04004A6B RID: 19051
		private static readonly Dictionary<PrefabDef, int> used = new Dictionary<PrefabDef, int>();
	}
}
