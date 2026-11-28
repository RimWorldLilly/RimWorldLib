using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A56 RID: 6742
	public class Blueprint_Storage : Blueprint_Build, IStorageGroupMember, IStoreSettingsParent
	{
		// Token: 0x17001DA7 RID: 7591
		// (get) Token: 0x0600A913 RID: 43283 RVA: 0x003BFE6C File Offset: 0x003BE06C
		// (set) Token: 0x0600A914 RID: 43284 RVA: 0x003BFE74 File Offset: 0x003BE074
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

		// Token: 0x17001DA8 RID: 7592
		// (get) Token: 0x0600A915 RID: 43285 RVA: 0x003BFE7D File Offset: 0x003BE07D
		Map IStorageGroupMember.Map
		{
			get
			{
				return base.Map;
			}
		}

		// Token: 0x17001DA9 RID: 7593
		// (get) Token: 0x0600A916 RID: 43286 RVA: 0x003BFE85 File Offset: 0x003BE085
		StorageSettings IStorageGroupMember.StoreSettings
		{
			get
			{
				return this.GetStoreSettings();
			}
		}

		// Token: 0x17001DAA RID: 7594
		// (get) Token: 0x0600A917 RID: 43287 RVA: 0x003BFE8D File Offset: 0x003BE08D
		StorageSettings IStorageGroupMember.ParentStoreSettings
		{
			get
			{
				return this.GetParentStoreSettings();
			}
		}

		// Token: 0x17001DAB RID: 7595
		// (get) Token: 0x0600A918 RID: 43288 RVA: 0x003BFE95 File Offset: 0x003BE095
		StorageSettings IStorageGroupMember.ThingStoreSettings
		{
			get
			{
				return this.settings;
			}
		}

		// Token: 0x17001DAC RID: 7596
		// (get) Token: 0x0600A919 RID: 43289 RVA: 0x003BFE9D File Offset: 0x003BE09D
		string IStorageGroupMember.StorageGroupTag
		{
			get
			{
				return base.BuildDef.building.storageGroupTag;
			}
		}

		// Token: 0x17001DAD RID: 7597
		// (get) Token: 0x0600A91A RID: 43290 RVA: 0x003BFEAF File Offset: 0x003BE0AF
		bool IStorageGroupMember.DrawConnectionOverlay
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001DAE RID: 7598
		// (get) Token: 0x0600A91B RID: 43291 RVA: 0x003BFEB2 File Offset: 0x003BE0B2
		bool IStorageGroupMember.DrawStorageTab
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001DAF RID: 7599
		// (get) Token: 0x0600A91C RID: 43292 RVA: 0x003BFEB5 File Offset: 0x003BE0B5
		bool IStorageGroupMember.ShowRenameButton
		{
			get
			{
				return base.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x17001DB0 RID: 7600
		// (get) Token: 0x0600A91D RID: 43293 RVA: 0x003BFEC4 File Offset: 0x003BE0C4
		bool IStoreSettingsParent.StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600A91E RID: 43294 RVA: 0x003BFEC7 File Offset: 0x003BE0C7
		public StorageSettings GetStoreSettings()
		{
			if (this.storageGroup != null)
			{
				return this.storageGroup.GetStoreSettings();
			}
			return this.settings;
		}

		// Token: 0x0600A91F RID: 43295 RVA: 0x003BFEE3 File Offset: 0x003BE0E3
		public StorageSettings GetParentStoreSettings()
		{
			return base.BuildDef.building.fixedStorageSettings;
		}

		// Token: 0x0600A920 RID: 43296 RVA: 0x003BFEF5 File Offset: 0x003BE0F5
		void IStoreSettingsParent.Notify_SettingsChanged()
		{
		}

		// Token: 0x0600A921 RID: 43297 RVA: 0x003BFEF8 File Offset: 0x003BE0F8
		public override void PostMake()
		{
			base.PostMake();
			this.settings = new StorageSettings(this);
			if (base.BuildDef.building.defaultStorageSettings != null)
			{
				this.settings.CopyFrom(base.BuildDef.building.defaultStorageSettings);
			}
		}

		// Token: 0x0600A922 RID: 43298 RVA: 0x003BFF44 File Offset: 0x003BE144
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			StorageGroup storageGroup = this.storageGroup;
			if (storageGroup != null)
			{
				storageGroup.RemoveMember(this, true);
			}
			this.storageGroup = null;
			base.Destroy(mode);
		}

		// Token: 0x0600A923 RID: 43299 RVA: 0x003BFF67 File Offset: 0x003BE167
		public override IEnumerable<InspectTabBase> GetInspectTabs()
		{
			Blueprint_Storage.<GetInspectTabs>d__28 <GetInspectTabs>d__ = new Blueprint_Storage.<GetInspectTabs>d__28(-2);
			<GetInspectTabs>d__.<>4__this = this;
			return <GetInspectTabs>d__;
		}

		// Token: 0x0600A924 RID: 43300 RVA: 0x003BFF77 File Offset: 0x003BE177
		protected override Thing MakeSolidThing(out bool shouldSelect)
		{
			Frame frame = (Frame)base.MakeSolidThing(out shouldSelect);
			frame.SetStorageGroup(this.storageGroup, true);
			frame.storageSettings = new StorageSettings();
			frame.storageSettings.CopyFrom(this.GetStoreSettings());
			this.SetStorageGroup(null, false);
			return frame;
		}

		// Token: 0x0600A925 RID: 43301 RVA: 0x003BFFB6 File Offset: 0x003BE1B6
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			StorageGroupUtility.DrawSelectionOverlaysFor(this);
		}

		// Token: 0x0600A926 RID: 43302 RVA: 0x003BFFC4 File Offset: 0x003BE1C4
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (this.storageGroup != null)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "LinkedStorageSettings".Translate() + ": ";
				if (this.storageGroup.MemberCount > 1)
				{
					text += "NumBuildings".Translate(this.storageGroup.MemberCount).CapitalizeFirst();
				}
				else
				{
					text += "OneBuilding".Translate().CapitalizeFirst();
				}
			}
			return text;
		}

		// Token: 0x0600A927 RID: 43303 RVA: 0x003C0074 File Offset: 0x003BE274
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Blueprint_Storage.<GetGizmos>d__32 <GetGizmos>d__ = new Blueprint_Storage.<GetGizmos>d__32(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600A928 RID: 43304 RVA: 0x003C0084 File Offset: 0x003BE284
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<StorageGroup>(ref this.storageGroup, "storageGroup", false);
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
		}

		// Token: 0x04006313 RID: 25363
		protected StorageGroup storageGroup;

		// Token: 0x04006314 RID: 25364
		public StorageSettings settings;
	}
}
