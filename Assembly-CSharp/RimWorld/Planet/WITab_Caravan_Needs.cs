using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020023F2 RID: 9202
	public class WITab_Caravan_Needs : WITab
	{
		// Token: 0x17002897 RID: 10391
		// (get) Token: 0x0600E5FC RID: 58876 RVA: 0x00557FA3 File Offset: 0x005561A3
		private float SpecificNeedsTabWidth
		{
			get
			{
				if (this.specificNeedsTabForPawn.DestroyedOrNull())
				{
					return 0f;
				}
				return NeedsCardUtility.GetSize(this.specificNeedsTabForPawn).x;
			}
		}

		// Token: 0x0600E5FD RID: 58877 RVA: 0x00557FC8 File Offset: 0x005561C8
		public WITab_Caravan_Needs()
		{
			this.labelKey = "TabCaravanNeeds";
		}

		// Token: 0x0600E5FE RID: 58878 RVA: 0x00557FDB File Offset: 0x005561DB
		protected override void FillTab()
		{
			this.EnsureSpecificNeedsTabForPawnValid();
			CaravanNeedsTabUtility.DoRows(this.size, base.SelCaravan.PawnsListForReading, base.SelCaravan, ref this.scrollPosition, ref this.scrollViewHeight, ref this.specificNeedsTabForPawn, this.doNeeds);
		}

		// Token: 0x0600E5FF RID: 58879 RVA: 0x00558018 File Offset: 0x00556218
		protected override void UpdateSize()
		{
			this.EnsureSpecificNeedsTabForPawnValid();
			base.UpdateSize();
			this.size = CaravanNeedsTabUtility.GetSize(base.SelCaravan.PawnsListForReading, this.PaneTopY, true);
			if (this.size.x + this.SpecificNeedsTabWidth > (float)UI.screenWidth)
			{
				this.doNeeds = false;
				this.size = CaravanNeedsTabUtility.GetSize(base.SelCaravan.PawnsListForReading, this.PaneTopY, false);
			}
			else
			{
				this.doNeeds = true;
			}
			this.size.y = Mathf.Max(this.size.y, NeedsCardUtility.FullSize.y);
		}

		// Token: 0x0600E600 RID: 58880 RVA: 0x005580BC File Offset: 0x005562BC
		protected override void ExtraOnGUI()
		{
			this.EnsureSpecificNeedsTabForPawnValid();
			base.ExtraOnGUI();
			Pawn localSpecificNeedsTabForPawn = this.specificNeedsTabForPawn;
			if (localSpecificNeedsTabForPawn != null)
			{
				Rect tabRect = base.TabRect;
				float specificNeedsTabWidth = this.SpecificNeedsTabWidth;
				Rect rect = new Rect(tabRect.xMax - 1f, tabRect.yMin, specificNeedsTabWidth, tabRect.height);
				Find.WindowStack.ImmediateWindow(1439870015, rect, WindowLayer.GameUI, delegate
				{
					if (localSpecificNeedsTabForPawn.DestroyedOrNull())
					{
						return;
					}
					NeedsCardUtility.DoNeedsMoodAndThoughts(rect.AtZero(), localSpecificNeedsTabForPawn, ref this.thoughtScrollPosition);
					if (Widgets.CloseButtonFor(rect.AtZero()))
					{
						this.specificNeedsTabForPawn = null;
						SoundDefOf.TabClose.PlayOneShotOnCamera(null);
					}
				}, true, false, 1f, null, false);
			}
		}

		// Token: 0x0600E601 RID: 58881 RVA: 0x00558156 File Offset: 0x00556356
		public override void Notify_ClearingAllMapsMemory()
		{
			base.Notify_ClearingAllMapsMemory();
			this.specificNeedsTabForPawn = null;
		}

		// Token: 0x0600E602 RID: 58882 RVA: 0x00558165 File Offset: 0x00556365
		private void EnsureSpecificNeedsTabForPawnValid()
		{
			if (this.specificNeedsTabForPawn != null && (this.specificNeedsTabForPawn.Destroyed || !base.SelCaravan.ContainsPawn(this.specificNeedsTabForPawn)))
			{
				this.specificNeedsTabForPawn = null;
			}
		}

		// Token: 0x04009236 RID: 37430
		private Vector2 scrollPosition;

		// Token: 0x04009237 RID: 37431
		private float scrollViewHeight;

		// Token: 0x04009238 RID: 37432
		private Pawn specificNeedsTabForPawn;

		// Token: 0x04009239 RID: 37433
		private Vector2 thoughtScrollPosition;

		// Token: 0x0400923A RID: 37434
		private bool doNeeds;
	}
}
