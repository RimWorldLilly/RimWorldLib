using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000845 RID: 2117
	public static class Toils_Goto
	{
		// Token: 0x06004299 RID: 17049 RVA: 0x001A43B5 File Offset: 0x001A25B5
		public static Toil Goto(TargetIndex ind, PathEndMode peMode)
		{
			return Toils_Goto.GotoThing(ind, peMode, false);
		}

		// Token: 0x0600429A RID: 17050 RVA: 0x001A43C0 File Offset: 0x001A25C0
		public static Toil GotoThing(TargetIndex ind, PathEndMode peMode, bool canGotoSpawnedParent = false)
		{
			Toil toil = ToilMaker.MakeToil("GotoThing");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				LocalTargetInfo dest = actor.jobs.curJob.GetTarget(ind);
				Thing thing = dest.Thing;
				if (thing != null & canGotoSpawnedParent)
				{
					dest = thing.SpawnedParentOrMe;
				}
				actor.pather.StartPath(dest, peMode);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			if (canGotoSpawnedParent)
			{
				toil.FailOnSelfAndParentsDespawnedOrNull(ind);
			}
			else
			{
				toil.FailOnDespawnedOrNull(ind);
			}
			return toil;
		}

		// Token: 0x0600429B RID: 17051 RVA: 0x001A4450 File Offset: 0x001A2650
		public static Toil GotoThing(TargetIndex ind, IntVec3 exactCell)
		{
			Toil toil = ToilMaker.MakeToil("GotoThing");
			toil.initAction = delegate()
			{
				toil.actor.pather.StartPath(exactCell, PathEndMode.OnCell);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			toil.FailOnDespawnedOrNull(ind);
			return toil;
		}

		// Token: 0x0600429C RID: 17052 RVA: 0x001A44B0 File Offset: 0x001A26B0
		public static Toil GotoCell(TargetIndex ind, PathEndMode peMode)
		{
			Toil toil = ToilMaker.MakeToil("GotoCell");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				LocalTargetInfo target = actor.jobs.curJob.GetTarget(ind);
				if (actor.Position == target.Cell)
				{
					actor.pather.StopDead();
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				actor.pather.StartPath(target, peMode);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x0600429D RID: 17053 RVA: 0x001A450C File Offset: 0x001A270C
		public static Toil GotoCell(IntVec3 cell, PathEndMode peMode)
		{
			Toil toil = ToilMaker.MakeToil("GotoCell");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				if (actor.Position == cell)
				{
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				actor.pather.StartPath(cell, peMode);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x0600429E RID: 17054 RVA: 0x001A4568 File Offset: 0x001A2768
		public static Toil MoveOffTargetBlueprint(TargetIndex targetInd)
		{
			Toil toil = ToilMaker.MakeToil("MoveOffTargetBlueprint");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Blueprint blueprint = actor.jobs.curJob.GetTarget(targetInd).Thing as Blueprint;
				if (blueprint.DestroyedOrNull() || !actor.Position.IsInside(blueprint))
				{
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				IntVec3 c;
				if (RCellFinder.TryFindGoodAdjacentSpotToTouch(actor, blueprint, out c))
				{
					actor.pather.StartPath(c, PathEndMode.OnCell);
					return;
				}
				actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x0600429F RID: 17055 RVA: 0x001A45BC File Offset: 0x001A27BC
		public static Toil GotoBuild(TargetIndex ind)
		{
			Toil toil = ToilMaker.MakeToil("GotoBuild");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				LocalTargetInfo target = actor.jobs.curJob.GetTarget(ind);
				Thing thing = target.Thing;
				IntVec3 c;
				if (RCellFinder.TryFindGoodAdjacentSpotToTouch(actor, thing, out c))
				{
					actor.pather.StartPath(c, PathEndMode.OnCell);
					return;
				}
				actor.pather.StartPath(target, PathEndMode.OnCell);
			};
			toil.tickIntervalAction = delegate(int _)
			{
				Pawn actor = toil.actor;
				Thing thing = actor.jobs.curJob.GetTarget(ind).Thing;
				if (actor.CanReachImmediate(thing, PathEndMode.Touch) && actor.Map.reservationManager.CanReserve(actor, actor.Position, 1, -1, null, false))
				{
					actor.Reserve(actor.Position, actor.CurJob, 1, -1, null, true, false);
					actor.pather.StopDead();
					actor.jobs.curDriver.ReadyForNextToil();
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			toil.FailOnDespawnedOrNull(ind);
			return toil;
		}
	}
}
