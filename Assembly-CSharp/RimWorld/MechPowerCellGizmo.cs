using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018F2 RID: 6386
	[StaticConstructorOnStartup]
	public class MechPowerCellGizmo : Gizmo
	{
		// Token: 0x0600A01F RID: 40991 RVA: 0x00395A99 File Offset: 0x00393C99
		public MechPowerCellGizmo(CompMechPowerCell carrier)
		{
			this.powerCell = carrier;
		}

		// Token: 0x0600A020 RID: 40992 RVA: 0x00395AA8 File Offset: 0x00393CA8
		public override float GetWidth(float maxWidth)
		{
			return 160f;
		}

		// Token: 0x0600A021 RID: 40993 RVA: 0x00395AB0 File Offset: 0x00393CB0
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(10f);
			Widgets.DrawWindowBackground(rect);
			string text;
			if (!this.powerCell.Props.labelOverride.NullOrEmpty())
			{
				text = this.powerCell.Props.labelOverride;
			}
			else
			{
				text = "MechPowerCell".Translate();
			}
			Rect rect3 = new Rect(rect2.x, rect2.y, rect2.width, Text.CalcHeight(text, rect2.width) + 8f);
			Text.Font = GameFont.Small;
			Widgets.Label(rect3, text);
			MechPowerCellGizmo.<>c__DisplayClass7_1 CS$<>8__locals2;
			CS$<>8__locals2.barRect = new Rect(rect2.x, rect3.yMax, rect2.width, rect2.height - rect3.height);
			Widgets.FillableBar(CS$<>8__locals2.barRect, this.powerCell.PercentFull, MechPowerCellGizmo.BarTex, MechPowerCellGizmo.EmptyBarTex, true);
			for (int i = 2500; i < this.powerCell.Props.totalPowerTicks; i += 2500)
			{
				MechPowerCellGizmo.<GizmoOnGUI>g__DoBarThreshold|7_1((float)i / (float)this.powerCell.Props.totalPowerTicks, ref CS$<>8__locals2);
			}
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(CS$<>8__locals2.barRect, Mathf.CeilToInt((float)this.powerCell.PowerTicksLeft / 2500f).ToString() + "LetterHour".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			string tooltip;
			if (!this.powerCell.Props.tooltipOverride.NullOrEmpty())
			{
				tooltip = this.powerCell.Props.tooltipOverride;
			}
			else
			{
				tooltip = "MechPowerCellTip".Translate();
			}
			TooltipHandler.TipRegion(rect2, () => tooltip, Gen.HashCombineInt(this.powerCell.GetHashCode(), 34242369));
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x0600A023 RID: 40995 RVA: 0x00395CEC File Offset: 0x00393EEC
		[CompilerGenerated]
		internal static void <GizmoOnGUI>g__DoBarThreshold|7_1(float percent, ref MechPowerCellGizmo.<>c__DisplayClass7_1 A_1)
		{
			GUI.DrawTexture(new Rect
			{
				x = A_1.barRect.x + 3f + (A_1.barRect.width - 8f) * percent,
				y = A_1.barRect.y + A_1.barRect.height - 9f,
				width = 2f,
				height = 6f
			}, BaseContent.BlackTex);
		}

		// Token: 0x04005EB3 RID: 24243
		private CompMechPowerCell powerCell;

		// Token: 0x04005EB4 RID: 24244
		private const float Width = 160f;

		// Token: 0x04005EB5 RID: 24245
		private static readonly Texture2D BarTex = SolidColorMaterials.NewSolidColorTexture(new Color32(12, 45, 45, byte.MaxValue));

		// Token: 0x04005EB6 RID: 24246
		private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(GenUI.FillableBar_Empty);

		// Token: 0x04005EB7 RID: 24247
		private const int BarThresholdTickIntervals = 2500;
	}
}
