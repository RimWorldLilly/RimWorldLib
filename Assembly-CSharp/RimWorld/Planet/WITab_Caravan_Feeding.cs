using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020023EE RID: 9198
	public class WITab_Caravan_Feeding : WITab
	{
		// Token: 0x17002891 RID: 10385
		// (get) Token: 0x0600E5C3 RID: 58819 RVA: 0x00555BC2 File Offset: 0x00553DC2
		private List<Pawn> Pawns
		{
			get
			{
				return base.SelCaravan.PawnsListForReading;
			}
		}

		// Token: 0x17002892 RID: 10386
		// (get) Token: 0x0600E5C4 RID: 58820 RVA: 0x00555BCF File Offset: 0x00553DCF
		public override bool IsVisible
		{
			get
			{
				return this.Pawns.Any(delegate(Pawn pawn)
				{
					ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
					return ChildcareUtility.CanSuckle(pawn, out breastfeedFailReason);
				});
			}
		}

		// Token: 0x0600E5C5 RID: 58821 RVA: 0x00555BFB File Offset: 0x00553DFB
		public WITab_Caravan_Feeding()
		{
			this.labelKey = "TabCaravanFeeding";
		}

		// Token: 0x0600E5C6 RID: 58822 RVA: 0x00555C0E File Offset: 0x00553E0E
		protected override void UpdateSize()
		{
			this.size = new Vector2(250f, Mathf.Min(550f, this.PaneTopY - 30f));
			base.UpdateSize();
		}

		// Token: 0x0600E5C7 RID: 58823 RVA: 0x00555C3C File Offset: 0x00553E3C
		protected override void FillTab()
		{
			Text.Font = GameFont.Small;
			RectDivider rect = new RectDivider(new Rect(0f, 15f, this.size.x, this.size.y - 15f).ContractedBy(10f), 1384745000, null);
			Widgets.ListSeparator(ref rect, "Babies".Translate().CapitalizeFirst());
			RectDivider rect2 = rect.CreateViewRect(this.Pawns.Count(delegate(Pawn pawn)
			{
				ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
				return ChildcareUtility.CanSuckle(pawn, out breastfeedFailReason);
			}), 34f);
			Widgets.BeginScrollView(rect, ref this.scrollPosition, rect2, true);
			this.DoRows(ref rect2);
			Widgets.EndScrollView();
		}

		// Token: 0x0600E5C8 RID: 58824 RVA: 0x00555D14 File Offset: 0x00553F14
		private void DoRows(ref RectDivider scrollViewRect)
		{
			if (this.selectedPawn != null && (this.selectedPawn.Destroyed || !base.SelCaravan.ContainsPawn(this.selectedPawn)))
			{
				this.selectedPawn = null;
			}
			foreach (Pawn pawn in this.Pawns)
			{
				ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
				if (ChildcareUtility.CanSuckle(pawn, out breastfeedFailReason))
				{
					this.DoRow(scrollViewRect.NewRow(34f, VerticalJustification.Top, null), pawn);
				}
			}
		}

		// Token: 0x0600E5C9 RID: 58825 RVA: 0x00555DBC File Offset: 0x00553FBC
		private void DoRow(Rect rect, Pawn pawn)
		{
			Widgets.BeginGroup(rect);
			Rect rect2 = rect.AtZero();
			CaravanThingsTabUtility.DoAbandonButton(rect2, pawn, base.SelCaravan);
			rect2.width -= 24f;
			Widgets.InfoCardButton(rect2.width - 24f, (rect.height - 24f) / 2f, pawn);
			rect2.width -= 24f;
			CaravanThingsTabUtility.DoOpenSpecificTabButton(rect2, pawn, ref this.selectedPawn);
			rect2.width -= 24f;
			CaravanThingsTabUtility.DoOpenSpecificTabButtonInvisible(rect2, pawn, ref this.selectedPawn);
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
			}
			Rect rect3 = new Rect(4f, (rect.height - 27f) / 2f, 27f, 27f);
			Widgets.ThingIcon(rect3, pawn, 1f, null, false, 1f, false);
			Rect bgRect = new Rect(rect3.xMax + 4f, 8f, 100f, 18f);
			GenMapUI.DrawPawnLabel(pawn, bgRect, 1f, 100f, null, GameFont.Small, false, false);
			if (pawn.Downed && !pawn.ageTracker.CurLifeStage.alwaysDowned)
			{
				GUI.color = new Color(1f, 0f, 0f, 0.5f);
				Widgets.DrawLineHorizontal(0f, rect.height / 2f, rect.width);
				GUI.color = Color.white;
			}
			Widgets.EndGroup();
		}

		// Token: 0x0600E5CA RID: 58826 RVA: 0x00555F50 File Offset: 0x00554150
		protected override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			if (this.selectedPawn != null && (this.selectedPawn.Destroyed || !base.SelCaravan.ContainsPawn(this.selectedPawn)))
			{
				this.selectedPawn = null;
			}
			Pawn captureSelectedPawn = this.selectedPawn;
			if (this.selectedPawn != null)
			{
				Rect tabRect = base.TabRect;
				float width = 500f;
				Rect rect = new Rect(tabRect.xMax - 1f, tabRect.yMin, width, tabRect.height);
				Find.WindowStack.ImmediateWindow(323826479, rect, WindowLayer.GameUI, delegate
				{
					if (captureSelectedPawn.DestroyedOrNull())
					{
						return;
					}
					if (this.SelCaravan == null)
					{
						return;
					}
					ITab_Pawn_Feeding.FillTab(captureSelectedPawn, rect.AtZero(), ref this.scrollPositionAutoBreastfeed, ref this.scrollPositionBabyConsumables, this.Pawns);
					if (Widgets.CloseButtonFor(rect.AtZero()))
					{
						captureSelectedPawn = null;
						SoundDefOf.TabClose.PlayOneShotOnCamera(null);
					}
				}, true, false, 1f, null, false);
			}
		}

		// Token: 0x0600E5CB RID: 58827 RVA: 0x00556012 File Offset: 0x00554212
		public override void Notify_ClearingAllMapsMemory()
		{
			base.Notify_ClearingAllMapsMemory();
			this.selectedPawn = null;
		}

		// Token: 0x0600E5CC RID: 58828 RVA: 0x00556024 File Offset: 0x00554224
		public override void OnOpen()
		{
			base.OnOpen();
			if (this.selectedPawn == null || !this.Pawns.Contains(this.selectedPawn))
			{
				this.selectedPawn = this.Pawns.FirstOrFallback(delegate(Pawn pawn)
				{
					ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
					return ChildcareUtility.CanSuckle(pawn, out breastfeedFailReason);
				}, null);
			}
		}

		// Token: 0x04009201 RID: 37377
		private Vector2 scrollPosition;

		// Token: 0x04009202 RID: 37378
		private Pawn selectedPawn;

		// Token: 0x04009203 RID: 37379
		private Vector2 scrollPositionAutoBreastfeed;

		// Token: 0x04009204 RID: 37380
		private Vector2 scrollPositionBabyConsumables;

		// Token: 0x04009205 RID: 37381
		private const int contextHash = 1384745000;

		// Token: 0x04009206 RID: 37382
		private const float RowHeight = 34f;

		// Token: 0x04009207 RID: 37383
		private const float ScrollViewTopMargin = 15f;

		// Token: 0x04009208 RID: 37384
		private const float PawnLabelHeight = 18f;

		// Token: 0x04009209 RID: 37385
		private const float PawnLabelColumnWidth = 100f;

		// Token: 0x0400920A RID: 37386
		private const float SpaceAroundIcon = 4f;

		// Token: 0x0400920B RID: 37387
		private const float BabyPickWindowWidth = 250f;
	}
}
