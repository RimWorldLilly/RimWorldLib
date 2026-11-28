using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020023F3 RID: 9203
	public class WITab_Caravan_Social : WITab
	{
		// Token: 0x17002898 RID: 10392
		// (get) Token: 0x0600E603 RID: 58883 RVA: 0x00558196 File Offset: 0x00556396
		private List<Pawn> Pawns
		{
			get
			{
				return base.SelCaravan.PawnsListForReading;
			}
		}

		// Token: 0x17002899 RID: 10393
		// (get) Token: 0x0600E604 RID: 58884 RVA: 0x005581A3 File Offset: 0x005563A3
		private float SpecificSocialTabWidth
		{
			get
			{
				this.EnsureSpecificSocialTabForPawnValid();
				if (this.specificSocialTabForPawn.DestroyedOrNull())
				{
					return 0f;
				}
				return 540f;
			}
		}

		// Token: 0x0600E605 RID: 58885 RVA: 0x005581C3 File Offset: 0x005563C3
		public WITab_Caravan_Social()
		{
			this.labelKey = "TabCaravanSocial";
		}

		// Token: 0x0600E606 RID: 58886 RVA: 0x005581D8 File Offset: 0x005563D8
		protected override void FillTab()
		{
			this.EnsureSpecificSocialTabForPawnValid();
			Text.Font = GameFont.Small;
			Rect rect = new Rect(0f, 15f, this.size.x, this.size.y - 15f).ContractedBy(10f);
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, this.scrollViewHeight);
			float num = 0f;
			Widgets.BeginScrollView(rect, ref this.scrollPosition, rect2, true);
			this.DoRows(ref num, rect2, rect);
			if (Event.current.type == EventType.layout)
			{
				this.scrollViewHeight = num + 30f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x0600E607 RID: 58887 RVA: 0x00558288 File Offset: 0x00556488
		protected override void UpdateSize()
		{
			this.EnsureSpecificSocialTabForPawnValid();
			base.UpdateSize();
			this.size.x = 243f;
			this.size.y = Mathf.Min(550f, this.PaneTopY - 30f);
		}

		// Token: 0x0600E608 RID: 58888 RVA: 0x005582C8 File Offset: 0x005564C8
		protected override void ExtraOnGUI()
		{
			this.EnsureSpecificSocialTabForPawnValid();
			base.ExtraOnGUI();
			Pawn localSpecificSocialTabForPawn = this.specificSocialTabForPawn;
			if (localSpecificSocialTabForPawn != null)
			{
				Rect tabRect = base.TabRect;
				float specificSocialTabWidth = this.SpecificSocialTabWidth;
				Rect rect = new Rect(tabRect.xMax - 1f, tabRect.yMin, specificSocialTabWidth, tabRect.height);
				Find.WindowStack.ImmediateWindow(1439870015, rect, WindowLayer.GameUI, delegate
				{
					if (localSpecificSocialTabForPawn.DestroyedOrNull())
					{
						return;
					}
					SocialCardUtility.DrawSocialCard(rect.AtZero(), localSpecificSocialTabForPawn);
					if (Widgets.CloseButtonFor(rect.AtZero()))
					{
						this.specificSocialTabForPawn = null;
						SoundDefOf.TabClose.PlayOneShotOnCamera(null);
					}
				}, true, false, 1f, null, false);
			}
		}

		// Token: 0x0600E609 RID: 58889 RVA: 0x00558364 File Offset: 0x00556564
		public override void OnOpen()
		{
			base.OnOpen();
			if ((this.specificSocialTabForPawn == null || !this.Pawns.Contains(this.specificSocialTabForPawn)) && this.Pawns.Any<Pawn>())
			{
				this.specificSocialTabForPawn = this.Pawns[0];
			}
		}

		// Token: 0x0600E60A RID: 58890 RVA: 0x005583B4 File Offset: 0x005565B4
		private void DoRows(ref float curY, Rect scrollViewRect, Rect scrollOutRect)
		{
			List<Pawn> pawns = this.Pawns;
			Pawn pawn = BestCaravanPawnUtility.FindBestNegotiator(base.SelCaravan, null, null);
			GUI.color = new Color(0.8f, 0.8f, 0.8f, 1f);
			Widgets.Label(new Rect(0f, curY, scrollViewRect.width, 24f), string.Format("{0}: {1}", "Negotiator".TranslateSimple(), (pawn != null) ? pawn.LabelShort : "NoneCapable".Translate().ToString()));
			curY += 24f;
			if (this.specificSocialTabForPawn != null && !pawns.Contains(this.specificSocialTabForPawn))
			{
				this.specificSocialTabForPawn = null;
			}
			bool flag = false;
			for (int i = 0; i < pawns.Count; i++)
			{
				Pawn pawn2 = pawns[i];
				if (pawn2.RaceProps.IsFlesh && pawn2.IsColonist)
				{
					if (!flag)
					{
						Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanColonists".Translate());
						flag = true;
					}
					this.DoRow(ref curY, scrollViewRect, scrollOutRect, pawn2);
				}
			}
			bool flag2 = false;
			for (int j = 0; j < pawns.Count; j++)
			{
				Pawn pawn3 = pawns[j];
				if (pawn3.RaceProps.IsFlesh && !pawn3.IsColonist)
				{
					if (!flag2)
					{
						Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanPrisonersAndAnimals".Translate());
						flag2 = true;
					}
					this.DoRow(ref curY, scrollViewRect, scrollOutRect, pawn3);
				}
			}
		}

		// Token: 0x0600E60B RID: 58891 RVA: 0x00558538 File Offset: 0x00556738
		private void DoRow(ref float curY, Rect viewRect, Rect scrollOutRect, Pawn p)
		{
			float num = this.scrollPosition.y - 34f;
			float num2 = this.scrollPosition.y + scrollOutRect.height;
			if (curY > num && curY < num2)
			{
				this.DoRow(new Rect(0f, curY, viewRect.width, 34f), p);
			}
			curY += 34f;
		}

		// Token: 0x0600E60C RID: 58892 RVA: 0x005585A0 File Offset: 0x005567A0
		private void DoRow(Rect rect, Pawn p)
		{
			Widgets.BeginGroup(rect);
			Rect rect2 = rect.AtZero();
			CaravanThingsTabUtility.DoAbandonButton(rect2, p, base.SelCaravan);
			rect2.width -= 24f;
			Widgets.InfoCardButton(rect2.width - 24f, (rect.height - 24f) / 2f, p);
			rect2.width -= 24f;
			CaravanThingsTabUtility.DoOpenSpecificTabButton(rect2, p, ref this.specificSocialTabForPawn);
			rect2.width -= 24f;
			CaravanThingsTabUtility.DoOpenSpecificTabButtonInvisible(rect2, p, ref this.specificSocialTabForPawn);
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
			}
			Rect rect3 = new Rect(4f, (rect.height - 27f) / 2f, 27f, 27f);
			Widgets.ThingIcon(rect3, p, 1f, null, false, 1f, false);
			Rect bgRect = new Rect(rect3.xMax + 4f, 8f, 100f, 18f);
			GenMapUI.DrawPawnLabel(p, bgRect, 1f, 100f, null, GameFont.Small, false, false);
			if (p.Downed && !p.ageTracker.CurLifeStage.alwaysDowned)
			{
				GUI.color = new Color(1f, 0f, 0f, 0.5f);
				Widgets.DrawLineHorizontal(0f, rect.height / 2f, rect.width);
				GUI.color = Color.white;
			}
			Widgets.EndGroup();
		}

		// Token: 0x0600E60D RID: 58893 RVA: 0x00558731 File Offset: 0x00556931
		public override void Notify_ClearingAllMapsMemory()
		{
			base.Notify_ClearingAllMapsMemory();
			this.specificSocialTabForPawn = null;
		}

		// Token: 0x0600E60E RID: 58894 RVA: 0x00558740 File Offset: 0x00556940
		private void EnsureSpecificSocialTabForPawnValid()
		{
			if (this.specificSocialTabForPawn != null && (this.specificSocialTabForPawn.Destroyed || !base.SelCaravan.ContainsPawn(this.specificSocialTabForPawn)))
			{
				this.specificSocialTabForPawn = null;
			}
		}

		// Token: 0x0400923B RID: 37435
		private Vector2 scrollPosition;

		// Token: 0x0400923C RID: 37436
		private float scrollViewHeight;

		// Token: 0x0400923D RID: 37437
		private Pawn specificSocialTabForPawn;

		// Token: 0x0400923E RID: 37438
		private const float RowHeight = 34f;

		// Token: 0x0400923F RID: 37439
		private const float ScrollViewTopMargin = 15f;

		// Token: 0x04009240 RID: 37440
		private const float PawnLabelHeight = 18f;

		// Token: 0x04009241 RID: 37441
		private const float PawnLabelColumnWidth = 100f;

		// Token: 0x04009242 RID: 37442
		private const float SpaceAroundIcon = 4f;
	}
}
