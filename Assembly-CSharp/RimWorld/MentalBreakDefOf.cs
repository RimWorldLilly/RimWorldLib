using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E86 RID: 7814
	[DefOf]
	public static class MentalBreakDefOf
	{
		// Token: 0x0600CA41 RID: 51777 RVA: 0x004A5D13 File Offset: 0x004A3F13
		static MentalBreakDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MentalBreakDefOf));
		}

		// Token: 0x04007A3F RID: 31295
		public static MentalBreakDef Berserk;

		// Token: 0x04007A40 RID: 31296
		public static MentalBreakDef CorpseObsession;

		// Token: 0x04007A41 RID: 31297
		public static MentalBreakDef Catatonic;

		// Token: 0x04007A42 RID: 31298
		[MayRequireAnomaly]
		public static MentalBreakDef BerserkShort;

		// Token: 0x04007A43 RID: 31299
		[MayRequireAnomaly]
		public static MentalBreakDef CubeSculpting;

		// Token: 0x04007A44 RID: 31300
		[MayRequireAnomaly]
		public static MentalBreakDef HumanityBreak;
	}
}
