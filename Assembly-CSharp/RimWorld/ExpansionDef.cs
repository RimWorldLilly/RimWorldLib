using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F52 RID: 3922
	public class ExpansionDef : Def
	{
		// Token: 0x17001135 RID: 4405
		// (get) Token: 0x060064EB RID: 25835 RVA: 0x0024040B File Offset: 0x0023E60B
		public Texture2D IconFromStatus
		{
			get
			{
				if (this.Status != ExpansionStatus.NotInstalled)
				{
					return this.Icon;
				}
				return this.NotOwnedIcon;
			}
		}

		// Token: 0x17001136 RID: 4406
		// (get) Token: 0x060064EC RID: 25836 RVA: 0x00240423 File Offset: 0x0023E623
		public Texture2D Icon
		{
			get
			{
				if (this.cachedIcon == null)
				{
					this.cachedIcon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				}
				return this.cachedIcon;
			}
		}

		// Token: 0x17001137 RID: 4407
		// (get) Token: 0x060064ED RID: 25837 RVA: 0x0024044B File Offset: 0x0023E64B
		private Texture2D NotOwnedIcon
		{
			get
			{
				if (this.notOwnedIconPath.NullOrEmpty())
				{
					return this.Icon;
				}
				if (this.cachedNotOwnedIcon == null)
				{
					this.cachedNotOwnedIcon = ContentFinder<Texture2D>.Get(this.notOwnedIconPath, true);
				}
				return this.cachedNotOwnedIcon;
			}
		}

		// Token: 0x17001138 RID: 4408
		// (get) Token: 0x060064EE RID: 25838 RVA: 0x00240487 File Offset: 0x0023E687
		public Texture2D BackgroundImage
		{
			get
			{
				if (this.cachedBG == null)
				{
					this.cachedBG = ContentFinder<Texture2D>.Get(this.backgroundPath, true);
				}
				return this.cachedBG;
			}
		}

		// Token: 0x17001139 RID: 4409
		// (get) Token: 0x060064EF RID: 25839 RVA: 0x002404AF File Offset: 0x0023E6AF
		public List<Texture2D> PreviewImages
		{
			get
			{
				if (this.cachedPreviewImages.NullOrEmpty<Texture2D>())
				{
					if (this.previewImagesFolderPath.NullOrEmpty())
					{
						return null;
					}
					this.cachedPreviewImages = new List<Texture2D>(ContentFinder<Texture2D>.GetAllInFolder(this.previewImagesFolderPath));
				}
				return this.cachedPreviewImages;
			}
		}

		// Token: 0x1700113A RID: 4410
		// (get) Token: 0x060064F0 RID: 25840 RVA: 0x002404E9 File Offset: 0x0023E6E9
		public string StoreURL
		{
			get
			{
				if (!this.steamUrl.NullOrEmpty())
				{
					return this.steamUrl;
				}
				return this.siteUrl;
			}
		}

		// Token: 0x1700113B RID: 4411
		// (get) Token: 0x060064F1 RID: 25841 RVA: 0x00240505 File Offset: 0x0023E705
		public ExpansionStatus Status
		{
			get
			{
				if (ModsConfig.IsActive(this.linkedMod))
				{
					return ExpansionStatus.Active;
				}
				if (ModLister.AllInstalledMods.Any((ModMetaData m) => m.SamePackageId(this.linkedMod, false)))
				{
					return ExpansionStatus.Installed;
				}
				return ExpansionStatus.NotInstalled;
			}
		}

		// Token: 0x1700113C RID: 4412
		// (get) Token: 0x060064F2 RID: 25842 RVA: 0x00240534 File Offset: 0x0023E734
		public string StatusDescription
		{
			get
			{
				ExpansionStatus status = this.Status;
				if (status == ExpansionStatus.Active)
				{
					return "ContentActive".Translate();
				}
				if (status != ExpansionStatus.Installed)
				{
					return "ContentNotInstalled".Translate();
				}
				return "ContentInstalledButNotActive".Translate();
			}
		}

		// Token: 0x060064F3 RID: 25843 RVA: 0x00240580 File Offset: 0x0023E780
		public override void PostLoad()
		{
			base.PostLoad();
			this.linkedMod = this.linkedMod.ToLower();
		}

		// Token: 0x060064F4 RID: 25844 RVA: 0x00240599 File Offset: 0x0023E799
		public override IEnumerable<string> ConfigErrors()
		{
			ExpansionDef.<ConfigErrors>d__30 <ConfigErrors>d__ = new ExpansionDef.<ConfigErrors>d__30(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003AD0 RID: 15056
		[NoTranslate]
		public string iconPath;

		// Token: 0x04003AD1 RID: 15057
		[NoTranslate]
		public string notOwnedIconPath;

		// Token: 0x04003AD2 RID: 15058
		[NoTranslate]
		public string backgroundPath;

		// Token: 0x04003AD3 RID: 15059
		[NoTranslate]
		public string linkedMod;

		// Token: 0x04003AD4 RID: 15060
		[NoTranslate]
		public string steamUrl;

		// Token: 0x04003AD5 RID: 15061
		[NoTranslate]
		public string siteUrl;

		// Token: 0x04003AD6 RID: 15062
		[NoTranslate]
		public string previewImagesFolderPath;

		// Token: 0x04003AD7 RID: 15063
		public bool isCore;

		// Token: 0x04003AD8 RID: 15064
		public Color primaryColor = Color.white;

		// Token: 0x04003AD9 RID: 15065
		private Texture2D cachedIcon;

		// Token: 0x04003ADA RID: 15066
		private Texture2D cachedNotOwnedIcon;

		// Token: 0x04003ADB RID: 15067
		private Texture2D cachedBG;

		// Token: 0x04003ADC RID: 15068
		private List<Texture2D> cachedPreviewImages;
	}
}
