using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A7C RID: 6780
	public sealed class HaulDestinationManager
	{
		// Token: 0x17001E49 RID: 7753
		// (get) Token: 0x0600AB14 RID: 43796 RVA: 0x003C87CF File Offset: 0x003C69CF
		public IEnumerable<IHaulDestination> AllHaulDestinations
		{
			get
			{
				return this.allHaulDestinationsInOrder;
			}
		}

		// Token: 0x17001E4A RID: 7754
		// (get) Token: 0x0600AB15 RID: 43797 RVA: 0x003C87D7 File Offset: 0x003C69D7
		public List<IHaulDestination> AllHaulDestinationsListForReading
		{
			get
			{
				return this.allHaulDestinationsInOrder;
			}
		}

		// Token: 0x17001E4B RID: 7755
		// (get) Token: 0x0600AB16 RID: 43798 RVA: 0x003C87DF File Offset: 0x003C69DF
		public List<IHaulDestination> AllHaulDestinationsListInPriorityOrder
		{
			get
			{
				return this.allHaulDestinationsInOrder;
			}
		}

		// Token: 0x17001E4C RID: 7756
		// (get) Token: 0x0600AB17 RID: 43799 RVA: 0x003C87E7 File Offset: 0x003C69E7
		public List<IHaulSource> AllHaulSourcesListInPriorityOrder
		{
			get
			{
				return this.allHaulSourcesInOrder;
			}
		}

		// Token: 0x17001E4D RID: 7757
		// (get) Token: 0x0600AB18 RID: 43800 RVA: 0x003C87EF File Offset: 0x003C69EF
		public IEnumerable<SlotGroup> AllGroups
		{
			get
			{
				return this.allGroupsInOrder;
			}
		}

		// Token: 0x17001E4E RID: 7758
		// (get) Token: 0x0600AB19 RID: 43801 RVA: 0x003C87F7 File Offset: 0x003C69F7
		public List<SlotGroup> AllGroupsListForReading
		{
			get
			{
				return this.allGroupsInOrder;
			}
		}

		// Token: 0x17001E4F RID: 7759
		// (get) Token: 0x0600AB1A RID: 43802 RVA: 0x003C87FF File Offset: 0x003C69FF
		public List<IHaulSource> AllHaulSourcesListForReading
		{
			get
			{
				return this.allHaulSourcesInOrder;
			}
		}

		// Token: 0x17001E50 RID: 7760
		// (get) Token: 0x0600AB1B RID: 43803 RVA: 0x003C8807 File Offset: 0x003C6A07
		public List<SlotGroup> AllGroupsListInPriorityOrder
		{
			get
			{
				return this.allGroupsInOrder;
			}
		}

		// Token: 0x17001E51 RID: 7761
		// (get) Token: 0x0600AB1C RID: 43804 RVA: 0x003C880F File Offset: 0x003C6A0F
		public IEnumerable<IntVec3> AllSlots
		{
			get
			{
				HaulDestinationManager.<get_AllSlots>d__22 <get_AllSlots>d__ = new HaulDestinationManager.<get_AllSlots>d__22(-2);
				<get_AllSlots>d__.<>4__this = this;
				return <get_AllSlots>d__;
			}
		}

		// Token: 0x0600AB1D RID: 43805 RVA: 0x003C8820 File Offset: 0x003C6A20
		public HaulDestinationManager(Map map)
		{
			this.map = map;
			this.groupGrid = new SlotGroup[map.Size.x, map.Size.y, map.Size.z];
		}

		// Token: 0x0600AB1E RID: 43806 RVA: 0x003C8888 File Offset: 0x003C6A88
		public void AddHaulDestination(IHaulDestination haulDestination)
		{
			if (this.allHaulDestinationsInOrder.Contains(haulDestination))
			{
				Log.Error("Double-added haul destination " + haulDestination.ToStringSafe<IHaulDestination>());
				return;
			}
			this.allHaulDestinationsInOrder.Add(haulDestination);
			this.allHaulDestinationsInOrder.InsertionSort(new Comparison<IHaulDestination>(HaulDestinationManager.CompareHaulDestinationPrioritiesDescending));
			ISlotGroupParent slotGroupParent = haulDestination as ISlotGroupParent;
			if (slotGroupParent != null)
			{
				SlotGroup slotGroup = slotGroupParent.GetSlotGroup();
				if (slotGroup == null)
				{
					Log.Error("ISlotGroupParent gave null slot group: " + slotGroupParent.ToStringSafe<ISlotGroupParent>());
					return;
				}
				this.allGroupsInOrder.Add(slotGroup);
				this.allGroupsInOrder.InsertionSort(new Comparison<SlotGroup>(HaulDestinationManager.CompareSlotGroupPrioritiesDescending));
				List<IntVec3> cellsList = slotGroup.CellsList;
				for (int i = 0; i < cellsList.Count; i++)
				{
					this.SetCellFor(cellsList[i], slotGroup);
				}
				this.map.listerHaulables.Notify_SlotGroupChanged(slotGroup);
				this.map.listerMergeables.Notify_SlotGroupChanged(slotGroup);
			}
		}

		// Token: 0x0600AB1F RID: 43807 RVA: 0x003C8974 File Offset: 0x003C6B74
		public void RemoveHaulDestination(IHaulDestination haulDestination)
		{
			if (!this.allHaulDestinationsInOrder.Contains(haulDestination))
			{
				Log.Error("Removing haul destination that isn't registered " + haulDestination.ToStringSafe<IHaulDestination>());
				return;
			}
			this.allHaulDestinationsInOrder.Remove(haulDestination);
			ISlotGroupParent slotGroupParent = haulDestination as ISlotGroupParent;
			if (slotGroupParent == null)
			{
				return;
			}
			SlotGroup slotGroup = slotGroupParent.GetSlotGroup();
			if (slotGroup == null)
			{
				Log.Error("ISlotGroupParent gave null slot group: " + slotGroupParent.ToStringSafe<ISlotGroupParent>());
				return;
			}
			this.allGroupsInOrder.Remove(slotGroup);
			List<IntVec3> cellsList = slotGroup.CellsList;
			for (int i = 0; i < cellsList.Count; i++)
			{
				IntVec3 intVec = cellsList[i];
				this.groupGrid[intVec.x, intVec.y, intVec.z] = null;
			}
			this.map.listerHaulables.Notify_SlotGroupChanged(slotGroup);
			this.map.listerMergeables.Notify_SlotGroupChanged(slotGroup);
		}

		// Token: 0x0600AB20 RID: 43808 RVA: 0x003C8A50 File Offset: 0x003C6C50
		public void AddHaulSource(IHaulSource source)
		{
			if (this.allHaulSourcesInOrder.Contains(source))
			{
				Log.Error("Double-added haul destination " + source.ToStringSafe<IHaulSource>());
				return;
			}
			this.allHaulSourcesInOrder.Add(source);
			this.allHaulSourcesInOrder.InsertionSort(new Comparison<IHaulSource>(HaulDestinationManager.CompareHaulSourcePrioritiesDescending));
			this.map.listerHaulables.Notify_HaulSourceChanged(source);
		}

		// Token: 0x0600AB21 RID: 43809 RVA: 0x003C8AB5 File Offset: 0x003C6CB5
		public void RemoveHaulSource(IHaulSource source)
		{
			if (!this.allHaulSourcesInOrder.Remove(source))
			{
				Log.Error("Removing haul source that isn't registered " + source.ToStringSafe<IHaulSource>());
			}
		}

		// Token: 0x0600AB22 RID: 43810 RVA: 0x003C8ADC File Offset: 0x003C6CDC
		public void Notify_HaulDestinationChangedPriority()
		{
			this.allHaulDestinationsInOrder.InsertionSort(new Comparison<IHaulDestination>(HaulDestinationManager.CompareHaulDestinationPrioritiesDescending));
			this.allGroupsInOrder.InsertionSort(new Comparison<SlotGroup>(HaulDestinationManager.CompareSlotGroupPrioritiesDescending));
			this.allHaulSourcesInOrder.InsertionSort(new Comparison<IHaulSource>(HaulDestinationManager.CompareHaulSourcePrioritiesDescending));
		}

		// Token: 0x0600AB23 RID: 43811 RVA: 0x003C8B30 File Offset: 0x003C6D30
		private static int CompareHaulDestinationPrioritiesDescending(IHaulDestination a, IHaulDestination b)
		{
			return ((int)b.GetStoreSettings().Priority).CompareTo((int)a.GetStoreSettings().Priority);
		}

		// Token: 0x0600AB24 RID: 43812 RVA: 0x003C8B5C File Offset: 0x003C6D5C
		private static int CompareHaulSourcePrioritiesDescending(IHaulSource a, IHaulSource b)
		{
			return ((int)b.GetStoreSettings().Priority).CompareTo((int)a.GetStoreSettings().Priority);
		}

		// Token: 0x0600AB25 RID: 43813 RVA: 0x003C8B88 File Offset: 0x003C6D88
		private static int CompareSlotGroupPrioritiesDescending(SlotGroup a, SlotGroup b)
		{
			return ((int)b.Settings.Priority).CompareTo((int)a.Settings.Priority);
		}

		// Token: 0x0600AB26 RID: 43814 RVA: 0x003C8BB3 File Offset: 0x003C6DB3
		public SlotGroup SlotGroupAt(IntVec3 loc)
		{
			return this.groupGrid[loc.x, loc.y, loc.z];
		}

		// Token: 0x0600AB27 RID: 43815 RVA: 0x003C8BD2 File Offset: 0x003C6DD2
		public ISlotGroupParent SlotGroupParentAt(IntVec3 loc)
		{
			SlotGroup slotGroup = this.SlotGroupAt(loc);
			if (slotGroup == null)
			{
				return null;
			}
			return slotGroup.parent;
		}

		// Token: 0x0600AB28 RID: 43816 RVA: 0x003C8BE8 File Offset: 0x003C6DE8
		public void SetCellFor(IntVec3 c, SlotGroup group)
		{
			if (this.SlotGroupAt(c) != null)
			{
				string[] array = new string[5];
				array[0] = ((group != null) ? group.ToString() : null);
				array[1] = " overwriting slot group square ";
				int num = 2;
				IntVec3 intVec = c;
				array[num] = intVec.ToString();
				array[3] = " of ";
				int num2 = 4;
				SlotGroup slotGroup = this.SlotGroupAt(c);
				array[num2] = ((slotGroup != null) ? slotGroup.ToString() : null);
				Log.Error(string.Concat(array));
			}
			this.groupGrid[c.x, c.y, c.z] = group;
		}

		// Token: 0x0600AB29 RID: 43817 RVA: 0x003C8C74 File Offset: 0x003C6E74
		public void ClearCellFor(IntVec3 c, SlotGroup group)
		{
			if (this.SlotGroupAt(c) != group)
			{
				string[] array = new string[5];
				array[0] = ((group != null) ? group.ToString() : null);
				array[1] = " clearing group grid square ";
				int num = 2;
				IntVec3 intVec = c;
				array[num] = intVec.ToString();
				array[3] = " containing ";
				int num2 = 4;
				SlotGroup slotGroup = this.SlotGroupAt(c);
				array[num2] = ((slotGroup != null) ? slotGroup.ToString() : null);
				Log.Error(string.Concat(array));
			}
			this.groupGrid[c.x, c.y, c.z] = null;
		}

		// Token: 0x040063C3 RID: 25539
		private Map map;

		// Token: 0x040063C4 RID: 25540
		private readonly List<IHaulDestination> allHaulDestinationsInOrder = new List<IHaulDestination>();

		// Token: 0x040063C5 RID: 25541
		private readonly List<IHaulSource> allHaulSourcesInOrder = new List<IHaulSource>();

		// Token: 0x040063C6 RID: 25542
		private readonly List<SlotGroup> allGroupsInOrder = new List<SlotGroup>();

		// Token: 0x040063C7 RID: 25543
		private readonly SlotGroup[,,] groupGrid;
	}
}
