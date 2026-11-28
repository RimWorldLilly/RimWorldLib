using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001C3 RID: 451
	public class StyleCategoryDef : Def
	{
		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000C5E RID: 3166 RVA: 0x00041184 File Offset: 0x0003F384
		public Texture2D Icon
		{
			get
			{
				if (this.cachedIcon == null)
				{
					if (this.iconPath.NullOrEmpty())
					{
						this.cachedIcon = BaseContent.BadTex;
					}
					else
					{
						this.cachedIcon = ContentFinder<Texture2D>.Get(this.iconPath, true);
					}
				}
				return this.cachedIcon;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000C5F RID: 3167 RVA: 0x000411D4 File Offset: 0x0003F3D4
		public List<BuildableDef> AllDesignatorBuildables
		{
			get
			{
				if (this.cachedAllDesignatorBuildables == null)
				{
					this.cachedAllDesignatorBuildables = new List<BuildableDef>();
					if (this.addDesignators != null)
					{
						foreach (BuildableDef item in this.addDesignators)
						{
							this.cachedAllDesignatorBuildables.Add(item);
						}
					}
					if (this.addDesignatorGroups != null)
					{
						foreach (DesignatorDropdownGroupDef designatorDropdownGroupDef in this.addDesignatorGroups)
						{
							this.cachedAllDesignatorBuildables.AddRange(designatorDropdownGroupDef.BuildablesWithoutDefaultDesignators());
						}
					}
				}
				return this.cachedAllDesignatorBuildables;
			}
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x000412A4 File Offset: 0x0003F4A4
		public ThingStyleDef GetStyleForThingDef(BuildableDef thingDef, Precept precept = null)
		{
			ThingStyleDef result;
			try
			{
				for (int i = 0; i < this.thingDefStyles.Count; i++)
				{
					if (this.thingDefStyles[i].ThingDef == thingDef)
					{
						StyleCategoryDef.tmpAvailableStyles.Add(this.thingDefStyles[i].StyleDef);
					}
				}
				if (StyleCategoryDef.tmpAvailableStyles.Count == 0)
				{
					result = null;
				}
				else if (StyleCategoryDef.tmpAvailableStyles.Count == 1 || precept == null)
				{
					result = StyleCategoryDef.tmpAvailableStyles[0];
				}
				else
				{
					result = StyleCategoryDef.tmpAvailableStyles[Rand.RangeSeeded(0, StyleCategoryDef.tmpAvailableStyles.Count, precept.randomSeed)];
				}
			}
			finally
			{
				StyleCategoryDef.tmpAvailableStyles.Clear();
			}
			return result;
		}

		// Token: 0x04000BB6 RID: 2998
		public List<ThingDefStyle> thingDefStyles;

		// Token: 0x04000BB7 RID: 2999
		[NoTranslate]
		public string iconPath;

		// Token: 0x04000BB8 RID: 3000
		public bool fixedIdeoOnly;

		// Token: 0x04000BB9 RID: 3001
		public List<BuildableDef> addDesignators;

		// Token: 0x04000BBA RID: 3002
		public List<DesignatorDropdownGroupDef> addDesignatorGroups;

		// Token: 0x04000BBB RID: 3003
		public SoundDef soundOngoingRitual;

		// Token: 0x04000BBC RID: 3004
		public RitualVisualEffectDef ritualVisualEffectDef;

		// Token: 0x04000BBD RID: 3005
		private Texture2D cachedIcon;

		// Token: 0x04000BBE RID: 3006
		private List<BuildableDef> cachedAllDesignatorBuildables;

		// Token: 0x04000BBF RID: 3007
		private static List<ThingStyleDef> tmpAvailableStyles = new List<ThingStyleDef>();
	}
}
