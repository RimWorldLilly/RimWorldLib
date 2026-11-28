using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000344 RID: 836
	[StaticConstructorOnStartup]
	public abstract class ModRequirement
	{
		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x060019F1 RID: 6641
		public abstract bool IsSatisfied { get; }

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x060019F2 RID: 6642
		public abstract string RequirementTypeLabel { get; }

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x060019F3 RID: 6643 RVA: 0x0009B1EF File Offset: 0x000993EF
		public virtual string Tooltip
		{
			get
			{
				return "ModPackageId".Translate() + ": " + this.packageId;
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x060019F4 RID: 6644 RVA: 0x0009B215 File Offset: 0x00099415
		public virtual Texture2D StatusIcon
		{
			get
			{
				if (!this.IsSatisfied)
				{
					return ModRequirement.NotResolved;
				}
				return ModRequirement.Resolved;
			}
		}

		// Token: 0x060019F5 RID: 6645
		public abstract void OnClicked(Page_ModsConfig window);

		// Token: 0x04001541 RID: 5441
		public string packageId;

		// Token: 0x04001542 RID: 5442
		public List<string> alternativePackageIds;

		// Token: 0x04001543 RID: 5443
		public string displayName;

		// Token: 0x04001544 RID: 5444
		public static Texture2D NotResolved = ContentFinder<Texture2D>.Get("UI/Icons/ModRequirements/NotResolved", true);

		// Token: 0x04001545 RID: 5445
		public static Texture2D NotInstalled = ContentFinder<Texture2D>.Get("UI/Icons/ModRequirements/NotInstalled", true);

		// Token: 0x04001546 RID: 5446
		public static Texture2D Installed = ContentFinder<Texture2D>.Get("UI/Icons/ModRequirements/Installed", true);

		// Token: 0x04001547 RID: 5447
		public static Texture2D Resolved = ContentFinder<Texture2D>.Get("UI/Widgets/CheckOn", true);
	}
}
