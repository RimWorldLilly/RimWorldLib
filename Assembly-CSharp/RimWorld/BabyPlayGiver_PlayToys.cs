using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BAA RID: 2986
	public class BabyPlayGiver_PlayToys : BabyPlayGiver
	{
		// Token: 0x06005627 RID: 22055 RVA: 0x0020076C File Offset: 0x001FE96C
		public override bool CanDo(Pawn pawn, Pawn baby)
		{
			Thing thing = this.FindNearbyUseableToyBox(pawn, baby);
			return thing != null && (pawn.IsCarryingPawn(baby) || pawn.CanReserveAndReach(baby, PathEndMode.Touch, Danger.Some, 1, -1, null, false)) && pawn.CanReserveAndReach(thing, PathEndMode.Touch, Danger.Some, 1, -1, null, false);
		}

		// Token: 0x06005628 RID: 22056 RVA: 0x002007C0 File Offset: 0x001FE9C0
		public override Job TryGiveJob(Pawn pawn, Pawn baby)
		{
			Thing thing = this.FindNearbyUseableToyBox(pawn, baby);
			if (thing != null)
			{
				Job job = JobMaker.MakeJob(this.def.jobDef, baby, thing);
				job.count = 1;
				return job;
			}
			return null;
		}

		// Token: 0x06005629 RID: 22057 RVA: 0x00200800 File Offset: 0x001FEA00
		private bool IsValidToybox(Thing toybox, Pawn hauler, Pawn baby)
		{
			return toybox.def == ThingDefOf.ToyBox && !toybox.IsForbidden(hauler) && !toybox.IsForbidden(baby) && !toybox.IsBurning() && hauler.CanReserveAndReach(toybox, PathEndMode.Touch, Danger.None, 1, -1, null, false) && hauler.Position.DistanceTo(toybox.Position) <= 15.9f;
		}

		// Token: 0x0600562A RID: 22058 RVA: 0x0020086C File Offset: 0x001FEA6C
		private Thing FindNearbyUseableToyBox(Pawn pawn, Pawn baby)
		{
			Room room = baby.GetRoom(RegionType.Set_All);
			if (room != null)
			{
				foreach (Thing thing in room.ContainedThings(ThingDefOf.ToyBox))
				{
					if (this.IsValidToybox(thing, pawn, baby))
					{
						return thing;
					}
				}
			}
			return GenClosest.ClosestThingReachable(baby.PositionHeld, baby.MapHeld, ThingRequest.ForDef(ThingDefOf.ToyBox), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 15.9f, (Thing t) => this.IsValidToybox(t, pawn, baby), null, 0, -1, false, RegionType.Set_Passable, false, false);
		}

		// Token: 0x0400330E RID: 13070
		private const float MaxToyBoxDistance = 15.9f;
	}
}
