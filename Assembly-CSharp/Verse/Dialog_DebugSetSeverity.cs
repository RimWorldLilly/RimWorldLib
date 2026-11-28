using System;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020005DA RID: 1498
	public class Dialog_DebugSetSeverity : Window
	{
		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06002F91 RID: 12177 RVA: 0x00131287 File Offset: 0x0012F487
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(300f, 125f);
			}
		}

		// Token: 0x06002F92 RID: 12178 RVA: 0x00131298 File Offset: 0x0012F498
		protected override void SetInitialSizeAndPosition()
		{
			Vector2 vector = UI.MousePositionOnUIInverted + Dialog_DebugSetSeverity.InitialPositionShift;
			if (vector.x + this.InitialSize.x > (float)UI.screenWidth)
			{
				vector.x = (float)UI.screenWidth - this.InitialSize.x;
			}
			if (vector.y + this.InitialSize.y > (float)UI.screenHeight)
			{
				vector.y = (float)UI.screenHeight - this.InitialSize.y;
			}
			this.windowRect = new Rect(vector.x, vector.y, this.InitialSize.x, this.InitialSize.y);
		}

		// Token: 0x06002F93 RID: 12179 RVA: 0x00131348 File Offset: 0x0012F548
		public Dialog_DebugSetSeverity(Hediff hediff) : base(null)
		{
			this.hediff = hediff;
			this.maxSeverity = ((hediff.def.maxSeverity >= float.MaxValue) ? 1f : hediff.def.maxSeverity);
			this.sliderValue = Mathf.InverseLerp(hediff.def.minSeverity, this.maxSeverity, hediff.Severity);
			this.layer = WindowLayer.Super;
			this.closeOnClickedOutside = true;
			this.doWindowBackground = false;
			this.drawShadow = false;
			this.preventCameraMotion = false;
			SoundDefOf.FloatMenu_Open.PlayOneShotOnCamera(null);
		}

		// Token: 0x06002F94 RID: 12180 RVA: 0x001313F4 File Offset: 0x0012F5F4
		public override void DoWindowContents(Rect inRect)
		{
			if (this.hediff == null || !this.hediff.pawn.health.hediffSet.hediffs.Contains(this.hediff))
			{
				this.Close(true);
			}
			Widgets.DrawWindowBackground(inRect);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(new Rect(0f, 0f, inRect.width, 40f), this.hediff.def.defName + " severity");
			Text.Anchor = TextAnchor.UpperLeft;
			Rect position = inRect;
			position.y = 40f;
			position.height = 10f;
			position.x += 20f;
			position.width -= 40f;
			this.sliderValue = GUI.HorizontalSlider(position, this.sliderValue, 0f, 1f);
			Rect rect = inRect;
			rect.y = inRect.height - 10f - this.BottomButtonSize.y;
			rect.height = this.BottomButtonSize.y;
			rect.x += 20f;
			rect.width -= 40f;
			float num = Mathf.Round(this.sliderValue * 100f);
			Rect rect2 = rect;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Set to ");
			defaultInterpolatedStringHandler.AppendFormatted<float>(num);
			defaultInterpolatedStringHandler.AppendLiteral("%");
			if (Widgets.ButtonText(rect2, defaultInterpolatedStringHandler.ToStringAndClear(), true, true, true, null))
			{
				float severity = Mathf.Lerp(this.hediff.def.minSeverity, this.maxSeverity, Mathf.Clamp01(num / 100f));
				this.hediff.Severity = severity;
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x04001E43 RID: 7747
		private Hediff hediff;

		// Token: 0x04001E44 RID: 7748
		private float maxSeverity;

		// Token: 0x04001E45 RID: 7749
		private float sliderValue;

		// Token: 0x04001E46 RID: 7750
		private static readonly Vector2 InitialPositionShift = new Vector2(4f, 0f);

		// Token: 0x04001E47 RID: 7751
		private readonly Vector2 BottomButtonSize = new Vector2(100f, 20f);

		// Token: 0x04001E48 RID: 7752
		private const float HeaderLabelHeight = 40f;

		// Token: 0x04001E49 RID: 7753
		private const float Padding = 20f;
	}
}
