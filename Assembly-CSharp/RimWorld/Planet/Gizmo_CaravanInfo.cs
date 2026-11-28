using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200239E RID: 9118
	public class Gizmo_CaravanInfo : Gizmo
	{
		// Token: 0x0600E2ED RID: 58093 RVA: 0x0054620C File Offset: 0x0054440C
		public Gizmo_CaravanInfo(Caravan caravan)
		{
			this.caravan = caravan;
			this.Order = -100f;
		}

		// Token: 0x0600E2EE RID: 58094 RVA: 0x00546226 File Offset: 0x00544426
		public override float GetWidth(float maxWidth)
		{
			return Mathf.Min(520f, maxWidth);
		}

		// Token: 0x0600E2EF RID: 58095 RVA: 0x00546234 File Offset: 0x00544434
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			if (!this.caravan.Spawned)
			{
				return new GizmoResult(GizmoState.Clear);
			}
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Widgets.DrawWindowBackground(rect);
			Widgets.BeginGroup(rect);
			Rect rect2 = rect.AtZero();
			int? ticksToArrive = this.caravan.pather.Moving ? new int?(CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this.caravan, true)) : null;
			StringBuilder stringBuilder = new StringBuilder();
			float tilesPerDay = TilesPerDayCalculator.ApproxTilesPerDay(this.caravan, stringBuilder);
			CaravanUIUtility.DrawCaravanInfo(new CaravanUIUtility.CaravanInfo(this.caravan.MassUsage, this.caravan.MassCapacity, this.caravan.MassCapacityExplanation, tilesPerDay, stringBuilder.ToString(), this.caravan.DaysWorthOfFood, this.caravan.forage.ForagedFoodPerDay, this.caravan.forage.ForagedFoodPerDayExplanation, this.caravan.Visibility, this.caravan.VisibilityExplanation, -1f, -1f, null), null, this.caravan.Tile, ticksToArrive, -9999f, rect2, true, null, true, true);
			Widgets.EndGroup();
			GenUI.AbsorbClicksInRect(rect);
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x040090D2 RID: 37074
		private Caravan caravan;
	}
}
