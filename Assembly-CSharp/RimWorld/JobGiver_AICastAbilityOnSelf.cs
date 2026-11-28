using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ACE RID: 2766
	public class JobGiver_AICastAbilityOnSelf : JobGiver_AICastAbility
	{
		// Token: 0x060052F6 RID: 21238 RVA: 0x001EEA10 File Offset: 0x001ECC10
		protected override LocalTargetInfo GetTarget(Pawn caster, Ability ability)
		{
			LocalTargetInfo localTargetInfo = new LocalTargetInfo(caster);
			if (!ability.def.targetRequired || ability.CanApplyOn(localTargetInfo))
			{
				return localTargetInfo;
			}
			return LocalTargetInfo.Invalid;
		}
	}
}
