using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B95 RID: 2965
	public class LearningGiver_Workwatching : LearningGiver
	{
		// Token: 0x060055EA RID: 21994 RVA: 0x001FF733 File Offset: 0x001FD933
		public override bool CanDo(Pawn pawn)
		{
			return base.CanDo(pawn) && LearningGiver_Workwatching.AdultToWorkwatch(pawn) != null;
		}

		// Token: 0x060055EB RID: 21995 RVA: 0x001FF74C File Offset: 0x001FD94C
		public static Pawn AdultToWorkwatch(Pawn child)
		{
			Pawn pawn = null;
			float num = 0f;
			foreach (Pawn pawn2 in child.Map.mapPawns.FreeColonistsSpawned)
			{
				float lengthHorizontal = (pawn2.Position - child.Position).LengthHorizontal;
				if (lengthHorizontal <= 100f && !pawn2.DevelopmentalStage.Juvenile() && LearningGiver_Workwatching.ChildCanLearnFromAdultJob(child, pawn2) && !pawn2.IsForbidden(child) && child.CanReach(pawn2, PathEndMode.Touch, Danger.Some, false, false, TraverseMode.ByPawn) && (pawn == null || lengthHorizontal < num))
				{
					num = lengthHorizontal;
					pawn = pawn2;
				}
			}
			return pawn;
		}

		// Token: 0x060055EC RID: 21996 RVA: 0x001FF810 File Offset: 0x001FDA10
		public static bool ChildCanLearnFromAdultJob(Pawn child, Pawn adult)
		{
			Job curJob = adult.CurJob;
			SkillDef skillDef;
			if (curJob == null)
			{
				skillDef = null;
			}
			else
			{
				RecipeDef recipeDef = curJob.RecipeDef;
				skillDef = ((recipeDef != null) ? recipeDef.workSkill : null);
			}
			SkillDef skillDef2 = skillDef;
			if (skillDef2 != null)
			{
				return !child.skills.GetSkill(skillDef2).TotallyDisabled;
			}
			Job curJob2 = adult.CurJob;
			List<SkillDef> list;
			if (curJob2 == null)
			{
				list = null;
			}
			else
			{
				WorkGiverDef workGiverDef = curJob2.workGiverDef;
				if (workGiverDef == null)
				{
					list = null;
				}
				else
				{
					WorkTypeDef workType = workGiverDef.workType;
					list = ((workType != null) ? workType.relevantSkills : null);
				}
			}
			List<SkillDef> list2 = list;
			if (list2.NullOrEmpty<SkillDef>())
			{
				return false;
			}
			foreach (SkillDef skillDef3 in list2)
			{
				if (child.skills.GetSkill(skillDef3).TotallyDisabled)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060055ED RID: 21997 RVA: 0x001FF8E0 File Offset: 0x001FDAE0
		public override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2 = LearningGiver_Workwatching.AdultToWorkwatch(pawn);
			if (pawn2 == null)
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, pawn2);
		}

		// Token: 0x040032F9 RID: 13049
		private const float MaxDistanceToAdult = 100f;
	}
}
