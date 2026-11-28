using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BA5 RID: 2981
	public class JobGiver_SlaughterEntity : ThinkNode_JobGiver
	{
		// Token: 0x0600561B RID: 22043 RVA: 0x00200564 File Offset: 0x001FE764
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_SlaughterThing mentalState_SlaughterThing = pawn.MentalState as MentalState_SlaughterThing;
			if (mentalState_SlaughterThing != null && mentalState_SlaughterThing.SlaughteredRecently)
			{
				return null;
			}
			Building_HoldingPlatform building_HoldingPlatform = (Building_HoldingPlatform)AnomalyUtility.FindEntityOnPlatform(pawn.Map, EntityQueryType.ForSlaughter).ParentHolder;
			if (building_HoldingPlatform == null || !pawn.CanReserveAndReach(building_HoldingPlatform, PathEndMode.ClosestTouch, Danger.Deadly, 1, -1, null, false))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.ExecuteEntity, building_HoldingPlatform);
			job.ignoreDesignations = true;
			return job;
		}
	}
}
