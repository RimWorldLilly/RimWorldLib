using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000AF3 RID: 2803
	public class JobGiver_UnloadMyLordCarriers : ThinkNode_JobGiver
	{
		// Token: 0x06005393 RID: 21395 RVA: 0x001F1B28 File Offset: 0x001EFD28
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return null;
			}
			Lord lord = pawn.GetLord();
			if (lord == null)
			{
				return null;
			}
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (UnloadCarriersJobGiverUtility.HasJobOnThing(pawn, lord.ownedPawns[i], false))
				{
					return JobMaker.MakeJob(JobDefOf.UnloadInventory, lord.ownedPawns[i]);
				}
			}
			return null;
		}
	}
}
