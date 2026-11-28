using System;

namespace Verse.AI
{
	// Token: 0x02000847 RID: 2119
	public static class Toils_ReserveAttackTarget
	{
		// Token: 0x060042A7 RID: 17063 RVA: 0x001A4900 File Offset: 0x001A2B00
		public static Toil TryReserve(TargetIndex ind)
		{
			Toil toil = ToilMaker.MakeToil("TryReserve");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				IAttackTarget attackTarget = actor.CurJob.GetTarget(ind).Thing as IAttackTarget;
				if (attackTarget != null)
				{
					actor.Map.attackTargetReservationManager.Reserve(actor, toil.actor.CurJob, attackTarget);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}
	}
}
