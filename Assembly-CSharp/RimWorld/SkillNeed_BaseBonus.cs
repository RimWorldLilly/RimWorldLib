using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F12 RID: 3858
	public class SkillNeed_BaseBonus : SkillNeed
	{
		// Token: 0x06006420 RID: 25632 RVA: 0x0023D50C File Offset: 0x0023B70C
		public override float ValueFor(Pawn pawn)
		{
			if (pawn.skills == null)
			{
				return 1f;
			}
			int level = pawn.skills.GetSkill(this.skill).Level;
			return this.ValueAtLevel(level);
		}

		// Token: 0x06006421 RID: 25633 RVA: 0x0023D545 File Offset: 0x0023B745
		private float ValueAtLevel(int level)
		{
			return this.baseValue + this.bonusPerLevel * (float)level;
		}

		// Token: 0x06006422 RID: 25634 RVA: 0x0023D557 File Offset: 0x0023B757
		public override IEnumerable<string> ConfigErrors()
		{
			SkillNeed_BaseBonus.<ConfigErrors>d__4 <ConfigErrors>d__ = new SkillNeed_BaseBonus.<ConfigErrors>d__4(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0400393B RID: 14651
		private float baseValue = 0.5f;

		// Token: 0x0400393C RID: 14652
		private float bonusPerLevel = 0.05f;
	}
}
