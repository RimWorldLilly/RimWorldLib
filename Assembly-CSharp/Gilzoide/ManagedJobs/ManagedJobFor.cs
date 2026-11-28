using System;
using System.Runtime.InteropServices;
using Unity.Jobs;

namespace Gilzoide.ManagedJobs
{
	// Token: 0x02000024 RID: 36
	public struct ManagedJobFor : IJobFor, IDisposable
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000103 RID: 259 RVA: 0x0000918C File Offset: 0x0000738C
		public IJobFor Job
		{
			get
			{
				if (!this._managedJobGcHandle.IsAllocated)
				{
					return null;
				}
				return (IJobFor)this._managedJobGcHandle.Target;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000104 RID: 260 RVA: 0x000091AD File Offset: 0x000073AD
		public bool HasJob
		{
			get
			{
				return this.Job != null;
			}
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000091B8 File Offset: 0x000073B8
		public ManagedJobFor(IJobFor managedJob)
		{
			this._managedJobGcHandle = ((managedJob != null) ? GCHandle.Alloc(managedJob) : default(GCHandle));
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000091DF File Offset: 0x000073DF
		public void Execute(int index)
		{
			IJobFor job = this.Job;
			if (job == null)
			{
				return;
			}
			job.Execute(index);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000091F2 File Offset: 0x000073F2
		public void Dispose()
		{
			if (this._managedJobGcHandle.IsAllocated)
			{
				this._managedJobGcHandle.Free();
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000920C File Offset: 0x0000740C
		public JobHandle Schedule(int arrayLength, JobHandle dependsOn = default(JobHandle))
		{
			JobHandle jobHandle = this.Schedule(arrayLength, dependsOn);
			new DisposeJob<ManagedJobFor>(this).Schedule(jobHandle);
			return jobHandle;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000923C File Offset: 0x0000743C
		public JobHandle ScheduleParallel(int arrayLength, int innerloopBatchCount, JobHandle dependsOn = default(JobHandle))
		{
			JobHandle jobHandle = this.ScheduleParallel(arrayLength, innerloopBatchCount, dependsOn);
			new DisposeJob<ManagedJobFor>(this).Schedule(jobHandle);
			return jobHandle;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000926C File Offset: 0x0000746C
		public void Run(int arrayLength)
		{
			try
			{
				this.Run(arrayLength);
			}
			finally
			{
				this.Dispose();
			}
		}

		// Token: 0x0400004C RID: 76
		private GCHandle _managedJobGcHandle;
	}
}
