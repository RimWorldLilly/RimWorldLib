using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AF6 RID: 2806
	public class JobGiver_AcceptRole : ThinkNode_JobGiver
	{
		// Token: 0x0600539F RID: 21407 RVA: 0x001F2124 File Offset: 0x001F0324
		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			if (duty == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.AcceptRole, pawn.Position, duty.spectateRect.CenterCell + new IntVec3(0, 0, -1));
			job.speechSoundMale = (this.soundDefMale ?? SoundDefOf.Speech_Leader_Male);
			job.speechSoundFemale = (this.soundDefFemale ?? SoundDefOf.Speech_Leader_Female);
			job.speechFaceSpectatorsIfPossible = true;
			return job;
		}

		// Token: 0x060053A0 RID: 21408 RVA: 0x001F21A5 File Offset: 0x001F03A5
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AcceptRole jobGiver_AcceptRole = (JobGiver_AcceptRole)base.DeepCopy(resolve);
			jobGiver_AcceptRole.soundDefMale = this.soundDefMale;
			jobGiver_AcceptRole.soundDefFemale = this.soundDefFemale;
			return jobGiver_AcceptRole;
		}

		// Token: 0x04003229 RID: 12841
		public SoundDef soundDefMale;

		// Token: 0x0400322A RID: 12842
		public SoundDef soundDefFemale;
	}
}
