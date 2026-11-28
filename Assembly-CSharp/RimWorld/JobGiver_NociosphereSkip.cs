using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B54 RID: 2900
	public class JobGiver_NociosphereSkip : ThinkNode_JobGiver
	{
		// Token: 0x060054C1 RID: 21697 RVA: 0x001F7C80 File Offset: 0x001F5E80
		protected override Job TryGiveJob(Pawn pawn)
		{
			AbilityDef entitySkip = AbilityDefOf.EntitySkip;
			Job curJob = pawn.CurJob;
			if (curJob != null && curJob.def == entitySkip.jobDef)
			{
				return curJob;
			}
			Ability ability = pawn.abilities.GetAbility(AbilityDefOf.EntitySkip, false);
			if (!ability.CanCast)
			{
				return null;
			}
			JobGiver_NociosphereSkip.targets.Clear();
			JobGiver_NociosphereSkip.targets.AddRange(pawn.Map.mapPawns.AllPawnsSpawned);
			JobGiver_NociosphereSkip.targets.SortBy((Pawn x) => x.Position.DistanceToSquared(pawn.Position));
			for (int i = JobGiver_NociosphereSkip.targets.Count - 1; i >= 0; i--)
			{
				if (JobGiver_NociosphereSkip.targets[i].DeadOrDowned || !pawn.HostileTo(JobGiver_NociosphereSkip.targets[i]))
				{
					JobGiver_NociosphereSkip.targets.RemoveAt(i);
				}
				else if (pawn.Position.InHorDistOf(JobGiver_NociosphereSkip.targets[i].Position, 6f))
				{
					JobGiver_NociosphereSkip.targets.RemoveAt(i);
				}
			}
			Pawn target;
			IntVec3 intVec;
			if (JobGiver_NociosphereSkip.targets.Empty<Pawn>() || !this.TryGetIdealTarget(out target))
			{
				if (!this.TryGetRandomPos(pawn, out intVec))
				{
					JobGiver_NociosphereSkip.targets.Clear();
					return null;
				}
			}
			else
			{
				intVec = target.Position;
				if (pawn.Position.InHorDistOf(target.Position, 19.9f) && CellFinder.TryFindRandomCellNear(target.Position, pawn.Map, Mathf.RoundToInt(19.9f), (IntVec3 c) => JobGiver_NociosphereSkip.IsValidTeleportCell(pawn, c) && GenSight.LineOfSightToThing(c, target, pawn.Map, false, null), out intVec, -1))
				{
					return ability.GetJob(pawn, intVec);
				}
				IntVec3 intVec2 = intVec - pawn.Position;
				intVec = pawn.Position + intVec2.ClampMagnitude(19.9f);
				float teleportRadius = JobGiver_NociosphereSkip.GetTeleportRadius(pawn.Position.DistanceTo(intVec), 30f);
				if (!CellFinder.TryFindRandomCellNear(intVec, pawn.Map, Mathf.RoundToInt(teleportRadius), (IntVec3 c) => JobGiver_NociosphereSkip.IsValidTeleportCell(pawn, c), out intVec, -1) && !this.TryGetRandomPos(pawn, out intVec))
				{
					JobGiver_NociosphereSkip.targets.Clear();
					return null;
				}
			}
			JobGiver_NociosphereSkip.targets.Clear();
			if (!JobGiver_NociosphereSkip.IsValidTeleportCell(pawn, intVec))
			{
				return null;
			}
			return ability.GetJob(pawn, intVec);
		}

		// Token: 0x060054C2 RID: 21698 RVA: 0x001F7F35 File Offset: 0x001F6135
		private bool TryGetIdealTarget(out Pawn target)
		{
			target = this.GetIdealTarget();
			return target != null;
		}

		// Token: 0x060054C3 RID: 21699 RVA: 0x001F7F44 File Offset: 0x001F6144
		private Pawn GetIdealTarget()
		{
			for (int i = 0; i < JobGiver_NociosphereSkip.targets.Count; i++)
			{
				Pawn pawn = JobGiver_NociosphereSkip.targets[i];
				if (!pawn.DeadOrDowned && (pawn.Faction != null || pawn.RaceProps.intelligence >= Intelligence.ToolUser))
				{
					return JobGiver_NociosphereSkip.targets[i];
				}
			}
			return null;
		}

		// Token: 0x060054C4 RID: 21700 RVA: 0x001F7FA0 File Offset: 0x001F61A0
		private bool TryGetRandomPos(Pawn pawn, out IntVec3 cell)
		{
			if (CellFinder.TryFindRandomCellNear(pawn.Position, pawn.Map, 14, (IntVec3 c) => JobGiver_NociosphereSkip.IsValidTeleportCell(pawn, c), out cell, -1))
			{
				return true;
			}
			cell = IntVec3.Invalid;
			return false;
		}

		// Token: 0x060054C5 RID: 21701 RVA: 0x001F7FF8 File Offset: 0x001F61F8
		private static bool IsValidTeleportCell(Pawn pawn, IntVec3 cell)
		{
			return !cell.Fogged(pawn.Map) && cell.Walkable(pawn.Map) && cell.InBounds(pawn.Map, 2) && pawn.Position.DistanceTo(cell) <= 19.9f && JobGiver_NociosphereSkip.AreaClearOfPawns(pawn, cell);
		}

		// Token: 0x060054C6 RID: 21702 RVA: 0x001F804C File Offset: 0x001F624C
		private static bool AreaClearOfPawns(Pawn pawn, IntVec3 cell)
		{
			int num = GenRadial.NumCellsInRadius(3f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 c = cell + GenRadial.RadialPattern[i];
				if (c.InBounds(pawn.Map) && c.GetFirstPawn(pawn.Map) != null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060054C7 RID: 21703 RVA: 0x001F80A1 File Offset: 0x001F62A1
		private static float GetTeleportRadius(float distance, float angleDegrees)
		{
			return distance * Mathf.Tan(angleDegrees / 2f * 0.017453292f);
		}

		// Token: 0x04003296 RID: 12950
		private const float TeleportAngleError = 30f;

		// Token: 0x04003297 RID: 12951
		private const int RandomJumpRadius = 14;

		// Token: 0x04003298 RID: 12952
		private const int PawnClearDistanceTiles = 3;

		// Token: 0x04003299 RID: 12953
		private const float MinJumpDist = 6f;

		// Token: 0x0400329A RID: 12954
		private const float MaxJumpDist = 19.9f;

		// Token: 0x0400329B RID: 12955
		private const Intelligence MinIntelligence = Intelligence.ToolUser;

		// Token: 0x0400329C RID: 12956
		private static readonly List<Pawn> targets = new List<Pawn>();
	}
}
