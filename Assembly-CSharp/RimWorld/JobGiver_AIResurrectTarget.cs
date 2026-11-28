using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000ADF RID: 2783
	public class JobGiver_AIResurrectTarget : ThinkNode_JobGiver
	{
		// Token: 0x06005345 RID: 21317 RVA: 0x001EFD7C File Offset: 0x001EDF7C
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			using (new ProfilerBlock("JobGiver_AIResurrectTarget.TryGiveJob"))
			{
				Ability ability = pawn.abilities.GetAbility(this.abilityDef, false);
				if (ability == null || !ability.CanCast)
				{
					result = null;
				}
				else if (!pawn.Spawned)
				{
					result = null;
				}
				else
				{
					this.UpdateResurrectTarget(pawn);
					if (pawn.mindState.resurrectTarget == null)
					{
						result = null;
					}
					else
					{
						Job job = ability.GetJob(pawn.mindState.resurrectTarget.corpse, pawn.mindState.resurrectTarget.castPosition);
						job.expiryInterval = this.expiryInterval;
						result = job;
					}
				}
			}
			return result;
		}

		// Token: 0x06005346 RID: 21318 RVA: 0x001EFE40 File Offset: 0x001EE040
		private static HashSet<Corpse> ReservedCorpsesForResurrection(Map map, Faction faction)
		{
			HashSet<Corpse> result;
			using (new ProfilerBlock("ReservedCorpsesForResurrection"))
			{
				JobGiver_AIResurrectTarget.tmpReservedCorpses.Clear();
				List<Pawn> list = map.mapPawns.SpawnedPawnsInFaction(faction);
				for (int i = 0; i < list.Count; i++)
				{
					Pawn_MindState mindState = list[i].mindState;
					Corpse corpse;
					if (mindState == null)
					{
						corpse = null;
					}
					else
					{
						ResurrectCorpseData resurrectTarget = mindState.resurrectTarget;
						corpse = ((resurrectTarget != null) ? resurrectTarget.corpse : null);
					}
					Corpse corpse2 = corpse;
					if (corpse2 != null)
					{
						JobGiver_AIResurrectTarget.tmpReservedCorpses.Add(corpse2);
					}
				}
				result = JobGiver_AIResurrectTarget.tmpReservedCorpses;
			}
			return result;
		}

		// Token: 0x06005347 RID: 21319 RVA: 0x001EFEE0 File Offset: 0x001EE0E0
		private void UpdateResurrectTarget(Pawn pawn)
		{
			pawn.mindState.resurrectTarget = null;
			Ability ability = pawn.abilities.GetAbility(this.abilityDef, false);
			List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse);
			list.SortBy((Thing c) => c.Position.DistanceToSquared(pawn.Position));
			HashSet<Corpse> hashSet = JobGiver_AIResurrectTarget.ReservedCorpsesForResurrection(pawn.Map, pawn.Faction);
			for (int i = 0; i < list.Count; i++)
			{
				Corpse corpse = (Corpse)list[i];
				if (!hashSet.Contains(corpse) && JobGiver_AIResurrectTarget.ShouldResurrectCorpse(corpse, pawn) && ability.CanApplyOn(new LocalTargetInfo(corpse)))
				{
					using (new ProfilerBlock("TryFindCastPosition"))
					{
						IntVec3 castPosition;
						if (CastPositionFinder.TryFindCastPosition(new CastPositionRequest
						{
							caster = pawn,
							target = corpse,
							verb = ability.verb,
							maxRangeFromTarget = ability.verb.EffectiveRange,
							wantCoverFromTarget = false,
							maxRegions = this.maxRegions
						}, out castPosition))
						{
							pawn.mindState.resurrectTarget = new ResurrectCorpseData(corpse, castPosition);
							break;
						}
					}
				}
			}
		}

		// Token: 0x06005348 RID: 21320 RVA: 0x001F0068 File Offset: 0x001EE268
		public static bool ShouldResurrectCorpse(Corpse corpse, Pawn pawn)
		{
			return corpse != null && corpse.Spawned && corpse.Map == pawn.Map && corpse.InnerPawn.Faction == pawn.Faction && pawn.CanReserve(corpse, 1, -1, null, false) && !corpse.InnerPawn.kindDef.abilities.NotNullAndContains(AbilityDefOf.ResurrectionMech);
		}

		// Token: 0x06005349 RID: 21321 RVA: 0x001F00D2 File Offset: 0x001EE2D2
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AIResurrectTarget jobGiver_AIResurrectTarget = (JobGiver_AIResurrectTarget)base.DeepCopy(resolve);
			jobGiver_AIResurrectTarget.abilityDef = this.abilityDef;
			jobGiver_AIResurrectTarget.expiryInterval = this.expiryInterval;
			jobGiver_AIResurrectTarget.maxRegions = this.maxRegions;
			return jobGiver_AIResurrectTarget;
		}

		// Token: 0x0400320B RID: 12811
		private AbilityDef abilityDef;

		// Token: 0x0400320C RID: 12812
		private int expiryInterval = 500;

		// Token: 0x0400320D RID: 12813
		private int maxRegions = 50;

		// Token: 0x0400320E RID: 12814
		private static readonly HashSet<Corpse> tmpReservedCorpses = new HashSet<Corpse>();
	}
}
