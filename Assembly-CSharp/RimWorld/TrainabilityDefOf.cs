using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EBA RID: 7866
	[DefOf]
	public static class TrainabilityDefOf
	{
		// Token: 0x0600CA75 RID: 51829 RVA: 0x004A6087 File Offset: 0x004A4287
		static TrainabilityDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TrainabilityDefOf));
		}

		// Token: 0x04007EDB RID: 32475
		public static TrainabilityDef None;

		// Token: 0x04007EDC RID: 32476
		public static TrainabilityDef Intermediate;

		// Token: 0x04007EDD RID: 32477
		public static TrainabilityDef Advanced;
	}
}
