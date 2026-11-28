using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B97 RID: 2967
	public class JobGiver_Berserk : ThinkNode_JobGiver
	{
		// Token: 0x060055F4 RID: 22004 RVA: 0x001FFA50 File Offset: 0x001FDC50
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (Rand.Value < 0.5f)
			{
				Job job = JobMaker.MakeJob(JobDefOf.Wait_Combat);
				job.expiryInterval = 90;
				job.canUseRangedWeapon = false;
				return job;
			}
			if (pawn.TryGetAttackVerb(null, false, false) == null)
			{
				return null;
			}
			Thing thing = this.FindAttackTarget(pawn);
			if (thing != null)
			{
				Job job2 = JobMaker.MakeJob(JobDefOf.AttackMelee, thing);
				job2.maxNumMeleeAttacks = 1;
				job2.expiryInterval = Rand.Range(420, 900);
				job2.canBashDoors = true;
				return job2;
			}
			return null;
		}

		// Token: 0x060055F5 RID: 22005 RVA: 0x001FFAD0 File Offset: 0x001FDCD0
		private Thing FindAttackTarget(Pawn pawn)
		{
			return (Thing)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedReachable, new Predicate<Thing>(this.IsGoodTarget), 0f, this.maxAttackDistance, default(IntVec3), float.MaxValue, true, true, false, false);
		}

		// Token: 0x060055F6 RID: 22006 RVA: 0x001FFB14 File Offset: 0x001FDD14
		protected virtual bool IsGoodTarget(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			return pawn != null && pawn.Spawned && !pawn.Downed && !pawn.IsPsychologicallyInvisible();
		}

		// Token: 0x060055F7 RID: 22007 RVA: 0x001FFB46 File Offset: 0x001FDD46
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Berserk jobGiver_Berserk = (JobGiver_Berserk)base.DeepCopy(resolve);
			jobGiver_Berserk.maxAttackDistance = this.maxAttackDistance;
			return jobGiver_Berserk;
		}

		// Token: 0x040032FF RID: 13055
		private const float WaitChance = 0.5f;

		// Token: 0x04003300 RID: 13056
		private const int WaitTicks = 90;

		// Token: 0x04003301 RID: 13057
		private const int MinMeleeChaseTicks = 420;

		// Token: 0x04003302 RID: 13058
		private const int MaxMeleeChaseTicks = 900;

		// Token: 0x04003303 RID: 13059
		private float maxAttackDistance = 40f;
	}
}
