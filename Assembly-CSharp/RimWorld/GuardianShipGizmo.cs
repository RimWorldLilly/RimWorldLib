using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010C6 RID: 4294
	[StaticConstructorOnStartup]
	public class GuardianShipGizmo : Gizmo
	{
		// Token: 0x06006B56 RID: 27478 RVA: 0x0025A680 File Offset: 0x00258880
		public GuardianShipGizmo(QuestPart_GuardianShipDelay delay)
		{
			this.delay = delay;
			this.Order = -100f;
		}

		// Token: 0x06006B57 RID: 27479 RVA: 0x0025A69A File Offset: 0x0025889A
		public override float GetWidth(float maxWidth)
		{
			return 110f;
		}

		// Token: 0x06006B58 RID: 27480 RVA: 0x0025A6A4 File Offset: 0x002588A4
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(6f);
			Widgets.DrawWindowBackground(rect);
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect2, "GuardianShipPayment".Translate(this.delay.TicksLeft.ToStringTicksToPeriod(true, false, true, true, false)));
			GenUI.ResetLabelAlign();
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x0400443B RID: 17467
		private QuestPart_GuardianShipDelay delay;
	}
}
