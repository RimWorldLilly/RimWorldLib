using System;

namespace RimWorld
{
	// Token: 0x02001EC9 RID: 7881
	[DefOf]
	public static class IncidentCategoryDefOf
	{
		// Token: 0x0600CA84 RID: 51844 RVA: 0x004A6186 File Offset: 0x004A4386
		static IncidentCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(IncidentCategoryDefOf));
		}

		// Token: 0x04007F4D RID: 32589
		public static IncidentCategoryDef Misc;

		// Token: 0x04007F4E RID: 32590
		public static IncidentCategoryDef ThreatSmall;

		// Token: 0x04007F4F RID: 32591
		public static IncidentCategoryDef ThreatBig;

		// Token: 0x04007F50 RID: 32592
		public static IncidentCategoryDef DiseaseHuman;

		// Token: 0x04007F51 RID: 32593
		public static IncidentCategoryDef GiveQuest;

		// Token: 0x04007F52 RID: 32594
		public static IncidentCategoryDef DeepDrillInfestation;

		// Token: 0x04007F53 RID: 32595
		public static IncidentCategoryDef Special;
	}
}
