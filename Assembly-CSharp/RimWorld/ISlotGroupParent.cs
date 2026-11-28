using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A85 RID: 6789
	public interface ISlotGroupParent : IStoreSettingsParent, IHaulDestination
	{
		// Token: 0x17001E64 RID: 7780
		// (get) Token: 0x0600AB45 RID: 43845
		bool IgnoreStoredThingsBeauty { get; }

		// Token: 0x0600AB46 RID: 43846
		IEnumerable<IntVec3> AllSlotCells();

		// Token: 0x0600AB47 RID: 43847
		List<IntVec3> AllSlotCellsList();

		// Token: 0x0600AB48 RID: 43848
		void Notify_ReceivedThing(Thing newItem);

		// Token: 0x0600AB49 RID: 43849
		void Notify_LostThing(Thing newItem);

		// Token: 0x0600AB4A RID: 43850
		string SlotYielderLabel();

		// Token: 0x17001E65 RID: 7781
		// (get) Token: 0x0600AB4B RID: 43851
		string GroupingLabel { get; }

		// Token: 0x17001E66 RID: 7782
		// (get) Token: 0x0600AB4C RID: 43852
		int GroupingOrder { get; }

		// Token: 0x0600AB4D RID: 43853
		SlotGroup GetSlotGroup();
	}
}
