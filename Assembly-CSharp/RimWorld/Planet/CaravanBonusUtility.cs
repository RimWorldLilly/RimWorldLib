using System;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002381 RID: 9089
	public static class CaravanBonusUtility
	{
		// Token: 0x0600E207 RID: 57863 RVA: 0x0053D6B5 File Offset: 0x0053B8B5
		public static bool HasCaravanBonus(Pawn pawn)
		{
			return pawn.RaceProps.Humanlike && !pawn.Downed && pawn.GetStatValue(StatDefOf.CaravanBonusSpeedFactor, true, -1) > 1f;
		}

		// Token: 0x0600E208 RID: 57864 RVA: 0x0053D6E4 File Offset: 0x0053B8E4
		public static string GetIconTooltipText(Pawn pawn)
		{
			float statValue = pawn.GetStatValue(StatDefOf.CaravanBonusSpeedFactor, true, -1);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("CaravanBonusTip".Translate());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(StatDefOf.CaravanBonusSpeedFactor.LabelCap);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted(statValue.ToStringPercent());
			stringBuilder.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
			return stringBuilder.ToString();
		}
	}
}
