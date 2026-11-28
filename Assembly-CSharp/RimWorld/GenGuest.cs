using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001550 RID: 5456
	public static class GenGuest
	{
		// Token: 0x06008557 RID: 34135 RVA: 0x0030117C File Offset: 0x002FF37C
		public static void PrisonerRelease(Pawn p)
		{
			if ((p.Faction == Faction.OfPlayer || p.IsWildMan()) && p.needs.mood != null)
			{
				p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.WasImprisoned, null, null);
			}
			if (p.SlaveFaction != null)
			{
				Faction hostFaction = p.HostFaction;
				p.SetFaction(p.SlaveFaction, null);
				p.guest.SetGuestStatus(hostFaction, GuestStatus.Prisoner);
			}
			GenGuest.GuestRelease(p);
		}

		// Token: 0x06008558 RID: 34136 RVA: 0x003011FC File Offset: 0x002FF3FC
		public static void SlaveRelease(Pawn p)
		{
			if ((p.Faction == Faction.OfPlayer || p.IsWildMan()) && p.needs.mood != null)
			{
				p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.WasEnslaved, null, null);
			}
			GenGuest.GuestRelease(p);
		}

		// Token: 0x06008559 RID: 34137 RVA: 0x00301252 File Offset: 0x002FF452
		private static bool ShouldStayOnMapOnRelease(Pawn pawn)
		{
			return pawn.IsWildMan() || (pawn.HomeFaction != null && pawn.HomeFaction.IsPlayer);
		}

		// Token: 0x0600855A RID: 34138 RVA: 0x00301274 File Offset: 0x002FF474
		private static void GuestRelease(Pawn p)
		{
			Pawn_Ownership ownership = p.ownership;
			if (ownership != null)
			{
				ownership.UnclaimAll();
			}
			if (p.Drafted)
			{
				p.drafter.Drafted = false;
			}
			p.guest.Released = true;
			p.guest.SetNoInteraction();
			IntVec3 c;
			if (GenGuest.ShouldStayOnMapOnRelease(p))
			{
				int interactionsToday = p.mindState.interactionsToday;
				int lastAssignedInteractTime = p.mindState.lastAssignedInteractTime;
				if (p.HomeFaction != null)
				{
					p.guest.SetGuestStatus(null, GuestStatus.Guest);
				}
				p.mindState.interactionsToday = interactionsToday;
				p.mindState.lastAssignedInteractTime = lastAssignedInteractTime;
				if (p.IsWildMan())
				{
					p.mindState.WildManEverReachedOutside = false;
				}
				if (ModsConfig.IdeologyActive && p.Ideo != null && p.IsColonist)
				{
					p.Ideo.RecacheColonistBelieverCount();
				}
			}
			else if (RCellFinder.TryFindBestExitSpot(p, out c, TraverseMode.ByPawn, true))
			{
				Job job = JobMaker.MakeJob(JobDefOf.Goto, c);
				job.exitMapOnArrival = true;
				p.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
			}
			p.Notify_Released();
		}

		// Token: 0x0600855B RID: 34139 RVA: 0x00301398 File Offset: 0x002FF598
		public static void AddHealthyPrisonerReleasedThoughts(Pawn prisoner)
		{
			if (!prisoner.IsColonist)
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners)
				{
					if (pawn.needs.mood != null && pawn != prisoner)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.ReleasedHealthyPrisoner, prisoner, null);
					}
				}
			}
		}

		// Token: 0x0600855C RID: 34140 RVA: 0x00301420 File Offset: 0x002FF620
		public static void RemoveHealthyPrisonerReleasedThoughts(Pawn prisoner)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists)
			{
				if (pawn.needs.mood != null && pawn != prisoner)
				{
					pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.ReleasedHealthyPrisoner, prisoner);
				}
			}
		}

		// Token: 0x0600855D RID: 34141 RVA: 0x0030149C File Offset: 0x002FF69C
		public static void EmancipateSlave(Pawn warden, Pawn slave)
		{
			if (!slave.IsSlave)
			{
				return;
			}
			GenGuest.SlaveRelease(slave);
			if (slave.IsWildMan())
			{
				slave.mindState.WildManEverReachedOutside = false;
			}
			Messages.Message("MessageSlaveEmancipated".Translate(slave, warden), new LookTargets(new TargetInfo[]
			{
				slave,
				warden
			}), MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x0600855E RID: 34142 RVA: 0x00301517 File Offset: 0x002FF717
		public static void EnslavePrisoner(Pawn warden, Pawn prisoner)
		{
			GenGuest.TryEnslavePrisoner(warden, prisoner);
		}

		// Token: 0x0600855F RID: 34143 RVA: 0x00301524 File Offset: 0x002FF724
		public static bool TryEnslavePrisoner(Pawn warden, Pawn prisoner)
		{
			if (prisoner.IsSlave)
			{
				return false;
			}
			if (prisoner.IsCreepJoiner && prisoner.creepjoiner.CanTriggerAggressive)
			{
				prisoner.creepjoiner.DoAggressive();
				return false;
			}
			if (prisoner.Faction != null && prisoner.Faction.def.hidden)
			{
				prisoner.SetFactionDirect(null);
			}
			bool everEnslaved = prisoner.guest.EverEnslaved;
			prisoner.guest.SetGuestStatus(warden.Faction, GuestStatus.Slave);
			Messages.Message("MessagePrisonerEnslaved".Translate(prisoner, warden), new LookTargets(new TargetInfo[]
			{
				prisoner,
				warden
			}), MessageTypeDefOf.NeutralEvent, true);
			Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.EnslavedPrisoner, warden.Named(HistoryEventArgsNames.Doer)), true);
			if (!everEnslaved)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.EnslavedPrisonerNotPreviouslyEnslaved, warden.Named(HistoryEventArgsNames.Doer)), true);
			}
			prisoner.apparel.UnlockAll();
			return true;
		}
	}
}
