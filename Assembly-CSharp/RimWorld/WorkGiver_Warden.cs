using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BDC RID: 3036
	public abstract class WorkGiver_Warden : WorkGiver_Scanner
	{
		// Token: 0x17000F41 RID: 3905
		// (get) Token: 0x06005739 RID: 22329 RVA: 0x00205504 File Offset: 0x00203704
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x17000F42 RID: 3906
		// (get) Token: 0x0600573A RID: 22330 RVA: 0x0020550D File Offset: 0x0020370D
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x0600573B RID: 22331 RVA: 0x00205510 File Offset: 0x00203710
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SlavesAndPrisonersOfColonySpawned;
		}

		// Token: 0x0600573C RID: 22332 RVA: 0x00205522 File Offset: 0x00203722
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.mapPawns.SlavesAndPrisonersOfColonySpawnedCount == 0;
		}

		// Token: 0x0600573D RID: 22333 RVA: 0x00205538 File Offset: 0x00203738
		protected bool ShouldTakeCareOfPrisoner(Pawn warden, Thing prisoner, bool forced = false)
		{
			Pawn pawn = prisoner as Pawn;
			return pawn != null && pawn.IsPrisonerOfColony && pawn.guest.PrisonerIsSecure && pawn.Spawned && !pawn.InAggroMentalState && !prisoner.IsForbidden(warden) && !pawn.IsFormingCaravan() && warden.CanReserveAndReach(pawn, PathEndMode.OnCell, warden.NormalMaxDanger(), 1, -1, null, forced);
		}

		// Token: 0x0600573E RID: 22334 RVA: 0x002055A4 File Offset: 0x002037A4
		protected bool ShouldTakeCareOfSlave(Pawn warden, Thing slave, bool forced = false)
		{
			Pawn pawn = slave as Pawn;
			return pawn != null && pawn.IsSlaveOfColony && pawn.guest.SlaveIsSecure && pawn.Spawned && !pawn.InAggroMentalState && !pawn.IsForbidden(warden) && !pawn.IsFormingCaravan() && warden.CanReserveAndReach(pawn, PathEndMode.Touch, warden.NormalMaxDanger(), 1, -1, null, forced);
		}

		// Token: 0x0600573F RID: 22335 RVA: 0x00205610 File Offset: 0x00203810
		protected bool IsExecutionIdeoAllowed(Pawn warden, Pawn victim)
		{
			return new HistoryEvent(HistoryEventDefOf.ExecutedPrisoner, warden.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job() && (!victim.guilt.IsGuilty || new HistoryEvent(HistoryEventDefOf.ExecutedPrisonerGuilty, warden.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job()) && (victim.guilt.IsGuilty || new HistoryEvent(HistoryEventDefOf.ExecutedPrisonerInnocent, warden.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job());
		}
	}
}
