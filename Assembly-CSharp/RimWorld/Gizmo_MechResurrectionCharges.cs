using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200143F RID: 5183
	[StaticConstructorOnStartup]
	public class Gizmo_MechResurrectionCharges : Gizmo
	{
		// Token: 0x06007FB3 RID: 32691 RVA: 0x002D923D File Offset: 0x002D743D
		public Gizmo_MechResurrectionCharges(CompAbilityEffect_ResurrectMech ability)
		{
			this.ability = ability;
			this.Order = -100f;
		}

		// Token: 0x06007FB4 RID: 32692 RVA: 0x002D9257 File Offset: 0x002D7457
		public override float GetWidth(float maxWidth)
		{
			return Gizmo_MechResurrectionCharges.Width;
		}

		// Token: 0x06007FB5 RID: 32693 RVA: 0x002D9260 File Offset: 0x002D7460
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(6f);
			Widgets.DrawWindowBackground(rect);
			Rect rect3 = rect2;
			rect3.height = rect.height / 2f;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(rect3, "MechResurrectionCharges".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect4 = rect;
			rect4.y += rect3.height - 5f;
			rect4.height = rect.height / 2f;
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect4, this.ability.ChargesRemaining.ToString());
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x04004FE5 RID: 20453
		private static readonly Texture2D FullShieldBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.2f, 0.24f));

		// Token: 0x04004FE6 RID: 20454
		private static readonly Texture2D EmptyShieldBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

		// Token: 0x04004FE7 RID: 20455
		private static readonly float Width = 110f;

		// Token: 0x04004FE8 RID: 20456
		private CompAbilityEffect_ResurrectMech ability;
	}
}
