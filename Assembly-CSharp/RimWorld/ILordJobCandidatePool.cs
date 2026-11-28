using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001702 RID: 5890
	public interface ILordJobCandidatePool
	{
		// Token: 0x17001939 RID: 6457
		// (get) Token: 0x06009315 RID: 37653
		List<Pawn> AllCandidatePawns { get; }

		// Token: 0x1700193A RID: 6458
		// (get) Token: 0x06009316 RID: 37654
		List<Pawn> NonAssignablePawns { get; }
	}
}
