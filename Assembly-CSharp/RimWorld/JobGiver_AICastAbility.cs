using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000ACD RID: 2765
	public abstract class JobGiver_AICastAbility : ThinkNode_JobGiver
	{
		// Token: 0x060052F2 RID: 21234 RVA: 0x001EE970 File Offset: 0x001ECB70
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job curJob = pawn.CurJob;
			AbilityDef abilityDef;
			if (curJob == null)
			{
				abilityDef = null;
			}
			else
			{
				Ability ability = curJob.ability;
				abilityDef = ((ability != null) ? ability.def : null);
			}
			if (abilityDef == this.ability)
			{
				return null;
			}
			Pawn_AbilityTracker abilities = pawn.abilities;
			Ability ability2 = (abilities != null) ? abilities.GetAbility(this.ability, false) : null;
			if (ability2 == null || !ability2.CanCast)
			{
				return null;
			}
			LocalTargetInfo target = this.GetTarget(pawn, ability2);
			if (!target.IsValid)
			{
				return null;
			}
			return ability2.GetJob(target, target);
		}

		// Token: 0x060052F3 RID: 21235
		protected abstract LocalTargetInfo GetTarget(Pawn caster, Ability ability);

		// Token: 0x060052F4 RID: 21236 RVA: 0x001EE9ED File Offset: 0x001ECBED
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AICastAbility jobGiver_AICastAbility = (JobGiver_AICastAbility)base.DeepCopy(resolve);
			jobGiver_AICastAbility.ability = this.ability;
			return jobGiver_AICastAbility;
		}

		// Token: 0x040031F1 RID: 12785
		protected AbilityDef ability;
	}
}
