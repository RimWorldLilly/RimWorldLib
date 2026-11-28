using System;

namespace RimWorld
{
	// Token: 0x02001EC0 RID: 7872
	[DefOf]
	public static class IncidentTargetTagDefOf
	{
		// Token: 0x0600CA7B RID: 51835 RVA: 0x004A60ED File Offset: 0x004A42ED
		static IncidentTargetTagDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(IncidentTargetTagDefOf));
		}

		// Token: 0x04007F0A RID: 32522
		public static IncidentTargetTagDef World;

		// Token: 0x04007F0B RID: 32523
		public static IncidentTargetTagDef Map_RaidBeacon;

		// Token: 0x04007F0C RID: 32524
		public static IncidentTargetTagDef Map_PlayerHome;

		// Token: 0x04007F0D RID: 32525
		public static IncidentTargetTagDef Map_Misc;
	}
}
