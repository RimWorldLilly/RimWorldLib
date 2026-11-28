using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AD5 RID: 2773
	public class JobGiver_AIDefendPoint : JobGiver_AIFightEnemy
	{
		// Token: 0x0600530E RID: 21262 RVA: 0x001EEED4 File Offset: 0x001ED0D4
		protected override bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest, Verb verbToUse = null)
		{
			Thing enemyTarget = pawn.mindState.enemyTarget;
			Verb verb = verbToUse ?? pawn.TryGetAttackVerb(enemyTarget, !pawn.IsColonist, false);
			if (verb == null)
			{
				dest = IntVec3.Invalid;
				return false;
			}
			return CastPositionFinder.TryFindCastPosition(new CastPositionRequest
			{
				caster = pawn,
				target = enemyTarget,
				verb = verb,
				maxRangeFromTarget = 9999f,
				locus = (IntVec3)pawn.mindState.duty.focus,
				maxRangeFromLocus = pawn.mindState.duty.radius,
				wantCoverFromTarget = (verb.EffectiveRange > 7f)
			}, out dest);
		}
	}
}
