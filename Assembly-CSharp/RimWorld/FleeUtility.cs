using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200099E RID: 2462
	public static class FleeUtility
	{
		// Token: 0x06004A58 RID: 19032 RVA: 0x001CE2F8 File Offset: 0x001CC4F8
		public static Job FleeJob(Pawn pawn, Thing danger, int fleeDistance)
		{
			IntVec3 intVec;
			if (pawn.CurJob != null && pawn.CurJob.def == JobDefOf.Flee)
			{
				intVec = pawn.CurJob.targetA.Cell;
			}
			else
			{
				FleeUtility.tmpThings.Clear();
				FleeUtility.tmpThings.Add(danger);
				intVec = CellFinderLoose.GetFleeDest(pawn, FleeUtility.tmpThings, (float)fleeDistance);
				FleeUtility.tmpThings.Clear();
			}
			if (intVec != pawn.Position)
			{
				return JobMaker.MakeJob(JobDefOf.Flee, intVec, danger);
			}
			return null;
		}

		// Token: 0x06004A59 RID: 19033 RVA: 0x001CE388 File Offset: 0x001CC588
		public static Job FleeLargeFireJob(Pawn pawn, int minFiresNearbyToFlee, int distToFireToFlee, int fleeDistance)
		{
			if (pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Fire).Count < minFiresNearbyToFlee)
			{
				return null;
			}
			TraverseParms tp = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true);
			Fire closestFire = null;
			float closestDistSq = -1f;
			int firesCount = 0;
			RegionTraverser.BreadthFirstTraverse(pawn.Position, pawn.Map, (Region from, Region to) => to.Allows(tp, false), delegate(Region x)
			{
				List<Thing> list = x.ListerThings.ThingsInGroup(ThingRequestGroup.Fire);
				int firesCount;
				for (int i = 0; i < list.Count; i++)
				{
					float num = (float)pawn.Position.DistanceToSquared(list[i].Position);
					if (num <= (float)(FleeUtility.MinFiresNearbyRadius * FleeUtility.MinFiresNearbyRadius))
					{
						if (closestFire == null || num < closestDistSq)
						{
							closestDistSq = num;
							closestFire = (Fire)list[i];
						}
						firesCount = firesCount;
						firesCount++;
					}
				}
				return closestDistSq <= (float)(distToFireToFlee * distToFireToFlee) && firesCount >= minFiresNearbyToFlee;
			}, FleeUtility.MinFiresNearbyRegionsToScan, RegionType.Set_Passable);
			if (closestDistSq <= (float)(distToFireToFlee * distToFireToFlee) && firesCount >= minFiresNearbyToFlee)
			{
				Job job = FleeUtility.FleeJob(pawn, closestFire, fleeDistance);
				if (job != null)
				{
					return job;
				}
			}
			return null;
		}

		// Token: 0x06004A5A RID: 19034 RVA: 0x001CE480 File Offset: 0x001CC680
		public static bool ShouldAnimalFleeDanger(Pawn pawn)
		{
			return pawn.IsAnimal && !pawn.InMentalState && !pawn.IsFighting() && !pawn.Downed && !pawn.Dead && !ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn) && pawn.GetLord() == null && (pawn.Faction != Faction.OfPlayer || !pawn.Map.IsPlayerHome) && (pawn.Faction == null || pawn.Faction.def.animalsFleeDanger) && (pawn.CurJob == null || !pawn.CurJobDef.neverFleeFromEnemies) && (pawn.CurJob == null || pawn.jobs.curJob.def != JobDefOf.Flee || pawn.jobs.curJob.startTick != Find.TickManager.TicksGame);
		}

		// Token: 0x06004A5B RID: 19035 RVA: 0x001CE568 File Offset: 0x001CC768
		public static bool ShouldFleeFrom(Thing t, Pawn pawn, bool checkDistance, bool checkLOS)
		{
			if (t == pawn || (checkDistance && !t.Position.InHorDistOf(pawn.Position, 8f)))
			{
				return false;
			}
			if (t.def.alwaysFlee)
			{
				return true;
			}
			if (!t.HostileTo(pawn))
			{
				return false;
			}
			IAttackTarget attackTarget = t as IAttackTarget;
			return attackTarget != null && !attackTarget.ThreatDisabled(pawn) && t is IAttackTargetSearcher && (!checkLOS || GenSight.LineOfSight(pawn.Position, t.Position, pawn.Map));
		}

		// Token: 0x04002F86 RID: 12166
		private static int MinFiresNearbyRadius = 20;

		// Token: 0x04002F87 RID: 12167
		private static int MinFiresNearbyRegionsToScan = 18;

		// Token: 0x04002F88 RID: 12168
		public const float FleeWhenDistToHostileLessThan = 8f;

		// Token: 0x04002F89 RID: 12169
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
