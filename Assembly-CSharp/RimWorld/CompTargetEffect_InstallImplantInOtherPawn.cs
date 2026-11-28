using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001A15 RID: 6677
	public class CompTargetEffect_InstallImplantInOtherPawn : CompTargetEffect
	{
		// Token: 0x17001D6D RID: 7533
		// (get) Token: 0x0600A7E6 RID: 42982 RVA: 0x003BBC16 File Offset: 0x003B9E16
		public CompProperties_TargetEffectInstallImplantInOtherPawn Props
		{
			get
			{
				return (CompProperties_TargetEffectInstallImplantInOtherPawn)this.props;
			}
		}

		// Token: 0x0600A7E7 RID: 42983 RVA: 0x003BBC24 File Offset: 0x003B9E24
		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (!user.IsColonistPlayerControlled)
			{
				return;
			}
			Job job = JobMaker.MakeJob(JobDefOf.InstallImplant, target, this.parent);
			job.count = 1;
			job.playerForced = true;
			user.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}
	}
}
