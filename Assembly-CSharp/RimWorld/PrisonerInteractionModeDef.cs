using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FCB RID: 4043
	public class PrisonerInteractionModeDef : Def
	{
		// Token: 0x04003F83 RID: 16259
		public int listOrder;

		// Token: 0x04003F84 RID: 16260
		public bool allowOnWildMan = true;

		// Token: 0x04003F85 RID: 16261
		public bool hideIfNoBloodfeeders;

		// Token: 0x04003F86 RID: 16262
		public bool hideOnHemogenicPawns;

		// Token: 0x04003F87 RID: 16263
		public bool mustBeAwake = true;

		// Token: 0x04003F88 RID: 16264
		public bool allowInClassicIdeoMode = true;

		// Token: 0x04003F89 RID: 16265
		public bool hideIfNotRecruitable;

		// Token: 0x04003F8A RID: 16266
		public bool hideIfNotStudiableAsPrisoner;

		// Token: 0x04003F8B RID: 16267
		public bool hideIfGrayFleshNotAppeared;

		// Token: 0x04003F8C RID: 16268
		public bool isNonExclusiveInteraction;

		// Token: 0x04003F8D RID: 16269
		public bool isChildInteraction;
	}
}
