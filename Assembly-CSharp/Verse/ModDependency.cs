using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000345 RID: 837
	public class ModDependency : ModRequirement
	{
		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x060019F8 RID: 6648 RVA: 0x0009B281 File Offset: 0x00099481
		public override string RequirementTypeLabel
		{
			get
			{
				return "ModDependsOn".Translate("");
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x060019F9 RID: 6649 RVA: 0x0009B29C File Offset: 0x0009949C
		public override bool IsSatisfied
		{
			get
			{
				if (ModLister.GetActiveModWithIdentifier(this.packageId, true) != null)
				{
					return true;
				}
				List<string> alternativePackageIds = this.alternativePackageIds;
				if (alternativePackageIds == null)
				{
					return false;
				}
				return alternativePackageIds.Any((string id) => ModLister.GetActiveModWithIdentifier(id, true) != null);
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x060019FA RID: 6650 RVA: 0x0009B2EC File Offset: 0x000994EC
		private ModMetaData Mod
		{
			get
			{
				ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.packageId, true);
				if (modWithIdentifier != null)
				{
					return modWithIdentifier;
				}
				if (this.alternativePackageIds.NullOrEmpty<string>())
				{
					return null;
				}
				foreach (string identifier in this.alternativePackageIds)
				{
					modWithIdentifier = ModLister.GetModWithIdentifier(identifier, true);
					if (modWithIdentifier != null)
					{
						return modWithIdentifier;
					}
				}
				return null;
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x060019FB RID: 6651 RVA: 0x0009B36C File Offset: 0x0009956C
		public override Texture2D StatusIcon
		{
			get
			{
				if (this.Mod == null)
				{
					return ModRequirement.NotInstalled;
				}
				if (!this.Mod.Active)
				{
					return ModRequirement.Installed;
				}
				return ModRequirement.Resolved;
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x060019FC RID: 6652 RVA: 0x0009B394 File Offset: 0x00099594
		public override string Tooltip
		{
			get
			{
				if (this.Mod == null)
				{
					return base.Tooltip + "\n" + "ContentNotInstalled".Translate() + "\n\n" + "ModClickToGoToWebsite".Translate();
				}
				if (!this.Mod.Active)
				{
					return base.Tooltip + "\n" + "ContentInstalledButNotActive".Translate() + "\n\n" + "ModClickToSelect".Translate();
				}
				return base.Tooltip;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x060019FD RID: 6653 RVA: 0x0009B438 File Offset: 0x00099638
		public string Url
		{
			get
			{
				return this.steamWorkshopUrl ?? this.downloadUrl;
			}
		}

		// Token: 0x060019FE RID: 6654 RVA: 0x0009B44A File Offset: 0x0009964A
		public override void OnClicked(Page_ModsConfig window)
		{
			if (this.Mod == null)
			{
				if (!this.Url.NullOrEmpty())
				{
					SteamUtility.OpenUrl(this.Url);
					return;
				}
			}
			else if (!this.Mod.Active)
			{
				window.SelectMod(this.Mod);
			}
		}

		// Token: 0x04001548 RID: 5448
		public string downloadUrl;

		// Token: 0x04001549 RID: 5449
		public string steamWorkshopUrl;
	}
}
