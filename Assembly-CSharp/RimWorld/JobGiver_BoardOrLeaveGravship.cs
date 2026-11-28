using System;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000AF8 RID: 2808
	public class JobGiver_BoardOrLeaveGravship : ThinkNode_JobGiver
	{
		// Token: 0x060053A4 RID: 21412 RVA: 0x001F2260 File Offset: 0x001F0460
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return null;
			}
			if (pawn.Downed)
			{
				return null;
			}
			Building_GravEngine building_GravEngine = pawn.Map.listerThings.ThingsOfDef(ThingDefOf.GravEngine).FirstOrDefault<Thing>() as Building_GravEngine;
			if (building_GravEngine == null)
			{
				return null;
			}
			if (building_GravEngine.pawnsToBoard != null && building_GravEngine.pawnsToBoard.Contains(pawn))
			{
				IntVec3 position;
				if (GravshipUtility.IsOnboardGravship_NewTemp(pawn.Position, building_GravEngine, pawn, false, false))
				{
					position = pawn.Position;
				}
				else if (!GravshipUtility.TryFindSpotOnGravship(pawn, building_GravEngine, out position))
				{
					Messages.Message("FailedToBoardGravship".Translate(pawn.Named("PAWN"), building_GravEngine.RenamableLabel), pawn, MessageTypeDefOf.NegativeEvent, false);
					building_GravEngine.pawnsToBoard.Remove(pawn);
					return null;
				}
				Lord lord = pawn.lord;
				LordJob_Ritual lordJob_Ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
				if (lordJob_Ritual != null)
				{
					lordJob_Ritual.Cancel();
				}
				Job job = JobMaker.MakeJob(JobDefOf.GotoShip, position);
				job.locomotionUrgency = LocomotionUrgency.Jog;
				return job;
			}
			if (building_GravEngine.pawnsToLeave != null && building_GravEngine.pawnsToLeave.Contains(pawn))
			{
				IntVec3 position2;
				if (!GravshipUtility.IsOnboardGravship_NewTemp(pawn.Position, building_GravEngine, pawn, true, false))
				{
					position2 = pawn.Position;
				}
				else if (!GravshipUtility.TryFindSpotOffGravship(pawn, building_GravEngine, out position2))
				{
					Messages.Message("FailedToLeaveGravship".Translate(pawn.Named("PAWN"), building_GravEngine.RenamableLabel), pawn, MessageTypeDefOf.NegativeEvent, false);
					building_GravEngine.pawnsToLeave.Remove(pawn);
					return null;
				}
				Lord lord2 = pawn.lord;
				if (lord2 != null)
				{
					LordJob lordJob = lord2.LordJob;
					if (lordJob != null)
					{
						lordJob.Notify_PawnLost(pawn, PawnLostCondition.ForcedByPlayerAction);
					}
				}
				Job job2 = JobMaker.MakeJob(JobDefOf.LeaveShip, position2);
				job2.locomotionUrgency = LocomotionUrgency.Jog;
				return job2;
			}
			return null;
		}
	}
}
