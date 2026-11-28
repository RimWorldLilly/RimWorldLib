using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B3D RID: 2877
	public class JobGiver_FleeFire : ThinkNode_JobGiver
	{
		// Token: 0x0600546E RID: 21614 RVA: 0x001F6124 File Offset: 0x001F4324
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = FleeUtility.FleeLargeFireJob(pawn, 1, 20, 24);
			if (job != null)
			{
				return job;
			}
			return null;
		}

		// Token: 0x0400327B RID: 12923
		private const int FleeDistance = 24;

		// Token: 0x0400327C RID: 12924
		private const int DistToFireToFlee = 20;
	}
}
