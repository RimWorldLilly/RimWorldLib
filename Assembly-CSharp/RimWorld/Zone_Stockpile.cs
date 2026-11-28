using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013C8 RID: 5064
	public class Zone_Stockpile : Zone, ISlotGroupParent, IStoreSettingsParent, IHaulDestination
	{
		// Token: 0x17001576 RID: 5494
		// (get) Token: 0x06007D95 RID: 32149 RVA: 0x002CE735 File Offset: 0x002CC935
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001577 RID: 5495
		// (get) Token: 0x06007D96 RID: 32150 RVA: 0x002CE738 File Offset: 0x002CC938
		public bool IgnoreStoredThingsBeauty
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001578 RID: 5496
		// (get) Token: 0x06007D97 RID: 32151 RVA: 0x002CE73B File Offset: 0x002CC93B
		public bool HaulDestinationEnabled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001579 RID: 5497
		// (get) Token: 0x06007D98 RID: 32152 RVA: 0x002CE73E File Offset: 0x002CC93E
		protected override Color NextZoneColor
		{
			get
			{
				return ZoneColorUtility.NextStorageZoneColor();
			}
		}

		// Token: 0x06007D99 RID: 32153 RVA: 0x002CE745 File Offset: 0x002CC945
		public Zone_Stockpile()
		{
			this.slotGroup = new SlotGroup(this);
		}

		// Token: 0x06007D9A RID: 32154 RVA: 0x002CE759 File Offset: 0x002CC959
		public Zone_Stockpile(StorageSettingsPreset preset, ZoneManager zoneManager) : base(preset.PresetName(), zoneManager)
		{
			this.settings = new StorageSettings(this);
			this.settings.SetFromPreset(preset);
			this.slotGroup = new SlotGroup(this);
		}

		// Token: 0x06007D9B RID: 32155 RVA: 0x002CE78C File Offset: 0x002CC98C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
		}

		// Token: 0x06007D9C RID: 32156 RVA: 0x002CE7AE File Offset: 0x002CC9AE
		public override void AddCell(IntVec3 sq)
		{
			base.AddCell(sq);
			SlotGroup slotGroup = this.slotGroup;
			if (slotGroup != null)
			{
				slotGroup.Notify_AddedCell(sq);
			}
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Shelves, OpportunityType.GoodToKnow);
		}

		// Token: 0x06007D9D RID: 32157 RVA: 0x002CE7D4 File Offset: 0x002CC9D4
		public override void RemoveCell(IntVec3 sq)
		{
			base.RemoveCell(sq);
			this.slotGroup.Notify_LostCell(sq);
		}

		// Token: 0x06007D9E RID: 32158 RVA: 0x002CE7E9 File Offset: 0x002CC9E9
		public override void PostDeregister()
		{
			base.PostDeregister();
			BillUtility.Notify_ISlotGroupRemoved(this.slotGroup);
		}

		// Token: 0x06007D9F RID: 32159 RVA: 0x002CE7FC File Offset: 0x002CC9FC
		public override IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return Zone_Stockpile.ITabs;
		}

		// Token: 0x06007DA0 RID: 32160 RVA: 0x002CE803 File Offset: 0x002CCA03
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Zone_Stockpile.<GetGizmos>d__18 <GetGizmos>d__ = new Zone_Stockpile.<GetGizmos>d__18(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x06007DA1 RID: 32161 RVA: 0x002CE813 File Offset: 0x002CCA13
		public override IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			return new Zone_Stockpile.<GetZoneAddGizmos>d__19(-2);
		}

		// Token: 0x06007DA2 RID: 32162 RVA: 0x002CE81C File Offset: 0x002CCA1C
		public SlotGroup GetSlotGroup()
		{
			return this.slotGroup;
		}

		// Token: 0x06007DA3 RID: 32163 RVA: 0x002CE824 File Offset: 0x002CCA24
		public IEnumerable<IntVec3> AllSlotCells()
		{
			Zone_Stockpile.<AllSlotCells>d__21 <AllSlotCells>d__ = new Zone_Stockpile.<AllSlotCells>d__21(-2);
			<AllSlotCells>d__.<>4__this = this;
			return <AllSlotCells>d__;
		}

		// Token: 0x06007DA4 RID: 32164 RVA: 0x002CE834 File Offset: 0x002CCA34
		public List<IntVec3> AllSlotCellsList()
		{
			return this.cells;
		}

		// Token: 0x06007DA5 RID: 32165 RVA: 0x002CE83C File Offset: 0x002CCA3C
		public StorageSettings GetParentStoreSettings()
		{
			return StorageSettings.EverStorableFixedSettings();
		}

		// Token: 0x06007DA6 RID: 32166 RVA: 0x002CE843 File Offset: 0x002CCA43
		public StorageSettings GetStoreSettings()
		{
			return this.settings;
		}

		// Token: 0x06007DA7 RID: 32167 RVA: 0x002CE84B File Offset: 0x002CCA4B
		public void Notify_SettingsChanged()
		{
			if (base.Map != null && this.slotGroup != null)
			{
				base.Map.listerHaulables.Notify_SlotGroupChanged(this.slotGroup);
			}
		}

		// Token: 0x06007DA8 RID: 32168 RVA: 0x002CE873 File Offset: 0x002CCA73
		public bool Accepts(Thing t)
		{
			return this.settings.AllowedToAccept(t);
		}

		// Token: 0x1700157A RID: 5498
		// (get) Token: 0x06007DA9 RID: 32169 RVA: 0x002CE881 File Offset: 0x002CCA81
		public int SpaceRemaining
		{
			get
			{
				return base.CellCount - base.HeldThingsCount;
			}
		}

		// Token: 0x06007DAA RID: 32170 RVA: 0x002CE890 File Offset: 0x002CCA90
		public string SlotYielderLabel()
		{
			return this.label;
		}

		// Token: 0x1700157B RID: 5499
		// (get) Token: 0x06007DAB RID: 32171 RVA: 0x002CE898 File Offset: 0x002CCA98
		public string GroupingLabel
		{
			get
			{
				return "StockpilePlural".Translate();
			}
		}

		// Token: 0x1700157C RID: 5500
		// (get) Token: 0x06007DAC RID: 32172 RVA: 0x002CE8A9 File Offset: 0x002CCAA9
		public int GroupingOrder
		{
			get
			{
				return -50;
			}
		}

		// Token: 0x06007DAD RID: 32173 RVA: 0x002CE8AD File Offset: 0x002CCAAD
		public void Notify_ReceivedThing(Thing newItem)
		{
			if (newItem.def.storedConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(newItem.def.storedConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
		}

		// Token: 0x06007DAE RID: 32174 RVA: 0x002CE8CD File Offset: 0x002CCACD
		public void Notify_LostThing(Thing newItem)
		{
		}

		// Token: 0x04004F15 RID: 20245
		public StorageSettings settings;

		// Token: 0x04004F16 RID: 20246
		public SlotGroup slotGroup;

		// Token: 0x04004F17 RID: 20247
		private static readonly ITab[] ITabs = new ITab[]
		{
			new ITab_Storage()
		};
	}
}
