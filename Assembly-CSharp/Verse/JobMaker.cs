using System;
using Verse.AI;

namespace Verse
{
	// Token: 0x020000E8 RID: 232
	public static class JobMaker
	{
		// Token: 0x06000873 RID: 2163 RVA: 0x000305B9 File Offset: 0x0002E7B9
		public static Job MakeJob()
		{
			Job job = SimplePool<Job>.Get();
			job.loadID = Find.UniqueIDsManager.GetNextJobID();
			return job;
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x000305D0 File Offset: 0x0002E7D0
		public static Job MakeJob(JobDef def)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			return job;
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x000305DE File Offset: 0x0002E7DE
		public static Job MakeJob(JobDef def, LocalTargetInfo targetA)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			job.targetA = targetA;
			return job;
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x000305F3 File Offset: 0x0002E7F3
		public static Job MakeJob(JobDef def, LocalTargetInfo targetA, LocalTargetInfo targetB)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			job.targetA = targetA;
			job.targetB = targetB;
			return job;
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x0003060F File Offset: 0x0002E80F
		public static Job MakeJob(JobDef def, LocalTargetInfo targetA, LocalTargetInfo targetB, LocalTargetInfo targetC)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			job.targetA = targetA;
			job.targetB = targetB;
			job.targetC = targetC;
			return job;
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x00030632 File Offset: 0x0002E832
		public static Job MakeJob(JobDef def, LocalTargetInfo targetA, int expiryInterval, bool checkOverrideOnExpiry = false)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			job.targetA = targetA;
			job.expiryInterval = expiryInterval;
			job.checkOverrideOnExpire = checkOverrideOnExpiry;
			return job;
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x00030655 File Offset: 0x0002E855
		public static Job MakeJob(JobDef def, int expiryInterval, bool checkOverrideOnExpiry = false)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			job.expiryInterval = expiryInterval;
			job.checkOverrideOnExpire = checkOverrideOnExpiry;
			return job;
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x00030671 File Offset: 0x0002E871
		public static void ReturnToPool(Job job)
		{
			if (job == null)
			{
				return;
			}
			if (SimplePool<Job>.FreeItemsCount >= 1000)
			{
				return;
			}
			job.Clear();
			SimplePool<Job>.Return(job);
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x00030690 File Offset: 0x0002E890
		public static Job WithCount(this Job job, int count)
		{
			job.count = count;
			return job;
		}

		// Token: 0x040003C9 RID: 969
		private const int MaxJobPoolSize = 1000;
	}
}
