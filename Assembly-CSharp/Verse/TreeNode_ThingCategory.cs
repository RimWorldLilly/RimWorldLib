using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200062B RID: 1579
	public class TreeNode_ThingCategory : TreeNode
	{
		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x0600318F RID: 12687 RVA: 0x0013D234 File Offset: 0x0013B434
		public string Label
		{
			get
			{
				return this.catDef.label;
			}
		}

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x06003190 RID: 12688 RVA: 0x0013D241 File Offset: 0x0013B441
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x06003191 RID: 12689 RVA: 0x0013D24E File Offset: 0x0013B44E
		public IEnumerable<TreeNode_ThingCategory> ChildCategoryNodesAndThis
		{
			get
			{
				TreeNode_ThingCategory.<get_ChildCategoryNodesAndThis>d__6 <get_ChildCategoryNodesAndThis>d__ = new TreeNode_ThingCategory.<get_ChildCategoryNodesAndThis>d__6(-2);
				<get_ChildCategoryNodesAndThis>d__.<>4__this = this;
				return <get_ChildCategoryNodesAndThis>d__;
			}
		}

		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x06003192 RID: 12690 RVA: 0x0013D25E File Offset: 0x0013B45E
		public IEnumerable<TreeNode_ThingCategory> ChildCategoryNodes
		{
			get
			{
				TreeNode_ThingCategory.<get_ChildCategoryNodes>d__8 <get_ChildCategoryNodes>d__ = new TreeNode_ThingCategory.<get_ChildCategoryNodes>d__8(-2);
				<get_ChildCategoryNodes>d__.<>4__this = this;
				return <get_ChildCategoryNodes>d__;
			}
		}

		// Token: 0x06003193 RID: 12691 RVA: 0x0013D26E File Offset: 0x0013B46E
		public TreeNode_ThingCategory(ThingCategoryDef def)
		{
			this.catDef = def;
		}

		// Token: 0x06003194 RID: 12692 RVA: 0x0013D27D File Offset: 0x0013B47D
		public override string ToString()
		{
			return this.catDef.defName;
		}

		// Token: 0x04001FB8 RID: 8120
		public ThingCategoryDef catDef;
	}
}
