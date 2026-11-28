using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001627 RID: 5671
	[StaticConstructorOnStartup]
	public static class IdeoImpactUtility
	{
		// Token: 0x06008DD5 RID: 36309 RVA: 0x0033C5F0 File Offset: 0x0033A7F0
		public static string MemeImpactLabel(int impact)
		{
			impact = Mathf.Clamp(impact, 1, 3);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
			defaultInterpolatedStringHandler.AppendLiteral("IdeoMemeImpactLabel_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(impact);
			return defaultInterpolatedStringHandler.ToStringAndClear().Translate();
		}

		// Token: 0x06008DD6 RID: 36310 RVA: 0x0033C638 File Offset: 0x0033A838
		public static string OverallImpactLabel(int impact)
		{
			impact = Mathf.Clamp(impact, 1, 9);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 1);
			defaultInterpolatedStringHandler.AppendLiteral("IdeoImpactLabel_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(impact);
			return defaultInterpolatedStringHandler.ToStringAndClear().Translate();
		}

		// Token: 0x06008DD7 RID: 36311 RVA: 0x0033C67F File Offset: 0x0033A87F
		public static void DrawImpactIcon(Rect rect, int impact)
		{
			impact = Mathf.Clamp(impact, 1, 3);
			Color color = GUI.color;
			GUI.color = IdeoImpactUtility.IconTint;
			GUI.DrawTexture(rect, IdeoImpactUtility.ImpactIcons[impact]);
			GUI.color = color;
		}

		// Token: 0x040056F2 RID: 22258
		public const int MaxMemeImpact = 3;

		// Token: 0x040056F3 RID: 22259
		public const int MaxCombinedImpact = 9;

		// Token: 0x040056F4 RID: 22260
		private static readonly Color IconTint = Color.Lerp(ColoredText.ImpactColor, new Color(0.3f, 0.3f, 0.3f, 1f), 0.9f);

		// Token: 0x040056F5 RID: 22261
		private static readonly Texture2D[] ImpactIcons = new Texture2D[]
		{
			null,
			ContentFinder<Texture2D>.Get("UI/Icons/Impact/MemeImpact1", true),
			ContentFinder<Texture2D>.Get("UI/Icons/Impact/MemeImpact2", true),
			ContentFinder<Texture2D>.Get("UI/Icons/Impact/MemeImpact3", true)
		};
	}
}
