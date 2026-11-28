using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BB2 RID: 2994
	public abstract class WorkGiver
	{
		// Token: 0x06005659 RID: 22105 RVA: 0x0020231A File Offset: 0x0020051A
		public virtual bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return false;
		}

		// Token: 0x0600565A RID: 22106 RVA: 0x0020231D File Offset: 0x0020051D
		public virtual Job NonScanJob(Pawn pawn)
		{
			return null;
		}

		// Token: 0x0600565B RID: 22107 RVA: 0x00202320 File Offset: 0x00200520
		public PawnCapacityDef MissingRequiredCapacity(Pawn pawn)
		{
			for (int i = 0; i < this.def.requiredCapacities.Count; i++)
			{
				if (!pawn.health.capacities.CapableOf(this.def.requiredCapacities[i]))
				{
					return this.def.requiredCapacities[i];
				}
			}
			return null;
		}

		// Token: 0x04003313 RID: 13075
		public WorkGiverDef def;
	}
}
