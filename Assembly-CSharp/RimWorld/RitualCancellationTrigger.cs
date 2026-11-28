using System;
using System.Collections.Generic;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200168F RID: 5775
	public abstract class RitualCancellationTrigger
	{
		// Token: 0x060090C5 RID: 37061
		public abstract IEnumerable<Trigger> CancellationTriggers(RitualRoleAssignments assignments);
	}
}
