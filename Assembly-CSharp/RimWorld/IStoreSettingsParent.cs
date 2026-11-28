using System;

namespace RimWorld
{
	// Token: 0x02001A84 RID: 6788
	public interface IStoreSettingsParent
	{
		// Token: 0x17001E63 RID: 7779
		// (get) Token: 0x0600AB41 RID: 43841
		bool StorageTabVisible { get; }

		// Token: 0x0600AB42 RID: 43842
		StorageSettings GetStoreSettings();

		// Token: 0x0600AB43 RID: 43843
		StorageSettings GetParentStoreSettings();

		// Token: 0x0600AB44 RID: 43844
		void Notify_SettingsChanged();
	}
}
