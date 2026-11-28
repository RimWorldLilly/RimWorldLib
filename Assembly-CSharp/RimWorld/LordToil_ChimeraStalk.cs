using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CCB RID: 3275
	public class LordToil_ChimeraStalk : LordToil
	{
		// Token: 0x06005CD9 RID: 23769 RVA: 0x00221354 File Offset: 0x0021F554
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty duty = this.lord.ownedPawns[i].mindState.duty;
				if (((duty != null) ? duty.def : null) != DutyDefOf.ChimeraStalkFlee)
				{
					this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.ChimeraStalkFlee);
					this.lord.ownedPawns[i].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}

		// Token: 0x06005CDA RID: 23770 RVA: 0x002213F4 File Offset: 0x0021F5F4
		public override void Notify_PawnJobDone(Pawn p, JobCondition condition)
		{
			base.Notify_PawnJobDone(p, condition);
			PawnDuty duty = p.mindState.duty;
			if (((duty != null) ? duty.def : null) == DutyDefOf.ChimeraStalkFlee && p.CurJobDef == JobDefOf.Wait_Wander)
			{
				p.mindState.duty = new PawnDuty(DutyDefOf.ChimeraStalkWander);
			}
		}
	}
}
