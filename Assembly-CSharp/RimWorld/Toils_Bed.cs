using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AA7 RID: 2727
	public static class Toils_Bed
	{
		// Token: 0x06005224 RID: 21028 RVA: 0x001E9800 File Offset: 0x001E7A00
		public static Toil GotoBed(TargetIndex bedIndex)
		{
			Toil gotoBed = ToilMaker.MakeToil("GotoBed");
			gotoBed.initAction = delegate()
			{
				Pawn actor = gotoBed.actor;
				Building_Bed bed = (Building_Bed)actor.CurJob.GetTarget(bedIndex).Thing;
				IntVec3 bedSleepingSlotPosFor = RestUtility.GetBedSleepingSlotPosFor(actor, bed);
				if (actor.Position == bedSleepingSlotPosFor)
				{
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				actor.pather.StartPath(bedSleepingSlotPosFor, PathEndMode.OnCell);
			};
			gotoBed.tickIntervalAction = delegate(int delta)
			{
				Pawn actor = gotoBed.actor;
				Building_Bed building_Bed = (Building_Bed)actor.CurJob.GetTarget(bedIndex).Thing;
				Pawn curOccupantAt = building_Bed.GetCurOccupantAt(actor.pather.Destination.Cell);
				if (curOccupantAt != null && curOccupantAt != actor)
				{
					actor.pather.StartPath(RestUtility.GetBedSleepingSlotPosFor(actor, building_Bed), PathEndMode.OnCell);
				}
			};
			gotoBed.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			gotoBed.FailOnBedNoLongerUsable(bedIndex);
			return gotoBed;
		}

		// Token: 0x06005225 RID: 21029 RVA: 0x001E987C File Offset: 0x001E7A7C
		public static Toil ClaimBedIfNonMedical(TargetIndex ind, TargetIndex claimantIndex = TargetIndex.None)
		{
			Toil claim = ToilMaker.MakeToil("ClaimBedIfNonMedical");
			claim.initAction = delegate()
			{
				Pawn actor = claim.GetActor();
				Pawn pawn = (claimantIndex == TargetIndex.None) ? actor : ((Pawn)actor.CurJob.GetTarget(claimantIndex).Thing);
				if (pawn.ownership != null && (!pawn.IsMutant || !pawn.mutant.Def.disableOwnership))
				{
					pawn.ownership.ClaimBedIfNonMedical((Building_Bed)actor.CurJob.GetTarget(ind).Thing);
				}
			};
			claim.FailOnDespawnedOrNull(ind);
			claim.defaultCompleteMode = ToilCompleteMode.Instant;
			claim.atomicWithPrevious = true;
			return claim;
		}

		// Token: 0x06005226 RID: 21030 RVA: 0x001E98F4 File Offset: 0x001E7AF4
		public static bool BedNoLongerUsable(Pawn actor, Thing bedThing, bool forcePrisoner)
		{
			if (actor.BeingTransportedOnGravship)
			{
				return false;
			}
			GuestStatus? guestStatusOverride = null;
			if (forcePrisoner)
			{
				guestStatusOverride = new GuestStatus?(GuestStatus.Prisoner);
			}
			return !RestUtility.CanUseBedNow(bedThing, actor, false, false, guestStatusOverride);
		}

		// Token: 0x06005227 RID: 21031 RVA: 0x001E992C File Offset: 0x001E7B2C
		private static Pawn TargetIndexToPawn(Toil toil, TargetIndex pawnIndex)
		{
			if (pawnIndex == TargetIndex.None)
			{
				return toil.actor;
			}
			return (Pawn)toil.actor.CurJob.GetTarget(pawnIndex).Thing;
		}

		// Token: 0x06005228 RID: 21032 RVA: 0x001E9961 File Offset: 0x001E7B61
		public static void FailOnBedNoLongerUsable(this Toil toil, TargetIndex bedIndex)
		{
			toil.FailOnBedNoLongerUsable(bedIndex, TargetIndex.None);
		}

		// Token: 0x06005229 RID: 21033 RVA: 0x001E996C File Offset: 0x001E7B6C
		public static void FailOnBedNoLongerUsable(this Toil toil, TargetIndex bedIndex, TargetIndex sleeperIndex)
		{
			toil.FailOn(() => Toils_Bed.BedNoLongerUsable(Toils_Bed.TargetIndexToPawn(toil, sleeperIndex), toil.actor.CurJob.GetTarget(bedIndex).Thing, toil.actor.CurJobDef.makeTargetPrisoner));
			toil.AddFinishAction(delegate
			{
				Building_Bed building_Bed = (Building_Bed)toil.actor.CurJob.GetTarget(bedIndex).Thing;
				Pawn pawn = Toils_Bed.TargetIndexToPawn(toil, sleeperIndex);
				if (pawn.GetPosture().InBed() && Toils_Bed.BedNoLongerUsable(pawn, building_Bed, toil.actor.CurJobDef.makeTargetPrisoner))
				{
					RestUtility.KickOutOfBed(pawn, building_Bed);
				}
			});
		}

		// Token: 0x0600522A RID: 21034 RVA: 0x001E99C4 File Offset: 0x001E7BC4
		public static Toil TuckIntoBed(Building_Bed bed, Pawn taker, Pawn takee, bool rescued = false)
		{
			Toil toil = ToilMaker.MakeToil("TuckIntoBed");
			toil.initAction = delegate()
			{
				RestUtility.TuckIntoBed(bed, taker, takee, rescued);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x0600522B RID: 21035 RVA: 0x001E9A18 File Offset: 0x001E7C18
		public static Toil TuckIntoBed(TargetIndex bedIndex, TargetIndex takeeIndex, bool rescued = false)
		{
			Toil tuckIntoBed = ToilMaker.MakeToil("TuckIntoBed");
			tuckIntoBed.initAction = delegate()
			{
				Pawn actor = tuckIntoBed.actor;
				Building_Bed building_Bed = actor.jobs.curJob.GetTarget(bedIndex).Thing as Building_Bed;
				Pawn takee = (Pawn)actor.jobs.curJob.GetTarget(takeeIndex).Thing;
				if (building_Bed != null)
				{
					RestUtility.TuckIntoBed(building_Bed, actor, takee, rescued);
					return;
				}
				Thing thing;
				actor.carryTracker.TryDropCarriedThing(actor.jobs.curJob.GetTarget(bedIndex).Cell, ThingPlaceMode.Direct, out thing, null);
			};
			tuckIntoBed.defaultCompleteMode = ToilCompleteMode.Instant;
			return tuckIntoBed;
		}
	}
}
