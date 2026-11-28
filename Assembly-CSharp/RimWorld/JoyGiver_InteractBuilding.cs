using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B6E RID: 2926
	public abstract class JoyGiver_InteractBuilding : JoyGiver
	{
		// Token: 0x17000F0E RID: 3854
		// (get) Token: 0x06005527 RID: 21799 RVA: 0x001FAA37 File Offset: 0x001F8C37
		protected virtual bool CanDoDuringGathering
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005528 RID: 21800 RVA: 0x001FAA3C File Offset: 0x001F8C3C
		public override Job TryGiveJob(Pawn pawn)
		{
			Thing thing = this.FindBestGame(pawn, false, IntVec3.Invalid);
			if (thing != null)
			{
				return this.TryGivePlayJob(pawn, thing);
			}
			return null;
		}

		// Token: 0x06005529 RID: 21801 RVA: 0x001FAA64 File Offset: 0x001F8C64
		public override Job TryGiveJobWhileInBed(Pawn pawn)
		{
			Thing thing = this.FindBestGame(pawn, true, IntVec3.Invalid);
			if (thing != null)
			{
				return this.TryGivePlayJobWhileInBed(pawn, thing);
			}
			return null;
		}

		// Token: 0x0600552A RID: 21802 RVA: 0x001FAA8C File Offset: 0x001F8C8C
		public override Job TryGiveJobInGatheringArea(Pawn pawn, IntVec3 gatheringSpot, float maxRadius = -1f)
		{
			if (!this.CanDoDuringGathering)
			{
				return null;
			}
			Thing thing = this.FindBestGame(pawn, false, gatheringSpot);
			if (thing != null)
			{
				return this.TryGivePlayJob(pawn, thing);
			}
			return null;
		}

		// Token: 0x0600552B RID: 21803 RVA: 0x001FAABC File Offset: 0x001F8CBC
		private Thing FindBestGame(Pawn pawn, bool inBed, IntVec3 gatheringSpot)
		{
			JoyGiver_InteractBuilding.tmpCandidates.Clear();
			this.GetSearchSet(pawn, JoyGiver_InteractBuilding.tmpCandidates);
			if (JoyGiver_InteractBuilding.tmpCandidates.Count == 0)
			{
				return null;
			}
			Predicate<Thing> predicate = (Thing t) => this.CanInteractWith(pawn, t, inBed);
			if (gatheringSpot.IsValid)
			{
				Predicate<Thing> oldValidator = predicate;
				predicate = ((Thing x) => GatheringsUtility.InGatheringArea(x.Position, gatheringSpot, pawn.Map) && oldValidator(x));
			}
			Thing result = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, JoyGiver_InteractBuilding.tmpCandidates, PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, predicate, null, false);
			JoyGiver_InteractBuilding.tmpCandidates.Clear();
			return result;
		}

		// Token: 0x0600552C RID: 21804 RVA: 0x001FAB94 File Offset: 0x001F8D94
		protected virtual bool CanInteractWith(Pawn pawn, Thing t, bool inBed)
		{
			if (!pawn.CanReserve(t, this.def.jobDef.joyMaxParticipants, -1, null, false))
			{
				return false;
			}
			if (t.IsForbidden(pawn))
			{
				return false;
			}
			if (t.Fogged())
			{
				return false;
			}
			if (!t.IsSociallyProper(pawn))
			{
				return false;
			}
			if (!t.IsPoliticallyProper(pawn))
			{
				return false;
			}
			if (t.VacuumConcernTo(pawn))
			{
				return false;
			}
			CompPowerTrader compPowerTrader = t.TryGetComp<CompPowerTrader>();
			return (compPowerTrader == null || compPowerTrader.PowerOn) && (!this.def.unroofedOnly || !t.Position.Roofed(t.Map));
		}

		// Token: 0x0600552D RID: 21805
		protected abstract Job TryGivePlayJob(Pawn pawn, Thing bestGame);

		// Token: 0x0600552E RID: 21806 RVA: 0x001FAC34 File Offset: 0x001F8E34
		protected virtual Job TryGivePlayJobWhileInBed(Pawn pawn, Thing bestGame)
		{
			Building_Bed t = pawn.CurrentBed();
			return JobMaker.MakeJob(this.def.jobDef, bestGame, pawn.Position, t);
		}

		// Token: 0x040032C5 RID: 12997
		private static List<Thing> tmpCandidates = new List<Thing>();
	}
}
