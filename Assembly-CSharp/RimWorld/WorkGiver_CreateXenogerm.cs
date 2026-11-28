using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BFD RID: 3069
	public class WorkGiver_CreateXenogerm : WorkGiver_Scanner
	{
		// Token: 0x17000F53 RID: 3923
		// (get) Token: 0x060057B4 RID: 22452 RVA: 0x00207613 File Offset: 0x00205813
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.GeneAssembler);
			}
		}

		// Token: 0x060057B5 RID: 22453 RVA: 0x0020761F File Offset: 0x0020581F
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.BiotechActive;
		}

		// Token: 0x060057B6 RID: 22454 RVA: 0x0020762C File Offset: 0x0020582C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_GeneAssembler building_GeneAssembler = t as Building_GeneAssembler;
			if (building_GeneAssembler == null)
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced) || !pawn.CanReserveSittableOrSpot(t.InteractionCell, forced))
			{
				return false;
			}
			if (building_GeneAssembler.ArchitesRequiredNow <= 0)
			{
				return building_GeneAssembler.CanBeWorkedOnNow.Accepted;
			}
			if (this.FindArchiteCapsule(pawn) == null)
			{
				JobFailReason.Is("NoIngredient".Translate(ThingDefOf.ArchiteCapsule), null);
				return false;
			}
			return true;
		}

		// Token: 0x060057B7 RID: 22455 RVA: 0x002076B0 File Offset: 0x002058B0
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_GeneAssembler building_GeneAssembler = t as Building_GeneAssembler;
			if (building_GeneAssembler == null)
			{
				return null;
			}
			if (building_GeneAssembler.ArchitesRequiredNow > 0)
			{
				Thing thing = this.FindArchiteCapsule(pawn);
				if (thing != null)
				{
					Job job = JobMaker.MakeJob(JobDefOf.HaulToContainer, thing, t);
					job.count = Mathf.Min(building_GeneAssembler.ArchitesRequiredNow, thing.stackCount);
					return job;
				}
			}
			return JobMaker.MakeJob(JobDefOf.CreateXenogerm, t, 1200, true);
		}

		// Token: 0x060057B8 RID: 22456 RVA: 0x00207724 File Offset: 0x00205924
		private Thing FindArchiteCapsule(Pawn pawn)
		{
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.ArchiteCapsule), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false), null, 0, -1, false, RegionType.Set_Passable, false, false);
		}
	}
}
