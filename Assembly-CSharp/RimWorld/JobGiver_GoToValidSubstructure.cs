using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B09 RID: 2825
	public class JobGiver_GoToValidSubstructure : ThinkNode_JobGiver
	{
		// Token: 0x060053D6 RID: 21462 RVA: 0x001F3564 File Offset: 0x001F1764
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.mindState.duty == null)
			{
				return null;
			}
			if (pawn.GetLord() != null)
			{
				LordJob_Ritual lordJob_Ritual = pawn.GetLord().LordJob as LordJob_Ritual;
				if (lordJob_Ritual != null)
				{
					Thing thing = lordJob_Ritual.selectedTarget.Thing;
					Building_GravEngine building_GravEngine;
					if (thing == null)
					{
						building_GravEngine = null;
					}
					else
					{
						CompPilotConsole compPilotConsole = thing.TryGetComp<CompPilotConsole>();
						building_GravEngine = ((compPilotConsole != null) ? compPilotConsole.engine : null);
					}
					Building_GravEngine building_GravEngine2 = building_GravEngine;
					if (building_GravEngine2 == null)
					{
						return null;
					}
					IntVec3 position;
					if (GravshipUtility.IsOnboardGravship(pawn.Position, building_GravEngine2, pawn, false))
					{
						position = pawn.Position;
					}
					else if (!GravshipUtility.TryFindSpotOnGravship(pawn, building_GravEngine2, out position))
					{
						return null;
					}
					Job job = JobMaker.MakeJob(JobDefOf.GotoShip, position);
					job.locomotionUrgency = LocomotionUrgency.Jog;
					return job;
				}
			}
			return null;
		}
	}
}
