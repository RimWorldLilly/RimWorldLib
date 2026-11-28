using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C4A RID: 3146
	public class WorkGiver_TendEntity : WorkGiver_EntityOnPlatform
	{
		// Token: 0x0600596C RID: 22892 RVA: 0x0020EB20 File Offset: 0x0020CD20
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			Pawn entity = this.GetEntity(t);
			return entity != null && HealthAIUtility.ShouldBeTendedNowByPlayer(entity);
		}

		// Token: 0x0600596D RID: 22893 RVA: 0x0020EB54 File Offset: 0x0020CD54
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn entity = this.GetEntity(t);
			if (entity == null)
			{
				return null;
			}
			Thing thing = HealthAIUtility.FindBestMedicine(pawn, entity, false);
			JobDef tendEntity = JobDefOf.TendEntity;
			LocalTargetInfo targetA = t;
			Thing thing2 = thing;
			return JobMaker.MakeJob(tendEntity, targetA, (thing2 != null) ? thing2 : LocalTargetInfo.Invalid);
		}

		// Token: 0x0600596E RID: 22894 RVA: 0x0020EB99 File Offset: 0x0020CD99
		protected override Pawn GetEntity(Thing potentialPlatform)
		{
			return WorkGiver_TendEntity.GetTendableEnityFromPotentialPlatform(potentialPlatform);
		}

		// Token: 0x0600596F RID: 22895 RVA: 0x0020EBA4 File Offset: 0x0020CDA4
		public static Pawn GetTendableEnityFromPotentialPlatform(Thing potentialPlatform)
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
			if (compHoldingPlatformTarget != null && (compHoldingPlatformTarget.containmentMode == EntityContainmentMode.Release || compHoldingPlatformTarget.containmentMode == EntityContainmentMode.Execute))
			{
				return null;
			}
			return heldPawn;
		}
	}
}
