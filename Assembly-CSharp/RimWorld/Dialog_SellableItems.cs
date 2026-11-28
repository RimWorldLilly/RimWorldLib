using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DF1 RID: 7665
	public class Dialog_SellableItems : Window
	{
		// Token: 0x170024CC RID: 9420
		// (get) Token: 0x0600C752 RID: 51026 RVA: 0x004924C8 File Offset: 0x004906C8
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(650f, (float)Mathf.Min(UI.screenHeight, 1000));
			}
		}

		// Token: 0x170024CD RID: 9421
		// (get) Token: 0x0600C753 RID: 51027 RVA: 0x004924E4 File Offset: 0x004906E4
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x0600C754 RID: 51028 RVA: 0x004924EC File Offset: 0x004906EC
		public Dialog_SellableItems(ITrader trader) : base(null)
		{
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.trader = trader;
			this.CalculateSellableItems(trader.TraderKind);
			this.CalculateTabs();
		}

		// Token: 0x0600C755 RID: 51029 RVA: 0x00492560 File Offset: 0x00490760
		public override void DoWindowContents(Rect inRect)
		{
			float num = 40f;
			Rect rect = new Rect(0f, 0f, inRect.width, 40f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, "SellableItemsTitle".Translate().CapitalizeFirst());
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			ITraderRestockingInfoProvider traderRestockingInfoProvider = this.trader as ITraderRestockingInfoProvider;
			if (traderRestockingInfoProvider != null)
			{
				int nextRestockTick = traderRestockingInfoProvider.NextRestockTick;
				if (nextRestockTick != -1)
				{
					float num2 = (nextRestockTick - Find.TickManager.TicksGame).TicksToDays();
					Widgets.Label(new Rect(0f, num, inRect.width, 20f), "NextTraderRestock".Translate(num2.ToString("0.0")));
					num += 20f;
				}
				else if (!traderRestockingInfoProvider.EverVisited)
				{
					Widgets.Label(new Rect(0f, num, inRect.width, 20f), "TraderNotVisitedYet".Translate());
					num += 20f;
				}
				else if (traderRestockingInfoProvider.RestockedSinceLastVisit)
				{
					Widgets.Label(new Rect(0f, num, inRect.width, 20f), "TraderRestockedSinceLastVisit".Translate());
					num += 20f;
				}
			}
			Text.Anchor = TextAnchor.UpperLeft;
			inRect.yMin += 64f + num;
			Widgets.DrawMenuSection(inRect);
			TabDrawer.DrawTabs<TabRecord>(inRect, this.tabs, 200f);
			inRect = inRect.ContractedBy(17f);
			Widgets.BeginGroup(inRect);
			Rect rect2 = inRect.AtZero();
			this.DoBottomButtons(rect2);
			Rect outRect = rect2;
			outRect.yMax -= 65f;
			List<ThingDef> sellableItemsInCategory = this.GetSellableItemsInCategory(this.currentCategory, this.pawnsTabOpen);
			if (sellableItemsInCategory.Any<ThingDef>())
			{
				float height = (float)sellableItemsInCategory.Count * 24f;
				num = 0f;
				Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, height);
				Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
				float num3 = this.scrollPosition.y - 24f;
				float num4 = this.scrollPosition.y + outRect.height;
				for (int i = 0; i < sellableItemsInCategory.Count; i++)
				{
					if (num > num3 && num < num4)
					{
						Widgets.DefLabelWithIcon(new Rect(0f, num, viewRect.width, 24f), sellableItemsInCategory[i], 2f, 6f);
					}
					num += 24f;
				}
				Widgets.EndScrollView();
			}
			else
			{
				Widgets.NoneLabel(0f, outRect.width, null);
			}
			Widgets.EndGroup();
		}

		// Token: 0x0600C756 RID: 51030 RVA: 0x0049280C File Offset: 0x00490A0C
		private void DoBottomButtons(Rect rect)
		{
			if (Widgets.ButtonText(new Rect(rect.width / 2f - this.BottomButtonSize.x / 2f, rect.height - 55f, this.BottomButtonSize.x, this.BottomButtonSize.y), "CloseButton".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
		}

		// Token: 0x0600C757 RID: 51031 RVA: 0x0049288C File Offset: 0x00490A8C
		private void CalculateSellableItems(TraderKindDef trader)
		{
			this.sellableItems.Clear();
			this.cachedSellableItemsByCategory.Clear();
			this.cachedSellablePawns = null;
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].PlayerAcquirable && !allDefsListForReading[i].IsCorpse && !typeof(MinifiedThing).IsAssignableFrom(allDefsListForReading[i].thingClass) && trader.WillTrade(allDefsListForReading[i]) && TradeUtility.EverPlayerSellable(allDefsListForReading[i]))
				{
					this.sellableItems.Add(allDefsListForReading[i]);
				}
			}
			this.sellableItems.SortBy((ThingDef x) => x.label);
		}

		// Token: 0x0600C758 RID: 51032 RVA: 0x00492960 File Offset: 0x00490B60
		private void CalculateTabs()
		{
			this.tabs.Clear();
			List<ThingCategoryDef> allDefsListForReading = DefDatabase<ThingCategoryDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ThingCategoryDef category = allDefsListForReading[i];
				if (category.parent == ThingCategoryDefOf.Root && category != ThingCategoryDefOf.Animals && this.GetSellableItemsInCategory(category, false).Count != 0)
				{
					if (this.currentCategory == null)
					{
						this.currentCategory = category;
					}
					this.tabs.Add(new TabRecord(category.LabelCap, delegate()
					{
						this.currentCategory = category;
						this.pawnsTabOpen = false;
					}, () => this.currentCategory == category));
				}
			}
			this.tabs.Add(new TabRecord("PawnsTabShort".Translate(), delegate()
			{
				this.currentCategory = null;
				this.pawnsTabOpen = true;
			}, () => this.pawnsTabOpen));
		}

		// Token: 0x0600C759 RID: 51033 RVA: 0x00492A68 File Offset: 0x00490C68
		private List<ThingDef> GetSellableItemsInCategory(ThingCategoryDef category, bool pawns)
		{
			if (pawns)
			{
				if (this.cachedSellablePawns == null)
				{
					this.cachedSellablePawns = new List<ThingDef>();
					for (int i = 0; i < this.sellableItems.Count; i++)
					{
						if (this.sellableItems[i].category == ThingCategory.Pawn && (!ModsConfig.AnomalyActive || this.sellableItems[i] != ThingDefOf.CreepJoiner))
						{
							this.cachedSellablePawns.Add(this.sellableItems[i]);
						}
					}
				}
				return this.cachedSellablePawns;
			}
			List<ThingDef> list;
			if (this.cachedSellableItemsByCategory.TryGetValue(category, out list))
			{
				return list;
			}
			list = new List<ThingDef>();
			for (int j = 0; j < this.sellableItems.Count; j++)
			{
				if (this.sellableItems[j].IsWithinCategory(category))
				{
					list.Add(this.sellableItems[j]);
				}
			}
			this.cachedSellableItemsByCategory.Add(category, list);
			return list;
		}

		// Token: 0x04007186 RID: 29062
		private ThingCategoryDef currentCategory;

		// Token: 0x04007187 RID: 29063
		private bool pawnsTabOpen;

		// Token: 0x04007188 RID: 29064
		private List<ThingDef> sellableItems = new List<ThingDef>();

		// Token: 0x04007189 RID: 29065
		private List<TabRecord> tabs = new List<TabRecord>();

		// Token: 0x0400718A RID: 29066
		private Vector2 scrollPosition;

		// Token: 0x0400718B RID: 29067
		private ITrader trader;

		// Token: 0x0400718C RID: 29068
		private List<ThingDef> cachedSellablePawns;

		// Token: 0x0400718D RID: 29069
		private Dictionary<ThingCategoryDef, List<ThingDef>> cachedSellableItemsByCategory = new Dictionary<ThingCategoryDef, List<ThingDef>>();

		// Token: 0x0400718E RID: 29070
		private const float RowHeight = 24f;

		// Token: 0x0400718F RID: 29071
		private const float TitleRectHeight = 40f;

		// Token: 0x04007190 RID: 29072
		private const float RestockTextHeight = 20f;

		// Token: 0x04007191 RID: 29073
		private const float BottomAreaHeight = 55f;

		// Token: 0x04007192 RID: 29074
		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);
	}
}
