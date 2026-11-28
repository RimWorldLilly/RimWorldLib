using System;

namespace RimWorld
{
	// Token: 0x02001EDC RID: 7900
	[DefOf]
	public static class IssueDefOf
	{
		// Token: 0x0600CA96 RID: 51862 RVA: 0x004A62B8 File Offset: 0x004A44B8
		static IssueDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(IssueDefOf));
		}

		// Token: 0x0400806D RID: 32877
		[MayRequireIdeology]
		public static IssueDef Charity;

		// Token: 0x0400806E RID: 32878
		[MayRequireIdeology]
		public static IssueDef Ranching;

		// Token: 0x0400806F RID: 32879
		[MayRequireIdeology]
		public static IssueDef Blindness;

		// Token: 0x04008070 RID: 32880
		[MayRequireIdeology]
		public static IssueDef IdeoRitualSeat;
	}
}
