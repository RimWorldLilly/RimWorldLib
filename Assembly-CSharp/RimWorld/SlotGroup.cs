using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A87 RID: 6791
	public class SlotGroup : ISlotGroup
	{
		// Token: 0x17001E6D RID: 7789
		// (get) Token: 0x0600AB54 RID: 43860 RVA: 0x003C8D18 File Offset: 0x003C6F18
		private Map Map
		{
			get
			{
				return this.parent.Map;
			}
		}

		// Token: 0x17001E6E RID: 7790
		// (get) Token: 0x0600AB55 RID: 43861 RVA: 0x003C8D25 File Offset: 0x003C6F25
		public StorageSettings Settings
		{
			get
			{
				return this.parent.GetStoreSettings();
			}
		}

		// Token: 0x17001E6F RID: 7791
		// (get) Token: 0x0600AB56 RID: 43862 RVA: 0x003C8D32 File Offset: 0x003C6F32
		public string GroupingLabel
		{
			get
			{
				return this.parent.GroupingLabel;
			}
		}

		// Token: 0x17001E70 RID: 7792
		// (get) Token: 0x0600AB57 RID: 43863 RVA: 0x003C8D3F File Offset: 0x003C6F3F
		public int GroupingOrder
		{
			get
			{
				return this.parent.GroupingOrder;
			}
		}

		// Token: 0x17001E71 RID: 7793
		// (get) Token: 0x0600AB58 RID: 43864 RVA: 0x003C8D4C File Offset: 0x003C6F4C
		public IEnumerable<Thing> HeldThings
		{
			get
			{
				SlotGroup.<get_HeldThings>d__10 <get_HeldThings>d__ = new SlotGroup.<get_HeldThings>d__10(-2);
				<get_HeldThings>d__.<>4__this = this;
				return <get_HeldThings>d__;
			}
		}

		// Token: 0x17001E72 RID: 7794
		// (get) Token: 0x0600AB59 RID: 43865 RVA: 0x003C8D5C File Offset: 0x003C6F5C
		public int HeldThingsCount
		{
			get
			{
				int num = 0;
				List<IntVec3> cellsList = this.CellsList;
				ThingGrid thingGrid = this.Map.thingGrid;
				for (int i = 0; i < cellsList.Count; i++)
				{
					List<Thing> list = thingGrid.ThingsListAt(cellsList[i]);
					for (int j = 0; j < list.Count; j++)
					{
						if (list[j].def.EverStorable(false))
						{
							num++;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x17001E73 RID: 7795
		// (get) Token: 0x0600AB5A RID: 43866 RVA: 0x003C8DCF File Offset: 0x003C6FCF
		public List<IntVec3> CellsList
		{
			get
			{
				return this.parent.AllSlotCellsList();
			}
		}

		// Token: 0x17001E74 RID: 7796
		// (get) Token: 0x0600AB5B RID: 43867 RVA: 0x003C8DDC File Offset: 0x003C6FDC
		public StorageGroup StorageGroup
		{
			get
			{
				IStorageGroupMember storageGroupMember = this.parent as IStorageGroupMember;
				if (storageGroupMember != null)
				{
					return storageGroupMember.Group;
				}
				return null;
			}
		}

		// Token: 0x0600AB5C RID: 43868 RVA: 0x003C8E00 File Offset: 0x003C7000
		public IEnumerator<IntVec3> GetEnumerator()
		{
			SlotGroup.<GetEnumerator>d__17 <GetEnumerator>d__ = new SlotGroup.<GetEnumerator>d__17(0);
			<GetEnumerator>d__.<>4__this = this;
			return <GetEnumerator>d__;
		}

		// Token: 0x0600AB5D RID: 43869 RVA: 0x003C8E0F File Offset: 0x003C700F
		public SlotGroup(ISlotGroupParent parent)
		{
			this.parent = parent;
		}

		// Token: 0x0600AB5E RID: 43870 RVA: 0x003C8E1E File Offset: 0x003C701E
		public void Notify_AddedCell(IntVec3 c)
		{
			this.Map.haulDestinationManager.SetCellFor(c, this);
			this.Map.listerHaulables.RecalcAllInCell(c);
			this.Map.listerMergeables.RecalcAllInCell(c);
		}

		// Token: 0x0600AB5F RID: 43871 RVA: 0x003C8E54 File Offset: 0x003C7054
		public void Notify_LostCell(IntVec3 c)
		{
			this.Map.haulDestinationManager.ClearCellFor(c, this);
			this.Map.listerHaulables.RecalcAllInCell(c);
			this.Map.listerMergeables.RecalcAllInCell(c);
		}

		// Token: 0x0600AB60 RID: 43872 RVA: 0x003C8E8C File Offset: 0x003C708C
		public void RemoveHaulDesignationOnStoredThings()
		{
			if (this.parent.Map == null)
			{
				return;
			}
			foreach (Thing t in this.HeldThings)
			{
				if (this.Settings.AllowedToAccept(t))
				{
					Designation designation = this.Map.designationManager.DesignationOn(t, DesignationDefOf.Haul);
					if (designation != null)
					{
						this.Map.designationManager.RemoveDesignation(designation);
					}
				}
			}
		}

		// Token: 0x0600AB61 RID: 43873 RVA: 0x003C8F1C File Offset: 0x003C711C
		public override string ToString()
		{
			if (this.parent != null)
			{
				return this.parent.ToString();
			}
			return "NullParent";
		}

		// Token: 0x0600AB62 RID: 43874 RVA: 0x003C8F38 File Offset: 0x003C7138
		public string GetName()
		{
			Zone_Stockpile zone_Stockpile = this.parent as Zone_Stockpile;
			if (zone_Stockpile != null)
			{
				return zone_Stockpile.label;
			}
			Building_Storage building_Storage = this.parent as Building_Storage;
			if (building_Storage != null)
			{
				return building_Storage.Label;
			}
			return "UnresolvedSlotGroupName";
		}

		// Token: 0x0600AB63 RID: 43875 RVA: 0x003C8F78 File Offset: 0x003C7178
		public static string GetGroupLabel(ISlotGroup group)
		{
			SlotGroup slotGroup = group as SlotGroup;
			if (slotGroup != null)
			{
				return slotGroup.parent.SlotYielderLabel();
			}
			IRenameable renameable = group as IRenameable;
			if (renameable != null)
			{
				return renameable.RenamableLabel;
			}
			return "UNABLE TO GET LABEL";
		}

		// Token: 0x040063C8 RID: 25544
		public ISlotGroupParent parent;
	}
}
