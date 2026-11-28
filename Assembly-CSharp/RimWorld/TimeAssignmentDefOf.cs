using System;

namespace RimWorld
{
	// Token: 0x02001E92 RID: 7826
	[DefOf]
	public static class TimeAssignmentDefOf
	{
		// Token: 0x0600CA4D RID: 51789 RVA: 0x004A5DDF File Offset: 0x004A3FDF
		static TimeAssignmentDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TimeAssignmentDefOf));
		}

		// Token: 0x04007AC6 RID: 31430
		public static TimeAssignmentDef Anything;

		// Token: 0x04007AC7 RID: 31431
		public static TimeAssignmentDef Work;

		// Token: 0x04007AC8 RID: 31432
		public static TimeAssignmentDef Joy;

		// Token: 0x04007AC9 RID: 31433
		public static TimeAssignmentDef Sleep;

		// Token: 0x04007ACA RID: 31434
		[MayRequireRoyalty]
		public static TimeAssignmentDef Meditate;
	}
}
