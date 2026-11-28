using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001CAC RID: 7340
	public class Dialog_EntityCodex : Window
	{
		// Token: 0x1700222C RID: 8748
		// (get) Token: 0x0600BC15 RID: 48149 RVA: 0x0043134A File Offset: 0x0042F54A
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(980f, 724f);
			}
		}

		// Token: 0x0600BC16 RID: 48150 RVA: 0x0043135C File Offset: 0x0042F55C
		public Dialog_EntityCodex(EntityCodexEntryDef selectedEntry = null) : base(null)
		{
			this.doCloseX = true;
			this.doCloseButton = true;
			this.forcePause = true;
			this.categoriesInOrder = (from x in DefDatabase<EntityCategoryDef>.AllDefsListForReading
			where DefDatabase<EntityCodexEntryDef>.AllDefs.Any((EntityCodexEntryDef y) => y.category == x && y.Visible)
			orderby x.listOrder
			select x).ToList<EntityCategoryDef>();
			foreach (EntityCategoryDef key in this.categoriesInOrder)
			{
				this.entriesByCategory.Add(key, new List<EntityCodexEntryDef>());
				this.categoryRectSizes.Add(key, 0f);
			}
			foreach (EntityCodexEntryDef entityCodexEntryDef in DefDatabase<EntityCodexEntryDef>.AllDefsListForReading)
			{
				if (entityCodexEntryDef.Visible)
				{
					this.entriesByCategory[entityCodexEntryDef.category].Add(entityCodexEntryDef);
				}
			}
			foreach (KeyValuePair<EntityCategoryDef, List<EntityCodexEntryDef>> keyValuePair in this.entriesByCategory)
			{
				EntityCategoryDef entityCategoryDef;
				List<EntityCodexEntryDef> list;
				keyValuePair.Deconstruct(out entityCategoryDef, out list);
				list.SortBy((EntityCodexEntryDef e) => e.orderInCategory, (EntityCodexEntryDef e) => e.label);
			}
			EntityCodexEntryDef entityCodexEntryDef2 = selectedEntry;
			if (selectedEntry == null)
			{
				entityCodexEntryDef2 = (from x in DefDatabase<EntityCodexEntryDef>.AllDefs
				orderby x.label
				select x).FirstOrDefault((EntityCodexEntryDef x) => x.Discovered);
			}
			this.selectedEntry = entityCodexEntryDef2;
		}

		// Token: 0x0600BC17 RID: 48151 RVA: 0x00431594 File Offset: 0x0042F794
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			Rect rect = inRect;
			rect.height -= Dialog_EntityCodex.ButSize.y + 10f;
			using (new TextBlock(GameFont.Medium))
			{
				Widgets.Label(new Rect(0f, 0f, rect.width, 30f), "EntityCodex".Translate());
			}
			if (Prefs.DevMode && DebugSettings.godMode)
			{
				Widgets.CheckboxLabeled(new Rect(rect.xMax - 150f, 0f, 150f, 30f), "DEV: Show all", ref this.devShowAll, false, null, null, true, false);
			}
			rect.yMin += 40f;
			TaggedString taggedString = "EntityCodexDesc".Translate();
			float num = Text.CalcHeight(taggedString, rect.width);
			Widgets.Label(new Rect(0f, rect.y, rect.width, num), taggedString);
			rect.yMin += num + 10f;
			Rect inRect2 = rect.LeftPart(0.35f);
			Rect rect2 = rect.RightPart(0.65f);
			this.LeftRect(inRect2);
			this.RightRect(rect2);
		}

		// Token: 0x0600BC18 RID: 48152 RVA: 0x004316EC File Offset: 0x0042F8EC
		private void LeftRect(Rect inRect)
		{
			Rect viewRect = new Rect(0f, 0f, inRect.width - 16f, this.leftScrollHeight);
			Widgets.BeginScrollView(inRect, ref this.leftScrollPos, viewRect, true);
			if (this.selectedEntry != null)
			{
				float num = 0f;
				bool flag = this.devShowAll || this.selectedEntry.Discovered;
				using (new TextBlock(GameFont.Medium))
				{
					Widgets.Label(new Rect(0f, num, viewRect.width, 30f), flag ? this.selectedEntry.LabelCap : "UndiscoveredEntity".Translate());
					num += 40f;
				}
				using (new TextBlock(true))
				{
					string text = flag ? this.selectedEntry.Description : "UndiscoveredEntityDesc".Translate();
					float num2 = Text.CalcHeight(text, viewRect.width);
					Widgets.Label(new Rect(0f, num, viewRect.width, num2), text);
					num += num2 + 10f;
				}
				if (flag)
				{
					if (this.selectedEntry.linkedThings.Count > 0)
					{
						foreach (ThingDef def in this.selectedEntry.linkedThings)
						{
							Rect rect = new Rect(0f, num, viewRect.width, Text.LineHeight);
							if (this.devShowAll || Find.EntityCodex.Discovered(def))
							{
								Widgets.HyperlinkWithIcon(rect, new Dialog_InfoCard.Hyperlink(def, -1), null, 2f, 6f, null, false, null);
							}
							else
							{
								rect.xMin += rect.height;
								using (new TextBlock(ColoredText.SubtleGrayColor))
								{
									Widgets.Label(rect, "Undiscovered".Translate());
								}
							}
							num += rect.height;
						}
						num += 10f;
					}
					if (this.selectedEntry.discoveredResearchProjects.Count <= 0)
					{
						goto IL_3ED;
					}
					Widgets.Label(new Rect(0f, num, viewRect.width, Text.LineHeight), "ResearchUnlocks".Translate() + ":");
					num += Text.LineHeight;
					using (List<ResearchProjectDef>.Enumerator enumerator2 = this.selectedEntry.discoveredResearchProjects.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							ResearchProjectDef researchProjectDef = enumerator2.Current;
							Rect rect2 = new Rect(0f, num, viewRect.width, Text.LineHeight);
							if (Widgets.ButtonText(rect2, "ViewHyperlink".Translate(researchProjectDef.LabelCap), false, true, true, null))
							{
								this.Close(true);
								Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Research, true);
								((MainTabWindow_Research)MainButtonDefOf.Research.TabWindow).Select(researchProjectDef);
							}
							num += rect2.height;
						}
						goto IL_3ED;
					}
				}
				if (Prefs.DevMode && DebugSettings.godMode)
				{
					if (Widgets.ButtonText(new Rect(0f, num, viewRect.width, Dialog_EntityCodex.ButSize.y), "DEV: Discover", true, true, true, null))
					{
						if (!this.selectedEntry.linkedThings.NullOrEmpty<ThingDef>())
						{
							for (int i = 0; i < this.selectedEntry.linkedThings.Count; i++)
							{
								Find.EntityCodex.SetDiscovered(this.selectedEntry, this.selectedEntry.linkedThings[i], null);
							}
						}
						else
						{
							Find.EntityCodex.SetDiscovered(this.selectedEntry, null, null);
						}
					}
					num += Dialog_EntityCodex.ButSize.y;
				}
				IL_3ED:
				this.leftScrollHeight = num;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x0600BC19 RID: 48153 RVA: 0x00431B34 File Offset: 0x0042FD34
		private void RightRect(Rect rect)
		{
			Rect viewRect = new Rect(0f, 0f, rect.width - 16f, this.rightScrollHeight);
			Widgets.BeginScrollView(rect, ref this.rightScrollPos, viewRect, true);
			float num = 0f;
			foreach (EntityCategoryDef entityCategoryDef in this.categoriesInOrder)
			{
				float num2 = num;
				float height = this.categoryRectSizes[entityCategoryDef];
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
				Widgets.DrawHighlight(new Rect(0f, num, rect.width, height));
				GUI.color = Color.white;
				Widgets.Label(new Rect(10f, num, rect.width, Text.LineHeight), entityCategoryDef.LabelCap);
				num += Text.LineHeight + 4f;
				List<EntityCodexEntryDef> list = this.entriesByCategory[entityCategoryDef];
				int num3 = Mathf.Min(Mathf.CeilToInt(Mathf.Sqrt((float)list.Count)) + 1, 7);
				int num4 = Mathf.CeilToInt((float)list.Count / (float)num3);
				for (int i = 0; i < list.Count; i++)
				{
					EntityCodexEntryDef entityCodexEntryDef = list[i];
					int num5 = i / num3;
					int num6 = i % num3;
					int num7 = (i >= list.Count - list.Count % num3) ? (list.Count % num3) : num3;
					float num8 = (viewRect.width - (float)num7 * 74f - (float)(num7 - 1) * 10f) / 2f;
					Rect rect2 = new Rect(num8 + (float)num6 * 74f + (float)num6 * 10f, num + (float)num5 * 74f + (float)num5 * 10f, 74f, 74f);
					bool flag = this.devShowAll || entityCodexEntryDef.Discovered;
					this.DrawEntry(rect2, entityCodexEntryDef, flag);
					if (flag)
					{
						Text.Font = GameFont.Tiny;
						float num9 = Text.CalcHeight(entityCodexEntryDef.LabelCap, rect2.width);
						Rect rect3 = new Rect(rect2.x, rect2.yMax - num9, rect2.width, num9);
						Widgets.DrawBoxSolid(rect3, new Color(0f, 0f, 0f, 0.3f));
						using (new TextBlock(TextAnchor.MiddleCenter))
						{
							Widgets.Label(rect3, entityCodexEntryDef.LabelCap);
						}
						Text.Font = GameFont.Small;
					}
				}
				num += 10f + (float)num4 * 74f + (float)(num4 - 1) * 10f;
				this.categoryRectSizes[entityCategoryDef] = num - num2;
				num += 10f;
			}
			this.rightScrollHeight = num;
			Widgets.EndScrollView();
		}

		// Token: 0x0600BC1A RID: 48154 RVA: 0x00431E54 File Offset: 0x00430054
		private void DrawEntry(Rect rect, EntityCodexEntryDef entry, bool discovered)
		{
			Widgets.DrawOptionBackground(rect, entry == this.selectedEntry);
			GUI.DrawTexture(rect.ContractedBy(2f), discovered ? entry.icon : entry.silhouette);
			if (Widgets.ButtonInvisible(rect, true))
			{
				this.selectedEntry = entry;
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x04006B61 RID: 27489
		private Vector2 leftScrollPos;

		// Token: 0x04006B62 RID: 27490
		private Vector2 rightScrollPos;

		// Token: 0x04006B63 RID: 27491
		private float leftScrollHeight;

		// Token: 0x04006B64 RID: 27492
		private float rightScrollHeight;

		// Token: 0x04006B65 RID: 27493
		private EntityCodexEntryDef selectedEntry;

		// Token: 0x04006B66 RID: 27494
		private List<EntityCategoryDef> categoriesInOrder;

		// Token: 0x04006B67 RID: 27495
		private Dictionary<EntityCategoryDef, List<EntityCodexEntryDef>> entriesByCategory = new Dictionary<EntityCategoryDef, List<EntityCodexEntryDef>>();

		// Token: 0x04006B68 RID: 27496
		private Dictionary<EntityCategoryDef, float> categoryRectSizes = new Dictionary<EntityCategoryDef, float>();

		// Token: 0x04006B69 RID: 27497
		private bool devShowAll;

		// Token: 0x04006B6A RID: 27498
		private static readonly Vector2 ButSize = new Vector2(150f, 38f);

		// Token: 0x04006B6B RID: 27499
		private const float HeaderHeight = 30f;

		// Token: 0x04006B6C RID: 27500
		private const float LeftRectWidthPct = 0.35f;

		// Token: 0x04006B6D RID: 27501
		private const float EntrySize = 74f;

		// Token: 0x04006B6E RID: 27502
		private const float EntryGap = 10f;

		// Token: 0x04006B6F RID: 27503
		private const int MaxEntriesPerRow = 7;
	}
}
