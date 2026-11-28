using System;
using UnityEngine;
using Waylith.Profiling;

namespace Verse.ProfilerUI
{
	// Token: 0x02000792 RID: 1938
	internal static class ProfilerPalette
	{
		// Token: 0x06003DAF RID: 15791 RVA: 0x001892D3 File Offset: 0x001874D3
		public static int ColorIndexFor(ProfilerLabelId labelId)
		{
			if (FrameProfiler.IsPauseLabel(labelId))
			{
				return 13;
			}
			return (labelId.Value * 1103515245 + 12345) % 13;
		}

		// Token: 0x06003DB0 RID: 15792 RVA: 0x001892F8 File Offset: 0x001874F8
		public static Color ColorAt(int colorIndex)
		{
			if (colorIndex < ProfilerPalette.BakedColors.Length)
			{
				return ProfilerPalette.BakedColors[colorIndex];
			}
			if (colorIndex == 13)
			{
				return ProfilerPalette.GcPauseColor;
			}
			return Color.HSVToRGB((float)(colorIndex - ProfilerPalette.BakedColors.Length) / 6f, 0.35f, 0.5f);
		}

		// Token: 0x06003DB1 RID: 15793 RVA: 0x00189345 File Offset: 0x00187545
		public static Color ColorFor(ProfilerLabelId labelId)
		{
			if (!labelId.IsValid)
			{
				return ProfilerPalette.UnprofiledColor;
			}
			return ProfilerPalette.ColorAt(ProfilerPalette.ColorIndexFor(labelId));
		}

		// Token: 0x04002882 RID: 10370
		private static readonly Color[] BakedColors = new Color[]
		{
			new Color32(90, 120, 200, byte.MaxValue),
			new Color32(96, 160, 96, byte.MaxValue),
			new Color32(150, 100, 190, byte.MaxValue),
			new Color32(84, 160, 176, byte.MaxValue),
			new Color32(70, 140, 150, byte.MaxValue),
			new Color32(190, 160, 80, byte.MaxValue),
			new Color32(96, 96, 104, byte.MaxValue)
		};

		// Token: 0x04002883 RID: 10371
		private const int GeneratedHues = 6;

		// Token: 0x04002884 RID: 10372
		private const int HashedColorCount = 13;

		// Token: 0x04002885 RID: 10373
		public const int GcColorIndex = 13;

		// Token: 0x04002886 RID: 10374
		public const int ColorCount = 14;

		// Token: 0x04002887 RID: 10375
		public static readonly Color UnprofiledColor = new Color32(110, 110, 110, byte.MaxValue);

		// Token: 0x04002888 RID: 10376
		public static readonly Color GcPauseColor = new Color32(224, 112, 95, byte.MaxValue);

		// Token: 0x04002889 RID: 10377
		public static readonly Color ZoneLabelColor = ProfilerGui.Hex(4043309055U);
	}
}
