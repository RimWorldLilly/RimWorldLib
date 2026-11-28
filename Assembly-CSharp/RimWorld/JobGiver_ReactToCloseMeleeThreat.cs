using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B5D RID: 2909
	public class JobGiver_ReactToCloseMeleeThreat : ThinkNode_JobGiver
	{
		// Token: 0x060054E9 RID: 21737 RVA: 0x001F9970 File Offset: 0x001F7B70
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn meleeThreat = pawn.mindState.meleeThreat;
			if (meleeThreat == null)
			{
				return null;
			}
			if (this.maxDistance > 0f && pawn.Position.DistanceTo(meleeThreat.Position) > this.maxDistance)
			{
				return null;
			}
			if (meleeThreat.IsPsychologicallyInvisible())
			{
				return null;
			}
			CompActivity comp = meleeThreat.GetComp<CompActivity>();
			if (comp != null && comp.IsDormant)
			{
				return null;
			}
			if (this.IsHunting(pawn, meleeThreat))
			{
				return null;
			}
			if (this.IsDueling(pawn, meleeThreat))
			{
				return null;
			}
			if (this.IsHateChanting(pawn))
			{
				return null;
			}
			if (pawn.IsAwokenCorpse)
			{
				return null;
			}
			if (MentalStateUtility.IsHavingMentalBreak(pawn))
			{
				return null;
			}
			if (PawnUtility.PlayerForcedJobNowOrSoon(pawn))
			{
				return null;
			}
			if (pawn.playerSettings != null && pawn.playerSettings.UsesConfigurableHostilityResponse && pawn.playerSettings.hostilityResponse != HostilityResponseMode.Attack)
			{
				return null;
			}
			if (!pawn.mindState.MeleeThreatStillThreat)
			{
				pawn.mindState.meleeThreat = null;
				return null;
			}
			if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, meleeThreat);
			job.maxNumMeleeAttacks = 1;
			job.expiryInterval = 200;
			job.reactingToMeleeThreat = true;
			return job;
		}

		// Token: 0x060054EA RID: 21738 RVA: 0x001F9A8C File Offset: 0x001F7C8C
		private bool IsDueling(Pawn pawn, Pawn other)
		{
			Lord lord = pawn.GetLord();
			LordJob_Ritual_Duel lordJob_Ritual_Duel = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual_Duel;
			return lordJob_Ritual_Duel != null && lordJob_Ritual_Duel.Opponent(pawn) == other;
		}

		// Token: 0x060054EB RID: 21739 RVA: 0x001F9AC0 File Offset: 0x001F7CC0
		private bool IsHunting(Pawn pawn, Pawn prey)
		{
			if (pawn.CurJob == null)
			{
				return false;
			}
			JobDriver_Hunt jobDriver_Hunt = pawn.jobs.curDriver as JobDriver_Hunt;
			if (jobDriver_Hunt != null)
			{
				return jobDriver_Hunt.Victim == prey;
			}
			JobDriver_PredatorHunt jobDriver_PredatorHunt = pawn.jobs.curDriver as JobDriver_PredatorHunt;
			return jobDriver_PredatorHunt != null && jobDriver_PredatorHunt.Prey == prey;
		}

		// Token: 0x060054EC RID: 21740 RVA: 0x001F9B14 File Offset: 0x001F7D14
		private bool IsHateChanting(Pawn pawn)
		{
			if (ModsConfig.AnomalyActive)
			{
				Lord lord = pawn.GetLord();
				return ((lord != null) ? lord.LordJob : null) is LordJob_HateChant;
			}
			return false;
		}

		// Token: 0x040032B2 RID: 12978
		private float maxDistance = -1f;

		// Token: 0x040032B3 RID: 12979
		private const int MaxMeleeChaseTicks = 200;
	}
}
