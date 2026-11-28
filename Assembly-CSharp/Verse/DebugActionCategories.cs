using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020005B3 RID: 1459
	public static class DebugActionCategories
	{
		// Token: 0x06002C5E RID: 11358 RVA: 0x0010C5B0 File Offset: 0x0010A7B0
		static DebugActionCategories()
		{
			DebugActionCategories.categoryOrders.Add("Incidents", 100);
			DebugActionCategories.categoryOrders.Add("Quests", 200);
			DebugActionCategories.categoryOrders.Add("Quests (old)", 250);
			DebugActionCategories.categoryOrders.Add("Translation", 300);
			DebugActionCategories.categoryOrders.Add("General", 400);
			DebugActionCategories.categoryOrders.Add("Sound", 450);
			DebugActionCategories.categoryOrders.Add("Pawns", 500);
			DebugActionCategories.categoryOrders.Add("Spawning", 600);
			DebugActionCategories.categoryOrders.Add("Ideoligion", 700);
			DebugActionCategories.categoryOrders.Add("Map", 800);
			DebugActionCategories.categoryOrders.Add("Lighting", 850);
			DebugActionCategories.categoryOrders.Add("Autotests", 900);
			DebugActionCategories.categoryOrders.Add("Mods", 100);
			DebugActionCategories.categoryOrders.Add("More debug actions", 1000);
			DebugActionCategories.categoryOrders.Add("Humanlike", 1100);
			DebugActionCategories.categoryOrders.Add("Animal", 1200);
			DebugActionCategories.categoryOrders.Add("Insect", 1300);
			DebugActionCategories.categoryOrders.Add("Mechanoid", 1400);
			DebugActionCategories.categoryOrders.Add("Anomaly", 1500);
			DebugActionCategories.categoryOrders.Add("Pathing", 1625);
			DebugActionCategories.categoryOrders.Add("Generation", 1650);
			DebugActionCategories.categoryOrders.Add("Other", 1700);
		}

		// Token: 0x06002C5F RID: 11359 RVA: 0x0010C77C File Offset: 0x0010A97C
		public static int GetOrderFor(string category)
		{
			int result;
			if (!category.NullOrEmpty() && DebugActionCategories.categoryOrders.TryGetValue(category, out result))
			{
				return result;
			}
			return int.MaxValue;
		}

		// Token: 0x04001E07 RID: 7687
		public const string Incidents = "Incidents";

		// Token: 0x04001E08 RID: 7688
		public const string Quests = "Quests";

		// Token: 0x04001E09 RID: 7689
		public const string QuestsOld = "Quests (old)";

		// Token: 0x04001E0A RID: 7690
		public const string Translation = "Translation";

		// Token: 0x04001E0B RID: 7691
		public const string General = "General";

		// Token: 0x04001E0C RID: 7692
		public const string Sound = "Sound";

		// Token: 0x04001E0D RID: 7693
		public const string Pawns = "Pawns";

		// Token: 0x04001E0E RID: 7694
		public const string Spawning = "Spawning";

		// Token: 0x04001E0F RID: 7695
		public const string Ideoligion = "Ideoligion";

		// Token: 0x04001E10 RID: 7696
		public const string MapManagement = "Map";

		// Token: 0x04001E11 RID: 7697
		public const string Autotests = "Autotests";

		// Token: 0x04001E12 RID: 7698
		public const string Lighting = "Lighting";

		// Token: 0x04001E13 RID: 7699
		public const string Mods = "Mods";

		// Token: 0x04001E14 RID: 7700
		public const string More = "More debug actions";

		// Token: 0x04001E15 RID: 7701
		public const string Anomaly = "Anomaly";

		// Token: 0x04001E16 RID: 7702
		public const string Generation = "Generation";

		// Token: 0x04001E17 RID: 7703
		public const string Pathing = "Pathing";

		// Token: 0x04001E18 RID: 7704
		public const string Humanlike = "Humanlike";

		// Token: 0x04001E19 RID: 7705
		public const string Animal = "Animal";

		// Token: 0x04001E1A RID: 7706
		public const string Insect = "Insect";

		// Token: 0x04001E1B RID: 7707
		public const string Mechanoid = "Mechanoid";

		// Token: 0x04001E1C RID: 7708
		public const string Other = "Other";

		// Token: 0x04001E1D RID: 7709
		public static readonly Dictionary<string, int> categoryOrders = new Dictionary<string, int>();
	}
}
