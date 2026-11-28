using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A88 RID: 6792
	public class StorageGroup : IExposable, ILoadReferenceable, IStoreSettingsParent, ISlotGroup, IRenameable
	{
		// Token: 0x17001E75 RID: 7797
		// (get) Token: 0x0600AB64 RID: 43876 RVA: 0x003C8FB1 File Offset: 0x003C71B1
		// (set) Token: 0x0600AB65 RID: 43877 RVA: 0x003C8FC3 File Offset: 0x003C71C3
		public string RenamableLabel
		{
			get
			{
				return this.label ?? this.BaseLabel;
			}
			set
			{
				this.label = value;
			}
		}

		// Token: 0x17001E76 RID: 7798
		// (get) Token: 0x0600AB66 RID: 43878 RVA: 0x003C8FCC File Offset: 0x003C71CC
		public string BaseLabel
		{
			get
			{
				return "StorageGroup".Translate();
			}
		}

		// Token: 0x17001E77 RID: 7799
		// (get) Token: 0x0600AB67 RID: 43879 RVA: 0x003C8FDD File Offset: 0x003C71DD
		public string GroupingLabel
		{
			get
			{
				return "GroupPlural".Translate();
			}
		}

		// Token: 0x17001E78 RID: 7800
		// (get) Token: 0x0600AB68 RID: 43880 RVA: 0x003C8FEE File Offset: 0x003C71EE
		public int GroupingOrder
		{
			get
			{
				return -100;
			}
		}

		// Token: 0x17001E79 RID: 7801
		// (get) Token: 0x0600AB69 RID: 43881 RVA: 0x003C8FF2 File Offset: 0x003C71F2
		public string InspectLabel
		{
			get
			{
				return this.RenamableLabel;
			}
		}

		// Token: 0x17001E7A RID: 7802
		// (get) Token: 0x0600AB6A RID: 43882 RVA: 0x003C8FFA File Offset: 0x003C71FA
		public Map Map
		{
			get
			{
				return this.map;
			}
		}

		// Token: 0x17001E7B RID: 7803
		// (get) Token: 0x0600AB6B RID: 43883 RVA: 0x003C9002 File Offset: 0x003C7202
		public int MemberCount
		{
			get
			{
				return this.members.Count;
			}
		}

		// Token: 0x17001E7C RID: 7804
		// (get) Token: 0x0600AB6C RID: 43884 RVA: 0x003C900F File Offset: 0x003C720F
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600AB6D RID: 43885 RVA: 0x003C9012 File Offset: 0x003C7212
		public StorageGroup()
		{
		}

		// Token: 0x0600AB6E RID: 43886 RVA: 0x003C9025 File Offset: 0x003C7225
		public StorageGroup(Map map, string label = null)
		{
			this.map = map;
			this.settings = new StorageSettings(this);
			this.RenamableLabel = (label ?? StorageGroupManager.NewStorageName(this.BaseLabel));
		}

		// Token: 0x17001E7D RID: 7805
		// (get) Token: 0x0600AB6F RID: 43887 RVA: 0x003C9061 File Offset: 0x003C7261
		StorageSettings ISlotGroup.Settings
		{
			get
			{
				return this.GetStoreSettings();
			}
		}

		// Token: 0x17001E7E RID: 7806
		// (get) Token: 0x0600AB70 RID: 43888 RVA: 0x003C9069 File Offset: 0x003C7269
		StorageGroup ISlotGroup.StorageGroup
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17001E7F RID: 7807
		// (get) Token: 0x0600AB71 RID: 43889 RVA: 0x003C906C File Offset: 0x003C726C
		public IEnumerable<Thing> HeldThings
		{
			get
			{
				StorageGroup.<get_HeldThings>d__29 <get_HeldThings>d__ = new StorageGroup.<get_HeldThings>d__29(-2);
				<get_HeldThings>d__.<>4__this = this;
				return <get_HeldThings>d__;
			}
		}

		// Token: 0x17001E80 RID: 7808
		// (get) Token: 0x0600AB72 RID: 43890 RVA: 0x003C907C File Offset: 0x003C727C
		public List<IntVec3> CellsList
		{
			get
			{
				StorageGroup.tmpCellsList.Clear();
				foreach (IStorageGroupMember storageGroupMember in this.members)
				{
					ISlotGroupParent slotGroupParent = storageGroupMember as ISlotGroupParent;
					if (slotGroupParent != null)
					{
						StorageGroup.tmpCellsList.AddRange(slotGroupParent.GetSlotGroup().CellsList);
					}
				}
				return StorageGroup.tmpCellsList;
			}
		}

		// Token: 0x17001E81 RID: 7809
		// (get) Token: 0x0600AB73 RID: 43891 RVA: 0x003C90F4 File Offset: 0x003C72F4
		public List<IHaulSource> HaulSourcesList
		{
			get
			{
				StorageGroup.tmpHaulSourceList.Clear();
				foreach (IStorageGroupMember storageGroupMember in this.members)
				{
					IHaulSource haulSource = storageGroupMember as IHaulSource;
					if (haulSource != null)
					{
						StorageGroup.tmpHaulSourceList.Add(haulSource);
					}
				}
				return StorageGroup.tmpHaulSourceList;
			}
		}

		// Token: 0x0600AB74 RID: 43892 RVA: 0x003C9164 File Offset: 0x003C7364
		public void InitFrom(IStorageGroupMember member)
		{
			this.settings.CopyFrom(member.StoreSettings);
		}

		// Token: 0x0600AB75 RID: 43893 RVA: 0x003C9178 File Offset: 0x003C7378
		public void RemoveMember(IStorageGroupMember member, bool removeIfEmpty = true)
		{
			if (this.members.Remove(member))
			{
				IStoreSettingsParent storeSettingsParent = member as IStoreSettingsParent;
				if (storeSettingsParent != null)
				{
					storeSettingsParent.Notify_SettingsChanged();
				}
				if (removeIfEmpty)
				{
					this.map.storageGroups.Notify_MemberRemoved(this);
				}
				BillUtility.Notify_ISlotGroupRemoved(this);
			}
		}

		// Token: 0x0600AB76 RID: 43894 RVA: 0x003C91BD File Offset: 0x003C73BD
		public StorageSettings GetStoreSettings()
		{
			return this.settings;
		}

		// Token: 0x0600AB77 RID: 43895 RVA: 0x003C91C5 File Offset: 0x003C73C5
		public StorageSettings GetParentStoreSettings()
		{
			if (this.members.Any<IStorageGroupMember>())
			{
				return this.members[0].ParentStoreSettings;
			}
			return null;
		}

		// Token: 0x0600AB78 RID: 43896 RVA: 0x003C91E8 File Offset: 0x003C73E8
		public void Notify_SettingsChanged()
		{
			foreach (IStorageGroupMember storageGroupMember in this.members)
			{
				ISlotGroupParent slotGroupParent = storageGroupMember as ISlotGroupParent;
				if (slotGroupParent != null)
				{
					slotGroupParent.Notify_SettingsChanged();
				}
			}
		}

		// Token: 0x0600AB79 RID: 43897 RVA: 0x003C9244 File Offset: 0x003C7444
		public string GetUniqueLoadID()
		{
			return "StorageGroup_" + this.loadID.ToString();
		}

		// Token: 0x0600AB7A RID: 43898 RVA: 0x003C925C File Offset: 0x003C745C
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_References.Look<Map>(ref this.map, "map", false);
			Scribe_Collections.Look<IStorageGroupMember>(ref this.members, "members", LookMode.Reference, Array.Empty<object>());
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.members.RemoveAll((IStorageGroupMember x) => x == null);
			}
		}

		// Token: 0x040063C9 RID: 25545
		public int loadID;

		// Token: 0x040063CA RID: 25546
		private Map map;

		// Token: 0x040063CB RID: 25547
		[LoadAlias("buildings")]
		public List<IStorageGroupMember> members = new List<IStorageGroupMember>();

		// Token: 0x040063CC RID: 25548
		private StorageSettings settings;

		// Token: 0x040063CD RID: 25549
		private string label;

		// Token: 0x040063CE RID: 25550
		private static readonly List<IntVec3> tmpCellsList = new List<IntVec3>(128);

		// Token: 0x040063CF RID: 25551
		private static readonly List<IHaulSource> tmpHaulSourceList = new List<IHaulSource>(64);
	}
}
