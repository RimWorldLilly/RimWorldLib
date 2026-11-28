using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200066E RID: 1646
	public class Dialog_RechargeSettings : Window
	{
		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x0600340F RID: 13327 RVA: 0x0014FC30 File Offset: 0x0014DE30
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(450f, 300f);
			}
		}

		// Token: 0x06003410 RID: 13328 RVA: 0x0014FC44 File Offset: 0x0014DE44
		public Dialog_RechargeSettings(MechanitorControlGroup controlGroup) : base(null)
		{
			this.controlGroup = controlGroup;
			this.range = controlGroup.mechRechargeThresholds;
			this.title = "MechRechargeSettingsTitle".Translate();
			this.text = "MechRechargeSettingsExplanation".Translate();
			this.forcePause = true;
			this.closeOnClickedOutside = true;
		}

		// Token: 0x06003411 RID: 13329 RVA: 0x0014FCA4 File Offset: 0x0014DEA4
		public override void DoWindowContents(Rect inRect)
		{
			float num = inRect.y;
			Rect rect = new Rect(inRect.x, num, inRect.width, 30f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect, this.title);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			num += rect.height + 17f;
			Rect rect2 = new Rect(inRect.x, num, inRect.width, Text.CalcHeight(this.text, inRect.width));
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.Font = GameFont.Small;
			Widgets.Label(rect2, this.text);
			Text.Anchor = TextAnchor.UpperLeft;
			num += rect2.height + 17f;
			Widgets.FloatRange(new Rect(inRect.x, num, inRect.width, 30f), this.GetHashCode(), ref this.range, 0f, 1f, null, ToStringStyle.PercentZero, 0.05f, GameFont.Small, new Color?(Color.white), 0f);
			this.range.min = GenMath.RoundTo(this.range.min, 0.01f);
			this.range.max = GenMath.RoundTo(this.range.max, 0.01f);
			if (Widgets.ButtonText(new Rect(inRect.x, inRect.yMax - Window.CloseButSize.y, Window.CloseButSize.x, Window.CloseButSize.y), "CancelButton".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(inRect.x + inRect.width / 2f - Window.CloseButSize.x / 2f, inRect.yMax - Window.CloseButSize.y, Window.CloseButSize.x, Window.CloseButSize.y), "Reset".Translate(), true, true, true, null))
			{
				this.range = MechanitorControlGroup.DefaultMechRechargeThresholds;
			}
			if (Widgets.ButtonText(new Rect(inRect.xMax - Window.CloseButSize.x, inRect.yMax - Window.CloseButSize.y, Window.CloseButSize.x, Window.CloseButSize.y), "OK".Translate(), true, true, true, null))
			{
				this.controlGroup.mechRechargeThresholds = this.range;
				this.Close(true);
			}
		}

		// Token: 0x04002208 RID: 8712
		private FloatRange range;

		// Token: 0x04002209 RID: 8713
		private MechanitorControlGroup controlGroup;

		// Token: 0x0400220A RID: 8714
		private string title;

		// Token: 0x0400220B RID: 8715
		private string text;

		// Token: 0x0400220C RID: 8716
		private const float HeaderHeight = 30f;

		// Token: 0x0400220D RID: 8717
		private const float SliderHeight = 30f;
	}
}
