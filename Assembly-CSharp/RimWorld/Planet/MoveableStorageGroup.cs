using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022F8 RID: 8952
	public class MoveableStorageGroup : IExposable
	{
		// Token: 0x0600DD44 RID: 56644 RVA: 0x00526248 File Offset: 0x00524448
		public MoveableStorageGroup()
		{
		}

		// Token: 0x0600DD45 RID: 56645 RVA: 0x0052625B File Offset: 0x0052445B
		public MoveableStorageGroup(StorageGroup storageGroup)
		{
			this.label = storageGroup.RenamableLabel;
			this.settings = new StorageSettings();
			this.settings.CopyFrom(storageGroup.GetStoreSettings());
		}

		// Token: 0x0600DD46 RID: 56646 RVA: 0x00526296 File Offset: 0x00524496
		public void ExposeData()
		{
			Scribe_Collections.Look<IStorageGroupMember>(ref this.members, "members", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", Array.Empty<object>());
		}

		// Token: 0x0600DD47 RID: 56647 RVA: 0x005262D8 File Offset: 0x005244D8
		public void TryCreateStorageGroup(Map map)
		{
			StorageGroup storageGroup = map.storageGroups.NewGroup(this.label);
			storageGroup.GetStoreSettings().CopyFrom(this.settings);
			foreach (IStorageGroupMember member in this.members)
			{
				member.SetStorageGroup(storageGroup, true);
			}
		}

		// Token: 0x04008DD3 RID: 36307
		public List<IStorageGroupMember> members = new List<IStorageGroupMember>();

		// Token: 0x04008DD4 RID: 36308
		private string label;

		// Token: 0x04008DD5 RID: 36309
		private StorageSettings settings;
	}
}
