using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200102B RID: 4139
	public interface IArchivable : IExposable, ILoadReferenceable
	{
		// Token: 0x170011E5 RID: 4581
		// (get) Token: 0x0600684A RID: 26698
		Texture ArchivedIcon { get; }

		// Token: 0x170011E6 RID: 4582
		// (get) Token: 0x0600684B RID: 26699
		Color ArchivedIconColor { get; }

		// Token: 0x170011E7 RID: 4583
		// (get) Token: 0x0600684C RID: 26700
		string ArchivedLabel { get; }

		// Token: 0x170011E8 RID: 4584
		// (get) Token: 0x0600684D RID: 26701
		string ArchivedTooltip { get; }

		// Token: 0x170011E9 RID: 4585
		// (get) Token: 0x0600684E RID: 26702
		int CreatedTicksGame { get; }

		// Token: 0x170011EA RID: 4586
		// (get) Token: 0x0600684F RID: 26703
		bool CanCullArchivedNow { get; }

		// Token: 0x170011EB RID: 4587
		// (get) Token: 0x06006850 RID: 26704
		LookTargets LookTargets { get; }

		// Token: 0x06006851 RID: 26705
		void OpenArchived();
	}
}
