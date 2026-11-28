using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D68 RID: 7528
	public class ITab_ContentsOutfitStand : ITab_ContentsBase
	{
		// Token: 0x1700241F RID: 9247
		// (get) Token: 0x0600C30E RID: 49934 RVA: 0x00472044 File Offset: 0x00470244
		public override IList<Thing> container
		{
			get
			{
				return this.OutfitStand.HeldItems.ToList<Thing>();
			}
		}

		// Token: 0x17002420 RID: 9248
		// (get) Token: 0x0600C30F RID: 49935 RVA: 0x00472056 File Offset: 0x00470256
		public override bool IsVisible
		{
			get
			{
				return base.SelThing != null && base.IsVisible;
			}
		}

		// Token: 0x17002421 RID: 9249
		// (get) Token: 0x0600C310 RID: 49936 RVA: 0x00472068 File Offset: 0x00470268
		public Building_OutfitStand OutfitStand
		{
			get
			{
				return base.SelThing as Building_OutfitStand;
			}
		}

		// Token: 0x17002422 RID: 9250
		// (get) Token: 0x0600C311 RID: 49937 RVA: 0x00472075 File Offset: 0x00470275
		public override bool VisibleInBlueprintMode
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C312 RID: 49938 RVA: 0x00472078 File Offset: 0x00470278
		public ITab_ContentsOutfitStand()
		{
			this.labelKey = "TabCasketContents";
			this.containedItemsKey = "TabCasketContents";
		}

		// Token: 0x0600C313 RID: 49939 RVA: 0x00472096 File Offset: 0x00470296
		protected override void DoItemsLists(Rect inRect, ref float curY)
		{
			this.ListContainedApparel(inRect, this.container, ref curY);
		}

		// Token: 0x0600C314 RID: 49940 RVA: 0x004720A8 File Offset: 0x004702A8
		private void ListContainedApparel(Rect inRect, IList<Thing> apparel, ref float curY)
		{
			GUI.BeginGroup(inRect);
			float num = curY;
			Widgets.ListSeparator(ref curY, inRect.width, this.containedItemsKey.Translate());
			bool flag = false;
			for (int i = 0; i < apparel.Count; i++)
			{
				Thing thing = apparel[i];
				if (thing != null)
				{
					flag = true;
					this.DoRow(thing, inRect.width, i, ref curY);
				}
			}
			if (!flag)
			{
				Widgets.NoneLabel(ref curY, inRect.width, null);
			}
			GUI.EndGroup();
		}

		// Token: 0x0600C315 RID: 49941 RVA: 0x00472124 File Offset: 0x00470324
		private void DoRow(Thing thing, float width, int i, ref float curY)
		{
			Rect rect = new Rect(0f, curY, width, 28f);
			Widgets.InfoCardButton(0f, curY, thing);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlightSelected(rect);
			}
			else if (i % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			Rect rect2 = new Rect(rect.width - 24f, curY, 24f, 24f);
			if (Widgets.ButtonImage(rect2, ITab_ContentsOutfitStand.DropTex.Texture, true, null))
			{
				IntVec3 position;
				if (!(from x in this.OutfitStand.OccupiedRect().AdjacentCells
				where x.Walkable(this.OutfitStand.Map)
				select x).TryRandomElement(out position))
				{
					position = this.OutfitStand.Position;
				}
				Thing thing2;
				this.OutfitStand.TryDrop(thing, position, ThingPlaceMode.Near, 1, out thing2);
				CompForbiddable compForbiddable;
				if (thing2.TryGetComp(out compForbiddable))
				{
					compForbiddable.Forbidden = true;
				}
			}
			else if (Widgets.ButtonInvisible(rect, true))
			{
				Find.Selector.ClearSelection();
				Find.Selector.Select(thing, true, true);
			}
			TooltipHandler.TipRegionByKey(rect2, "EjectApparelTooltip");
			Widgets.ThingIcon(new Rect(24f, curY, 28f, 28f), thing, 1f, null, false, 1f, false);
			Rect rect3 = new Rect(60f, curY, rect.width - 36f, rect.height);
			rect3.xMax = rect2.xMin;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect3, thing.LabelCap.Truncate(rect3.width, null));
			Text.Anchor = TextAnchor.UpperLeft;
			if (Mouse.IsOver(rect))
			{
				TargetHighlighter.Highlight(thing, true, false, false);
				TooltipHandler.TipRegion(rect, thing.DescriptionDetailed);
			}
			curY += 28f;
		}

		// Token: 0x04006F80 RID: 28544
		private static readonly CachedTexture DropTex = new CachedTexture("UI/Buttons/Drop");
	}
}
