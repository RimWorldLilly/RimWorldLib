using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B05 RID: 2821
	public class JobGiver_GetJoyInGatheringArea : JobGiver_GetJoy
	{
		// Token: 0x060053C8 RID: 21448 RVA: 0x001F3080 File Offset: 0x001F1280
		protected override Job TryGiveJobFromJoyGiverDefDirect(JoyGiverDef def, Pawn pawn)
		{
			if (pawn.mindState.duty == null)
			{
				return null;
			}
			if (pawn.needs.joy == null)
			{
				return null;
			}
			if (pawn.needs.joy.CurLevelPercentage > 0.92f)
			{
				return null;
			}
			IntVec3 cell = pawn.mindState.duty.focus.Cell;
			return def.Worker.TryGiveJobInGatheringArea(pawn, cell, this.desiredRadius);
		}

		// Token: 0x060053C9 RID: 21449 RVA: 0x001F30ED File Offset: 0x001F12ED
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.needs.joy == null)
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x060053CA RID: 21450 RVA: 0x001F3105 File Offset: 0x001F1305
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GetJoyInGatheringArea jobGiver_GetJoyInGatheringArea = (JobGiver_GetJoyInGatheringArea)base.DeepCopy(resolve);
			jobGiver_GetJoyInGatheringArea.desiredRadius = this.desiredRadius;
			return jobGiver_GetJoyInGatheringArea;
		}

		// Token: 0x04003235 RID: 12853
		public float desiredRadius = -1f;
	}
}
