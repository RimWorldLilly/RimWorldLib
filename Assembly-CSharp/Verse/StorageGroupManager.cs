using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000314 RID: 788
	public class StorageGroupManager : IExposable
	{
		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06001828 RID: 6184 RVA: 0x00090F57 File Offset: 0x0008F157
		public List<StorageGroup> StorageGroupsForReading
		{
			get
			{
				return this.groups;
			}
		}

		// Token: 0x06001829 RID: 6185 RVA: 0x00090F5F File Offset: 0x0008F15F
		public StorageGroupManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600182A RID: 6186 RVA: 0x00090F7C File Offset: 0x0008F17C
		public StorageGroup NewGroup(string label = null)
		{
			StorageGroup storageGroup = new StorageGroup(this.map, label);
			storageGroup.loadID = Find.UniqueIDsManager.GetNextStorageGroupID();
			this.groups.Add(storageGroup);
			return storageGroup;
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x00090FB4 File Offset: 0x0008F1B4
		public void Notify_MemberRemoved(StorageGroup group)
		{
			if (group.MemberCount <= 1)
			{
				for (int i = group.MemberCount - 1; i >= 0; i--)
				{
					group.members[i].SetStorageGroup(null, true);
				}
				this.groups.Remove(group);
			}
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x00091000 File Offset: 0x0008F200
		public static string NewStorageName(string nameBase)
		{
			for (int i = 1; i <= 1000; i++)
			{
				string cand = nameBase + " " + i.ToString();
				if (!Current.Game.CurrentMap.zoneManager.AllZones.Any((Zone z) => z.label == cand) && !Current.Game.CurrentMap.haulDestinationManager.AllGroups.Any(delegate(SlotGroup x)
				{
					Building_Storage building_Storage = x.parent as Building_Storage;
					return building_Storage != null && building_Storage.label == cand;
				}) && !Current.Game.CurrentMap.storageGroups.groups.Any((StorageGroup g) => g.RenamableLabel == cand))
				{
					return cand;
				}
			}
			Log.Error("Ran out of storage " + nameBase + " names.");
			return nameBase + " X";
		}

		// Token: 0x0600182D RID: 6189 RVA: 0x000910E0 File Offset: 0x0008F2E0
		public bool HasGroupWithName(string label)
		{
			return this.groups.Any((StorageGroup x) => x.RenamableLabel == label);
		}

		// Token: 0x0600182E RID: 6190 RVA: 0x00091111 File Offset: 0x0008F311
		public bool HasStorageGroup(StorageGroup group)
		{
			return this.groups.Contains(group);
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x0009111F File Offset: 0x0008F31F
		public void ExposeData()
		{
			Scribe_Collections.Look<StorageGroup>(ref this.groups, "groups", LookMode.Deep, Array.Empty<object>());
			Scribe_References.Look<Map>(ref this.map, "map", false);
		}

		// Token: 0x04001438 RID: 5176
		public Map map;

		// Token: 0x04001439 RID: 5177
		private List<StorageGroup> groups = new List<StorageGroup>();
	}
}
