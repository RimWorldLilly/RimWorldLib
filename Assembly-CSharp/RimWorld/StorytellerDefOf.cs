using System;

namespace RimWorld
{
	// Token: 0x02001E8E RID: 7822
	[DefOf]
	public static class StorytellerDefOf
	{
		// Token: 0x0600CA49 RID: 51785 RVA: 0x004A5D9B File Offset: 0x004A3F9B
		static StorytellerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(StorytellerDefOf));
		}

		// Token: 0x04007ABD RID: 31421
		public static StorytellerDef Cassandra;

		// Token: 0x04007ABE RID: 31422
		public static StorytellerDef Tutor;
	}
}
