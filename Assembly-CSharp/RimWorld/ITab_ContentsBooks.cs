using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D64 RID: 7524
	public class ITab_ContentsBooks : ITab_ContentsBase
	{
		// Token: 0x17002413 RID: 9235
		// (get) Token: 0x0600C2ED RID: 49901 RVA: 0x004711EF File Offset: 0x0046F3EF
		public override IList<Thing> container
		{
			get
			{
				return this.Bookcase.GetDirectlyHeldThings().ToList<Thing>();
			}
		}

		// Token: 0x17002414 RID: 9236
		// (get) Token: 0x0600C2EE RID: 49902 RVA: 0x00471201 File Offset: 0x0046F401
		public override bool IsVisible
		{
			get
			{
				return base.SelThing != null && base.IsVisible;
			}
		}

		// Token: 0x17002415 RID: 9237
		// (get) Token: 0x0600C2EF RID: 49903 RVA: 0x00471213 File Offset: 0x0046F413
		public Building_Bookcase Bookcase
		{
			get
			{
				return base.SelThing as Building_Bookcase;
			}
		}

		// Token: 0x17002416 RID: 9238
		// (get) Token: 0x0600C2F0 RID: 49904 RVA: 0x00471220 File Offset: 0x0046F420
		public override bool VisibleInBlueprintMode
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C2F1 RID: 49905 RVA: 0x00471223 File Offset: 0x0046F423
		public ITab_ContentsBooks()
		{
			this.labelKey = "TabCasketContents";
			this.containedItemsKey = "TabCasketContents";
		}

		// Token: 0x0600C2F2 RID: 49906 RVA: 0x00471241 File Offset: 0x0046F441
		protected override void DoItemsLists(Rect inRect, ref float curY)
		{
			this.ListContainedBooks(inRect, this.container, ref curY);
		}

		// Token: 0x0600C2F3 RID: 49907 RVA: 0x00471254 File Offset: 0x0046F454
		private void ListContainedBooks(Rect inRect, IList<Thing> books, ref float curY)
		{
			GUI.BeginGroup(inRect);
			float num = curY;
			Widgets.ListSeparator(ref curY, inRect.width, this.containedItemsKey.Translate());
			Rect rect = new Rect(0f, num, inRect.width, curY - num - 3f);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegionByKey(rect, "ContainedBooksDesc");
			}
			bool flag = false;
			for (int i = 0; i < books.Count; i++)
			{
				Book book = books[i] as Book;
				if (book != null)
				{
					flag = true;
					this.DoRow(book, inRect.width, i, ref curY);
				}
			}
			if (!flag)
			{
				Widgets.NoneLabel(ref curY, inRect.width, null);
			}
			GUI.EndGroup();
		}

		// Token: 0x0600C2F4 RID: 49908 RVA: 0x0047130C File Offset: 0x0046F50C
		private void DoRow(Book book, float width, int i, ref float curY)
		{
			Rect rect = new Rect(0f, curY, width, 28f);
			Widgets.InfoCardButton(0f, curY, book);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlightSelected(rect);
			}
			else if (i % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			Rect rect2 = new Rect(rect.width - 24f, curY, 24f, 24f);
			if (Widgets.ButtonImage(rect2, ITab_ContentsBooks.DropTex.Texture, true, null))
			{
				IntVec3 position;
				if (!(from x in this.Bookcase.OccupiedRect().AdjacentCells
				where x.Walkable(this.Bookcase.Map)
				select x).TryRandomElement(out position))
				{
					position = this.Bookcase.Position;
				}
				Thing thing;
				this.Bookcase.GetDirectlyHeldThings().TryDrop(book, position, this.Bookcase.Map, ThingPlaceMode.Near, 1, out thing, null, null);
				CompForbiddable compForbiddable;
				if (thing.TryGetComp(out compForbiddable))
				{
					compForbiddable.Forbidden = true;
				}
			}
			else if (Widgets.ButtonInvisible(rect, true))
			{
				Find.Selector.ClearSelection();
				Find.Selector.Select(book, true, true);
				InspectPaneUtility.OpenTab(typeof(ITab_Book));
			}
			TooltipHandler.TipRegionByKey(rect2, "EjectBookTooltip");
			Widgets.ThingIcon(new Rect(24f, curY, 28f, 28f), book, 1f, null, false, 1f, false);
			Rect rect3 = new Rect(60f, curY, rect.width - 36f, rect.height);
			rect3.xMax = rect2.xMin;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect3, book.LabelCap.Truncate(rect3.width, null));
			Text.Anchor = TextAnchor.UpperLeft;
			if (Mouse.IsOver(rect))
			{
				TargetHighlighter.Highlight(book, true, false, false);
				TooltipHandler.TipRegion(rect, book.DescriptionDetailed);
			}
			curY += 28f;
		}

		// Token: 0x04006F79 RID: 28537
		private static readonly CachedTexture DropTex = new CachedTexture("UI/Buttons/Drop");
	}
}
