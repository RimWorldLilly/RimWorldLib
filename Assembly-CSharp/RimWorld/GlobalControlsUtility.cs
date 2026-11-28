using System;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D4B RID: 7499
	public static class GlobalControlsUtility
	{
		// Token: 0x0600C208 RID: 49672 RVA: 0x0046B0E4 File Offset: 0x004692E4
		public static void DoPlaySettings(WidgetRow rowVisibility, bool worldView, ref float curBaseY)
		{
			float y = curBaseY - TimeControls.TimeButSize.y;
			rowVisibility.Init((float)UI.screenWidth, y, UIDirection.LeftThenUp, 141f, 4f);
			Find.PlaySettings.DoPlaySettingsGlobalControls(rowVisibility, worldView);
			curBaseY = rowVisibility.FinalY;
		}

		// Token: 0x0600C209 RID: 49673 RVA: 0x0046B12C File Offset: 0x0046932C
		public static void DoTimespeedControls(float leftX, float width, ref float curBaseY)
		{
			leftX += Mathf.Max(0f, width - 150f);
			width = Mathf.Min(width, 150f);
			float y = TimeControls.TimeButSize.y;
			Rect timerRect = new Rect(leftX + 16f, curBaseY - y, width, y);
			TimeControls.DoTimeControlsGUI(timerRect);
			curBaseY -= timerRect.height;
		}

		// Token: 0x0600C20A RID: 49674 RVA: 0x0046B18C File Offset: 0x0046938C
		public static void DoDate(float leftX, float width, ref float curBaseY)
		{
			Rect dateRect = new Rect(leftX, curBaseY - DateReadout.Height, width, DateReadout.Height);
			DateReadout.DateOnGUI(dateRect);
			curBaseY -= dateRect.height;
		}

		// Token: 0x0600C20B RID: 49675 RVA: 0x0046B1C4 File Offset: 0x004693C4
		public static void DoRealtimeClock(float leftX, float width, ref float curBaseY)
		{
			Rect rect = new Rect(leftX - 20f, curBaseY - 26f, width + 20f - 7f, 26f);
			string format = Prefs.TwelveHourClockMode ? "hh:mm" : "HH:mm";
			string str = "";
			if (Prefs.TwelveHourClockMode)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 1);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>((DateTime.Now.Hour >= 12) ? "PM".Translate() : "AM".Translate());
				str = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			using (new TextBlock(TextAnchor.MiddleRight))
			{
				Widgets.Label(rect, DateTime.Now.ToString(format) + str);
			}
			curBaseY -= 26f;
		}

		// Token: 0x0600C20C RID: 49676 RVA: 0x0046B2B4 File Offset: 0x004694B4
		public static void DrawFpsCounter(float leftX, float width, ref float curBaseY)
		{
			float averageFrameTime = Root.AverageFrameTime;
			float value = 1000f / averageFrameTime;
			Rect rect = new Rect(leftX, curBaseY - 26f, width - 7f, 26f);
			Text.Anchor = TextAnchor.MiddleRight;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 2);
			defaultInterpolatedStringHandler.AppendLiteral("FPS: ");
			defaultInterpolatedStringHandler.AppendFormatted<float>(value, "F1");
			defaultInterpolatedStringHandler.AppendLiteral(" (");
			defaultInterpolatedStringHandler.AppendFormatted<float>(averageFrameTime, "F2");
			defaultInterpolatedStringHandler.AppendLiteral("ms/frame)");
			Widgets.Label(rect, defaultInterpolatedStringHandler.ToStringAndClear());
			Text.Anchor = TextAnchor.UpperLeft;
			curBaseY -= 26f;
		}

		// Token: 0x0600C20D RID: 49677 RVA: 0x0046B354 File Offset: 0x00469554
		public static void DrawTpsCounter(float leftX, float width, ref float curBaseY)
		{
			float meanTickTime = Find.TickManager.MeanTickTime;
			float num = 1000f / meanTickTime;
			float b = 60f * Find.TickManager.TickRateMultiplier;
			num = Mathf.Min(num, b);
			Rect rect = new Rect(leftX, curBaseY - 26f, width - 7f, 26f);
			Text.Anchor = TextAnchor.MiddleRight;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 2);
			defaultInterpolatedStringHandler.AppendLiteral("TPS: ");
			defaultInterpolatedStringHandler.AppendFormatted<float>(num, "F1");
			defaultInterpolatedStringHandler.AppendLiteral(" (");
			defaultInterpolatedStringHandler.AppendFormatted<float>(meanTickTime, "F2");
			defaultInterpolatedStringHandler.AppendLiteral("ms/tick)");
			Widgets.Label(rect, defaultInterpolatedStringHandler.ToStringAndClear());
			Text.Anchor = TextAnchor.UpperLeft;
			curBaseY -= 26f;
		}

		// Token: 0x0600C20E RID: 49678 RVA: 0x0046B414 File Offset: 0x00469614
		private static void FormatMemoryUsage(StringBuilder sb, long bytes)
		{
			if (bytes > 1073741824L)
			{
				sb.Append(((float)bytes / 1.0737418E+09f).ToString("F2"));
				sb.Append(" GiB");
				return;
			}
			if (bytes > 1048576L)
			{
				sb.Append(((float)bytes / 1048576f).ToString("F0"));
				sb.Append(" MiB");
				return;
			}
			if (bytes > 1024L)
			{
				sb.Append(((float)bytes / 1024f).ToString("F0"));
				sb.Append(" KiB");
				return;
			}
			sb.Append(bytes.ToString("F0"));
			sb.Append(" B");
		}

		// Token: 0x0600C20F RID: 49679 RVA: 0x0046B4D8 File Offset: 0x004696D8
		public static void DrawMemoryInfo(float leftX, float width, ref float curBaseY)
		{
			width += 40f;
			leftX -= 40f;
			long osMemoryUsageBytes = MemoryUsageUtility.OsMemoryUsageBytes;
			long graphicsMemoryUsageBytes = MemoryUsageUtility.GraphicsMemoryUsageBytes;
			long graphicsMemoryTotalBytes = MemoryUsageUtility.GraphicsMemoryTotalBytes;
			long managedMemoryUsageBytes = MemoryUsageUtility.ManagedMemoryUsageBytes;
			long managedMemoryReservedBytes = MemoryUsageUtility.ManagedMemoryReservedBytes;
			long audioMemoryUsageBytes = MemoryUsageUtility.AudioMemoryUsageBytes;
			long graphicsTextureMemoryUsageBytes = MemoryUsageUtility.GraphicsTextureMemoryUsageBytes;
			long graphicsMeshMemoryUsageBytes = MemoryUsageUtility.GraphicsMeshMemoryUsageBytes;
			Rect rect = new Rect(leftX, curBaseY - 104f, width - 7f, 104f);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Memory: ");
			GlobalControlsUtility.FormatMemoryUsage(stringBuilder, osMemoryUsageBytes);
			stringBuilder.AppendLine();
			stringBuilder.Append("C#: ");
			GlobalControlsUtility.FormatMemoryUsage(stringBuilder, managedMemoryUsageBytes);
			stringBuilder.Append('/');
			GlobalControlsUtility.FormatMemoryUsage(stringBuilder, managedMemoryReservedBytes);
			stringBuilder.AppendLine();
			stringBuilder.Append("VRAM: ");
			GlobalControlsUtility.FormatMemoryUsage(stringBuilder, graphicsMemoryUsageBytes);
			if (graphicsMemoryTotalBytes > 0L)
			{
				stringBuilder.Append(" / ");
				GlobalControlsUtility.FormatMemoryUsage(stringBuilder, graphicsMemoryTotalBytes);
			}
			stringBuilder.AppendLine();
			stringBuilder.Append('(');
			GlobalControlsUtility.FormatMemoryUsage(stringBuilder, graphicsTextureMemoryUsageBytes);
			stringBuilder.Append(" textures, ");
			GlobalControlsUtility.FormatMemoryUsage(stringBuilder, graphicsMeshMemoryUsageBytes);
			stringBuilder.AppendLine(" meshes)");
			stringBuilder.Append("Audio: ");
			GlobalControlsUtility.FormatMemoryUsage(stringBuilder, audioMemoryUsageBytes);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect, stringBuilder.ToString());
			Text.Anchor = TextAnchor.UpperLeft;
			curBaseY -= 104f;
		}

		// Token: 0x04006EF0 RID: 28400
		private const int VisibilityControlsPerRow = 5;
	}
}
