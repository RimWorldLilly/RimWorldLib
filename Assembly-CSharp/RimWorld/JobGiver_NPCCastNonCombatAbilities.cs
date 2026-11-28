using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B55 RID: 2901
	public class JobGiver_NPCCastNonCombatAbilities : ThinkNode_JobGiver
	{
		// Token: 0x060054CA RID: 21706 RVA: 0x001F80CC File Offset: 0x001F62CC
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.abilities == null)
			{
				return null;
			}
			List<Ability> list = pawn.abilities.AICastableAbilities(pawn, false);
			if (list.NullOrEmpty<Ability>())
			{
				return null;
			}
			Ability ability = list.RandomElement<Ability>();
			VerbProperties verbProperties = ability.def.verbProperties;
			TargetingParameters targetingParameters = (verbProperties != null) ? verbProperties.targetParams : null;
			if (!ability.def.targetRequired || (targetingParameters != null && targetingParameters.canTargetSelf))
			{
				return ability.GetJob(pawn, pawn);
			}
			return null;
		}
	}
}
