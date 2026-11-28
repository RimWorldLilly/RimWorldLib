using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B4A RID: 6986
	[StaticConstructorOnStartup]
	public class Gizmo_EnergyShieldStatus : Gizmo
	{
		// Token: 0x0600B35A RID: 45914 RVA: 0x003F3600 File Offset: 0x003F1800
		public Gizmo_EnergyShieldStatus()
		{
			this.Order = -100f;
		}

		// Token: 0x0600B35B RID: 45915 RVA: 0x003F3613 File Offset: 0x003F1813
		public override float GetWidth(float maxWidth)
		{
			return 140f;
		}

		// Token: 0x0600B35C RID: 45916 RVA: 0x003F361C File Offset: 0x003F181C
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(6f);
			Widgets.DrawWindowBackground(rect);
			Rect rect3 = rect2;
			rect3.height = rect.height / 2f;
			Text.Font = GameFont.Tiny;
			Widgets.Label(rect3, this.shield.IsApparel ? this.shield.parent.LabelCap : "ShieldInbuilt".Translate().Resolve());
			Rect rect4 = rect2;
			rect4.yMin = rect2.y + rect2.height / 2f;
			float fillPercent = this.shield.Energy / Mathf.Max(1f, this.shield.parent.GetStatValue(StatDefOf.EnergyShieldEnergyMax, true, -1));
			Widgets.FillableBar(rect4, fillPercent, Gizmo_EnergyShieldStatus.FullShieldBarTex, Gizmo_EnergyShieldStatus.EmptyShieldBarTex, false);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect4, (this.shield.Energy * 100f).ToString("F0") + " / " + (this.shield.parent.GetStatValue(StatDefOf.EnergyShieldEnergyMax, true, -1) * 100f).ToString("F0"));
			Text.Anchor = TextAnchor.UpperLeft;
			TooltipHandler.TipRegion(rect2, "ShieldPersonalTip".Translate());
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x04006776 RID: 26486
		public CompShield shield;

		// Token: 0x04006777 RID: 26487
		private static readonly Texture2D FullShieldBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.2f, 0.24f));

		// Token: 0x04006778 RID: 26488
		private static readonly Texture2D EmptyShieldBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);
	}
}
