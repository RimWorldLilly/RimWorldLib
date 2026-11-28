using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D8E RID: 3470
	public class ThinkNode_Priority_Learn : ThinkNode_Priority
	{
		// Token: 0x06005FAB RID: 24491 RVA: 0x0022D110 File Offset: 0x0022B310
		public override float GetPriority(Pawn pawn)
		{
			if (!ModsConfig.BiotechActive)
			{
				return 0f;
			}
			Pawn_TimetableTracker timetable = pawn.timetable;
			if (!(((timetable != null) ? timetable.CurrentAssignment : null) ?? TimeAssignmentDefOf.Anything).allowJoy)
			{
				return 0f;
			}
			if (pawn.learning == null)
			{
				return 0f;
			}
			if (Find.TickManager.TicksGame < 5000)
			{
				return 0f;
			}
			if (!pawn.DevelopmentalStage.Child())
			{
				return 0f;
			}
			if (LearningUtility.LearningSatisfied(pawn))
			{
				return 0f;
			}
			return 9.1f;
		}

		// Token: 0x040035D6 RID: 13782
		private const int GameStartNoLearningTicks = 5000;
	}
}
