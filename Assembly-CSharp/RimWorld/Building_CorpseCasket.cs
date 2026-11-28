using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AA6 RID: 6822
	public abstract class Building_CorpseCasket : Building_Casket, IHaulDestination, IStoreSettingsParent, IHaulEnroute, ILoadReferenceable, IThingHolderTickable, IThingHolder
	{
		// Token: 0x17001EAC RID: 7852
		// (get) Token: 0x0600AC4E RID: 44110 RVA: 0x003CCEE5 File Offset: 0x003CB0E5
		public virtual bool StorageTabVisible
		{
			get
			{
				return !this.HasCorpse;
			}
		}

		// Token: 0x17001EAD RID: 7853
		// (get) Token: 0x0600AC4F RID: 44111 RVA: 0x003CCEF0 File Offset: 0x003CB0F0
		public bool HasCorpse
		{
			get
			{
				return this.Corpse != null;
			}
		}

		// Token: 0x17001EAE RID: 7854
		// (get) Token: 0x0600AC50 RID: 44112 RVA: 0x003CCEFB File Offset: 0x003CB0FB
		public bool HaulDestinationEnabled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001EAF RID: 7855
		// (get) Token: 0x0600AC51 RID: 44113 RVA: 0x003CCEFE File Offset: 0x003CB0FE
		public virtual bool ShouldTickContents
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001EB0 RID: 7856
		// (get) Token: 0x0600AC52 RID: 44114 RVA: 0x003CCF04 File Offset: 0x003CB104
		public Corpse Corpse
		{
			get
			{
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					Corpse corpse = this.innerContainer[i] as Corpse;
					if (corpse != null)
					{
						return corpse;
					}
				}
				return null;
			}
		}

		// Token: 0x0600AC53 RID: 44115 RVA: 0x003CCF3F File Offset: 0x003CB13F
		public StorageSettings GetStoreSettings()
		{
			return this.storageSettings;
		}

		// Token: 0x0600AC54 RID: 44116 RVA: 0x003CCF47 File Offset: 0x003CB147
		public StorageSettings GetParentStoreSettings()
		{
			return this.def.building.fixedStorageSettings;
		}

		// Token: 0x0600AC55 RID: 44117 RVA: 0x003CCF59 File Offset: 0x003CB159
		public void Notify_SettingsChanged()
		{
		}

		// Token: 0x0600AC56 RID: 44118 RVA: 0x003CCF5B File Offset: 0x003CB15B
		public int SpaceRemainingFor(ThingDef _)
		{
			return (!this.HasCorpse) ? 1 : 0;
		}

		// Token: 0x0600AC57 RID: 44119 RVA: 0x003CCF68 File Offset: 0x003CB168
		public override void PostMake()
		{
			base.PostMake();
			this.storageSettings = new StorageSettings(this);
			if (this.def.building.defaultStorageSettings != null)
			{
				this.storageSettings.CopyFrom(this.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x0600AC58 RID: 44120 RVA: 0x003CCFB4 File Offset: 0x003CB1B4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.storageSettings, "storageSettings", new object[]
			{
				this
			});
		}

		// Token: 0x0400641F RID: 25631
		protected StorageSettings storageSettings;
	}
}
