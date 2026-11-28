using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200184E RID: 6222
	public class CompChangeableProjectile : ThingComp, IStoreSettingsParent
	{
		// Token: 0x17001A95 RID: 6805
		// (get) Token: 0x06009BC4 RID: 39876 RVA: 0x0038262F File Offset: 0x0038082F
		public CompProperties_ChangeableProjectile Props
		{
			get
			{
				return (CompProperties_ChangeableProjectile)this.props;
			}
		}

		// Token: 0x17001A96 RID: 6806
		// (get) Token: 0x06009BC5 RID: 39877 RVA: 0x0038263C File Offset: 0x0038083C
		public ThingDef LoadedShell
		{
			get
			{
				if (this.loadedCount <= 0)
				{
					return null;
				}
				return this.loadedShell;
			}
		}

		// Token: 0x17001A97 RID: 6807
		// (get) Token: 0x06009BC6 RID: 39878 RVA: 0x0038264F File Offset: 0x0038084F
		public ThingDef Projectile
		{
			get
			{
				if (!this.Loaded)
				{
					return null;
				}
				return this.LoadedShell.projectileWhenLoaded;
			}
		}

		// Token: 0x17001A98 RID: 6808
		// (get) Token: 0x06009BC7 RID: 39879 RVA: 0x00382666 File Offset: 0x00380866
		public bool Loaded
		{
			get
			{
				return this.LoadedShell != null;
			}
		}

		// Token: 0x17001A99 RID: 6809
		// (get) Token: 0x06009BC8 RID: 39880 RVA: 0x00382671 File Offset: 0x00380871
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06009BC9 RID: 39881 RVA: 0x00382674 File Offset: 0x00380874
		public override void PostExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.loadedShell, "loadedShell");
			Scribe_Values.Look<int>(ref this.loadedCount, "loadedCount", 0, false);
			Scribe_Deep.Look<StorageSettings>(ref this.allowedShellsSettings, "allowedShellsSettings", Array.Empty<object>());
		}

		// Token: 0x06009BCA RID: 39882 RVA: 0x003826B0 File Offset: 0x003808B0
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.allowedShellsSettings = new StorageSettings(this);
			if (this.parent.def.building.defaultStorageSettings != null)
			{
				this.allowedShellsSettings.CopyFrom(this.parent.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x06009BCB RID: 39883 RVA: 0x00382707 File Offset: 0x00380907
		public virtual void Notify_ProjectileLaunched()
		{
			if (this.loadedCount > 0)
			{
				this.loadedCount--;
			}
			if (this.loadedCount <= 0)
			{
				this.loadedShell = null;
			}
		}

		// Token: 0x06009BCC RID: 39884 RVA: 0x00382730 File Offset: 0x00380930
		public void LoadShell(ThingDef shell, int count)
		{
			this.loadedCount = Mathf.Max(count, 0);
			this.loadedShell = ((count > 0) ? shell : null);
		}

		// Token: 0x06009BCD RID: 39885 RVA: 0x0038274D File Offset: 0x0038094D
		public Thing RemoveShell()
		{
			Thing thing = ThingMaker.MakeThing(this.loadedShell, null);
			thing.stackCount = this.loadedCount;
			this.loadedCount = 0;
			this.loadedShell = null;
			return thing;
		}

		// Token: 0x06009BCE RID: 39886 RVA: 0x00382775 File Offset: 0x00380975
		public StorageSettings GetStoreSettings()
		{
			return this.allowedShellsSettings;
		}

		// Token: 0x06009BCF RID: 39887 RVA: 0x0038277D File Offset: 0x0038097D
		public StorageSettings GetParentStoreSettings()
		{
			return this.parent.def.building.fixedStorageSettings;
		}

		// Token: 0x06009BD0 RID: 39888 RVA: 0x00382794 File Offset: 0x00380994
		public void Notify_SettingsChanged()
		{
		}

		// Token: 0x04005CA5 RID: 23717
		private ThingDef loadedShell;

		// Token: 0x04005CA6 RID: 23718
		public int loadedCount;

		// Token: 0x04005CA7 RID: 23719
		public StorageSettings allowedShellsSettings;
	}
}
