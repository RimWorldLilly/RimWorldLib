using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C06 RID: 3078
	public class WorkGiver_ExtractBioferrite : WorkGiver_EntityOnPlatform
	{
		// Token: 0x060057F1 RID: 22513 RVA: 0x0020908C File Offset: 0x0020728C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return pawn.CanReserve(t, 1, -1, null, forced) && this.GetEntity(t) != null;
		}

		// Token: 0x060057F2 RID: 22514 RVA: 0x002090AC File Offset: 0x002072AC
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (this.GetEntity(t) == null)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.ExtractBioferrite, t);
		}

		// Token: 0x060057F3 RID: 22515 RVA: 0x002090CC File Offset: 0x002072CC
		protected override Pawn GetEntity(Thing potentialPlatform)
		{
			Building_HoldingPlatform building_HoldingPlatform = potentialPlatform as Building_HoldingPlatform;
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
			if (compHoldingPlatformTarget == null || !compHoldingPlatformTarget.extractBioferrite)
			{
				return null;
			}
			if (heldPawn.health.hediffSet.HasHediff(HediffDefOf.BioferriteExtracted, false))
			{
				JobFailReason.Is("BioferriteAlreadyExtracted".Translate(), "ExtractBioferrite".Translate(heldPawn.LabelShort));
				return null;
			}
			if (CompProducesBioferrite.BioferritePerDay(heldPawn) <= 0f)
			{
				return null;
			}
			return heldPawn;
		}
	}
}
