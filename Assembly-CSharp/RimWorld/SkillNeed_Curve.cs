using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F14 RID: 3860
	public class SkillNeed_Curve : SkillNeed
	{
		// Token: 0x06006427 RID: 25639 RVA: 0x0023D61C File Offset: 0x0023B81C
		public override float ValueFor(Pawn pawn)
		{
			if (pawn.skills == null)
			{
				return this.curve.MinY;
			}
			return this.curve.Evaluate((float)pawn.skills.GetSkill(this.skill).Level);
		}

		// Token: 0x0400393E RID: 14654
		public SimpleCurve curve;
	}
}
