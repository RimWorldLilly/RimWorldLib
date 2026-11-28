using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C2A RID: 7210
	public static class MoodThresholdExtensions
	{
		// Token: 0x0600B7EE RID: 47086 RVA: 0x0040EAE0 File Offset: 0x0040CCE0
		public static Color GetColor(this MoodThreshold m)
		{
			Color c = Color.clear;
			float transparency = 0f;
			switch (m)
			{
			case MoodThreshold.Minor:
				c = ColorLibrary.Gold;
				transparency = 0.9f;
				break;
			case MoodThreshold.Major:
				c = ColorLibrary.Orange;
				transparency = 0.9f;
				break;
			case MoodThreshold.Extreme:
				c = ColorLibrary.RedReadable;
				transparency = 1f * Pulser.PulseBrightness(0.9f, 0.6f) + 0.1f;
				break;
			}
			return c.ToTransparent(transparency);
		}

		// Token: 0x0600B7EF RID: 47087 RVA: 0x0040EB54 File Offset: 0x0040CD54
		public static float EdgeExpansion(this MoodThreshold m)
		{
			switch (m)
			{
			case MoodThreshold.None:
				return 0f;
			case MoodThreshold.Minor:
				return 0f;
			case MoodThreshold.Major:
				return 5f;
			case MoodThreshold.Extreme:
				return 6f;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600B7F0 RID: 47088 RVA: 0x0040EB8C File Offset: 0x0040CD8C
		public static MoodThreshold CurrentMoodThresholdFor(Pawn pawn)
		{
			if (pawn.mindState == null)
			{
				return MoodThreshold.None;
			}
			if (pawn.mindState.mentalBreaker.CanDoRandomMentalBreaks)
			{
				if (pawn.mindState.mentalBreaker.BreakExtremeIsImminent)
				{
					return MoodThreshold.Extreme;
				}
				if (pawn.mindState.mentalBreaker.BreakMajorIsImminent)
				{
					return MoodThreshold.Major;
				}
				if (pawn.mindState.mentalBreaker.BreakMinorIsImminent)
				{
					return MoodThreshold.Minor;
				}
			}
			return MoodThreshold.None;
		}

		// Token: 0x0400698F RID: 27023
		private const float Alpha_Off = 0f;

		// Token: 0x04006990 RID: 27024
		private const float Alpha_Subtle = 0.9f;

		// Token: 0x04006991 RID: 27025
		private const float Alpha_Intense = 1f;
	}
}
