using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E8A RID: 7818
	[DefOf]
	public static class BodyPartDefOf
	{
		// Token: 0x0600CA45 RID: 51781 RVA: 0x004A5D57 File Offset: 0x004A3F57
		static BodyPartDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyPartDefOf));
		}

		// Token: 0x04007A98 RID: 31384
		public static BodyPartDef Leg;

		// Token: 0x04007A99 RID: 31385
		public static BodyPartDef Eye;

		// Token: 0x04007A9A RID: 31386
		public static BodyPartDef Shoulder;

		// Token: 0x04007A9B RID: 31387
		public static BodyPartDef Arm;

		// Token: 0x04007A9C RID: 31388
		public static BodyPartDef Hand;

		// Token: 0x04007A9D RID: 31389
		public static BodyPartDef Head;

		// Token: 0x04007A9E RID: 31390
		public static BodyPartDef Lung;

		// Token: 0x04007A9F RID: 31391
		public static BodyPartDef Torso;

		// Token: 0x04007AA0 RID: 31392
		public static BodyPartDef Heart;

		// Token: 0x04007AA1 RID: 31393
		public static BodyPartDef Neck;
	}
}
