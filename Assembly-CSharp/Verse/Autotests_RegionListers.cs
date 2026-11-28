using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000D6 RID: 214
	public static class Autotests_RegionListers
	{
		// Token: 0x0600083B RID: 2107 RVA: 0x0002F87F File Offset: 0x0002DA7F
		public static void CheckBugs(Map map)
		{
			Autotests_RegionListers.CalculateExpectedListers(map);
			Autotests_RegionListers.CheckThingRegisteredTwice(map);
			Autotests_RegionListers.CheckThingNotRegisteredButShould();
			Autotests_RegionListers.CheckThingRegisteredButShouldnt(map);
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0002F898 File Offset: 0x0002DA98
		private static void CheckThingRegisteredTwice(Map map)
		{
			foreach (KeyValuePair<Region, List<Thing>> keyValuePair in Autotests_RegionListers.expectedListers)
			{
				Autotests_RegionListers.CheckDuplicates(keyValuePair.Value, keyValuePair.Key, true);
			}
			foreach (Region region in map.regionGrid.AllRegions)
			{
				Autotests_RegionListers.CheckDuplicates(region.ListerThings.AllThings, region, false);
			}
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0002F944 File Offset: 0x0002DB44
		private static void CheckDuplicates(List<Thing> lister, Region region, bool expected)
		{
			for (int i = 1; i < lister.Count; i++)
			{
				for (int j = 0; j < i; j++)
				{
					if (lister[i] == lister[j])
					{
						if (expected)
						{
							string[] array = new string[5];
							array[0] = "Region error: thing ";
							int num = 1;
							Thing thing = lister[i];
							array[num] = ((thing != null) ? thing.ToString() : null);
							array[2] = " is expected to be registered twice in ";
							array[3] = ((region != null) ? region.ToString() : null);
							array[4] = "? This should never happen.";
							Log.Error(string.Concat(array));
						}
						else
						{
							string str = "Region error: thing ";
							Thing thing2 = lister[i];
							Log.Error(str + ((thing2 != null) ? thing2.ToString() : null) + " is registered twice in " + ((region != null) ? region.ToString() : null));
						}
					}
				}
			}
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0002FA14 File Offset: 0x0002DC14
		private static void CheckThingNotRegisteredButShould()
		{
			foreach (KeyValuePair<Region, List<Thing>> keyValuePair in Autotests_RegionListers.expectedListers)
			{
				List<Thing> value = keyValuePair.Value;
				List<Thing> allThings = keyValuePair.Key.ListerThings.AllThings;
				for (int i = 0; i < value.Count; i++)
				{
					if (!allThings.Contains(value[i]))
					{
						string[] array = new string[7];
						array[0] = "Region error: thing ";
						int num = 1;
						Thing thing = value[i];
						array[num] = ((thing != null) ? thing.ToString() : null);
						array[2] = " at ";
						array[3] = value[i].Position.ToString();
						array[4] = " should be registered in ";
						int num2 = 5;
						Region key = keyValuePair.Key;
						array[num2] = ((key != null) ? key.ToString() : null);
						array[6] = " but it's not.";
						Log.Error(string.Concat(array));
					}
				}
			}
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0002FB28 File Offset: 0x0002DD28
		private static void CheckThingRegisteredButShouldnt(Map map)
		{
			foreach (Region region in map.regionGrid.AllRegions)
			{
				List<Thing> list;
				if (!Autotests_RegionListers.expectedListers.TryGetValue(region, out list))
				{
					list = null;
				}
				List<Thing> allThings = region.ListerThings.AllThings;
				for (int i = 0; i < allThings.Count; i++)
				{
					if (list == null || !list.Contains(allThings[i]))
					{
						string[] array = new string[7];
						array[0] = "Region error: thing ";
						int num = 1;
						Thing thing = allThings[i];
						array[num] = ((thing != null) ? thing.ToString() : null);
						array[2] = " at ";
						array[3] = allThings[i].Position.ToString();
						array[4] = " is registered in ";
						int num2 = 5;
						Region region2 = region;
						array[num2] = ((region2 != null) ? region2.ToString() : null);
						array[6] = " but it shouldn't be.";
						Log.Error(string.Concat(array));
					}
				}
			}
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0002FC3C File Offset: 0x0002DE3C
		private static void CalculateExpectedListers(Map map)
		{
			Autotests_RegionListers.expectedListers.Clear();
			List<Thing> allThings = map.listerThings.AllThings;
			for (int i = 0; i < allThings.Count; i++)
			{
				Thing thing = allThings[i];
				if (ListerThings.EverListable(thing.def, ListerThingsUse.Region))
				{
					RegionListersUpdater.GetTouchableRegions(thing, map, Autotests_RegionListers.tmpTouchableRegions, false);
					for (int j = 0; j < Autotests_RegionListers.tmpTouchableRegions.Count; j++)
					{
						Region key = Autotests_RegionListers.tmpTouchableRegions[j];
						List<Thing> list;
						if (!Autotests_RegionListers.expectedListers.TryGetValue(key, out list))
						{
							list = new List<Thing>();
							Autotests_RegionListers.expectedListers.Add(key, list);
						}
						list.Add(allThings[i]);
					}
				}
			}
		}

		// Token: 0x040003B7 RID: 951
		private static Dictionary<Region, List<Thing>> expectedListers = new Dictionary<Region, List<Thing>>();

		// Token: 0x040003B8 RID: 952
		private static List<Region> tmpTouchableRegions = new List<Region>();
	}
}
