using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B38 RID: 2872
	public class JobGiver_ExtinguishSelf : ThinkNode_JobGiver
	{
		// Token: 0x06005461 RID: 21601 RVA: 0x001F5D08 File Offset: 0x001F3F08
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (Rand.Value < 0.1f)
			{
				Fire fire = (Fire)pawn.GetAttachment(ThingDefOf.Fire);
				if (fire != null)
				{
					return JobMaker.MakeJob(JobDefOf.ExtinguishSelf, fire);
				}
			}
			return null;
		}

		// Token: 0x04003270 RID: 12912
		private const float ActivateChance = 0.1f;
	}
}
