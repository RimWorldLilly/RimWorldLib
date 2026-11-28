using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200112C RID: 4396
	public static class AnomalyIncidentUtility
	{
		// Token: 0x06006E2F RID: 28207 RVA: 0x0026A0A0 File Offset: 0x002682A0
		public static bool IncidentShardChance(float combatPoints)
		{
			return Rand.Chance(AnomalyIncidentUtility.CombatPointShardChanceCurve.Evaluate(combatPoints));
		}

		// Token: 0x06006E30 RID: 28208 RVA: 0x0026A0B4 File Offset: 0x002682B4
		public static void PawnShardOnDeath(Pawn pawn)
		{
			pawn.health.AddHediff(HediffDefOf.ShardHolder, null, null, null);
		}

		// Token: 0x04004676 RID: 18038
		private static readonly SimpleCurve CombatPointShardChanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.15f),
				true
			},
			{
				new CurvePoint(500f, 0.3f),
				true
			},
			{
				new CurvePoint(1000f, 0.5f),
				true
			},
			{
				new CurvePoint(5000f, 0.8f),
				true
			}
		};
	}
}
