using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200121C RID: 4636
	public class ComplexThreatWorker_SleepingInsects : ComplexThreatWorker_SleepingThreat
	{
		// Token: 0x06007302 RID: 29442 RVA: 0x00283C58 File Offset: 0x00281E58
		protected override bool CanResolveInt(ComplexResolveParams parms)
		{
			return base.CanResolveInt(parms) && (parms.hostileFaction == null || parms.hostileFaction == Faction.OfInsects);
		}

		// Token: 0x06007303 RID: 29443 RVA: 0x00283C7C File Offset: 0x00281E7C
		protected override IEnumerable<PawnKindDef> GetPawnKindsForPoints(float points)
		{
			return PawnUtility.GetCombatPawnKindsForPoints((PawnKindDef k) => k.RaceProps.Insect, points, null);
		}
	}
}
