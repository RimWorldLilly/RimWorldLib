using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B2F RID: 2863
	public class JobGiver_AIAbilityFight : JobGiver_AIFightEnemy
	{
		// Token: 0x17000F05 RID: 3845
		// (get) Token: 0x06005445 RID: 21573 RVA: 0x001F56A5 File Offset: 0x001F38A5
		protected override bool OnlyUseAbilityVerbs
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000F06 RID: 3846
		// (get) Token: 0x06005446 RID: 21574 RVA: 0x001F56A8 File Offset: 0x001F38A8
		protected override bool OnlyUseRangedSearch
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005447 RID: 21575 RVA: 0x001F56AB File Offset: 0x001F38AB
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AIAbilityFight jobGiver_AIAbilityFight = (JobGiver_AIAbilityFight)base.DeepCopy(resolve);
			jobGiver_AIAbilityFight.ability = this.ability;
			jobGiver_AIAbilityFight.skipIfCantTargetNow = this.skipIfCantTargetNow;
			return jobGiver_AIAbilityFight;
		}

		// Token: 0x06005448 RID: 21576 RVA: 0x001F56D4 File Offset: 0x001F38D4
		protected override bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest, Verb verbToUse = null)
		{
			dest = IntVec3.Invalid;
			Thing enemyTarget = pawn.mindState.enemyTarget;
			Ability ability = pawn.abilities.GetAbility(this.ability, false);
			return CastPositionFinder.TryFindCastPosition(new CastPositionRequest
			{
				caster = pawn,
				target = enemyTarget,
				verb = ability.verb,
				maxRangeFromTarget = ability.verb.EffectiveRange,
				wantCoverFromTarget = false,
				preferredCastPosition = new IntVec3?(pawn.Position)
			}, out dest);
		}

		// Token: 0x06005449 RID: 21577 RVA: 0x001F5763 File Offset: 0x001F3963
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.abilities.GetAbility(this.ability, false).OnCooldown && this.skipIfCantTargetNow)
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x0600544A RID: 21578 RVA: 0x001F578F File Offset: 0x001F398F
		protected override bool ShouldLoseTarget(Pawn pawn)
		{
			return base.ShouldLoseTarget(pawn) || !this.CanTarget(pawn, pawn.mindState.enemyTarget);
		}

		// Token: 0x0600544B RID: 21579 RVA: 0x001F57B1 File Offset: 0x001F39B1
		protected override bool ExtraTargetValidator(Pawn pawn, Thing target)
		{
			return base.ExtraTargetValidator(pawn, target) && this.CanTarget(pawn, target);
		}

		// Token: 0x0600544C RID: 21580 RVA: 0x001F57C8 File Offset: 0x001F39C8
		private bool CanTarget(Pawn pawn, Thing target)
		{
			if (!this.ability.verbProperties.targetParams.CanTarget(target, null))
			{
				return false;
			}
			Ability ability = pawn.abilities.GetAbility(this.ability, false);
			return ability.CanApplyOn(target) && (!this.skipIfCantTargetNow || ability.AICanTargetNow(target));
		}

		// Token: 0x0400326C RID: 12908
		private AbilityDef ability;

		// Token: 0x0400326D RID: 12909
		private bool skipIfCantTargetNow = true;
	}
}
