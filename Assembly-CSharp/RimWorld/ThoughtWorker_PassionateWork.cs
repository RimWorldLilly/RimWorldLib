using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E45 RID: 3653
	public class ThoughtWorker_PassionateWork : ThoughtWorker
	{
		// Token: 0x06006181 RID: 24961 RVA: 0x00233280 File Offset: 0x00231480
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			JobDriver curDriver = p.jobs.curDriver;
			if (curDriver == null)
			{
				return ThoughtState.Inactive;
			}
			if (p.skills == null)
			{
				return ThoughtState.Inactive;
			}
			if (curDriver.ActiveSkill == null)
			{
				return ThoughtState.Inactive;
			}
			SkillRecord skill = p.skills.GetSkill(curDriver.ActiveSkill);
			if (skill == null)
			{
				return ThoughtState.Inactive;
			}
			if (skill.passion == Passion.Minor)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (skill.passion == Passion.Major)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			return ThoughtState.Inactive;
		}
	}
}
