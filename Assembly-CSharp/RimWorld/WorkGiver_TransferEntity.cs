using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C52 RID: 3154
	public class WorkGiver_TransferEntity : WorkGiver_Scanner
	{
		// Token: 0x06005989 RID: 22921 RVA: 0x0020EE14 File Offset: 0x0020D014
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.AnomalyActive;
		}

		// Token: 0x17000FAB RID: 4011
		// (get) Token: 0x0600598A RID: 22922 RVA: 0x0020EE1E File Offset: 0x0020D01E
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.EntityHolder);
			}
		}

		// Token: 0x0600598B RID: 22923 RVA: 0x0020EE28 File Offset: 0x0020D028
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			Pawn entity = this.GetEntity(t);
			if (entity == null)
			{
				return false;
			}
			CompHoldingPlatformTarget compHoldingPlatformTarget = entity.TryGetComp<CompHoldingPlatformTarget>();
			if (compHoldingPlatformTarget == null)
			{
				return false;
			}
			if (compHoldingPlatformTarget.targetHolder == null)
			{
				return false;
			}
			if (!pawn.CanReserve(compHoldingPlatformTarget.targetHolder, 1, -1, null, forced) || !pawn.CanReserve(entity, 1, -1, null, forced))
			{
				return false;
			}
			if (compHoldingPlatformTarget.HeldPlatform.IsForbidden(pawn) || compHoldingPlatformTarget.targetHolder.IsForbidden(pawn))
			{
				JobFailReason.Is("ForbiddenLower".Translate(), null);
				return false;
			}
			return compHoldingPlatformTarget.HeldPlatform != compHoldingPlatformTarget.targetHolder;
		}

		// Token: 0x0600598C RID: 22924 RVA: 0x0020EEDC File Offset: 0x0020D0DC
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn entity = this.GetEntity(t);
			if (entity == null)
			{
				return null;
			}
			CompHoldingPlatformTarget compHoldingPlatformTarget = entity.TryGetComp<CompHoldingPlatformTarget>();
			if (compHoldingPlatformTarget == null)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.TransferBetweenEntityHolders, t, compHoldingPlatformTarget.targetHolder, entity).WithCount(1);
		}

		// Token: 0x0600598D RID: 22925 RVA: 0x0020EF2C File Offset: 0x0020D12C
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
			if (compHoldingPlatformTarget != null && compHoldingPlatformTarget.HeldPlatform == compHoldingPlatformTarget.targetHolder)
			{
				return null;
			}
			return heldPawn;
		}
	}
}
