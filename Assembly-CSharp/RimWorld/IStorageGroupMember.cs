using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A83 RID: 6787
	public interface IStorageGroupMember
	{
		// Token: 0x17001E5A RID: 7770
		// (get) Token: 0x0600AB37 RID: 43831
		// (set) Token: 0x0600AB38 RID: 43832
		StorageGroup Group { get; set; }

		// Token: 0x17001E5B RID: 7771
		// (get) Token: 0x0600AB39 RID: 43833
		Map Map { get; }

		// Token: 0x17001E5C RID: 7772
		// (get) Token: 0x0600AB3A RID: 43834
		StorageSettings StoreSettings { get; }

		// Token: 0x17001E5D RID: 7773
		// (get) Token: 0x0600AB3B RID: 43835
		StorageSettings ParentStoreSettings { get; }

		// Token: 0x17001E5E RID: 7774
		// (get) Token: 0x0600AB3C RID: 43836
		StorageSettings ThingStoreSettings { get; }

		// Token: 0x17001E5F RID: 7775
		// (get) Token: 0x0600AB3D RID: 43837
		string StorageGroupTag { get; }

		// Token: 0x17001E60 RID: 7776
		// (get) Token: 0x0600AB3E RID: 43838
		bool DrawConnectionOverlay { get; }

		// Token: 0x17001E61 RID: 7777
		// (get) Token: 0x0600AB3F RID: 43839
		bool DrawStorageTab { get; }

		// Token: 0x17001E62 RID: 7778
		// (get) Token: 0x0600AB40 RID: 43840
		bool ShowRenameButton { get; }
	}
}
