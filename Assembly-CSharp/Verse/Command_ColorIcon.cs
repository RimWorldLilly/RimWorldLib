using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000605 RID: 1541
	[StaticConstructorOnStartup]
	public class Command_ColorIcon : Command_Action
	{
		// Token: 0x06003052 RID: 12370 RVA: 0x00135DB0 File Offset: 0x00133FB0
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth, parms);
			if (this.color != null)
			{
				Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
				RectDivider rectDivider = new RectDivider(rect.ContractedBy(4f), 1552930585, null);
				GUI.DrawTexture(rectDivider.NewCol(16f, HorizontalJustification.Right, null).NewRow(16f, VerticalJustification.Top, null), Command_ColorIcon.ColorIndicatorTex, ScaleMode.ScaleToFit, true, 1f, this.color.Value, 0f, 0f);
			}
			return result;
		}

		// Token: 0x04001EFB RID: 7931
		private static readonly Texture2D ColorIndicatorTex = ContentFinder<Texture2D>.Get("UI/Icons/ColorIndicatorBulb", true);

		// Token: 0x04001EFC RID: 7932
		public Color32? color;

		// Token: 0x04001EFD RID: 7933
		private const int colorCircleDiameter = 16;

		// Token: 0x04001EFE RID: 7934
		private const float colorCircleGap = 4f;
	}
}
