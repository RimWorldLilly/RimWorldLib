using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EB8 RID: 7864
	[DefOf]
	public static class ReservationLayerDefOf
	{
		// Token: 0x0600CA73 RID: 51827 RVA: 0x004A6065 File Offset: 0x004A4265
		static ReservationLayerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ReservationLayerDefOf));
		}

		// Token: 0x04007ED7 RID: 32471
		public static ReservationLayerDef Floor;

		// Token: 0x04007ED8 RID: 32472
		public static ReservationLayerDef Ceiling;

		// Token: 0x04007ED9 RID: 32473
		public static ReservationLayerDef Empty;
	}
}
