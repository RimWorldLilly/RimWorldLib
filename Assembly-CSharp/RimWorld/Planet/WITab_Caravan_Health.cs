using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020023F0 RID: 9200
	[StaticConstructorOnStartup]
	public class WITab_Caravan_Health : WITab
	{
		// Token: 0x17002894 RID: 10388
		// (get) Token: 0x0600E5E3 RID: 58851 RVA: 0x005572AD File Offset: 0x005554AD
		private List<Pawn> Pawns
		{
			get
			{
				return base.SelCaravan.PawnsListForReading;
			}
		}

		// Token: 0x17002895 RID: 10389
		// (get) Token: 0x0600E5E4 RID: 58852 RVA: 0x005572BC File Offset: 0x005554BC
		private List<PawnCapacityDef> CapacitiesToDisplay
		{
			get
			{
				WITab_Caravan_Health.capacitiesToDisplay.Clear();
				List<PawnCapacityDef> allDefsListForReading = DefDatabase<PawnCapacityDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].showOnCaravanHealthTab)
					{
						WITab_Caravan_Health.capacitiesToDisplay.Add(allDefsListForReading[i]);
					}
				}
				WITab_Caravan_Health.capacitiesToDisplay.SortBy((PawnCapacityDef x) => x.listOrder);
				return WITab_Caravan_Health.capacitiesToDisplay;
			}
		}

		// Token: 0x17002896 RID: 10390
		// (get) Token: 0x0600E5E5 RID: 58853 RVA: 0x00557337 File Offset: 0x00555537
		private float SpecificHealthTabWidth
		{
			get
			{
				this.EnsureSpecificHealthTabForPawnValid();
				if (this.specificHealthTabForPawn.DestroyedOrNull())
				{
					return 0f;
				}
				return 630f;
			}
		}

		// Token: 0x0600E5E6 RID: 58854 RVA: 0x00557357 File Offset: 0x00555557
		public WITab_Caravan_Health()
		{
			this.labelKey = "TabCaravanHealth";
		}

		// Token: 0x0600E5E7 RID: 58855 RVA: 0x0055736C File Offset: 0x0055556C
		protected override void FillTab()
		{
			this.EnsureSpecificHealthTabForPawnValid();
			Text.Font = GameFont.Small;
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y).ContractedBy(10f);
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, this.scrollViewHeight);
			float num = 0f;
			Widgets.BeginScrollView(rect, ref this.scrollPosition, rect2, true);
			this.DoColumnHeaders(ref num);
			this.DoRows(ref num, rect2, rect);
			if (Event.current.type == EventType.layout)
			{
				this.scrollViewHeight = num + 30f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x0600E5E8 RID: 58856 RVA: 0x00557420 File Offset: 0x00555620
		protected override void UpdateSize()
		{
			this.EnsureSpecificHealthTabForPawnValid();
			base.UpdateSize();
			this.size = this.GetRawSize(false);
			if (this.size.x + this.SpecificHealthTabWidth > (float)UI.screenWidth)
			{
				this.compactMode = true;
				this.size = this.GetRawSize(true);
				return;
			}
			this.compactMode = false;
		}

		// Token: 0x0600E5E9 RID: 58857 RVA: 0x0055747C File Offset: 0x0055567C
		protected override void ExtraOnGUI()
		{
			this.EnsureSpecificHealthTabForPawnValid();
			base.ExtraOnGUI();
			Pawn localSpecificHealthTabForPawn = this.specificHealthTabForPawn;
			if (localSpecificHealthTabForPawn != null)
			{
				Rect tabRect = base.TabRect;
				float specificHealthTabWidth = this.SpecificHealthTabWidth;
				Rect rect = new Rect(tabRect.xMax - 1f, tabRect.yMin, specificHealthTabWidth, tabRect.height);
				Find.WindowStack.ImmediateWindow(1439870015, rect, WindowLayer.GameUI, delegate
				{
					if (localSpecificHealthTabForPawn.DestroyedOrNull())
					{
						return;
					}
					HealthCardUtility.DrawPawnHealthCard(new Rect(Vector2.zero, rect.size), localSpecificHealthTabForPawn, false, true, localSpecificHealthTabForPawn);
					if (Widgets.CloseButtonFor(rect.AtZero()))
					{
						this.specificHealthTabForPawn = null;
						SoundDefOf.TabClose.PlayOneShotOnCamera(null);
					}
				}, true, false, 1f, null, false);
			}
		}

		// Token: 0x0600E5EA RID: 58858 RVA: 0x00557518 File Offset: 0x00555718
		private void DoColumnHeaders(ref float curY)
		{
			if (!this.compactMode)
			{
				float num = 135f;
				Text.Anchor = TextAnchor.UpperCenter;
				GUI.color = Widgets.SeparatorLabelColor;
				Widgets.Label(new Rect(num, 3f, 100f, 100f), "Pain".Translate());
				num += 100f;
				List<PawnCapacityDef> list = this.CapacitiesToDisplay;
				for (int i = 0; i < list.Count; i++)
				{
					Widgets.Label(new Rect(num, 3f, 100f, 100f), list[i].LabelCap.Truncate(100f, null));
					num += 100f;
				}
				Rect rect = new Rect(num + 8f, 0f, 24f, 24f);
				GUI.DrawTexture(rect, WITab_Caravan_Health.BeCarriedIfSickIcon);
				TooltipHandler.TipRegionByKey(rect, "BeCarriedIfSickTip");
				num += 40f;
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
			}
		}

		// Token: 0x0600E5EB RID: 58859 RVA: 0x0055760C File Offset: 0x0055580C
		private void DoRows(ref float curY, Rect scrollViewRect, Rect scrollOutRect)
		{
			List<Pawn> pawns = this.Pawns;
			if (this.specificHealthTabForPawn != null && !pawns.Contains(this.specificHealthTabForPawn))
			{
				this.specificHealthTabForPawn = null;
			}
			bool flag = false;
			for (int i = 0; i < pawns.Count; i++)
			{
				Pawn pawn = pawns[i];
				if (pawn.IsColonist)
				{
					if (!flag)
					{
						Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanColonists".Translate());
						flag = true;
					}
					this.DoRow(ref curY, scrollViewRect, scrollOutRect, pawn);
				}
			}
			bool flag2 = false;
			for (int j = 0; j < pawns.Count; j++)
			{
				Pawn pawn2 = pawns[j];
				if (!pawn2.IsColonist)
				{
					if (!flag2)
					{
						Widgets.ListSeparator(ref curY, scrollViewRect.width, ModsConfig.BiotechActive ? "CaravanPrisonersAnimalsAndMechs".Translate() : "CaravanPrisonersAndAnimals".Translate());
						flag2 = true;
					}
					this.DoRow(ref curY, scrollViewRect, scrollOutRect, pawn2);
				}
			}
		}

		// Token: 0x0600E5EC RID: 58860 RVA: 0x005576F8 File Offset: 0x005558F8
		private Vector2 GetRawSize(bool compactMode)
		{
			float num = 100f;
			if (!compactMode)
			{
				num += 100f;
				num += (float)this.CapacitiesToDisplay.Count * 100f;
				num += 40f;
			}
			Vector2 result;
			result.x = 127f + num + 16f;
			result.y = Mathf.Min(550f, this.PaneTopY - 30f);
			return result;
		}

		// Token: 0x0600E5ED RID: 58861 RVA: 0x00557768 File Offset: 0x00555968
		private void DoRow(ref float curY, Rect viewRect, Rect scrollOutRect, Pawn p)
		{
			float num = this.scrollPosition.y - 40f;
			float num2 = this.scrollPosition.y + scrollOutRect.height;
			if (curY > num && curY < num2)
			{
				this.DoRow(new Rect(0f, curY, viewRect.width, 40f), p);
			}
			curY += 40f;
		}

		// Token: 0x0600E5EE RID: 58862 RVA: 0x005577D0 File Offset: 0x005559D0
		private void DoRow(Rect rect, Pawn p)
		{
			Widgets.BeginGroup(rect);
			Rect rect2 = rect.AtZero();
			CaravanThingsTabUtility.DoAbandonButton(rect2, p, base.SelCaravan);
			rect2.width -= 24f;
			Widgets.InfoCardButton(rect2.width - 24f, (rect.height - 24f) / 2f, p);
			rect2.width -= 24f;
			CaravanThingsTabUtility.DoOpenSpecificTabButton(rect2, p, ref this.specificHealthTabForPawn);
			rect2.width -= 24f;
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
			}
			Rect rect3 = new Rect(4f, (rect.height - 27f) / 2f, 27f, 27f);
			Widgets.ThingIcon(rect3, p, 1f, null, false, 1f, false);
			Rect bgRect = new Rect(rect3.xMax + 4f, 11f, 100f, 18f);
			GenMapUI.DrawPawnLabel(p, bgRect, 1f, 100f, null, GameFont.Small, false, false);
			float num = bgRect.xMax;
			if (!this.compactMode)
			{
				if (p.RaceProps.IsFlesh)
				{
					Rect rect4 = new Rect(num, 0f, 100f, 40f);
					this.DoPain(rect4, p);
				}
				num += 100f;
				List<PawnCapacityDef> list = this.CapacitiesToDisplay;
				for (int i = 0; i < list.Count; i++)
				{
					Rect rect5 = new Rect(num, 0f, 100f, 40f);
					if ((p.RaceProps.Humanlike && !list[i].showOnHumanlikes) || (p.RaceProps.Animal && !list[i].showOnAnimals) || (p.RaceProps.IsAnomalyEntity && !list[i].showOnAnomalyEntities) || (p.RaceProps.IsMechanoid && !list[i].showOnMechanoids) || (p.RaceProps.IsDrone && !list[i].showOnDrones) || !PawnCapacityUtility.BodyCanEverDoCapacity(p.RaceProps.body, list[i]))
					{
						num += 100f;
					}
					else
					{
						this.DoCapacity(rect5, p, list[i]);
						num += 100f;
					}
				}
			}
			if (!this.compactMode)
			{
				Vector2 vector = new Vector2(num + 8f, 8f);
				Widgets.Checkbox(vector, ref p.health.beCarriedByCaravanIfSick, 24f, false, true, null, null);
				TooltipHandler.TipRegionByKey(new Rect(vector, new Vector2(24f, 24f)), "BeCarriedIfSickTip");
				num += 40f;
			}
			if (p.Downed && !p.ageTracker.CurLifeStage.alwaysDowned)
			{
				GUI.color = new Color(1f, 0f, 0f, 0.5f);
				Widgets.DrawLineHorizontal(0f, rect.height / 2f, rect.width);
				GUI.color = Color.white;
			}
			Widgets.EndGroup();
		}

		// Token: 0x0600E5EF RID: 58863 RVA: 0x00557B0C File Offset: 0x00555D0C
		private void DoPain(Rect rect, Pawn pawn)
		{
			Pair<string, Color> painLabel = HealthCardUtility.GetPainLabel(pawn);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			GUI.color = painLabel.Second;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, painLabel.First);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			if (Mouse.IsOver(rect))
			{
				string painTip = HealthCardUtility.GetPainTip(pawn);
				TooltipHandler.TipRegion(rect, painTip);
			}
		}

		// Token: 0x0600E5F0 RID: 58864 RVA: 0x00557B78 File Offset: 0x00555D78
		private void DoCapacity(Rect rect, Pawn pawn, PawnCapacityDef capacity)
		{
			Pair<string, Color> efficiencyLabel = HealthCardUtility.GetEfficiencyLabel(pawn, capacity);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			GUI.color = efficiencyLabel.Second;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, efficiencyLabel.First);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			if (Mouse.IsOver(rect))
			{
				string pawnCapacityTip = HealthCardUtility.GetPawnCapacityTip(pawn, capacity);
				TooltipHandler.TipRegion(rect, pawnCapacityTip);
			}
		}

		// Token: 0x0600E5F1 RID: 58865 RVA: 0x00557BE6 File Offset: 0x00555DE6
		public override void Notify_ClearingAllMapsMemory()
		{
			base.Notify_ClearingAllMapsMemory();
			this.specificHealthTabForPawn = null;
		}

		// Token: 0x0600E5F2 RID: 58866 RVA: 0x00557BF5 File Offset: 0x00555DF5
		private void EnsureSpecificHealthTabForPawnValid()
		{
			if (this.specificHealthTabForPawn != null && (this.specificHealthTabForPawn.Destroyed || !base.SelCaravan.ContainsPawn(this.specificHealthTabForPawn)))
			{
				this.specificHealthTabForPawn = null;
			}
		}

		// Token: 0x04009220 RID: 37408
		private Vector2 scrollPosition;

		// Token: 0x04009221 RID: 37409
		private float scrollViewHeight;

		// Token: 0x04009222 RID: 37410
		private Pawn specificHealthTabForPawn;

		// Token: 0x04009223 RID: 37411
		private bool compactMode;

		// Token: 0x04009224 RID: 37412
		private static List<PawnCapacityDef> capacitiesToDisplay = new List<PawnCapacityDef>();

		// Token: 0x04009225 RID: 37413
		private const float RowHeight = 40f;

		// Token: 0x04009226 RID: 37414
		private const float PawnLabelHeight = 18f;

		// Token: 0x04009227 RID: 37415
		private const float PawnLabelColumnWidth = 100f;

		// Token: 0x04009228 RID: 37416
		private const float SpaceAroundIcon = 4f;

		// Token: 0x04009229 RID: 37417
		private const float PawnCapacityColumnWidth = 100f;

		// Token: 0x0400922A RID: 37418
		private const float BeCarriedIfSickColumnWidth = 40f;

		// Token: 0x0400922B RID: 37419
		private const float BeCarriedIfSickIconSize = 24f;

		// Token: 0x0400922C RID: 37420
		private static readonly Texture2D BeCarriedIfSickIcon = ContentFinder<Texture2D>.Get("UI/Icons/CarrySick", true);
	}
}
