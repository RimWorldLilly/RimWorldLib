using System;

namespace RimWorld
{
	// Token: 0x02001ED6 RID: 7894
	[DefOf]
	public static class MeditationFocusDefOf
	{
		// Token: 0x0600CA91 RID: 51857 RVA: 0x004A6263 File Offset: 0x004A4463
		static MeditationFocusDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MeditationFocusDefOf));
		}

		// Token: 0x04007F9D RID: 32669
		[MayRequireRoyalty]
		public static MeditationFocusDef Natural;
	}
}
