using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099C RID: 2460
	public interface IBillGiver
	{
		// Token: 0x17000D98 RID: 3480
		// (get) Token: 0x06004A50 RID: 19024
		Map Map { get; }

		// Token: 0x17000D99 RID: 3481
		// (get) Token: 0x06004A51 RID: 19025
		BillStack BillStack { get; }

		// Token: 0x17000D9A RID: 3482
		// (get) Token: 0x06004A52 RID: 19026
		IEnumerable<IntVec3> IngredientStackCells { get; }

		// Token: 0x17000D9B RID: 3483
		// (get) Token: 0x06004A53 RID: 19027
		string LabelShort { get; }

		// Token: 0x06004A54 RID: 19028
		bool CurrentlyUsableForBills();

		// Token: 0x06004A55 RID: 19029
		bool UsableForBillsAfterFueling();

		// Token: 0x06004A56 RID: 19030
		void Notify_BillDeleted(Bill bill);
	}
}
