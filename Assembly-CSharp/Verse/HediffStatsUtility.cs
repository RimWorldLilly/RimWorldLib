using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200017A RID: 378
	public static class HediffStatsUtility
	{
		// Token: 0x06000B13 RID: 2835 RVA: 0x0003C6F7 File Offset: 0x0003A8F7
		public static IEnumerable<StatDrawEntry> SpecialDisplayStats(HediffStage stage, Hediff instance)
		{
			HediffStatsUtility.<SpecialDisplayStats>d__0 <SpecialDisplayStats>d__ = new HediffStatsUtility.<SpecialDisplayStats>d__0(-2);
			<SpecialDisplayStats>d__.<>3__stage = stage;
			<SpecialDisplayStats>d__.<>3__instance = instance;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x0003C710 File Offset: 0x0003A910
		public static float GetStatOffsetForSeverity(StatDef stat, HediffStage stage, Pawn pawn, float severity)
		{
			float num = stage.statOffsets.GetStatOffsetFromList(stat);
			if (num != 0f)
			{
				if (stage.statOffsetEffectMultiplier != null)
				{
					num *= pawn.GetStatValue(stage.statOffsetEffectMultiplier, true, -1);
				}
				if (stage.multiplyStatChangesBySeverity)
				{
					num *= severity;
				}
				return num;
			}
			if (stage.statOffsetsBySeverity != null)
			{
				for (int i = 0; i < stage.statOffsetsBySeverity.Count; i++)
				{
					if (stage.statOffsetsBySeverity[i].stat == stat)
					{
						return stage.statOffsetsBySeverity[i].valueBySeverity.Evaluate(severity);
					}
				}
			}
			return 0f;
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x0003C7A8 File Offset: 0x0003A9A8
		public static float GetStatFactorForSeverity(StatDef stat, HediffStage stage, Pawn pawn, float severity)
		{
			float num = stage.statFactors.GetStatFactorFromList(stat);
			if (Math.Abs(num - 1f) > 1E-45f)
			{
				if (stage.statFactorEffectMultiplier != null)
				{
					num = StatWorker.ScaleFactor(num, pawn.GetStatValue(stage.statFactorEffectMultiplier, true, -1));
				}
				if (stage.multiplyStatChangesBySeverity)
				{
					num = StatWorker.ScaleFactor(num, severity);
				}
				return num;
			}
			if (stage.statFactorsBySeverity != null)
			{
				for (int i = 0; i < stage.statFactorsBySeverity.Count; i++)
				{
					if (stage.statFactorsBySeverity[i].stat == stat)
					{
						return stage.statFactorsBySeverity[i].valueBySeverity.Evaluate(severity);
					}
				}
			}
			return 1f;
		}
	}
}
