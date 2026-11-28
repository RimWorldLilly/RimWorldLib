using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C0C RID: 3084
	internal class WorkGiver_FightFires : WorkGiver_Scanner
	{
		// Token: 0x17000F64 RID: 3940
		// (get) Token: 0x06005813 RID: 22547 RVA: 0x0020981B File Offset: 0x00207A1B
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.Fire);
			}
		}

		// Token: 0x17000F65 RID: 3941
		// (get) Token: 0x06005814 RID: 22548 RVA: 0x00209827 File Offset: 0x00207A27
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06005815 RID: 22549 RVA: 0x0020982A File Offset: 0x00207A2A
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06005816 RID: 22550 RVA: 0x00209830 File Offset: 0x00207A30
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Fire fire = t as Fire;
			if (fire == null)
			{
				return false;
			}
			Pawn pawn2 = fire.parent as Pawn;
			if (pawn2 != null)
			{
				if (pawn2 == pawn)
				{
					return false;
				}
				if ((pawn2.Faction == null || pawn2.Faction != pawn.Faction) && (pawn2.HostFaction == null || (pawn2.HostFaction != pawn.Faction && pawn2.HostFaction != pawn.HostFaction)))
				{
					return false;
				}
				if (!pawn.Map.areaManager.Home[fire.Position] && IntVec3Utility.ManhattanDistanceFlat(pawn.Position, pawn2.Position) > 15)
				{
					return false;
				}
				if (!pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return false;
				}
			}
			else
			{
				if (pawn.WorkTagIsDisabled(WorkTags.Firefighting))
				{
					JobFailReason.Is("IncapableOfFirefighting".Translate(), null);
					return false;
				}
				if (!pawn.Map.areaManager.Home[fire.Position])
				{
					JobFailReason.Is(WorkGiver_FixBrokenDownBuilding.NotInHomeAreaTrans, null);
					return false;
				}
			}
			return ((pawn.Position - fire.Position).LengthHorizontalSquared <= 225 || pawn.CanReserve(fire, 1, -1, null, forced)) && (forced || !WorkGiver_FightFires.FireIsBeingHandled(fire, pawn));
		}

		// Token: 0x06005817 RID: 22551 RVA: 0x00209982 File Offset: 0x00207B82
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.BeatFire, t);
		}

		// Token: 0x06005818 RID: 22552 RVA: 0x00209994 File Offset: 0x00207B94
		public static bool FireIsBeingHandled(Fire f, Pawn potentialHandler)
		{
			if (!f.Spawned)
			{
				return false;
			}
			Pawn pawn = f.Map.reservationManager.FirstRespectedReserver(f, potentialHandler, null);
			return pawn != null && pawn.Position.InHorDistOf(f.Position, 5f);
		}

		// Token: 0x04003343 RID: 13123
		public const int NearbyPawnRadius = 15;

		// Token: 0x04003344 RID: 13124
		private const int MaxReservationCheckDistance = 15;

		// Token: 0x04003345 RID: 13125
		private const float HandledDistance = 5f;
	}
}
