using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EC2 RID: 7874
	[DefOf]
	public static class ImplementOwnerTypeDefOf
	{
		// Token: 0x0600CA7D RID: 51837 RVA: 0x004A610F File Offset: 0x004A430F
		static ImplementOwnerTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ImplementOwnerTypeDefOf));
		}

		// Token: 0x04007F12 RID: 32530
		public static ImplementOwnerTypeDef Weapon;

		// Token: 0x04007F13 RID: 32531
		public static ImplementOwnerTypeDef Bodypart;

		// Token: 0x04007F14 RID: 32532
		public static ImplementOwnerTypeDef Hediff;

		// Token: 0x04007F15 RID: 32533
		public static ImplementOwnerTypeDef Terrain;

		// Token: 0x04007F16 RID: 32534
		public static ImplementOwnerTypeDef NativeVerb;
	}
}
