using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AD0 RID: 2768
	public abstract class JobGiver_AIDefendPawn : JobGiver_AIFightEnemy
	{
		// Token: 0x060052FB RID: 21243 RVA: 0x001EEBFF File Offset: 0x001ECDFF
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AIDefendPawn jobGiver_AIDefendPawn = (JobGiver_AIDefendPawn)base.DeepCopy(resolve);
			jobGiver_AIDefendPawn.attackMeleeThreatEvenIfNotHostile = this.attackMeleeThreatEvenIfNotHostile;
			return jobGiver_AIDefendPawn;
		}

		// Token: 0x060052FC RID: 21244
		protected abstract Pawn GetDefendee(Pawn pawn);

		// Token: 0x060052FD RID: 21245 RVA: 0x001EEC1C File Offset: 0x001ECE1C
		protected override IntVec3 GetFlagPosition(Pawn pawn)
		{
			Pawn defendee = this.GetDefendee(pawn);
			if (defendee.Spawned || defendee.CarriedBy != null)
			{
				return defendee.PositionHeld;
			}
			return IntVec3.Invalid;
		}

		// Token: 0x060052FE RID: 21246 RVA: 0x001EEC50 File Offset: 0x001ECE50
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn defendee = this.GetDefendee(pawn);
			if (defendee == null)
			{
				Type type = base.GetType();
				Log.Error(((type != null) ? type.ToString() : null) + " has null defendee. pawn=" + pawn.ToStringSafe<Pawn>());
				return null;
			}
			Pawn carriedBy = defendee.CarriedBy;
			if (carriedBy != null)
			{
				if (!pawn.CanReach(carriedBy, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return null;
				}
			}
			else if (!defendee.Spawned || !pawn.CanReach(defendee, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x060052FF RID: 21247 RVA: 0x001EECD4 File Offset: 0x001ECED4
		protected override Thing FindAttackTarget(Pawn pawn)
		{
			if (this.attackMeleeThreatEvenIfNotHostile)
			{
				Pawn defendee = this.GetDefendee(pawn);
				if (defendee.Spawned && !defendee.InMentalState && defendee.mindState.meleeThreat != null && defendee.mindState.meleeThreat != pawn && defendee.mindState.MeleeThreatStillThreat && pawn.CanReach(defendee.mindState.meleeThreat, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return defendee.mindState.meleeThreat;
				}
			}
			return base.FindAttackTarget(pawn);
		}

		// Token: 0x06005300 RID: 21248 RVA: 0x001EED5C File Offset: 0x001ECF5C
		protected override bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest, Verb verbToUse = null)
		{
			Verb verb = verbToUse ?? pawn.TryGetAttackVerb(null, !pawn.IsColonist, false);
			if (verb == null)
			{
				dest = IntVec3.Invalid;
				return false;
			}
			return CastPositionFinder.TryFindCastPosition(new CastPositionRequest
			{
				caster = pawn,
				target = pawn.mindState.enemyTarget,
				verb = verb,
				maxRangeFromTarget = 9999f,
				locus = this.GetDefendee(pawn).PositionHeld,
				maxRangeFromLocus = this.GetFlagRadius(pawn),
				wantCoverFromTarget = (verb.EffectiveRange > 7f),
				maxRegions = 50
			}, out dest);
		}

		// Token: 0x040031F6 RID: 12790
		private bool attackMeleeThreatEvenIfNotHostile;
	}
}
