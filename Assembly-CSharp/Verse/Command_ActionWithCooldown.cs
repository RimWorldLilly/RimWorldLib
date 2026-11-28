using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000603 RID: 1539
	[StaticConstructorOnStartup]
	public class Command_ActionWithCooldown : Command_Action
	{
		// Token: 0x0600304C RID: 12364 RVA: 0x00135C94 File Offset: 0x00133E94
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth, parms);
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			if (this.cooldownPercentGetter != null)
			{
				float num = this.cooldownPercentGetter();
				if (num < 1f)
				{
					Widgets.FillableBar(rect, Mathf.Clamp01(num), Command_ActionWithCooldown.CooldownBarTex, null, false);
					Text.Font = GameFont.Tiny;
					Text.Anchor = TextAnchor.UpperCenter;
					Widgets.Label(rect, num.ToStringPercent("F0"));
					Text.Anchor = TextAnchor.UpperLeft;
				}
			}
			return result;
		}

		// Token: 0x04001EF7 RID: 7927
		public Func<float> cooldownPercentGetter;

		// Token: 0x04001EF8 RID: 7928
		private static readonly Texture2D CooldownBarTex = SolidColorMaterials.NewSolidColorTexture(new Color32(9, 203, 4, 64));
	}
}
