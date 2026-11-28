using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E89 RID: 7817
	[DefOf]
	public static class BodyPartGroupDefOf
	{
		// Token: 0x0600CA44 RID: 51780 RVA: 0x004A5D46 File Offset: 0x004A3F46
		static BodyPartGroupDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyPartGroupDefOf));
		}

		// Token: 0x04007A8F RID: 31375
		public static BodyPartGroupDef Torso;

		// Token: 0x04007A90 RID: 31376
		public static BodyPartGroupDef Legs;

		// Token: 0x04007A91 RID: 31377
		public static BodyPartGroupDef LeftHand;

		// Token: 0x04007A92 RID: 31378
		public static BodyPartGroupDef RightHand;

		// Token: 0x04007A93 RID: 31379
		public static BodyPartGroupDef FullHead;

		// Token: 0x04007A94 RID: 31380
		public static BodyPartGroupDef UpperHead;

		// Token: 0x04007A95 RID: 31381
		public static BodyPartGroupDef Eyes;

		// Token: 0x04007A96 RID: 31382
		public static BodyPartGroupDef LeftBlade;

		// Token: 0x04007A97 RID: 31383
		public static BodyPartGroupDef FrontLeftPaw;
	}
}
