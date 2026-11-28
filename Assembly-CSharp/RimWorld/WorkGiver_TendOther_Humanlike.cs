using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C4C RID: 3148
	public class WorkGiver_TendOther_Humanlike : WorkGiver_TendOther
	{
		// Token: 0x06005973 RID: 22899 RVA: 0x0020EC0E File Offset: 0x0020CE0E
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedHumanlikesWithAnyHediff;
		}

		// Token: 0x06005974 RID: 22900 RVA: 0x0020EC20 File Offset: 0x0020CE20
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			return (pawn2 == null || pawn2.RaceProps.Humanlike) && base.HasJobOnThing(pawn, t, forced);
		}
	}
}
