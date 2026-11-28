using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C4D RID: 3149
	public class WorkGiver_TendOther_Animal : WorkGiver_TendOther
	{
		// Token: 0x06005976 RID: 22902 RVA: 0x0020EC57 File Offset: 0x0020CE57
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedAnimalsWithAnyHediff;
		}

		// Token: 0x06005977 RID: 22903 RVA: 0x0020EC6C File Offset: 0x0020CE6C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			return (pawn2 == null || pawn2.IsAnimal) && base.HasJobOnThing(pawn, t, forced);
		}
	}
}
