using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E9C RID: 7836
	[DefOf]
	public static class SongDefOf
	{
		// Token: 0x0600CA57 RID: 51799 RVA: 0x004A5E89 File Offset: 0x004A4089
		static SongDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SongDefOf));
		}

		// Token: 0x04007CD6 RID: 31958
		public static SongDef EntrySong;

		// Token: 0x04007CD7 RID: 31959
		public static SongDef EndCreditsSong;

		// Token: 0x04007CD8 RID: 31960
		[MayRequireIdeology]
		public static SongDef ArchonexusVictorySong;

		// Token: 0x04007CD9 RID: 31961
		[MayRequireAnomaly]
		public static SongDef AnomalyCreditsSong;

		// Token: 0x04007CDA RID: 31962
		[MayRequireOdyssey]
		public static SongDef OdysseyCreditsSong;
	}
}
