using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F13 RID: 3859
	public class SkillNeed_Direct : SkillNeed
	{
		// Token: 0x06006425 RID: 25637 RVA: 0x0023D590 File Offset: 0x0023B790
		public override float ValueFor(Pawn pawn)
		{
			if (pawn.skills == null)
			{
				return 1f;
			}
			int level = pawn.skills.GetSkill(this.skill).Level;
			if (this.valuesPerLevel.Count > level)
			{
				return this.valuesPerLevel[level];
			}
			if (this.valuesPerLevel.Count > 0)
			{
				return this.valuesPerLevel[this.valuesPerLevel.Count - 1];
			}
			return 1f;
		}

		// Token: 0x0400393D RID: 14653
		public List<float> valuesPerLevel = new List<float>();
	}
}
