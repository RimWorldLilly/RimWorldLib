using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020015E7 RID: 5607
	public static class TraitUtility
	{
		// Token: 0x06008B75 RID: 35701 RVA: 0x003294D0 File Offset: 0x003276D0
		public static void ApplySkillGainFromTrait(Pawn pawn, Trait trait)
		{
			List<SkillGain> skillGains = trait.CurrentData.skillGains;
			if (!trait.Suppressed && skillGains != null && pawn.skills != null)
			{
				foreach (SkillGain skillGain in skillGains)
				{
					SkillRecord skill = pawn.skills.GetSkill(skillGain.skill);
					if (skill != null && !skill.PermanentlyDisabled)
					{
						skill.Level = skill.GetLevel(false) + skillGain.amount;
					}
				}
			}
		}
	}
}
