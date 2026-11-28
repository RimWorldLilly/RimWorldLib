using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x0200175C RID: 5980
	public class RoleRequirement_MinSkillAny : RoleRequirement
	{
		// Token: 0x060094CE RID: 38094 RVA: 0x00363760 File Offset: 0x00361960
		public override string GetLabel(Precept_Role role)
		{
			if (this.labelCached == null)
			{
				if (this.skills.Count == 1)
				{
					this.labelCached = "RoleRequirementSkill".Translate() + ": " + RoleRequirement_MinSkillAny.<GetLabel>g__GetSkillStr|2_0(this.skills[0]);
				}
				else
				{
					this.labelCached = "RoleRequirementSkillAny".Translate() + ": " + (from s in this.skills
					select RoleRequirement_MinSkillAny.<GetLabel>g__GetSkillStr|2_0(s)).ToCommaList(false, false);
				}
			}
			return this.labelCached;
		}

		// Token: 0x060094CF RID: 38095 RVA: 0x00363818 File Offset: 0x00361A18
		public override bool Met(Pawn p, Precept_Role role)
		{
			foreach (SkillRequirement skillRequirement in this.skills)
			{
				if (p.skills.GetSkill(skillRequirement.skill).Level >= skillRequirement.minLevel)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060094D1 RID: 38097 RVA: 0x00363894 File Offset: 0x00361A94
		[CompilerGenerated]
		internal static string <GetLabel>g__GetSkillStr|2_0(SkillRequirement requirement)
		{
			return requirement.skill.LabelCap + " " + requirement.minLevel.ToString();
		}

		// Token: 0x040059A8 RID: 22952
		public List<SkillRequirement> skills;

		// Token: 0x040059A9 RID: 22953
		[NoTranslate]
		private string labelCached;
	}
}
