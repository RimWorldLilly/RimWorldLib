using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B0C RID: 2828
	public class JobGiver_PlayTargetInstrument : ThinkNode_JobGiver
	{
		// Token: 0x060053DD RID: 21469 RVA: 0x001F3708 File Offset: 0x001F1908
		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			Building_MusicalInstrument building_MusicalInstrument = ((duty != null) ? duty.focusSecond.Thing : null) as Building_MusicalInstrument;
			if (building_MusicalInstrument == null || !building_MusicalInstrument.Spawned)
			{
				return null;
			}
			if (!GatheringWorker_Concert.InstrumentAccessible(building_MusicalInstrument, pawn))
			{
				return null;
			}
			LordJob_Ritual lordJob_Ritual = pawn.GetLord().LordJob as LordJob_Ritual;
			Job job = JobMaker.MakeJob(JobDefOf.Play_MusicalInstrument, building_MusicalInstrument, building_MusicalInstrument.InteractionCell);
			job.doUntilGatheringEnded = true;
			if (lordJob_Ritual != null)
			{
				job.expiryInterval = lordJob_Ritual.DurationTicks;
			}
			else
			{
				job.expiryInterval = 2000;
			}
			return job;
		}
	}
}
