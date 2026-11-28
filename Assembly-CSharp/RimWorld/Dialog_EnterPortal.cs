using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001CAB RID: 7339
	public class Dialog_EnterPortal : Window
	{
		// Token: 0x1700222A RID: 8746
		// (get) Token: 0x0600BC06 RID: 48134 RVA: 0x00430C04 File Offset: 0x0042EE04
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight);
			}
		}

		// Token: 0x1700222B RID: 8747
		// (get) Token: 0x0600BC07 RID: 48135 RVA: 0x00430C16 File Offset: 0x0042EE16
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x0600BC08 RID: 48136 RVA: 0x00430C1D File Offset: 0x0042EE1D
		public Dialog_EnterPortal(MapPortal portal) : base(null)
		{
			this.portal = portal;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600BC09 RID: 48137 RVA: 0x00430C50 File Offset: 0x0042EE50
		public override void PostOpen()
		{
			base.PostOpen();
			this.CalculateAndRecacheTransferables();
		}

		// Token: 0x0600BC0A RID: 48138 RVA: 0x00430C60 File Offset: 0x0042EE60
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, inRect.width, 35f);
			using (new TextBlock(GameFont.Medium, TextAnchor.MiddleCenter))
			{
				Widgets.Label(rect, this.portal.EnterString);
			}
			Dialog_EnterPortal.tabsList.Clear();
			Dialog_EnterPortal.tabsList.Add(new TabRecord("PawnsTab".Translate(), delegate()
			{
				this.tab = Dialog_EnterPortal.Tab.Pawns;
			}, this.tab == Dialog_EnterPortal.Tab.Pawns));
			Dialog_EnterPortal.tabsList.Add(new TabRecord("ItemsTab".Translate(), delegate()
			{
				this.tab = Dialog_EnterPortal.Tab.Items;
			}, this.tab == Dialog_EnterPortal.Tab.Items));
			inRect.yMin += 67f;
			Widgets.DrawMenuSection(inRect);
			TabDrawer.DrawTabs<TabRecord>(inRect, Dialog_EnterPortal.tabsList, 200f);
			inRect = inRect.ContractedBy(17f);
			Widgets.BeginGroup(inRect);
			Rect rect2 = inRect.AtZero();
			this.DoBottomButtons(rect2);
			Rect inRect2 = rect2;
			inRect2.yMax -= 76f;
			bool flag = false;
			Dialog_EnterPortal.Tab tab = this.tab;
			if (tab != Dialog_EnterPortal.Tab.Pawns)
			{
				if (tab == Dialog_EnterPortal.Tab.Items)
				{
					this.itemsTransfer.OnGUI(inRect2, out flag);
				}
			}
			else
			{
				this.pawnsTransfer.OnGUI(inRect2, out flag);
			}
			Widgets.EndGroup();
		}

		// Token: 0x0600BC0B RID: 48139 RVA: 0x00430DD0 File Offset: 0x0042EFD0
		private void DoBottomButtons(Rect rect)
		{
			if (Widgets.ButtonText(new Rect(rect.width / 2f - this.BottomButtonSize.x / 2f, rect.height - 55f - 17f, this.BottomButtonSize.x, this.BottomButtonSize.y), "ResetButton".Translate(), true, true, true, null))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				this.CalculateAndRecacheTransferables();
			}
			if (Widgets.ButtonText(new Rect(0f, rect.height - 55f - 17f, this.BottomButtonSize.x, this.BottomButtonSize.y), "CancelButton".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(rect.width - this.BottomButtonSize.x, rect.height - 55f - 17f, this.BottomButtonSize.x, this.BottomButtonSize.y), "AcceptButton".Translate(), true, true, true, null) && this.TryAccept())
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				this.Close(false);
			}
		}

		// Token: 0x0600BC0C RID: 48140 RVA: 0x00430F38 File Offset: 0x0042F138
		private bool TryAccept()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			this.portal.leftToLoad = new List<TransferableOneWay>();
			foreach (TransferableOneWay transferableOneWay in this.transferables)
			{
				this.portal.AddToTheToLoadList(transferableOneWay, transferableOneWay.CountToTransfer);
			}
			EnterPortalUtility.MakeLordsAsAppropriate(pawnsFromTransferables, this.portal);
			return true;
		}

		// Token: 0x0600BC0D RID: 48141 RVA: 0x00430FC0 File Offset: 0x0042F1C0
		private void CalculateAndRecacheTransferables()
		{
			this.transferables = new List<TransferableOneWay>();
			if (this.portal.LoadInProgress)
			{
				foreach (TransferableOneWay item in this.portal.leftToLoad)
				{
					this.transferables.Add(item);
				}
			}
			this.AddPawnsToTransferables();
			this.AddItemsToTransferables();
			foreach (Thing t in EnterPortalUtility.ThingsBeingHauledTo(this.portal))
			{
				this.AddToTransferables(t);
			}
			this.pawnsTransfer = new TransferableOneWayWidget(null, null, null, "TransferMapPortalColonyThingCountTip".Translate(), true, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, true, () => float.MaxValue, 0f, false, new PlanetTile?(this.portal.Map.Tile), false, true, false, false, false, false, false, false, false, false);
			CaravanUIUtility.AddPawnsSections(this.pawnsTransfer, this.transferables);
			this.itemsTransfer = new TransferableOneWayWidget(from x in this.transferables
			where x.ThingDef.category != ThingCategory.Pawn
			select x, null, null, "TransferMapPortalColonyThingCountTip".Translate(), true, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, true, () => float.MaxValue, 0f, false, new PlanetTile?(this.portal.Map.Tile), false, false, false, false, false, false, false, false, false, false);
		}

		// Token: 0x0600BC0E RID: 48142 RVA: 0x00431184 File Offset: 0x0042F384
		private void AddToTransferables(Thing t)
		{
			if (this.portal.LoadInProgress && this.portal.leftToLoad.Any((TransferableOneWay trans) => trans.things.Contains(t)))
			{
				return;
			}
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(t, this.transferables, TransferAsOneMode.PodsOrCaravanPacking);
			if (transferableOneWay == null)
			{
				transferableOneWay = new TransferableOneWay();
				this.transferables.Add(transferableOneWay);
			}
			if (transferableOneWay.things.Contains(t))
			{
				string str = "Tried to add the same thing twice to TransferableOneWay: ";
				Thing t2 = t;
				Log.Error(str + ((t2 != null) ? t2.ToString() : null));
				return;
			}
			transferableOneWay.things.Add(t);
		}

		// Token: 0x0600BC0F RID: 48143 RVA: 0x00431238 File Offset: 0x0042F438
		private void AddPawnsToTransferables()
		{
			foreach (Pawn t in CaravanFormingUtility.AllSendablePawns(this.portal.Map, true, false, false, false, true, -1))
			{
				this.AddToTransferables(t);
			}
		}

		// Token: 0x0600BC10 RID: 48144 RVA: 0x0043129C File Offset: 0x0042F49C
		private void AddItemsToTransferables()
		{
			bool isPocketMap = this.portal.Map.IsPocketMap;
			foreach (Thing t in CaravanFormingUtility.AllReachableColonyItems(this.portal.Map, isPocketMap, isPocketMap, false))
			{
				this.AddToTransferables(t);
			}
		}

		// Token: 0x0600BC11 RID: 48145 RVA: 0x00431310 File Offset: 0x0042F510
		public override void OnAcceptKeyPressed()
		{
			if (this.TryAccept())
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				this.Close(false);
			}
		}

		// Token: 0x04006B58 RID: 27480
		private const float TitleRectHeight = 35f;

		// Token: 0x04006B59 RID: 27481
		private const float BottomAreaHeight = 55f;

		// Token: 0x04006B5A RID: 27482
		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x04006B5B RID: 27483
		private MapPortal portal;

		// Token: 0x04006B5C RID: 27484
		private List<TransferableOneWay> transferables;

		// Token: 0x04006B5D RID: 27485
		private TransferableOneWayWidget pawnsTransfer;

		// Token: 0x04006B5E RID: 27486
		private TransferableOneWayWidget itemsTransfer;

		// Token: 0x04006B5F RID: 27487
		private Dialog_EnterPortal.Tab tab;

		// Token: 0x04006B60 RID: 27488
		private static List<TabRecord> tabsList = new List<TabRecord>();

		// Token: 0x0200394F RID: 14671
		private enum Tab
		{
			// Token: 0x0400E486 RID: 58502
			Pawns,
			// Token: 0x0400E487 RID: 58503
			Items
		}
	}
}
