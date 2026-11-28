using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AE1 RID: 2785
	public static class DigUtility
	{
		// Token: 0x0600534F RID: 21327 RVA: 0x001F0340 File Offset: 0x001EE540
		public static Job PassBlockerJob(Pawn pawn, Thing blocker, IntVec3 cellBeforeBlocker, bool canMineMineables, bool canMineNonMineables)
		{
			if (StatDefOf.MiningSpeed.Worker.IsDisabledFor(pawn))
			{
				canMineMineables = false;
				canMineNonMineables = false;
			}
			if (blocker.def.mineable)
			{
				if (canMineMineables)
				{
					return DigUtility.MineOrWaitJob(pawn, blocker, cellBeforeBlocker);
				}
				return DigUtility.MeleeOrWaitJob(pawn, blocker, cellBeforeBlocker);
			}
			else
			{
				if (pawn.equipment != null && pawn.equipment.Primary != null)
				{
					Verb primaryVerb = pawn.equipment.PrimaryEq.PrimaryVerb;
					if (primaryVerb.verbProps.ai_IsBuildingDestroyer && (!primaryVerb.IsIncendiary_Ranged() || blocker.FlammableNow))
					{
						Job job = JobMaker.MakeJob(JobDefOf.UseVerbOnThing);
						job.targetA = blocker;
						job.verbToUse = primaryVerb;
						job.expiryInterval = JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange;
						return job;
					}
				}
				if (canMineNonMineables)
				{
					return DigUtility.MineOrWaitJob(pawn, blocker, cellBeforeBlocker);
				}
				return DigUtility.MeleeOrWaitJob(pawn, blocker, cellBeforeBlocker);
			}
		}

		// Token: 0x06005350 RID: 21328 RVA: 0x001F0410 File Offset: 0x001EE610
		private static Job MeleeOrWaitJob(Pawn pawn, Thing blocker, IntVec3 cellBeforeBlocker)
		{
			if (!pawn.CanReserve(blocker, 1, -1, null, false))
			{
				return DigUtility.WaitNearJob(pawn, cellBeforeBlocker);
			}
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, blocker);
			job.ignoreDesignations = true;
			job.expiryInterval = JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange;
			job.checkOverrideOnExpire = true;
			return job;
		}

		// Token: 0x06005351 RID: 21329 RVA: 0x001F0468 File Offset: 0x001EE668
		private static Job MineOrWaitJob(Pawn pawn, Thing blocker, IntVec3 cellBeforeBlocker)
		{
			if (!pawn.CanReserve(blocker, 1, -1, null, false))
			{
				return DigUtility.WaitNearJob(pawn, cellBeforeBlocker);
			}
			Job job = JobMaker.MakeJob(JobDefOf.Mine, blocker);
			job.ignoreDesignations = true;
			job.expiryInterval = JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange;
			job.checkOverrideOnExpire = true;
			return job;
		}

		// Token: 0x06005352 RID: 21330 RVA: 0x001F04C0 File Offset: 0x001EE6C0
		private static Job WaitNearJob(Pawn pawn, IntVec3 cellBeforeBlocker)
		{
			IntVec3 intVec = CellFinder.RandomClosewalkCellNear(cellBeforeBlocker, pawn.Map, 10, null);
			if (intVec == pawn.Position)
			{
				return JobMaker.MakeJob(JobDefOf.Wait, 20, true);
			}
			return JobMaker.MakeJob(JobDefOf.Goto, intVec, 500, true);
		}

		// Token: 0x04003213 RID: 12819
		private const int CheckOverrideInterval = 500;
	}
}
