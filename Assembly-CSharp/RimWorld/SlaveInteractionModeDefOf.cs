using System;

namespace RimWorld
{
	// Token: 0x02001EB5 RID: 7861
	[DefOf]
	public static class SlaveInteractionModeDefOf
	{
		// Token: 0x0600CA70 RID: 51824 RVA: 0x004A6032 File Offset: 0x004A4232
		static SlaveInteractionModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SlaveInteractionModeDefOf));
		}

		// Token: 0x04007ECC RID: 32460
		[MayRequireIdeology]
		public static SlaveInteractionModeDef NoInteraction;

		// Token: 0x04007ECD RID: 32461
		[MayRequireIdeology]
		public static SlaveInteractionModeDef Imprison;

		// Token: 0x04007ECE RID: 32462
		[MayRequireIdeology]
		public static SlaveInteractionModeDef Suppress;

		// Token: 0x04007ECF RID: 32463
		[MayRequireIdeology]
		public static SlaveInteractionModeDef Emancipate;

		// Token: 0x04007ED0 RID: 32464
		[MayRequireIdeology]
		public static SlaveInteractionModeDef Execute;
	}
}
