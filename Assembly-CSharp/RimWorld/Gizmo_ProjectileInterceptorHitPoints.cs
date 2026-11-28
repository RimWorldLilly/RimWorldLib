using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001941 RID: 6465
	[StaticConstructorOnStartup]
	public class Gizmo_ProjectileInterceptorHitPoints : Gizmo
	{
		// Token: 0x0600A23B RID: 41531 RVA: 0x0039FD3F File Offset: 0x0039DF3F
		public Gizmo_ProjectileInterceptorHitPoints()
		{
			this.Order = -100f;
		}

		// Token: 0x0600A23C RID: 41532 RVA: 0x0039FD52 File Offset: 0x0039DF52
		public override float GetWidth(float maxWidth)
		{
			return 140f;
		}

		// Token: 0x0600A23D RID: 41533 RVA: 0x0039FD5C File Offset: 0x0039DF5C
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(6f);
			Widgets.DrawWindowBackground(rect);
			bool flag = this.interceptor.ChargingTicksLeft > 0;
			TaggedString label = (!flag) ? "ShieldEnergy".Translate() : "ShieldTimeToRecovery".Translate();
			float fillPercent = (!flag) ? ((float)this.interceptor.currentHitPoints / (float)this.interceptor.HitPointsMax) : ((float)this.interceptor.ChargingTicksLeft / (float)this.interceptor.Props.chargeDurationTicks);
			string label2 = (!flag) ? (this.interceptor.currentHitPoints.ToString() + " / " + this.interceptor.HitPointsMax.ToString()) : this.interceptor.ChargingTicksLeft.ToStringTicksToPeriod(true, false, true, true, false);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect3 = new Rect(rect2.x, rect2.y - 2f, rect2.width, rect2.height / 2f);
			Widgets.Label(rect3, label);
			Rect rect4 = new Rect(rect2.x, rect3.yMax, rect2.width, rect2.height / 2f);
			Widgets.FillableBar(rect4, fillPercent, Gizmo_ProjectileInterceptorHitPoints.FullBarTex, Gizmo_ProjectileInterceptorHitPoints.EmptyBarTex, false);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect4, label2);
			Text.Anchor = TextAnchor.UpperLeft;
			if (!this.interceptor.Props.gizmoTipKey.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect2, this.interceptor.Props.gizmoTipKey.Translate());
			}
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x04005FDC RID: 24540
		public CompProjectileInterceptor interceptor;

		// Token: 0x04005FDD RID: 24541
		private static readonly Texture2D FullBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.2f, 0.24f));

		// Token: 0x04005FDE RID: 24542
		private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

		// Token: 0x04005FDF RID: 24543
		private const float Width = 140f;

		// Token: 0x04005FE0 RID: 24544
		public const int InRectPadding = 6;
	}
}
