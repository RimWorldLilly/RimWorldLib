using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000405 RID: 1029
	public class HediffGiver_RandomAgeCurved : HediffGiver
	{
		// Token: 0x06001DD7 RID: 7639 RVA: 0x000B803E File Offset: 0x000B623E
		public override float ChanceFactor(Pawn pawn)
		{
			if (pawn.IsMutant && pawn.mutant.Def.disableAging)
			{
				return 0f;
			}
			return base.ChanceFactor(pawn);
		}

		// Token: 0x06001DD8 RID: 7640 RVA: 0x000B8068 File Offset: 0x000B6268
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			float x = (float)pawn.ageTracker.AgeBiologicalYears / pawn.RaceProps.lifeExpectancy;
			if (Rand.MTBEventOccurs(this.ageFractionMtbDaysCurve.Evaluate(x), 60000f, 60f))
			{
				if (this.minPlayerPopulation > 0 && pawn.Faction == Faction.OfPlayer && PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoSuspended.Count<Pawn>() < this.minPlayerPopulation)
				{
					return;
				}
				if (base.TryApply(pawn, null))
				{
					base.SendLetter(pawn, cause);
				}
			}
		}

		// Token: 0x040017E0 RID: 6112
		public SimpleCurve ageFractionMtbDaysCurve;

		// Token: 0x040017E1 RID: 6113
		public int minPlayerPopulation;
	}
}
