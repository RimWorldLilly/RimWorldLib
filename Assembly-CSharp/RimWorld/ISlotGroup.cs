using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A86 RID: 6790
	public interface ISlotGroup
	{
		// Token: 0x17001E67 RID: 7783
		// (get) Token: 0x0600AB4E RID: 43854
		StorageSettings Settings { get; }

		// Token: 0x17001E68 RID: 7784
		// (get) Token: 0x0600AB4F RID: 43855
		IEnumerable<Thing> HeldThings { get; }

		// Token: 0x17001E69 RID: 7785
		// (get) Token: 0x0600AB50 RID: 43856
		StorageGroup StorageGroup { get; }

		// Token: 0x17001E6A RID: 7786
		// (get) Token: 0x0600AB51 RID: 43857
		List<IntVec3> CellsList { get; }

		// Token: 0x17001E6B RID: 7787
		// (get) Token: 0x0600AB52 RID: 43858
		string GroupingLabel { get; }

		// Token: 0x17001E6C RID: 7788
		// (get) Token: 0x0600AB53 RID: 43859
		int GroupingOrder { get; }
	}
}
