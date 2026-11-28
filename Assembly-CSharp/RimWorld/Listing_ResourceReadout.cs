using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DE5 RID: 7653
	[StaticConstructorOnStartup]
	public class Listing_ResourceReadout : Listing_Tree
	{
		// Token: 0x170024CA RID: 9418
		// (get) Token: 0x0600C6FE RID: 50942 RVA: 0x0048DF6B File Offset: 0x0048C16B
		protected override float LabelWidth
		{
			get
			{
				return base.ColumnWidth;
			}
		}

		// Token: 0x0600C6FF RID: 50943 RVA: 0x0048DF73 File Offset: 0x0048C173
		public Listing_ResourceReadout(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600C700 RID: 50944 RVA: 0x0048DF84 File Offset: 0x0048C184
		public void DoCategory(TreeNode_ThingCategory node, int nestLevel, int openMask)
		{
			int countIn = this.map.resourceCounter.GetCountIn(node.catDef);
			if (countIn == 0)
			{
				return;
			}
			base.OpenCloseWidget(node, nestLevel, openMask);
			Rect rect = new Rect(0f, this.curY, this.LabelWidth, this.lineHeight)
			{
				xMin = base.XAtIndentLevel(nestLevel) + 18f
			};
			Rect position = rect;
			position.width = 80f;
			position.yMax -= 3f;
			position.yMin += 3f;
			GUI.DrawTexture(position, Listing_ResourceReadout.SolidCategoryBG);
			if (Mouse.IsOver(rect))
			{
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, new TipSignal(node.catDef.LabelCap, node.catDef.GetHashCode()));
			}
			Rect position2 = new Rect(rect);
			position2.width = (position2.height = 28f);
			position2.y = rect.y + rect.height / 2f - position2.height / 2f;
			GUI.DrawTexture(position2, node.catDef.icon);
			Widgets.Label(new Rect(rect)
			{
				xMin = position2.xMax + 6f
			}, countIn.ToStringCached());
			base.EndLine();
			if (this.IsOpen(node, openMask))
			{
				this.DoCategoryChildren(node, nestLevel + 1, openMask);
			}
		}

		// Token: 0x0600C701 RID: 50945 RVA: 0x0048E108 File Offset: 0x0048C308
		public void DoCategoryChildren(TreeNode_ThingCategory node, int indentLevel, int openMask)
		{
			foreach (TreeNode_ThingCategory treeNode_ThingCategory in node.ChildCategoryNodes)
			{
				if (!treeNode_ThingCategory.catDef.resourceReadoutRoot)
				{
					this.DoCategory(treeNode_ThingCategory, indentLevel, openMask);
				}
			}
			foreach (ThingDef thingDef in node.catDef.childThingDefs)
			{
				if (thingDef.PlayerAcquirable)
				{
					this.DoThingDef(thingDef, indentLevel + 1);
				}
			}
		}

		// Token: 0x0600C702 RID: 50946 RVA: 0x0048E1B8 File Offset: 0x0048C3B8
		private void DoThingDef(ThingDef thingDef, int nestLevel)
		{
			int count = this.map.resourceCounter.GetCount(thingDef);
			if (count == 0)
			{
				return;
			}
			Rect rect = new Rect(0f, this.curY, this.LabelWidth, this.lineHeight)
			{
				xMin = base.XAtIndentLevel(nestLevel) + 18f
			};
			if (Mouse.IsOver(rect))
			{
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, new TipSignal(() => thingDef.LabelCap + ": " + thingDef.description.CapitalizeFirst(), (int)thingDef.shortHash));
			}
			Rect rect2 = new Rect(rect);
			rect2.width = (rect2.height = 28f);
			rect2.y = rect.y + rect.height / 2f - rect2.height / 2f;
			Widgets.ThingIcon(rect2, thingDef, null, null, 1f, null, null, 1f);
			Widgets.Label(new Rect(rect)
			{
				xMin = rect2.xMax + 6f
			}, count.ToStringCached());
			base.EndLine();
		}

		// Token: 0x04007129 RID: 28969
		private Map map;

		// Token: 0x0400712A RID: 28970
		private static Texture2D SolidCategoryBG = SolidColorMaterials.NewSolidColorTexture(new Color(0.1f, 0.1f, 0.1f, 0.6f));
	}
}
