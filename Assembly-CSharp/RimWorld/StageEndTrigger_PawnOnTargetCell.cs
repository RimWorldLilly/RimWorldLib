using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200166C RID: 5740
	public class StageEndTrigger_PawnOnTargetCell : StageEndTrigger
	{
		// Token: 0x0600901D RID: 36893 RVA: 0x0034F098 File Offset: 0x0034D298
		public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
		{
			if (ritual.Ritual.behavior.def.roles.FirstOrDefault((RitualRole r) => r.id == this.roleId) == null)
			{
				return null;
			}
			Pawn pawn = ritual.assignments.FirstAssignedPawn(this.roleId);
			if (pawn == null)
			{
				return null;
			}
			PawnStagePosition dest = ritual.PawnPositionForStage(pawn, stage);
			return new Trigger_TicksPassedAfterConditionMet(this.waitTicks, () => pawn.CanReachImmediate(dest.cell, PathEndMode.ClosestTouch) || pawn.Dead, 1);
		}

		// Token: 0x0600901E RID: 36894 RVA: 0x0034F129 File Offset: 0x0034D329
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.roleId, "roleId", null, false);
			Scribe_Values.Look<int>(ref this.waitTicks, "waitTicks", 0, false);
		}

		// Token: 0x040057F5 RID: 22517
		public string roleId;

		// Token: 0x040057F6 RID: 22518
		public int waitTicks = 50;
	}
}
