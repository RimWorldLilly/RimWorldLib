using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C05 RID: 3077
	public class WorkGiver_ExecuteEntity : WorkGiver_EntityOnPlatform
	{
		// Token: 0x060057ED RID: 22509 RVA: 0x00209013 File Offset: 0x00207213
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return pawn.CanReserve(t, 1, -1, null, forced) && this.GetEntity(t) != null;
		}

		// Token: 0x060057EE RID: 22510 RVA: 0x00209033 File Offset: 0x00207233
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.ExecuteEntity, t);
		}

		// Token: 0x060057EF RID: 22511 RVA: 0x00209048 File Offset: 0x00207248
		protected override Pawn GetEntity(Thing potentialPlatform)
		{
			Building_HoldingPlatform building_HoldingPlatform = potentialPlatform as Building_HoldingPlatform;
			if (building_HoldingPlatform == null)
			{
				return null;
			}
			Pawn heldPawn = building_HoldingPlatform.HeldPawn;
			CompHoldingPlatformTarget compHoldingPlatformTarget = (heldPawn != null) ? heldPawn.TryGetComp<CompHoldingPlatformTarget>() : null;
			if (compHoldingPlatformTarget == null || compHoldingPlatformTarget.containmentMode != EntityContainmentMode.Execute)
			{
				return null;
			}
			return heldPawn;
		}
	}
}
