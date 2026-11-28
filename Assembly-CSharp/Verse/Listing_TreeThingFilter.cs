using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000629 RID: 1577
	public class Listing_TreeThingFilter : Listing_Tree
	{
		// Token: 0x06003172 RID: 12658 RVA: 0x0013C2A8 File Offset: 0x0013A4A8
		public Listing_TreeThingFilter(ThingFilter filter, ThingFilter parentFilter, IEnumerable<ThingDef> forceHiddenDefs, IEnumerable<SpecialThingFilterDef> forceHiddenFilters, List<ThingDef> suppressSmallVolumeTags, QuickSearchFilter searchFilter)
		{
			this.filter = filter;
			this.parentFilter = parentFilter;
			if (forceHiddenDefs != null)
			{
				this.forceHiddenDefs = forceHiddenDefs.ToList<ThingDef>();
			}
			if (forceHiddenFilters != null)
			{
				this.tempForceHiddenSpecialFilters = forceHiddenFilters.ToList<SpecialThingFilterDef>();
			}
			this.suppressSmallVolumeTags = suppressSmallVolumeTags;
			this.searchFilter = searchFilter;
		}

		// Token: 0x06003173 RID: 12659 RVA: 0x0013C2FC File Offset: 0x0013A4FC
		public void ListCategoryChildren(TreeNode_ThingCategory node, int openMask, Map map, Rect visibleRect)
		{
			this.visibleRect = visibleRect;
			int num = 0;
			foreach (SpecialThingFilterDef specialThingFilterDef in node.catDef.ParentsSpecialThingFilterDefs)
			{
				if (this.Visible(specialThingFilterDef, node))
				{
					this.DoSpecialFilter(specialThingFilterDef, num);
				}
			}
			this.DoCategoryChildren(node, num, openMask, map, false);
		}

		// Token: 0x06003174 RID: 12660 RVA: 0x0013C370 File Offset: 0x0013A570
		private void DoCategoryChildren(TreeNode_ThingCategory node, int indentLevel, int openMask, Map map, bool subtreeMatchedSearch)
		{
			Listing_TreeThingFilter.<>c__DisplayClass15_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.subtreeMatchedSearch = subtreeMatchedSearch;
			List<SpecialThingFilterDef> childSpecialFilters = node.catDef.childSpecialFilters;
			for (int i = 0; i < childSpecialFilters.Count; i++)
			{
				if (this.Visible(childSpecialFilters[i], node))
				{
					this.DoSpecialFilter(childSpecialFilters[i], indentLevel);
				}
			}
			foreach (TreeNode_ThingCategory treeNode_ThingCategory in node.ChildCategoryNodes)
			{
				if (this.Visible(treeNode_ThingCategory) && !this.<DoCategoryChildren>g__HideCategoryDueToSearch|15_0(treeNode_ThingCategory, ref CS$<>8__locals1))
				{
					this.DoCategory(treeNode_ThingCategory, indentLevel, openMask, map, CS$<>8__locals1.subtreeMatchedSearch);
				}
			}
			List<ThingDef> list = new List<ThingDef>();
			foreach (ThingDef thingDef in node.catDef.SortedChildThingDefs)
			{
				if (Find.HiddenItemsManager.Hidden(thingDef))
				{
					list.Add(thingDef);
				}
				else if (this.Visible(thingDef) && !this.<DoCategoryChildren>g__HideThingDueToSearch|15_1(thingDef, ref CS$<>8__locals1))
				{
					this.DoThingDef(thingDef, indentLevel, map);
				}
			}
			if (!this.searchFilter.Active && list.Count > 0)
			{
				this.DoUndiscoveredEntry(indentLevel, node.catDef.parent != ThingCategoryDefOf.Corpses, list);
			}
		}

		// Token: 0x06003175 RID: 12661 RVA: 0x0013C4E4 File Offset: 0x0013A6E4
		private void DoSpecialFilter(SpecialThingFilterDef sfDef, int nestLevel)
		{
			if (!sfDef.configurable)
			{
				return;
			}
			Color? textColor = null;
			if (this.searchFilter.Matches(sfDef))
			{
				this.matchCount++;
			}
			else
			{
				textColor = new Color?(Listing_TreeThingFilter.NoMatchColor);
			}
			if (this.CurrentRowVisibleOnScreen())
			{
				base.LabelLeft("*" + sfDef.LabelCap, sfDef.description, nestLevel, 0f, textColor, 0f);
				bool flag = this.filter.Allows(sfDef);
				bool flag2 = flag;
				Widgets.Checkbox(new Vector2(this.LabelWidth, this.curY), ref flag, this.lineHeight, false, true, null, null);
				if (flag != flag2)
				{
					this.filter.SetAllow(sfDef, flag);
				}
			}
			base.EndLine();
		}

		// Token: 0x06003176 RID: 12662 RVA: 0x0013C5AC File Offset: 0x0013A7AC
		private void DoCategory(TreeNode_ThingCategory node, int indentLevel, int openMask, Map map, bool subtreeMatchedSearch)
		{
			Color? textColor = null;
			if (this.searchFilter.Active)
			{
				if (this.CategoryMatches(node))
				{
					subtreeMatchedSearch = true;
					this.matchCount++;
				}
				else
				{
					textColor = new Color?(Listing_TreeThingFilter.NoMatchColor);
				}
			}
			if (this.CurrentRowVisibleOnScreen())
			{
				base.OpenCloseWidget(node, indentLevel, openMask);
				base.LabelLeft(node.LabelCap, node.catDef.description, indentLevel, 0f, textColor, 0f);
				MultiCheckboxState multiCheckboxState = this.AllowanceStateOf(node);
				MultiCheckboxState multiCheckboxState2 = Widgets.CheckboxMulti(new Rect(this.LabelWidth, this.curY, this.lineHeight, this.lineHeight), multiCheckboxState, true);
				if (multiCheckboxState != multiCheckboxState2)
				{
					this.filter.SetAllow(node.catDef, multiCheckboxState2 == MultiCheckboxState.On, this.forceHiddenDefs, this.hiddenSpecialFilters);
				}
			}
			base.EndLine();
			if (this.IsOpen(node, openMask))
			{
				this.DoCategoryChildren(node, indentLevel + 1, openMask, map, subtreeMatchedSearch);
			}
		}

		// Token: 0x06003177 RID: 12663 RVA: 0x0013C69C File Offset: 0x0013A89C
		private void DoThingDef(ThingDef tDef, int nestLevel, Map map)
		{
			Color? color = null;
			if (this.searchFilter.Matches(tDef))
			{
				this.matchCount++;
			}
			else
			{
				color = new Color?(Listing_TreeThingFilter.NoMatchColor);
			}
			if (tDef.uiIcon != null && tDef.uiIcon != BaseContent.BadTex)
			{
				nestLevel++;
				Widgets.DefIcon(new Rect(base.XAtIndentLevel(nestLevel) - 6f, this.curY, 20f, 20f), tDef, null, 1f, null, true, color, null, null, 1f);
			}
			if (this.CurrentRowVisibleOnScreen())
			{
				object obj = (this.suppressSmallVolumeTags == null || !this.suppressSmallVolumeTags.Contains(tDef)) && tDef.IsStuff && tDef.smallVolume;
				string text = tDef.DescriptionDetailed;
				object obj2 = obj;
				if (obj2 != null)
				{
					text += "\n\n" + "ThisIsSmallVolume".Translate(10.ToStringCached());
				}
				float num = -4f;
				if (obj2 != null)
				{
					Rect rect = new Rect(this.LabelWidth - 19f, this.curY, 19f, 20f);
					Text.Font = GameFont.Small;
					Text.Anchor = TextAnchor.UpperRight;
					GUI.color = Color.gray;
					Widgets.Label(rect, "/" + 10.ToStringCached());
					Text.Font = GameFont.Small;
					GenUI.ResetLabelAlign();
					GUI.color = Color.white;
				}
				num -= 19f;
				if (map != null)
				{
					int count = map.resourceCounter.GetCount(tDef);
					if (count > 0)
					{
						string text2 = count.ToStringCached();
						Rect rect2 = new Rect(0f, this.curY, this.LabelWidth + num, 40f);
						Text.Font = GameFont.Small;
						Text.Anchor = TextAnchor.UpperRight;
						GUI.color = new Color(0.5f, 0.5f, 0.1f);
						Widgets.Label(rect2, text2);
						num -= Text.CalcSize(text2).x;
						GenUI.ResetLabelAlign();
						Text.Font = GameFont.Small;
						GUI.color = Color.white;
					}
				}
				base.LabelLeft(tDef.LabelCap, text, nestLevel, num, color, 0f);
				bool flag = this.filter.Allows(tDef);
				bool flag2 = flag;
				Widgets.Checkbox(new Vector2(this.LabelWidth, this.curY), ref flag, this.lineHeight, false, true, null, null);
				if (flag != flag2)
				{
					this.filter.SetAllow(tDef, flag);
				}
			}
			base.EndLine();
		}

		// Token: 0x06003178 RID: 12664 RVA: 0x0013C914 File Offset: 0x0013AB14
		private void DoUndiscoveredEntry(int nestLevel, bool useIconOffset, List<ThingDef> toggledThingDefs)
		{
			if (this.CurrentRowVisibleOnScreen())
			{
				TaggedString taggedString = "UndiscoveredItemLabel".Translate();
				string tipText = "UndiscoveredItemDesc".Translate().Resolve();
				base.LabelLeft(taggedString, tipText, nestLevel, 0f, null, useIconOffset ? 10f : 0f);
				bool flag = this.filter.Allows(toggledThingDefs[0]);
				bool flag2 = flag;
				Widgets.Checkbox(new Vector2(this.LabelWidth, this.curY), ref flag, this.lineHeight, false, true, null, null);
				if (flag != flag2)
				{
					foreach (ThingDef thingDef in toggledThingDefs)
					{
						this.filter.SetAllow(thingDef, flag);
					}
				}
			}
			base.EndLine();
		}

		// Token: 0x06003179 RID: 12665 RVA: 0x0013CA04 File Offset: 0x0013AC04
		public MultiCheckboxState AllowanceStateOf(TreeNode_ThingCategory cat)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			foreach (ThingDef thingDef in cat.catDef.DescendantThingDefs)
			{
				if (this.Visible(thingDef))
				{
					num++;
					if (this.filter.Allows(thingDef))
					{
						num2++;
					}
				}
			}
			foreach (SpecialThingFilterDef specialThingFilterDef in cat.catDef.DescendantSpecialThingFilterDefs)
			{
				if (this.Visible(specialThingFilterDef, cat))
				{
					num3++;
					if (this.filter.Allows(specialThingFilterDef))
					{
						num4++;
					}
				}
			}
			if (this.filter.OnlySpecialFilters)
			{
				if (num4 == 0)
				{
					return MultiCheckboxState.Off;
				}
				if (num4 < num3)
				{
					return MultiCheckboxState.Partial;
				}
				return MultiCheckboxState.On;
			}
			else
			{
				if (num2 == 0)
				{
					return MultiCheckboxState.Off;
				}
				if (num == num2 && num3 == num4)
				{
					return MultiCheckboxState.On;
				}
				return MultiCheckboxState.Partial;
			}
		}

		// Token: 0x0600317A RID: 12666 RVA: 0x0013CB0C File Offset: 0x0013AD0C
		private bool Visible(ThingDef td)
		{
			if (!td.PlayerAcquirable)
			{
				return false;
			}
			if (td.virtualDefParent != null)
			{
				return false;
			}
			if (this.forceHiddenDefs != null && this.forceHiddenDefs.Contains(td))
			{
				return false;
			}
			if (this.parentFilter != null)
			{
				if (!this.parentFilter.Allows(td))
				{
					return false;
				}
				if (this.parentFilter.IsAlwaysDisallowedDueToSpecialFilters(td))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600317B RID: 12667 RVA: 0x0013CB70 File Offset: 0x0013AD70
		public override bool IsOpen(TreeNode node, int openMask)
		{
			if (base.IsOpen(node, openMask))
			{
				return true;
			}
			TreeNode_ThingCategory treeNode_ThingCategory = node as TreeNode_ThingCategory;
			return treeNode_ThingCategory != null && this.searchFilter.Active && this.ThisOrDescendantsVisibleAndMatchesSearch(treeNode_ThingCategory);
		}

		// Token: 0x0600317C RID: 12668 RVA: 0x0013CBAC File Offset: 0x0013ADAC
		private bool ThisOrDescendantsVisibleAndMatchesSearch(TreeNode_ThingCategory node)
		{
			if (this.Visible(node) && this.CategoryMatches(node))
			{
				return true;
			}
			foreach (ThingDef td in node.catDef.childThingDefs)
			{
				if (this.<ThisOrDescendantsVisibleAndMatchesSearch>g__ThingDefVisibleAndMatches|23_0(td))
				{
					return true;
				}
			}
			foreach (SpecialThingFilterDef sf in node.catDef.childSpecialFilters)
			{
				if (this.<ThisOrDescendantsVisibleAndMatchesSearch>g__SpecialFilterVisibleAndMatches|23_1(sf, node))
				{
					return true;
				}
			}
			foreach (ThingCategoryDef thingCategoryDef in node.catDef.childCategories)
			{
				if (this.ThisOrDescendantsVisibleAndMatchesSearch(thingCategoryDef.treeNode))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600317D RID: 12669 RVA: 0x0013CCCC File Offset: 0x0013AECC
		private bool CategoryMatches(TreeNode_ThingCategory node)
		{
			return this.searchFilter.Matches(node.catDef.label);
		}

		// Token: 0x0600317E RID: 12670 RVA: 0x0013CCE4 File Offset: 0x0013AEE4
		private bool Visible(TreeNode_ThingCategory node)
		{
			if (this.filter.OnlySpecialFilters)
			{
				return node.catDef.DescendantSpecialThingFilterDefs.Any(new Func<SpecialThingFilterDef, bool>(this.Visible));
			}
			return node.catDef.DescendantThingDefs.Any(new Func<ThingDef, bool>(this.Visible));
		}

		// Token: 0x0600317F RID: 12671 RVA: 0x0013CD37 File Offset: 0x0013AF37
		private bool Visible(SpecialThingFilterDef f)
		{
			return this.parentFilter == null || this.parentFilter.Allows(f);
		}

		// Token: 0x06003180 RID: 12672 RVA: 0x0013CD54 File Offset: 0x0013AF54
		private bool Visible(SpecialThingFilterDef filterDef, TreeNode_ThingCategory node)
		{
			if (this.parentFilter != null && !this.parentFilter.Allows(filterDef))
			{
				return false;
			}
			if (this.filter.OnlySpecialFilters)
			{
				return true;
			}
			if (this.hiddenSpecialFilters == null)
			{
				this.CalculateHiddenSpecialFilters(node);
			}
			for (int i = 0; i < this.hiddenSpecialFilters.Count; i++)
			{
				if (this.hiddenSpecialFilters[i] == filterDef)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003181 RID: 12673 RVA: 0x0013CDC0 File Offset: 0x0013AFC0
		private bool CurrentRowVisibleOnScreen()
		{
			Rect other = new Rect(0f, this.curY, base.ColumnWidth, this.lineHeight);
			return this.visibleRect.Overlaps(other);
		}

		// Token: 0x06003182 RID: 12674 RVA: 0x0013CDF7 File Offset: 0x0013AFF7
		private void CalculateHiddenSpecialFilters(TreeNode_ThingCategory node)
		{
			this.hiddenSpecialFilters = Listing_TreeThingFilter.GetCachedHiddenSpecialFilters(node, this.parentFilter);
			if (this.tempForceHiddenSpecialFilters == null)
			{
				return;
			}
			this.hiddenSpecialFilters = new List<SpecialThingFilterDef>(this.hiddenSpecialFilters);
			this.hiddenSpecialFilters.AddRange(this.tempForceHiddenSpecialFilters);
		}

		// Token: 0x06003183 RID: 12675 RVA: 0x0013CE38 File Offset: 0x0013B038
		private static List<SpecialThingFilterDef> GetCachedHiddenSpecialFilters(TreeNode_ThingCategory node, ThingFilter parentFilter)
		{
			ValueTuple<TreeNode_ThingCategory, ThingFilter> key = new ValueTuple<TreeNode_ThingCategory, ThingFilter>(node, parentFilter);
			List<SpecialThingFilterDef> list;
			if (Listing_TreeThingFilter.cachedHiddenSpecialFilters.TryGetValue(key, out list))
			{
				return list;
			}
			list = Listing_TreeThingFilter.CalculateHiddenSpecialFilters(node, parentFilter);
			Listing_TreeThingFilter.cachedHiddenSpecialFilters.Add(key, list);
			return list;
		}

		// Token: 0x06003184 RID: 12676 RVA: 0x0013CE74 File Offset: 0x0013B074
		private static List<SpecialThingFilterDef> CalculateHiddenSpecialFilters(TreeNode_ThingCategory node, ThingFilter parentFilter)
		{
			List<SpecialThingFilterDef> list = new List<SpecialThingFilterDef>();
			IEnumerable<SpecialThingFilterDef> enumerable = node.catDef.ParentsSpecialThingFilterDefs.Concat(node.catDef.DescendantSpecialThingFilterDefs);
			IEnumerable<ThingDef> enumerable2 = node.catDef.DescendantThingDefs;
			if (parentFilter != null)
			{
				enumerable2 = enumerable2.Where(new Func<ThingDef, bool>(parentFilter.Allows));
			}
			foreach (SpecialThingFilterDef specialThingFilterDef in enumerable)
			{
				bool flag = false;
				foreach (ThingDef def in enumerable2)
				{
					if (specialThingFilterDef.Worker.CanEverMatch(def))
					{
						flag = true;
						break;
					}
				}
				if (parentFilter != null && parentFilter.hiddenSpecialFilters.Contains(specialThingFilterDef))
				{
					flag = false;
				}
				if (!flag)
				{
					list.Add(specialThingFilterDef);
				}
			}
			return list;
		}

		// Token: 0x06003185 RID: 12677 RVA: 0x0013CF64 File Offset: 0x0013B164
		public static void ResetStaticData()
		{
			Listing_TreeThingFilter.cachedHiddenSpecialFilters.Clear();
		}

		// Token: 0x06003187 RID: 12679 RVA: 0x0013CF8B File Offset: 0x0013B18B
		[CompilerGenerated]
		private bool <DoCategoryChildren>g__HideCategoryDueToSearch|15_0(TreeNode_ThingCategory subCat, ref Listing_TreeThingFilter.<>c__DisplayClass15_0 A_2)
		{
			return !(!this.searchFilter.Active | A_2.subtreeMatchedSearch) && !this.CategoryMatches(subCat) && !this.ThisOrDescendantsVisibleAndMatchesSearch(subCat);
		}

		// Token: 0x06003188 RID: 12680 RVA: 0x0013CFBD File Offset: 0x0013B1BD
		[CompilerGenerated]
		private bool <DoCategoryChildren>g__HideThingDueToSearch|15_1(ThingDef tDef, ref Listing_TreeThingFilter.<>c__DisplayClass15_0 A_2)
		{
			return !(!this.searchFilter.Active | A_2.subtreeMatchedSearch) && !this.searchFilter.Matches(tDef);
		}

		// Token: 0x06003189 RID: 12681 RVA: 0x0013CFE7 File Offset: 0x0013B1E7
		[CompilerGenerated]
		private bool <ThisOrDescendantsVisibleAndMatchesSearch>g__ThingDefVisibleAndMatches|23_0(ThingDef td)
		{
			return this.Visible(td) && this.searchFilter.Matches(td);
		}

		// Token: 0x0600318A RID: 12682 RVA: 0x0013D000 File Offset: 0x0013B200
		[CompilerGenerated]
		private bool <ThisOrDescendantsVisibleAndMatchesSearch>g__SpecialFilterVisibleAndMatches|23_1(SpecialThingFilterDef sf, TreeNode_ThingCategory subCat)
		{
			return this.Visible(sf, subCat) && this.searchFilter.Matches(sf);
		}

		// Token: 0x04001FA8 RID: 8104
		private static readonly Color NoMatchColor = Color.grey;

		// Token: 0x04001FA9 RID: 8105
		private static readonly LRUCache<ValueTuple<TreeNode_ThingCategory, ThingFilter>, List<SpecialThingFilterDef>> cachedHiddenSpecialFilters = new LRUCache<ValueTuple<TreeNode_ThingCategory, ThingFilter>, List<SpecialThingFilterDef>>(500);

		// Token: 0x04001FAA RID: 8106
		private ThingFilter filter;

		// Token: 0x04001FAB RID: 8107
		private ThingFilter parentFilter;

		// Token: 0x04001FAC RID: 8108
		private const float IconSize = 20f;

		// Token: 0x04001FAD RID: 8109
		private const float IconOffset = 6f;

		// Token: 0x04001FAE RID: 8110
		private List<SpecialThingFilterDef> hiddenSpecialFilters;

		// Token: 0x04001FAF RID: 8111
		private List<ThingDef> forceHiddenDefs;

		// Token: 0x04001FB0 RID: 8112
		private List<SpecialThingFilterDef> tempForceHiddenSpecialFilters;

		// Token: 0x04001FB1 RID: 8113
		private List<ThingDef> suppressSmallVolumeTags;

		// Token: 0x04001FB2 RID: 8114
		protected QuickSearchFilter searchFilter;

		// Token: 0x04001FB3 RID: 8115
		public int matchCount;

		// Token: 0x04001FB4 RID: 8116
		private Rect visibleRect;
	}
}
