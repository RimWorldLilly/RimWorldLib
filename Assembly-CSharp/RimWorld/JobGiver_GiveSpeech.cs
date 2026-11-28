using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B06 RID: 2822
	public class JobGiver_GiveSpeech : ThinkNode_JobGiver
	{
		// Token: 0x060053CC RID: 21452 RVA: 0x001F3134 File Offset: 0x001F1334
		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			if (duty == null)
			{
				return null;
			}
			Building_Throne building_Throne = duty.focusSecond.Thing as Building_Throne;
			if (building_Throne == null || building_Throne.AssignedPawn != pawn)
			{
				return null;
			}
			if (!pawn.CanReach(building_Throne, PathEndMode.InteractionCell, Danger.None, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.GiveSpeech, duty.focusSecond);
			job.speechSoundMale = (this.soundDefMale ?? SoundDefOf.Speech_Leader_Male);
			job.speechSoundFemale = (this.soundDefFemale ?? SoundDefOf.Speech_Leader_Female);
			return job;
		}

		// Token: 0x060053CD RID: 21453 RVA: 0x001F31C0 File Offset: 0x001F13C0
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GiveSpeech jobGiver_GiveSpeech = (JobGiver_GiveSpeech)base.DeepCopy(resolve);
			jobGiver_GiveSpeech.soundDefMale = this.soundDefMale;
			jobGiver_GiveSpeech.soundDefFemale = this.soundDefFemale;
			return jobGiver_GiveSpeech;
		}

		// Token: 0x04003236 RID: 12854
		public SoundDef soundDefMale;

		// Token: 0x04003237 RID: 12855
		public SoundDef soundDefFemale;
	}
}
