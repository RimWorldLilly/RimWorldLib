using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B50 RID: 2896
	public class JobGiver_MeditateInBed : JobGiver_Meditate
	{
		// Token: 0x060054B5 RID: 21685 RVA: 0x001F794F File Offset: 0x001F5B4F
		protected override bool ValidatePawnState(Pawn pawn)
		{
			return pawn.CurrentBed() != null && pawn.Awake() && MeditationUtility.ShouldMeditateInBed(pawn);
		}

		// Token: 0x060054B6 RID: 21686 RVA: 0x001F796C File Offset: 0x001F5B6C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!MeditationUtility.CanMeditateNow(pawn))
			{
				return null;
			}
			LocalTargetInfo targetC = ModsConfig.RoyaltyActive ? MeditationUtility.BestFocusAt(pawn.Position, pawn) : LocalTargetInfo.Invalid;
			Job job = JobMaker.MakeJob(JobDefOf.Meditate, pawn.Position, pawn.InBed() ? pawn.CurrentBed() : new LocalTargetInfo(pawn.Position), targetC);
			job.ignoreJoyTimeAssignment = true;
			return job;
		}
	}
}
