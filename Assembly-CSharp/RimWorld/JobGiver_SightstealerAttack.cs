using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B6A RID: 2922
	public class JobGiver_SightstealerAttack : ThinkNode_JobGiver
	{
		// Token: 0x06005519 RID: 21785 RVA: 0x001FA679 File Offset: 0x001F8879
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_SightstealerAttack jobGiver_SightstealerAttack = (JobGiver_SightstealerAttack)base.DeepCopy(resolve);
			jobGiver_SightstealerAttack.targetAcquireRadius = this.targetAcquireRadius;
			return jobGiver_SightstealerAttack;
		}

		// Token: 0x0600551A RID: 21786 RVA: 0x001FA694 File Offset: 0x001F8894
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2 = this.FindPawnTarget(pawn);
			if (pawn2 != null)
			{
				return this.MeleeAttackJob(pawn2, false);
			}
			return null;
		}

		// Token: 0x0600551B RID: 21787 RVA: 0x001FA6B6 File Offset: 0x001F88B6
		private Job MeleeAttackJob(Thing target, bool canBashFences)
		{
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, target);
			job.maxNumMeleeAttacks = 1;
			job.expiryInterval = Rand.Range(420, 900);
			job.attackDoorIfTargetLost = true;
			job.canBashFences = canBashFences;
			return job;
		}

		// Token: 0x0600551C RID: 21788 RVA: 0x001FA6F4 File Offset: 0x001F88F4
		private Pawn FindPawnTarget(Pawn pawn)
		{
			return (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable, (Thing thing) => !thing.Isnt<Pawn>() && thing.def.race.intelligence >= Intelligence.ToolUser && (this.targetAcquireRadius <= 0 || pawn.Position.DistanceTo(thing.PositionHeld) <= (float)this.targetAcquireRadius), 0f, 9999f, default(IntVec3), float.MaxValue, false, true, false, false);
		}

		// Token: 0x040032BE RID: 12990
		private const int MinMeleeChaseTicks = 420;

		// Token: 0x040032BF RID: 12991
		private const int MaxMeleeChaseTicks = 900;

		// Token: 0x040032C0 RID: 12992
		private int targetAcquireRadius = -1;
	}
}
