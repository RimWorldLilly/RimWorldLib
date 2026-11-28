using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E4D RID: 7757
	public struct LocationCandidate
	{
		// Token: 0x0600C922 RID: 51490 RVA: 0x0049E53E File Offset: 0x0049C73E
		public LocationCandidate(IntVec3 cell, float score)
		{
			this.cell = cell;
			this.score = score;
		}

		// Token: 0x0400729C RID: 29340
		public IntVec3 cell;

		// Token: 0x0400729D RID: 29341
		public float score;
	}
}
