using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BDB RID: 3035
	public class WorkGiver_ExecuteGuiltyColonist : WorkGiver_Scanner
	{
		// Token: 0x17000F3F RID: 3903
		// (get) Token: 0x06005732 RID: 22322 RVA: 0x002053ED File Offset: 0x002035ED
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x17000F40 RID: 3904
		// (get) Token: 0x06005733 RID: 22323 RVA: 0x002053F0 File Offset: 0x002035F0
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x06005734 RID: 22324 RVA: 0x002053F9 File Offset: 0x002035F9
		public static void ResetStaticData()
		{
			WorkGiver_ExecuteGuiltyColonist.IncapableOfViolenceLowerTrans = "IncapableOfViolenceLower".Translate();
		}

		// Token: 0x06005735 RID: 22325 RVA: 0x0020540F File Offset: 0x0020360F
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.FreeColonistsSpawned;
		}

		// Token: 0x06005736 RID: 22326 RVA: 0x00205421 File Offset: 0x00203621
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.mapPawns.FreeColonistsSpawnedCount == 0;
		}

		// Token: 0x06005737 RID: 22327 RVA: 0x00205438 File Offset: 0x00203638
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = (Pawn)t;
			if (((pawn2 != null) ? pawn2.guilt : null) == null || !pawn2.guilt.IsGuilty || !pawn2.guilt.awaitingExecution || !pawn2.IsColonist || !pawn2.Spawned || pawn2.InAggroMentalState || pawn2.IsForbidden(pawn) || pawn2.IsFormingCaravan() || !pawn.CanReserveAndReach(pawn2, PathEndMode.Touch, pawn.NormalMaxDanger(), 1, -1, null, forced))
			{
				return null;
			}
			if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				JobFailReason.Is(WorkGiver_ExecuteGuiltyColonist.IncapableOfViolenceLowerTrans, null);
				return null;
			}
			if (!new HistoryEvent(HistoryEventDefOf.ExecutedColonist, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.GuiltyColonistExecution, t);
		}

		// Token: 0x0400332F RID: 13103
		private static string IncapableOfViolenceLowerTrans;
	}
}
