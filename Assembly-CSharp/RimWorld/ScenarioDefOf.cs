using System;

namespace RimWorld
{
	// Token: 0x02001E8F RID: 7823
	[DefOf]
	public static class ScenarioDefOf
	{
		// Token: 0x0600CA4A RID: 51786 RVA: 0x004A5DAC File Offset: 0x004A3FAC
		static ScenarioDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ScenarioDefOf));
		}

		// Token: 0x04007ABF RID: 31423
		public static ScenarioDef Crashlanded;

		// Token: 0x04007AC0 RID: 31424
		public static ScenarioDef Tutorial;
	}
}
