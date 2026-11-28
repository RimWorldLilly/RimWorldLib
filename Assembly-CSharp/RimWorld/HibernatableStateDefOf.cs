using System;

namespace RimWorld
{
	// Token: 0x02001EBE RID: 7870
	[DefOf]
	public static class HibernatableStateDefOf
	{
		// Token: 0x0600CA79 RID: 51833 RVA: 0x004A60CB File Offset: 0x004A42CB
		static HibernatableStateDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(HibernatableStateDefOf));
		}

		// Token: 0x04007F05 RID: 32517
		public static HibernatableStateDef Running;

		// Token: 0x04007F06 RID: 32518
		public static HibernatableStateDef Starting;

		// Token: 0x04007F07 RID: 32519
		public static HibernatableStateDef Hibernating;
	}
}
