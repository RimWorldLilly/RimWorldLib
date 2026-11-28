using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200083E RID: 2110
	public static class ToilJumpConditions
	{
		// Token: 0x06004270 RID: 17008 RVA: 0x001A37DC File Offset: 0x001A19DC
		public static Toil JumpIf(this Toil toil, Func<bool> jumpCondition, Toil jumpToil)
		{
			toil.AddPreTickAction(delegate
			{
				if (jumpCondition())
				{
					toil.actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			});
			return toil;
		}

		// Token: 0x06004271 RID: 17009 RVA: 0x001A3824 File Offset: 0x001A1A24
		public static Toil JumpIfDespawnedOrNull(this Toil toil, TargetIndex ind, Toil jumpToil)
		{
			return toil.JumpIf(delegate
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				return thing == null || !thing.Spawned;
			}, jumpToil);
		}

		// Token: 0x06004272 RID: 17010 RVA: 0x001A3860 File Offset: 0x001A1A60
		public static Toil JumpIfDespawnedOrNullOrForbidden(this Toil toil, TargetIndex ind, Toil jumpToil)
		{
			return toil.JumpIf(delegate
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				return thing == null || !thing.Spawned || thing.IsForbidden(toil.actor);
			}, jumpToil);
		}

		// Token: 0x06004273 RID: 17011 RVA: 0x001A389C File Offset: 0x001A1A9C
		public static Toil JumpIfOutsideHomeArea(this Toil toil, TargetIndex ind, Toil jumpToil)
		{
			return toil.JumpIf(delegate
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				return !toil.actor.Map.areaManager.Home[thing.Position];
			}, jumpToil);
		}

		// Token: 0x06004274 RID: 17012 RVA: 0x001A38D8 File Offset: 0x001A1AD8
		public static Toil JumpIfThingMissingDesignation(this Toil toil, TargetIndex ind, DesignationDef desDef, Toil jumpToil)
		{
			return toil.JumpIf(delegate
			{
				Pawn actor = toil.GetActor();
				Job curJob = actor.jobs.curJob;
				if (curJob.ignoreDesignations)
				{
					return false;
				}
				Thing thing = curJob.GetTarget(ind).Thing;
				return thing == null || actor.Map.designationManager.DesignationOn(thing, desDef) == null;
			}, jumpToil);
		}
	}
}
