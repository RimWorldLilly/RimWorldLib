using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BA1 RID: 2977
	public class JobGiver_Manhunter : ThinkNode_JobGiver
	{
		// Token: 0x06005610 RID: 22032 RVA: 0x0020016C File Offset: 0x001FE36C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.TryGetAttackVerb(null, false, false) == null)
			{
				return null;
			}
			bool fenceBlocked = pawn.FenceBlocked;
			Pawn pawn2 = this.FindPawnTarget(pawn, fenceBlocked, this.canBashDoors);
			if (pawn2 != null && pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, fenceBlocked, TraverseMode.ByPawn))
			{
				Job abilityJob = JobGiver_AIFightEnemy.GetAbilityJob(pawn, pawn2);
				if (abilityJob != null)
				{
					return abilityJob;
				}
				return this.MeleeAttackJob(pawn2, fenceBlocked, this.canBashDoors);
			}
			else
			{
				Building building = this.FindTurretTarget(pawn, fenceBlocked);
				if (building != null)
				{
					return this.MeleeAttackJob(building, fenceBlocked, this.canBashDoors);
				}
				if (pawn2 != null)
				{
					using (PawnPath pawnPath = pawn.Map.pathFinder.FindPathNow(pawn.Position, pawn2.Position, TraverseParms.For(pawn, Danger.Deadly, this.canBashDoors ? TraverseMode.PassDoors : TraverseMode.NoPassClosedDoors, false, false, false, true), null, PathEndMode.OnCell, null))
					{
						if (!pawnPath.Found)
						{
							return null;
						}
						IntVec3 loc;
						if (!pawnPath.TryFindLastCellBeforeBlockingDoor(pawn, out loc))
						{
							if (pawn2.Position.GetDoor(pawn2.Map) == null)
							{
								Log.Error(((pawn != null) ? pawn.ToString() : null) + " did TryFindLastCellBeforeDoor but found none when it should have been one. Target: " + pawn2.LabelCap);
								return null;
							}
							loc = pawnPath.NodesReversed[1];
						}
						IntVec3 randomCell = CellFinder.RandomRegionNear(loc.GetRegion(pawn.Map, RegionType.Set_Passable), 9, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), null, null, RegionType.Set_Passable).RandomCell;
						if (randomCell == pawn.Position)
						{
							return JobMaker.MakeJob(JobDefOf.Wait, 30, false);
						}
						return JobMaker.MakeJob(JobDefOf.Goto, randomCell);
					}
				}
				return null;
			}
		}

		// Token: 0x06005611 RID: 22033 RVA: 0x00200320 File Offset: 0x001FE520
		private Job MeleeAttackJob(Thing target, bool canBashFences, bool canBashDoors)
		{
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, target);
			job.maxNumMeleeAttacks = 1;
			job.expiryInterval = Rand.Range(420, 900);
			job.attackDoorIfTargetLost = canBashDoors;
			job.canBashFences = canBashFences;
			return job;
		}

		// Token: 0x06005612 RID: 22034 RVA: 0x0020035C File Offset: 0x001FE55C
		private Pawn FindPawnTarget(Pawn pawn, bool canBashFences, bool canBashDoors)
		{
			return (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable, (Thing x) => x is Pawn && x.def.race.intelligence >= Intelligence.ToolUser, 0f, 9999f, default(IntVec3), float.MaxValue, canBashDoors, true, canBashFences, false);
		}

		// Token: 0x06005613 RID: 22035 RVA: 0x002003B8 File Offset: 0x001FE5B8
		private Building FindTurretTarget(Pawn pawn, bool canBashFences)
		{
			return (Building)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedLOSToNonPawns | TargetScanFlags.NeedReachable | TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable, (Thing t) => t is Building, 0f, 70f, default(IntVec3), float.MaxValue, false, true, canBashFences, false);
		}

		// Token: 0x04003307 RID: 13063
		public bool canBashDoors = true;

		// Token: 0x04003308 RID: 13064
		private const float WaitChance = 0.75f;

		// Token: 0x04003309 RID: 13065
		private const int WaitTicks = 90;

		// Token: 0x0400330A RID: 13066
		private const int MinMeleeChaseTicks = 420;

		// Token: 0x0400330B RID: 13067
		private const int MaxMeleeChaseTicks = 900;

		// Token: 0x0400330C RID: 13068
		private const int WanderOutsideDoorRegions = 9;
	}
}
