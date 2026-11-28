using System;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000045 RID: 69
	[StaticConstructorOnStartup]
	public class Dialog_DevCelestial : Window_Dev
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001CB RID: 459 RVA: 0x0000CEA2 File Offset: 0x0000B0A2
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001CC RID: 460 RVA: 0x0000CEA5 File Offset: 0x0000B0A5
		protected override float Margin
		{
			get
			{
				return 4f;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001CD RID: 461 RVA: 0x0000CEAC File Offset: 0x0000B0AC
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(230f, 275f);
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000CEC0 File Offset: 0x0000B0C0
		public Dialog_DevCelestial()
		{
			this.draggable = true;
			this.focusWhenOpened = false;
			this.drawShadow = false;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.preventCameraMotion = false;
			this.drawInScreenshotMode = false;
			this.windowPosition = Prefs.DevPalettePosition;
			this.onlyDrawInDevMode = true;
			this.doCloseX = true;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000CF20 File Offset: 0x0000B120
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			Rect rect = new Rect(inRect.x, inRect.y, inRect.width, 24f);
			DevGUI.Label(rect, "Celestial Debugger");
			float num = rect.height + 6f;
			Text.Font = GameFont.Tiny;
			Map currentMap = Find.CurrentMap;
			GenCelestial.LightInfo lightSourceInfo = GenCelestial.GetLightSourceInfo(currentMap, GenCelestial.LightType.Shadow);
			GenCelestial.LightInfo lightSourceInfo2 = GenCelestial.GetLightSourceInfo(currentMap, GenCelestial.LightType.LightingSun);
			Vector2 vector = Find.WorldGrid.LongLatOf(currentMap.Tile);
			this.PrintLabel("Map", inRect, ref num);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Day progress: ");
			defaultInterpolatedStringHandler.AppendFormatted<float>(GenLocalDate.DayPercent(currentMap) * 100f, "0");
			defaultInterpolatedStringHandler.AppendLiteral("%");
			this.PrintLabel(defaultInterpolatedStringHandler.ToStringAndClear(), inRect, ref num);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(13, 2);
			defaultInterpolatedStringHandler2.AppendLiteral("Long, Lat: ");
			defaultInterpolatedStringHandler2.AppendFormatted<float>(vector.x, "0.0");
			defaultInterpolatedStringHandler2.AppendLiteral(", ");
			defaultInterpolatedStringHandler2.AppendFormatted<float>(vector.y, "0.0");
			this.PrintLabel(defaultInterpolatedStringHandler2.ToStringAndClear(), inRect, ref num);
			this.PrintLabel("", inRect, ref num);
			this.PrintLabel("Shadow Info", inRect, ref num);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(8, 1);
			defaultInterpolatedStringHandler3.AppendLiteral("Vector: ");
			defaultInterpolatedStringHandler3.AppendFormatted<Vector2>(lightSourceInfo.vector);
			this.PrintLabel(defaultInterpolatedStringHandler3.ToStringAndClear(), inRect, ref num);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(12, 1);
			defaultInterpolatedStringHandler4.AppendLiteral("Intensity: ");
			defaultInterpolatedStringHandler4.AppendFormatted<float>(lightSourceInfo.intensity * 100f, "0");
			defaultInterpolatedStringHandler4.AppendLiteral("%");
			this.PrintLabel(defaultInterpolatedStringHandler4.ToStringAndClear(), inRect, ref num);
			this.PrintLabel("", inRect, ref num);
			this.PrintLabel("Sun Info", inRect, ref num);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler5 = new DefaultInterpolatedStringHandler(8, 1);
			defaultInterpolatedStringHandler5.AppendLiteral("Vector: ");
			defaultInterpolatedStringHandler5.AppendFormatted<Vector2>(lightSourceInfo2.vector);
			this.PrintLabel(defaultInterpolatedStringHandler5.ToStringAndClear(), inRect, ref num);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler6 = new DefaultInterpolatedStringHandler(12, 1);
			defaultInterpolatedStringHandler6.AppendLiteral("Intensity: ");
			defaultInterpolatedStringHandler6.AppendFormatted<float>(lightSourceInfo2.intensity * 100f, "0");
			defaultInterpolatedStringHandler6.AppendLiteral("%");
			this.PrintLabel(defaultInterpolatedStringHandler6.ToStringAndClear(), inRect, ref num);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000D17E File Offset: 0x0000B37E
		private void PrintLabel(string text, Rect container, ref float y)
		{
			DevGUI.Label(new Rect(container.x, y, container.width, 20f), text);
			y += 20f;
		}

		// Token: 0x040000DC RID: 220
		private Vector2 windowPosition;

		// Token: 0x040000DD RID: 221
		private const string Title = "Celestial Debugger";
	}
}
