using System;

namespace RimWorld
{
	// Token: 0x02001EB7 RID: 7863
	[DefOf]
	public static class BillStoreModeDefOf
	{
		// Token: 0x0600CA72 RID: 51826 RVA: 0x004A6054 File Offset: 0x004A4254
		static BillStoreModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BillStoreModeDefOf));
		}

		// Token: 0x04007ED4 RID: 32468
		public static BillStoreModeDef DropOnFloor;

		// Token: 0x04007ED5 RID: 32469
		public static BillStoreModeDef BestStockpile;

		// Token: 0x04007ED6 RID: 32470
		public static BillStoreModeDef SpecificStockpile;
	}
}
