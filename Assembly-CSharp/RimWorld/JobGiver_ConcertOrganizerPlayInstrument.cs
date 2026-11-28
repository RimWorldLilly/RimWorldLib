using System;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000AF9 RID: 2809
	public class JobGiver_ConcertOrganizerPlayInstrument : ThinkNode_JobGiver
	{
		// Token: 0x060053A6 RID: 21414 RVA: 0x001F242C File Offset: 0x001F062C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.mindState.duty == null)
			{
				return null;
			}
			LordJob_Joinable_Concert lordJob_Joinable_Concert = pawn.GetLord().LordJob as LordJob_Joinable_Concert;
			if (lordJob_Joinable_Concert == null || lordJob_Joinable_Concert.Organizer != pawn)
			{
				return null;
			}
			IntVec3 gatherSpot = pawn.mindState.duty.focus.Cell;
			Building_MusicalInstrument building_MusicalInstrument = (from i in pawn.Map.listerBuildings.AllBuildingsColonistOfClass<Building_MusicalInstrument>()
			where GatheringsUtility.InGatheringArea(i.InteractionCell, gatherSpot, pawn.Map) && GatheringWorker_Concert.InstrumentAccessible(i, pawn)
			select i).RandomElementWithFallback(null);
			if (building_MusicalInstrument != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.Play_MusicalInstrument, building_MusicalInstrument, building_MusicalInstrument.InteractionCell);
				job.doUntilGatheringEnded = true;
				job.expiryInterval = lordJob_Joinable_Concert.DurationTicks;
				return job;
			}
			return null;
		}
	}
}
