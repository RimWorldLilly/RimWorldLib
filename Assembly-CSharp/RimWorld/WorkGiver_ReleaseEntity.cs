using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C38 RID: 3128
	public class WorkGiver_ReleaseEntity : WorkGiver_Scanner
	{
		// Token: 0x06005904 RID: 22788 RVA: 0x0020DB85 File Offset: 0x0020BD85
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.AnomalyActive;
		}

		// Token: 0x17000F94 RID: 3988
		// (get) Token: 0x06005905 RID: 22789 RVA: 0x0020DB8F File Offset: 0x0020BD8F
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.EntityHolder);
			}
		}

		// Token: 0x06005906 RID: 22790 RVA: 0x0020DB98 File Offset: 0x0020BD98
		public override string PostProcessedGerund(Job job)
		{
			return "ReleasingEntity".Translate(this.def.gerund.Named("GERUND"), job.targetB.Label.Named("TARGETLABEL"));
		}

		// Token: 0x06005907 RID: 22791 RVA: 0x0020DBD3 File Offset: 0x0020BDD3
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return pawn.CanReserve(t, 1, -1, null, forced) && this.GetEntity(t) != null;
		}

		// Token: 0x06005908 RID: 22792 RVA: 0x0020DBF8 File Offset: 0x0020BDF8
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn entity = this.GetEntity(t);
			if (entity == null)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.ReleaseEntity, t, entity).WithCount(1);
		}

		// Token: 0x06005909 RID: 22793 RVA: 0x0020DC30 File Offset: 0x0020BE30
		private Pawn GetEntity(Thing thing)
		{
			Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
			if (building_HoldingPlatform == null)
			{
				return null;
			}
			Pawn heldPawn = building_HoldingPlatform.HeldPawn;
			if (heldPawn == null)
			{
				return null;
			}
			CompHoldingPlatformTarget compHoldingPlatformTarget = heldPawn.TryGetComp<CompHoldingPlatformTarget>();
			if (compHoldingPlatformTarget != null && compHoldingPlatformTarget.containmentMode != EntityContainmentMode.Release)
			{
				return null;
			}
			return heldPawn;
		}
	}
}
