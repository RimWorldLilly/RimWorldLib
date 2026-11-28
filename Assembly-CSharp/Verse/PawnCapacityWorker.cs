using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200019C RID: 412
	public class PawnCapacityWorker
	{
		// Token: 0x06000B75 RID: 2933 RVA: 0x0003DB0E File Offset: 0x0003BD0E
		public virtual float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return 1f;
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x0003DB15 File Offset: 0x0003BD15
		public virtual bool CanHaveCapacity(BodyDef body)
		{
			return true;
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x0003DB18 File Offset: 0x0003BD18
		protected float CalculateCapacityAndRecord(HediffSet diffSet, PawnCapacityDef capacity, List<PawnCapacityUtility.CapacityImpactor> impactors)
		{
			float level = diffSet.pawn.health.capacities.GetLevel(capacity);
			if (impactors != null && level != 1f)
			{
				impactors.Add(new PawnCapacityUtility.CapacityImpactorCapacity
				{
					capacity = capacity
				});
			}
			return level;
		}
	}
}
