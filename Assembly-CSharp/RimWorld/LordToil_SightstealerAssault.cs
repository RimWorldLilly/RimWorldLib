using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CF6 RID: 3318
	public class LordToil_SightstealerAssault : LordToil
	{
		// Token: 0x06005DA3 RID: 23971 RVA: 0x00224FD8 File Offset: 0x002231D8
		public override void UpdateAllDuties()
		{
			foreach (Pawn pawn in this.lord.ownedPawns)
			{
				pawn.mindState.duty = new PawnDuty(DutyDefOf.SightstealerAssault);
			}
		}

		// Token: 0x06005DA4 RID: 23972 RVA: 0x0022503C File Offset: 0x0022323C
		public override void Notify_PawnDamaged(Pawn victim, DamageInfo dinfo)
		{
			if (dinfo.Instigator == null)
			{
				return;
			}
			if (!victim.HostileTo(dinfo.Instigator))
			{
				return;
			}
			this.lord.Notify_PawnLost(victim, PawnLostCondition.LeftVoluntarily, null);
			victim.mindState.enemyTarget = dinfo.Instigator;
			victim.mindState.Notify_EngagedTarget();
			victim.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
		}
	}
}
