using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200032D RID: 813
	public static class ZoneColorUtility
	{
		// Token: 0x060018FD RID: 6397 RVA: 0x00095648 File Offset: 0x00093848
		static ZoneColorUtility()
		{
			foreach (Color color in ZoneColorUtility.GrowingZoneColors())
			{
				Color item = new Color(color.r, color.g, color.b, 0.09f);
				ZoneColorUtility.growingZoneColors.Add(item);
			}
			foreach (Color color2 in ZoneColorUtility.StorageZoneColors())
			{
				Color item2 = new Color(color2.r, color2.g, color2.b, 0.09f);
				ZoneColorUtility.storageZoneColors.Add(item2);
			}
			foreach (Color color3 in ZoneColorUtility.FishingZoneColors())
			{
				Color item3 = new Color(color3.r, color3.g, color3.b, 0.09f);
				ZoneColorUtility.fishingZoneColors.Add(item3);
			}
		}

		// Token: 0x060018FE RID: 6398 RVA: 0x000957A8 File Offset: 0x000939A8
		public static Color NextGrowingZoneColor()
		{
			Color result = ZoneColorUtility.growingZoneColors[ZoneColorUtility.nextGrowingZoneColorIndex];
			ZoneColorUtility.nextGrowingZoneColorIndex++;
			if (ZoneColorUtility.nextGrowingZoneColorIndex >= ZoneColorUtility.growingZoneColors.Count)
			{
				ZoneColorUtility.nextGrowingZoneColorIndex = 0;
			}
			return result;
		}

		// Token: 0x060018FF RID: 6399 RVA: 0x000957DC File Offset: 0x000939DC
		public static Color NextStorageZoneColor()
		{
			Color result = ZoneColorUtility.storageZoneColors[ZoneColorUtility.nextStorageZoneColorIndex];
			ZoneColorUtility.nextStorageZoneColorIndex++;
			if (ZoneColorUtility.nextStorageZoneColorIndex >= ZoneColorUtility.storageZoneColors.Count)
			{
				ZoneColorUtility.nextStorageZoneColorIndex = 0;
			}
			return result;
		}

		// Token: 0x06001900 RID: 6400 RVA: 0x00095810 File Offset: 0x00093A10
		public static Color NextFishingZoneColor()
		{
			Color result = ZoneColorUtility.fishingZoneColors[ZoneColorUtility.nextFishingZoneColorIndex];
			ZoneColorUtility.nextFishingZoneColorIndex++;
			if (ZoneColorUtility.nextFishingZoneColorIndex >= ZoneColorUtility.fishingZoneColors.Count)
			{
				ZoneColorUtility.nextFishingZoneColorIndex = 0;
			}
			return result;
		}

		// Token: 0x06001901 RID: 6401 RVA: 0x00095844 File Offset: 0x00093A44
		public static IEnumerable<Color> GrowingZoneColors()
		{
			return new ZoneColorUtility.<GrowingZoneColors>d__11(-2);
		}

		// Token: 0x06001902 RID: 6402 RVA: 0x0009584D File Offset: 0x00093A4D
		public static IEnumerable<Color> StorageZoneColors()
		{
			return new ZoneColorUtility.<StorageZoneColors>d__12(-2);
		}

		// Token: 0x06001903 RID: 6403 RVA: 0x00095856 File Offset: 0x00093A56
		public static IEnumerable<Color> FishingZoneColors()
		{
			return new ZoneColorUtility.<FishingZoneColors>d__13(-2);
		}

		// Token: 0x040014A9 RID: 5289
		private static List<Color> growingZoneColors = new List<Color>();

		// Token: 0x040014AA RID: 5290
		private static List<Color> storageZoneColors = new List<Color>();

		// Token: 0x040014AB RID: 5291
		private static List<Color> fishingZoneColors = new List<Color>();

		// Token: 0x040014AC RID: 5292
		private static int nextGrowingZoneColorIndex = 0;

		// Token: 0x040014AD RID: 5293
		private static int nextStorageZoneColorIndex = 0;

		// Token: 0x040014AE RID: 5294
		private static int nextFishingZoneColorIndex = 0;

		// Token: 0x040014AF RID: 5295
		private const float ZoneOpacity = 0.09f;
	}
}
