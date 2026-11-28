using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001A1F RID: 6687
	public class CompTargetEffect_Resurrect : CompTargetEffect
	{
		// Token: 0x17001D70 RID: 7536
		// (get) Token: 0x0600A7FC RID: 43004 RVA: 0x003BBF5E File Offset: 0x003BA15E
		public CompProperties_TargetEffectResurrect Props
		{
			get
			{
				return (CompProperties_TargetEffectResurrect)this.props;
			}
		}

		// Token: 0x0600A7FD RID: 43005 RVA: 0x003BBF6C File Offset: 0x003BA16C
		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (!user.IsColonistPlayerControlled)
			{
				return;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Resurrect, target, this.parent);
			job.count = 1;
			job.playerForced = true;
			user.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}
	}
}
