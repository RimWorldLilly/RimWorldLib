using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001C9 RID: 457
	public class ThingCategoryDef : Def
	{
		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000C84 RID: 3204 RVA: 0x00041B9C File Offset: 0x0003FD9C
		public List<ThingDef> SortedChildThingDefs
		{
			get
			{
				return this.sortedChildThingDefsCached;
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000C85 RID: 3205 RVA: 0x00041BA4 File Offset: 0x0003FDA4
		public IEnumerable<ThingCategoryDef> Parents
		{
			get
			{
				ThingCategoryDef.<get_Parents>d__13 <get_Parents>d__ = new ThingCategoryDef.<get_Parents>d__13(-2);
				<get_Parents>d__.<>4__this = this;
				return <get_Parents>d__;
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000C86 RID: 3206 RVA: 0x00041BB4 File Offset: 0x0003FDB4
		public IEnumerable<ThingCategoryDef> ThisAndChildCategoryDefs
		{
			get
			{
				ThingCategoryDef.<get_ThisAndChildCategoryDefs>d__15 <get_ThisAndChildCategoryDefs>d__ = new ThingCategoryDef.<get_ThisAndChildCategoryDefs>d__15(-2);
				<get_ThisAndChildCategoryDefs>d__.<>4__this = this;
				return <get_ThisAndChildCategoryDefs>d__;
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000C87 RID: 3207 RVA: 0x00041BC4 File Offset: 0x0003FDC4
		public IEnumerable<ThingDef> DescendantThingDefs
		{
			get
			{
				ThingCategoryDef.<get_DescendantThingDefs>d__17 <get_DescendantThingDefs>d__ = new ThingCategoryDef.<get_DescendantThingDefs>d__17(-2);
				<get_DescendantThingDefs>d__.<>4__this = this;
				return <get_DescendantThingDefs>d__;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000C88 RID: 3208 RVA: 0x00041BD4 File Offset: 0x0003FDD4
		public IEnumerable<SpecialThingFilterDef> DescendantSpecialThingFilterDefs
		{
			get
			{
				ThingCategoryDef.<get_DescendantSpecialThingFilterDefs>d__19 <get_DescendantSpecialThingFilterDefs>d__ = new ThingCategoryDef.<get_DescendantSpecialThingFilterDefs>d__19(-2);
				<get_DescendantSpecialThingFilterDefs>d__.<>4__this = this;
				return <get_DescendantSpecialThingFilterDefs>d__;
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000C89 RID: 3209 RVA: 0x00041BE4 File Offset: 0x0003FDE4
		public IEnumerable<SpecialThingFilterDef> ParentsSpecialThingFilterDefs
		{
			get
			{
				ThingCategoryDef.<get_ParentsSpecialThingFilterDefs>d__21 <get_ParentsSpecialThingFilterDefs>d__ = new ThingCategoryDef.<get_ParentsSpecialThingFilterDefs>d__21(-2);
				<get_ParentsSpecialThingFilterDefs>d__.<>4__this = this;
				return <get_ParentsSpecialThingFilterDefs>d__;
			}
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x00041BF4 File Offset: 0x0003FDF4
		public bool ContainedInThisOrDescendant(ThingDef thingDef)
		{
			return this.allChildThingDefsCached.Contains(thingDef);
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x00041C04 File Offset: 0x0003FE04
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			this.allChildThingDefsCached = new HashSet<ThingDef>();
			foreach (ThingCategoryDef thingCategoryDef in this.ThisAndChildCategoryDefs)
			{
				foreach (ThingDef item in thingCategoryDef.childThingDefs)
				{
					this.allChildThingDefsCached.Add(item);
				}
			}
			this.sortedChildThingDefsCached = (from n in this.childThingDefs
			orderby n.label
			select n).ToList<ThingDef>();
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x00041CD8 File Offset: 0x0003FED8
		public override void PostLoad()
		{
			this.treeNode = new TreeNode_ThingCategory(this);
			if (!this.iconPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				});
			}
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x00041D04 File Offset: 0x0003FF04
		public static ThingCategoryDef Named(string defName)
		{
			return DefDatabase<ThingCategoryDef>.GetNamed(defName, true);
		}

		// Token: 0x04000C26 RID: 3110
		public ThingCategoryDef parent;

		// Token: 0x04000C27 RID: 3111
		[NoTranslate]
		public string iconPath;

		// Token: 0x04000C28 RID: 3112
		public bool resourceReadoutRoot;

		// Token: 0x04000C29 RID: 3113
		[Unsaved(false)]
		public TreeNode_ThingCategory treeNode;

		// Token: 0x04000C2A RID: 3114
		[Unsaved(false)]
		public List<ThingCategoryDef> childCategories = new List<ThingCategoryDef>();

		// Token: 0x04000C2B RID: 3115
		[Unsaved(false)]
		public List<ThingDef> childThingDefs = new List<ThingDef>();

		// Token: 0x04000C2C RID: 3116
		[Unsaved(false)]
		private HashSet<ThingDef> allChildThingDefsCached;

		// Token: 0x04000C2D RID: 3117
		[Unsaved(false)]
		private List<ThingDef> sortedChildThingDefsCached;

		// Token: 0x04000C2E RID: 3118
		[Unsaved(false)]
		public List<SpecialThingFilterDef> childSpecialFilters = new List<SpecialThingFilterDef>();

		// Token: 0x04000C2F RID: 3119
		[Unsaved(false)]
		public Texture2D icon = BaseContent.BadTex;
	}
}
