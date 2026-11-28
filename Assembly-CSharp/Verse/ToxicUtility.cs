using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200028B RID: 651
	public static class ToxicUtility
	{
		// Token: 0x060013A3 RID: 5027 RVA: 0x0006F4B4 File Offset: 0x0006D6B4
		public static void PawnToxicTickInterval(Pawn pawn, int delta)
		{
			if (!pawn.IsHashIntervalTick(3451, delta) || !pawn.Spawned)
			{
				return;
			}
			float num = pawn.Position.GetTerrain(pawn.Map).toxicBuildupFactor;
			if (ModsConfig.BiotechActive && pawn.Position.IsPolluted(pawn.Map))
			{
				num += 1f;
			}
			if (num > 0f)
			{
				ToxicUtility.DoPawnToxicDamage(pawn, num);
			}
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x0006F520 File Offset: 0x0006D720
		public static void DoAirbornePawnToxicDamage(Pawn p, float extraFactor = 1f)
		{
			if (p.Spawned && p.Position.Roofed(p.Map))
			{
				return;
			}
			ToxicUtility.DoPawnToxicDamage(p, extraFactor);
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x0006F548 File Offset: 0x0006D748
		public static void DoPawnToxicDamage(Pawn p, float extraFactor = 1f)
		{
			float num = 0.023006668f;
			num *= Mathf.Max(1f - p.GetStatValue(StatDefOf.ToxicResistance, true, -1), 0f);
			num *= Mathf.Max(1f - p.GetStatValue(StatDefOf.ToxicEnvironmentResistance, true, -1), 0f);
			num *= extraFactor;
			if (num != 0f)
			{
				float num2 = Mathf.Lerp(0.85f, 1.15f, Rand.ValueSeeded(p.thingIDNumber ^ 74374237));
				num *= num2;
				HealthUtility.AdjustSeverity(p, HediffDefOf.ToxicBuildup, num);
			}
		}

		// Token: 0x04001103 RID: 4355
		public const int CheckInterval = 3451;

		// Token: 0x04001104 RID: 4356
		private const float ToxicPerDay = 0.4f;
	}
}
