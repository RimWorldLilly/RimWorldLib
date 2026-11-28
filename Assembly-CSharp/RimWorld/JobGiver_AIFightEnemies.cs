using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000ACB RID: 2763
	public class JobGiver_AIFightEnemies : JobGiver_AIFightEnemy
	{
		// Token: 0x060052EC RID: 21228 RVA: 0x001EE57C File Offset: 0x001EC77C
		protected override bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest, Verb verbToUse = null)
		{
			Thing enemyTarget = pawn.mindState.enemyTarget;
			bool allowManualCastWeapons = !pawn.IsColonist && !pawn.IsColonySubhuman;
			Verb verb = verbToUse ?? pawn.TryGetAttackVerb(enemyTarget, allowManualCastWeapons, this.allowTurrets);
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
				maxRangeFromTarget = verb.EffectiveRange,
				wantCoverFromTarget = (verb.EffectiveRange > 5f)
			}, out dest);
		}
	}
}
