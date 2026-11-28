using System;

namespace RimWorld
{
	// Token: 0x02001EE4 RID: 7908
	[DefOf]
	public static class ExpansionDefOf
	{
		// Token: 0x0600CA9E RID: 51870 RVA: 0x004A6340 File Offset: 0x004A4540
		static ExpansionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ExpansionDefOf));
		}

		// Token: 0x0400809C RID: 32924
		public static ExpansionDef Core;

		// Token: 0x0400809D RID: 32925
		public static ExpansionDef Anomaly;
	}
}
