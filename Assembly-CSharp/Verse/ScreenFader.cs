using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000723 RID: 1827
	[StaticConstructorOnStartup]
	public static class ScreenFader
	{
		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x060039C0 RID: 14784 RVA: 0x0016FED7 File Offset: 0x0016E0D7
		private static float CurTime
		{
			get
			{
				return Time.realtimeSinceStartup;
			}
		}

		// Token: 0x060039C1 RID: 14785 RVA: 0x0016FEE0 File Offset: 0x0016E0E0
		static ScreenFader()
		{
			ScreenFader.fadeTexture = new Texture2D(1, 1);
			ScreenFader.fadeTexture.name = "ScreenFader";
			ScreenFader.backgroundStyle.normal.background = ScreenFader.fadeTexture;
			ScreenFader.fadeTextureDirty = true;
		}

		// Token: 0x060039C2 RID: 14786 RVA: 0x0016FF84 File Offset: 0x0016E184
		public static void OverlayOnGUI(Vector2 windowSize)
		{
			Color color = ScreenFader.CurrentInstantColor();
			if (color.a > 0f)
			{
				if (ScreenFader.fadeTextureDirty)
				{
					ScreenFader.fadeTexture.SetPixel(0, 0, color);
					ScreenFader.fadeTexture.Apply();
				}
				GUI.Label(new Rect(-10f, -10f, windowSize.x + 10f, windowSize.y + 10f), ScreenFader.fadeTexture, ScreenFader.backgroundStyle);
			}
		}

		// Token: 0x060039C3 RID: 14787 RVA: 0x0016FFF8 File Offset: 0x0016E1F8
		private static Color CurrentInstantColor()
		{
			if (ScreenFader.CurTime > ScreenFader.targetTime || ScreenFader.targetTime == ScreenFader.sourceTime)
			{
				return ScreenFader.targetColor;
			}
			return Color.Lerp(ScreenFader.sourceColor, ScreenFader.targetColor, (ScreenFader.CurTime - ScreenFader.sourceTime) / (ScreenFader.targetTime - ScreenFader.sourceTime));
		}

		// Token: 0x060039C4 RID: 14788 RVA: 0x00170049 File Offset: 0x0016E249
		public static void SetColor(Color newColor)
		{
			ScreenFader.sourceColor = newColor;
			ScreenFader.targetColor = newColor;
			ScreenFader.targetTime = 0f;
			ScreenFader.sourceTime = 0f;
			ScreenFader.fadeTextureDirty = true;
		}

		// Token: 0x060039C5 RID: 14789 RVA: 0x00170071 File Offset: 0x0016E271
		public static void StartFade(Color finalColor, float duration)
		{
			ScreenFader.StartFade(finalColor, duration, 0f);
		}

		// Token: 0x060039C6 RID: 14790 RVA: 0x0017007F File Offset: 0x0016E27F
		public static void StartFade(Color finalColor, float duration, float delay)
		{
			if (duration <= 0f)
			{
				ScreenFader.SetColor(finalColor);
				return;
			}
			ScreenFader.sourceColor = ScreenFader.CurrentInstantColor();
			ScreenFader.targetColor = finalColor;
			ScreenFader.sourceTime = ScreenFader.CurTime + delay;
			ScreenFader.targetTime = ScreenFader.sourceTime + duration;
		}

		// Token: 0x060039C7 RID: 14791 RVA: 0x001700B8 File Offset: 0x0016E2B8
		public static bool IsFading()
		{
			return Current.ProgramState == ProgramState.Playing && !LongEventHandler.ShouldWaitForEvent && ScreenFader.CurTime < ScreenFader.targetTime;
		}

		// Token: 0x0400260C RID: 9740
		private static GUIStyle backgroundStyle = new GUIStyle();

		// Token: 0x0400260D RID: 9741
		private static Texture2D fadeTexture;

		// Token: 0x0400260E RID: 9742
		private static Color sourceColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x0400260F RID: 9743
		private static Color targetColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x04002610 RID: 9744
		private static float sourceTime = 0f;

		// Token: 0x04002611 RID: 9745
		private static float targetTime = 0f;

		// Token: 0x04002612 RID: 9746
		private static bool fadeTextureDirty = true;
	}
}
