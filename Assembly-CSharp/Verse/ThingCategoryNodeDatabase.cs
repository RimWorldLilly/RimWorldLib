using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x0200062A RID: 1578
	public static class ThingCategoryNodeDatabase
	{
		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x0600318B RID: 12683 RVA: 0x0013D01A File Offset: 0x0013B21A
		public static TreeNode_ThingCategory RootNode
		{
			get
			{
				return ThingCategoryNodeDatabase.rootNode;
			}
		}

		// Token: 0x0600318C RID: 12684 RVA: 0x0013D021 File Offset: 0x0013B221
		public static void Clear()
		{
			ThingCategoryNodeDatabase.rootNode = null;
			ThingCategoryNodeDatabase.initialized = false;
		}

		// Token: 0x0600318D RID: 12685 RVA: 0x0013D030 File Offset: 0x0013B230
		public static void FinalizeInit()
		{
			ThingCategoryNodeDatabase.rootNode = ThingCategoryDefOf.Root.treeNode;
			foreach (ThingCategoryDef thingCategoryDef in DefDatabase<ThingCategoryDef>.AllDefs)
			{
				if (thingCategoryDef.parent != null)
				{
					thingCategoryDef.parent.childCategories.Add(thingCategoryDef);
				}
			}
			ThingCategoryNodeDatabase.SetNestLevelRecursive(ThingCategoryNodeDatabase.rootNode, 0);
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.thingCategories != null)
				{
					foreach (ThingCategoryDef thingCategoryDef2 in thingDef.thingCategories)
					{
						thingCategoryDef2.childThingDefs.Add(thingDef);
					}
				}
			}
			foreach (SpecialThingFilterDef specialThingFilterDef in DefDatabase<SpecialThingFilterDef>.AllDefs)
			{
				specialThingFilterDef.parentCategory.childSpecialFilters.Add(specialThingFilterDef);
			}
			if (ThingCategoryNodeDatabase.rootNode.catDef.childCategories.Any<ThingCategoryDef>())
			{
				ThingCategoryNodeDatabase.rootNode.catDef.childCategories[0].treeNode.SetOpen(-1, true);
			}
			ThingCategoryNodeDatabase.allThingCategoryNodes = ThingCategoryNodeDatabase.rootNode.ChildCategoryNodesAndThis.ToList<TreeNode_ThingCategory>();
			ThingCategoryNodeDatabase.initialized = true;
		}

		// Token: 0x0600318E RID: 12686 RVA: 0x0013D1C8 File Offset: 0x0013B3C8
		private static void SetNestLevelRecursive(TreeNode_ThingCategory node, int nestDepth)
		{
			foreach (ThingCategoryDef thingCategoryDef in node.catDef.childCategories)
			{
				thingCategoryDef.treeNode.nestDepth = nestDepth;
				ThingCategoryNodeDatabase.SetNestLevelRecursive(thingCategoryDef.treeNode, nestDepth + 1);
			}
		}

		// Token: 0x04001FB5 RID: 8117
		public static bool initialized;

		// Token: 0x04001FB6 RID: 8118
		private static TreeNode_ThingCategory rootNode;

		// Token: 0x04001FB7 RID: 8119
		public static List<TreeNode_ThingCategory> allThingCategoryNodes;
	}
}
