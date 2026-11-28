using System;

namespace Verse.AI
{
	// Token: 0x02000844 RID: 2116
	public static class Toils_Jump
	{
		// Token: 0x06004291 RID: 17041 RVA: 0x001A4138 File Offset: 0x001A2338
		public static Toil Jump(Toil jumpTarget)
		{
			Toil toil = ToilMaker.MakeToil("Jump");
			toil.initAction = delegate()
			{
				toil.actor.jobs.curDriver.JumpToToil(jumpTarget);
			};
			return toil;
		}

		// Token: 0x06004292 RID: 17042 RVA: 0x001A4180 File Offset: 0x001A2380
		public static Toil JumpIf(Toil jumpTarget, Func<bool> condition)
		{
			Toil toil = ToilMaker.MakeToil("JumpIf");
			toil.initAction = delegate()
			{
				if (condition())
				{
					toil.actor.jobs.curDriver.JumpToToil(jumpTarget);
				}
			};
			return toil;
		}

		// Token: 0x06004293 RID: 17043 RVA: 0x001A41D0 File Offset: 0x001A23D0
		public static Toil JumpIfTargetDespawnedOrNull(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = ToilMaker.MakeToil("JumpIfTargetDespawnedOrNull");
			toil.initAction = delegate()
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				if (thing == null || !thing.Spawned)
				{
					toil.actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x06004294 RID: 17044 RVA: 0x001A4220 File Offset: 0x001A2420
		public static Toil JumpIfTargetInvalid(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = ToilMaker.MakeToil("JumpIfTargetInvalid");
			toil.initAction = delegate()
			{
				if (!toil.actor.jobs.curJob.GetTarget(ind).IsValid)
				{
					toil.actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x06004295 RID: 17045 RVA: 0x001A4270 File Offset: 0x001A2470
		public static Toil JumpIfTargetNotHittable(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = ToilMaker.MakeToil("JumpIfTargetNotHittable");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				LocalTargetInfo target = curJob.GetTarget(ind);
				if (curJob.verbToUse == null || !curJob.verbToUse.IsStillUsableBy(actor) || !curJob.verbToUse.CanHitTarget(target))
				{
					actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x06004296 RID: 17046 RVA: 0x001A42C0 File Offset: 0x001A24C0
		public static Toil JumpIfTargetDowned(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = ToilMaker.MakeToil("JumpIfTargetDowned");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = actor.jobs.curJob.GetTarget(ind).Thing as Pawn;
				if (pawn != null && pawn.Downed)
				{
					actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x06004297 RID: 17047 RVA: 0x001A4310 File Offset: 0x001A2510
		public static Toil JumpIfHaveTargetInQueue(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = ToilMaker.MakeToil("JumpIfHaveTargetInQueue");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				if (!actor.jobs.curJob.GetTargetQueue(ind).NullOrEmpty<LocalTargetInfo>())
				{
					actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x06004298 RID: 17048 RVA: 0x001A4360 File Offset: 0x001A2560
		public static Toil JumpIfCannotTouch(TargetIndex ind, PathEndMode peMode, Toil jumpToil)
		{
			Toil toil = ToilMaker.MakeToil("JumpIfCannotTouch");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				LocalTargetInfo target = actor.jobs.curJob.GetTarget(ind);
				if (!actor.CanReachImmediate(target, peMode))
				{
					actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}
	}
}
