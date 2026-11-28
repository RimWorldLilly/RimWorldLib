using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D63 RID: 7523
	public abstract class ITab_ContentsBase : ITab
	{
		// Token: 0x1700240F RID: 9231
		// (get) Token: 0x0600C2E2 RID: 49890
		public abstract IList<Thing> container { get; }

		// Token: 0x17002410 RID: 9232
		// (get) Token: 0x0600C2E3 RID: 49891 RVA: 0x00470B87 File Offset: 0x0046ED87
		public override bool IsVisible
		{
			get
			{
				return base.SelThing.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x17002411 RID: 9233
		// (get) Token: 0x0600C2E4 RID: 49892 RVA: 0x00470B9B File Offset: 0x0046ED9B
		public virtual IntVec3 DropOffset
		{
			get
			{
				return IntVec3.Zero;
			}
		}

		// Token: 0x17002412 RID: 9234
		// (get) Token: 0x0600C2E5 RID: 49893 RVA: 0x00470BA2 File Offset: 0x0046EDA2
		public virtual bool UseDiscardMessage
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C2E6 RID: 49894 RVA: 0x00470BA5 File Offset: 0x0046EDA5
		public ITab_ContentsBase()
		{
			this.size = new Vector2(460f, 450f);
		}

		// Token: 0x0600C2E7 RID: 49895 RVA: 0x00470BD4 File Offset: 0x0046EDD4
		protected override void FillTab()
		{
			this.thingsToSelect.Clear();
			Rect outRect = new Rect(default(Vector2), this.size).ContractedBy(10f);
			outRect.yMin += 20f;
			Rect rect = new Rect(0f, 0f, outRect.width - 16f, Mathf.Max(this.lastDrawnHeight, outRect.height));
			Text.Font = GameFont.Small;
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect, true);
			float num = 0f;
			this.DoItemsLists(rect, ref num);
			this.lastDrawnHeight = num;
			Widgets.EndScrollView();
			if (this.thingsToSelect.Any<Thing>())
			{
				ITab_Pawn_FormingCaravan.SelectNow(this.thingsToSelect);
				this.thingsToSelect.Clear();
			}
		}

		// Token: 0x0600C2E8 RID: 49896 RVA: 0x00470CA0 File Offset: 0x0046EEA0
		protected virtual void DoItemsLists(Rect inRect, ref float curY)
		{
			Widgets.BeginGroup(inRect);
			Widgets.ListSeparator(ref curY, inRect.width, this.containedItemsKey.Translate());
			IList<Thing> container = this.container;
			bool flag = false;
			for (int i = 0; i < container.Count; i++)
			{
				Thing t = container[i];
				if (t != null)
				{
					flag = true;
					ITab_ContentsBase.tmpSingleThing.Clear();
					ITab_ContentsBase.tmpSingleThing.Add(t);
					this.DoThingRow(t.def, t.stackCount, ITab_ContentsBase.tmpSingleThing, inRect.width, ref curY, delegate(int x)
					{
						this.OnDropThing(t, x);
					});
					ITab_ContentsBase.tmpSingleThing.Clear();
				}
			}
			if (!flag)
			{
				Widgets.NoneLabel(ref curY, inRect.width, null);
			}
			Widgets.EndGroup();
		}

		// Token: 0x0600C2E9 RID: 49897 RVA: 0x00470D84 File Offset: 0x0046EF84
		protected virtual void OnDropThing(Thing t, int count)
		{
			Thing thing;
			GenDrop.TryDropSpawn(t.SplitOff(count), base.SelThing.Position + this.DropOffset, base.SelThing.Map, ThingPlaceMode.Near, out thing, null, null, true);
		}

		// Token: 0x0600C2EA RID: 49898 RVA: 0x00470DC8 File Offset: 0x0046EFC8
		protected void DoThingRow(ThingDef thingDef, int count, List<Thing> things, float width, ref float curY, Action<int> discardAction)
		{
			Rect rect = new Rect(0f, curY, width, 28f);
			if (this.canRemoveThings)
			{
				if (count != 1 && Widgets.ButtonImage(new Rect(rect.x + rect.width - 24f, rect.y + (rect.height - 24f) / 2f, 24f, 24f), CaravanThingsTabUtility.AbandonSpecificCountButtonTex, true, null))
				{
					Find.WindowStack.Add(new Dialog_Slider("RemoveSliderText".Translate(thingDef.label), 1, count, discardAction, int.MinValue, 1f));
				}
				rect.width -= 24f;
				if (Widgets.ButtonImage(new Rect(rect.x + rect.width - 24f, rect.y + (rect.height - 24f) / 2f, 24f, 24f), CaravanThingsTabUtility.AbandonButtonTex, true, null))
				{
					if (this.UseDiscardMessage)
					{
						string value = thingDef.label;
						if (things.Count == 1 && things[0] is Pawn)
						{
							value = ((Pawn)things[0]).LabelShortCap;
						}
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmRemoveItemDialog".Translate(value), delegate()
						{
							discardAction(count);
						}, false, null, WindowLayer.Dialog));
					}
					else
					{
						discardAction(count);
					}
				}
				rect.width -= 24f;
			}
			if (things.Count == 1)
			{
				Widgets.InfoCardButton(rect.width - 24f, curY, things[0]);
			}
			else
			{
				Widgets.InfoCardButton(rect.width - 24f, curY, thingDef);
			}
			rect.width -= 24f;
			if (Mouse.IsOver(rect))
			{
				GUI.color = ITab_ContentsBase.ThingHighlightColor;
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			if (thingDef.DrawMatSingle != null && thingDef.DrawMatSingle.mainTexture != null)
			{
				Rect rect2 = new Rect(4f, curY, 28f, 28f);
				if (things.Count == 1)
				{
					Widgets.ThingIcon(rect2, things[0], 1f, null, false, 1f, false);
				}
				else
				{
					Widgets.ThingIcon(rect2, thingDef, null, null, 1f, null, null, 1f);
				}
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.color = ITab_ContentsBase.ThingLabelColor;
			Rect rect3 = new Rect(36f, curY, rect.width - 36f, rect.height);
			string text;
			if (things.Count == 1 && count == things[0].stackCount)
			{
				text = things[0].LabelCap;
			}
			else if (thingDef == ThingDefOf.MinifiedThing)
			{
				text = GenLabel.ThingLabel(things[0].GetInnerIfMinified(), count, true, true).CapitalizeFirst();
			}
			else
			{
				text = GenLabel.ThingLabel(thingDef, null, count).CapitalizeFirst();
			}
			Text.WordWrap = false;
			Widgets.Label(rect3, text.StripTags().Truncate(rect3.width, null));
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
			TooltipHandler.TipRegion(rect, text);
			if (Widgets.ButtonInvisible(rect, true))
			{
				this.SelectLater(things);
			}
			if (Mouse.IsOver(rect))
			{
				for (int i = 0; i < things.Count; i++)
				{
					TargetHighlighter.Highlight(things[i], true, true, false);
				}
			}
			curY += 28f;
		}

		// Token: 0x0600C2EB RID: 49899 RVA: 0x004711B6 File Offset: 0x0046F3B6
		private void SelectLater(List<Thing> things)
		{
			this.thingsToSelect.Clear();
			this.thingsToSelect.AddRange(things);
		}

		// Token: 0x04006F6C RID: 28524
		private Vector2 scrollPosition;

		// Token: 0x04006F6D RID: 28525
		private float lastDrawnHeight;

		// Token: 0x04006F6E RID: 28526
		private List<Thing> thingsToSelect = new List<Thing>();

		// Token: 0x04006F6F RID: 28527
		public bool canRemoveThings = true;

		// Token: 0x04006F70 RID: 28528
		protected static List<Thing> tmpSingleThing = new List<Thing>();

		// Token: 0x04006F71 RID: 28529
		protected const float TopPadding = 20f;

		// Token: 0x04006F72 RID: 28530
		protected const float SpaceBetweenItemsLists = 10f;

		// Token: 0x04006F73 RID: 28531
		protected const float ThingRowHeight = 28f;

		// Token: 0x04006F74 RID: 28532
		protected const float ThingIconSize = 28f;

		// Token: 0x04006F75 RID: 28533
		protected const float ThingLeftX = 36f;

		// Token: 0x04006F76 RID: 28534
		protected static readonly Color ThingLabelColor = ITab_Pawn_Gear.ThingLabelColor;

		// Token: 0x04006F77 RID: 28535
		protected static readonly Color ThingHighlightColor = ITab_Pawn_Gear.HighlightColor;

		// Token: 0x04006F78 RID: 28536
		public string containedItemsKey;
	}
}
