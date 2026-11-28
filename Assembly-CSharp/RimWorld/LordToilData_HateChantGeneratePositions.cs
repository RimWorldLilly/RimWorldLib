using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CDF RID: 3295
	public class LordToilData_HateChantGeneratePositions : LordToilData
	{
		// Token: 0x06005D31 RID: 23857 RVA: 0x002230A2 File Offset: 0x002212A2
		public override void ExposeData()
		{
			Scribe_Collections.Look<PsychicRitualParticipant>(ref this.chanters, "chanters", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<IntVec3>(ref this.foundPositions, "foundPositions", LookMode.Deep);
		}

		// Token: 0x04003504 RID: 13572
		public List<PsychicRitualParticipant> chanters;

		// Token: 0x04003505 RID: 13573
		public HashSet<IntVec3> foundPositions;
	}
}
