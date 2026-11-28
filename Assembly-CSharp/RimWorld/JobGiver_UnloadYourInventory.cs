using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B6D RID: 2925
	public class JobGiver_UnloadYourInventory : ThinkNode_JobGiver
	{
		// Token: 0x06005525 RID: 21797 RVA: 0x001FAA14 File Offset: 0x001F8C14
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.inventory.UnloadEverything)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.UnloadYourInventory);
		}
	}
}
