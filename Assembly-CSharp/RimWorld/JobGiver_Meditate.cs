using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B4F RID: 2895
	public class JobGiver_Meditate : ThinkNode_JobGiver
	{
		// Token: 0x060054B1 RID: 21681 RVA: 0x001F7848 File Offset: 0x001F5A48
		public override float GetPriority(Pawn pawn)
		{
			Pawn_PsychicEntropyTracker psychicEntropy = pawn.psychicEntropy;
			bool flag = pawn.HasPsylink && psychicEntropy != null && psychicEntropy.CurrentPsyfocus < Mathf.Min(psychicEntropy.TargetPsyfocus, 0.95f) && psychicEntropy.PsychicSensitivity > float.Epsilon;
			if (!this.ValidatePawnState(pawn))
			{
				return 0f;
			}
			Pawn_TimetableTracker timetable = pawn.timetable;
			if (((timetable != null) ? timetable.CurrentAssignment : null) == TimeAssignmentDefOf.Meditate)
			{
				return 9f;
			}
			if (pawn.CurrentBed() == null)
			{
				Pawn_TimetableTracker timetable2 = pawn.timetable;
				if (((timetable2 != null) ? timetable2.CurrentAssignment : null) == TimeAssignmentDefOf.Anything && flag)
				{
					return 7.1f;
				}
			}
			else if (flag && pawn.health.hediffSet.PainTotal <= 0.3f && pawn.CurrentBed() != null)
			{
				return 6f;
			}
			return 0f;
		}

		// Token: 0x060054B2 RID: 21682 RVA: 0x001F7916 File Offset: 0x001F5B16
		protected virtual bool ValidatePawnState(Pawn pawn)
		{
			return pawn.CurrentBed() == null && !MeditationUtility.CanOnlyMeditateInBed(pawn);
		}

		// Token: 0x060054B3 RID: 21683 RVA: 0x001F792B File Offset: 0x001F5B2B
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return null;
			}
			if (!MeditationUtility.CanMeditateNow(pawn))
			{
				return null;
			}
			return MeditationUtility.GetMeditationJob(pawn, false);
		}
	}
}
