using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A7B RID: 6779
	[StaticConstructorOnStartup]
	public class Building_Storage : Building, ISlotGroupParent, IStoreSettingsParent, IHaulDestination, IStorageGroupMember, IHaulEnroute, ILoadReferenceable
	{
		// Token: 0x0600AAEF RID: 43759 RVA: 0x003C827F File Offset: 0x003C647F
		public Building_Storage()
		{
			this.slotGroup = new SlotGroup(this);
		}

		// Token: 0x17001E3B RID: 7739
		// (get) Token: 0x0600AAF0 RID: 43760 RVA: 0x003C8293 File Offset: 0x003C6493
		// (set) Token: 0x0600AAF1 RID: 43761 RVA: 0x003C829B File Offset: 0x003C649B
		StorageGroup IStorageGroupMember.Group
		{
			get
			{
				return this.storageGroup;
			}
			set
			{
				this.storageGroup = value;
			}
		}

		// Token: 0x17001E3C RID: 7740
		// (get) Token: 0x0600AAF2 RID: 43762 RVA: 0x003C82A4 File Offset: 0x003C64A4
		bool IStorageGroupMember.DrawConnectionOverlay
		{
			get
			{
				return base.Spawned;
			}
		}

		// Token: 0x17001E3D RID: 7741
		// (get) Token: 0x0600AAF3 RID: 43763 RVA: 0x003C82AC File Offset: 0x003C64AC
		Map IStorageGroupMember.Map
		{
			get
			{
				return base.MapHeld;
			}
		}

		// Token: 0x17001E3E RID: 7742
		// (get) Token: 0x0600AAF4 RID: 43764 RVA: 0x003C82B4 File Offset: 0x003C64B4
		string IStorageGroupMember.StorageGroupTag
		{
			get
			{
				return this.def.building.storageGroupTag;
			}
		}

		// Token: 0x17001E3F RID: 7743
		// (get) Token: 0x0600AAF5 RID: 43765 RVA: 0x003C82C6 File Offset: 0x003C64C6
		StorageSettings IStorageGroupMember.StoreSettings
		{
			get
			{
				return this.GetStoreSettings();
			}
		}

		// Token: 0x17001E40 RID: 7744
		// (get) Token: 0x0600AAF6 RID: 43766 RVA: 0x003C82CE File Offset: 0x003C64CE
		StorageSettings IStorageGroupMember.ParentStoreSettings
		{
			get
			{
				return this.GetParentStoreSettings();
			}
		}

		// Token: 0x17001E41 RID: 7745
		// (get) Token: 0x0600AAF7 RID: 43767 RVA: 0x003C82D6 File Offset: 0x003C64D6
		StorageSettings IStorageGroupMember.ThingStoreSettings
		{
			get
			{
				return this.settings;
			}
		}

		// Token: 0x17001E42 RID: 7746
		// (get) Token: 0x0600AAF8 RID: 43768 RVA: 0x003C82DE File Offset: 0x003C64DE
		bool IStorageGroupMember.DrawStorageTab
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001E43 RID: 7747
		// (get) Token: 0x0600AAF9 RID: 43769 RVA: 0x003C82E1 File Offset: 0x003C64E1
		bool IStorageGroupMember.ShowRenameButton
		{
			get
			{
				return base.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x17001E44 RID: 7748
		// (get) Token: 0x0600AAFA RID: 43770 RVA: 0x003C82F0 File Offset: 0x003C64F0
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001E45 RID: 7749
		// (get) Token: 0x0600AAFB RID: 43771 RVA: 0x003C82F3 File Offset: 0x003C64F3
		public bool IgnoreStoredThingsBeauty
		{
			get
			{
				return this.def.building.ignoreStoredThingsBeauty;
			}
		}

		// Token: 0x0600AAFC RID: 43772 RVA: 0x003C8305 File Offset: 0x003C6505
		public SlotGroup GetSlotGroup()
		{
			return this.slotGroup;
		}

		// Token: 0x0600AAFD RID: 43773 RVA: 0x003C830D File Offset: 0x003C650D
		public virtual void Notify_ReceivedThing(Thing newItem)
		{
			if (base.Faction == Faction.OfPlayer && newItem.def.storedConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(newItem.def.storedConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
		}

		// Token: 0x0600AAFE RID: 43774 RVA: 0x003C833A File Offset: 0x003C653A
		public virtual void Notify_LostThing(Thing newItem)
		{
		}

		// Token: 0x0600AAFF RID: 43775 RVA: 0x003C833C File Offset: 0x003C653C
		public virtual IEnumerable<IntVec3> AllSlotCells()
		{
			Building_Storage.<AllSlotCells>d__32 <AllSlotCells>d__ = new Building_Storage.<AllSlotCells>d__32(-2);
			<AllSlotCells>d__.<>4__this = this;
			return <AllSlotCells>d__;
		}

		// Token: 0x0600AB00 RID: 43776 RVA: 0x003C834C File Offset: 0x003C654C
		public List<IntVec3> AllSlotCellsList()
		{
			List<IntVec3> result;
			if ((result = this.cachedOccupiedCells) == null)
			{
				result = (this.cachedOccupiedCells = this.AllSlotCells().ToList<IntVec3>());
			}
			return result;
		}

		// Token: 0x0600AB01 RID: 43777 RVA: 0x003C8377 File Offset: 0x003C6577
		public StorageSettings GetStoreSettings()
		{
			if (this.storageGroup != null)
			{
				return this.storageGroup.GetStoreSettings();
			}
			return this.settings;
		}

		// Token: 0x0600AB02 RID: 43778 RVA: 0x003C8394 File Offset: 0x003C6594
		public StorageSettings GetParentStoreSettings()
		{
			StorageSettings fixedStorageSettings = this.def.building.fixedStorageSettings;
			if (fixedStorageSettings != null)
			{
				return fixedStorageSettings;
			}
			return StorageSettings.EverStorableFixedSettings();
		}

		// Token: 0x0600AB03 RID: 43779 RVA: 0x003C83BC File Offset: 0x003C65BC
		public void Notify_SettingsChanged()
		{
			if (base.Spawned && this.slotGroup != null)
			{
				base.Map.listerHaulables.Notify_SlotGroupChanged(this.slotGroup);
			}
		}

		// Token: 0x0600AB04 RID: 43780 RVA: 0x003C83E4 File Offset: 0x003C65E4
		public string SlotYielderLabel()
		{
			return this.LabelCap;
		}

		// Token: 0x17001E46 RID: 7750
		// (get) Token: 0x0600AB05 RID: 43781 RVA: 0x003C83EC File Offset: 0x003C65EC
		public string GroupingLabel
		{
			get
			{
				return this.def.building.groupingLabel;
			}
		}

		// Token: 0x17001E47 RID: 7751
		// (get) Token: 0x0600AB06 RID: 43782 RVA: 0x003C83FE File Offset: 0x003C65FE
		public int GroupingOrder
		{
			get
			{
				return this.def.building.groupingOrder;
			}
		}

		// Token: 0x17001E48 RID: 7752
		// (get) Token: 0x0600AB07 RID: 43783 RVA: 0x003C8410 File Offset: 0x003C6610
		public bool HaulDestinationEnabled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600AB08 RID: 43784 RVA: 0x003C8413 File Offset: 0x003C6613
		public bool Accepts(Thing t)
		{
			return this.GetStoreSettings().AllowedToAccept(t);
		}

		// Token: 0x0600AB09 RID: 43785 RVA: 0x003C8424 File Offset: 0x003C6624
		public int SpaceRemainingFor(ThingDef _)
		{
			return this.slotGroup.HeldThingsCount - this.def.building.maxItemsInCell * this.def.Size.Area;
		}

		// Token: 0x0600AB0A RID: 43786 RVA: 0x003C8464 File Offset: 0x003C6664
		public override void PostMake()
		{
			base.PostMake();
			this.settings = new StorageSettings(this);
			if (this.def.building.defaultStorageSettings != null)
			{
				this.settings.CopyFrom(this.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x0600AB0B RID: 43787 RVA: 0x003C84B0 File Offset: 0x003C66B0
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			this.cachedOccupiedCells = null;
			base.SpawnSetup(map, respawningAfterLoad);
			if (this.storageGroup != null && map != this.storageGroup.Map)
			{
				StorageSettings storeSettings = this.storageGroup.GetStoreSettings();
				this.storageGroup.RemoveMember(this, true);
				this.storageGroup = null;
				this.settings.CopyFrom(storeSettings);
			}
		}

		// Token: 0x0600AB0C RID: 43788 RVA: 0x003C850E File Offset: 0x003C670E
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			this.cachedOccupiedCells = null;
		}

		// Token: 0x0600AB0D RID: 43789 RVA: 0x003C851E File Offset: 0x003C671E
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			if (this.storageGroup != null)
			{
				StorageGroup storageGroup = this.storageGroup;
				if (storageGroup != null)
				{
					storageGroup.RemoveMember(this, true);
				}
				this.storageGroup = null;
			}
			BillUtility.Notify_ISlotGroupRemoved(this.slotGroup);
		}

		// Token: 0x0600AB0E RID: 43790 RVA: 0x003C8554 File Offset: 0x003C6754
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
			Scribe_References.Look<StorageGroup>(ref this.storageGroup, "storageGroup", false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
		}

		// Token: 0x0600AB0F RID: 43791 RVA: 0x003C85A4 File Offset: 0x003C67A4
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			StorageGroupUtility.DrawSelectionOverlaysFor(this);
		}

		// Token: 0x0600AB10 RID: 43792 RVA: 0x003C85B4 File Offset: 0x003C67B4
		public override string GetInspectString()
		{
			Building_Storage.sb.Clear();
			Building_Storage.sb.Append(base.GetInspectString());
			if (base.Spawned)
			{
				if (this.storageGroup != null)
				{
					Building_Storage.sb.AppendLineIfNotEmpty();
					StringBuilder stringBuilder = Building_Storage.sb;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("StorageGroupLabel".Translate());
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted(this.storageGroup.RenamableLabel.CapitalizeFirst());
					defaultInterpolatedStringHandler.AppendLiteral(" ");
					stringBuilder.Append(defaultInterpolatedStringHandler.ToStringAndClear());
					if (this.storageGroup.MemberCount > 1)
					{
						StringBuilder stringBuilder2 = Building_Storage.sb;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(2, 1);
						defaultInterpolatedStringHandler2.AppendLiteral("(");
						defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("NumBuildings".Translate(this.storageGroup.MemberCount));
						defaultInterpolatedStringHandler2.AppendLiteral(")");
						stringBuilder2.Append(defaultInterpolatedStringHandler2.ToStringAndClear());
					}
					else
					{
						StringBuilder stringBuilder3 = Building_Storage.sb;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(2, 1);
						defaultInterpolatedStringHandler3.AppendLiteral("(");
						defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>("OneBuilding".Translate());
						defaultInterpolatedStringHandler3.AppendLiteral(")");
						stringBuilder3.Append(defaultInterpolatedStringHandler3.ToStringAndClear());
					}
				}
				if (this.slotGroup.HeldThings.Any<Thing>())
				{
					Building_Storage.sb.AppendLineIfNotEmpty();
					Building_Storage.sb.Append("StoresThings".Translate());
					Building_Storage.sb.Append(": ");
					Building_Storage.sb.Append((from x in this.slotGroup.HeldThings
					select x.LabelShortCap).Distinct<string>().ToCommaList(false, false));
					Building_Storage.sb.Append(".");
				}
			}
			return Building_Storage.sb.ToString();
		}

		// Token: 0x0600AB11 RID: 43793 RVA: 0x003C87AB File Offset: 0x003C69AB
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_Storage.<GetGizmos>d__54 <GetGizmos>d__ = new Building_Storage.<GetGizmos>d__54(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x040063BD RID: 25533
		public StorageSettings settings;

		// Token: 0x040063BE RID: 25534
		public StorageGroup storageGroup;

		// Token: 0x040063BF RID: 25535
		public string label;

		// Token: 0x040063C0 RID: 25536
		public SlotGroup slotGroup;

		// Token: 0x040063C1 RID: 25537
		private List<IntVec3> cachedOccupiedCells;

		// Token: 0x040063C2 RID: 25538
		private static StringBuilder sb = new StringBuilder();
	}
}
