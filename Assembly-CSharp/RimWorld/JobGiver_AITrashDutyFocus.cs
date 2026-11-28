using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AE5 RID: 2789
	public class JobGiver_AITrashDutyFocus : ThinkNode_JobGiver
	{
		// Token: 0x06005360 RID: 21344 RVA: 0x001F0BB3 File Offset: 0x001EEDB3
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			return (JobGiver_AITrashDutyFocus)base.DeepCopy(resolve);
		}

		// Token: 0x06005361 RID: 21345 RVA: 0x001F0BC4 File Offset: 0x001EEDC4
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.mindState.duty == null || !pawn.mindState.duty.focus.IsValid)
			{
				return null;
			}
			LocalTargetInfo focus = pawn.mindState.duty.focus;
			if (!focus.Thing.Spawned || focus.ThingDestroyed || !pawn.HostileTo(focus.Thing) || !pawn.CanReach(focus, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Job job = TrashUtility.TrashJob(pawn, focus.Thing, false, true);
			if (job != null)
			{
				return job;
			}
			return null;
		}
	}
}
