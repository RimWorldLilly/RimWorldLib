using System;
using System.Runtime.InteropServices;
using Unity.Jobs;

namespace Gilzoide.ManagedJobs
{
	// Token: 0x02000023 RID: 35
	public struct ManagedJob : IJob, IDisposable
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000FC RID: 252 RVA: 0x000090A5 File Offset: 0x000072A5
		public IJob Job
		{
			get
			{
				if (!this._managedJobGcHandle.IsAllocated)
				{
					return null;
				}
				return (IJob)this._managedJobGcHandle.Target;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000FD RID: 253 RVA: 0x000090C6 File Offset: 0x000072C6
		public bool HasJob
		{
			get
			{
				return this.Job != null;
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000090D4 File Offset: 0x000072D4
		public ManagedJob(IJob managedJob)
		{
			this._managedJobGcHandle = ((managedJob != null) ? GCHandle.Alloc(managedJob) : default(GCHandle));
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000090FB File Offset: 0x000072FB
		public void Execute()
		{
			IJob job = this.Job;
			if (job == null)
			{
				return;
			}
			job.Execute();
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000910D File Offset: 0x0000730D
		public void Dispose()
		{
			if (this._managedJobGcHandle.IsAllocated)
			{
				this._managedJobGcHandle.Free();
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00009128 File Offset: 0x00007328
		public JobHandle Schedule(JobHandle dependsOn = default(JobHandle))
		{
			JobHandle jobHandle = this.Schedule(dependsOn);
			new DisposeJob<ManagedJob>(this).Schedule(jobHandle);
			return jobHandle;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00009158 File Offset: 0x00007358
		public void Run()
		{
			try
			{
				this.Run<ManagedJob>();
			}
			finally
			{
				this.Dispose();
			}
		}

		// Token: 0x0400004B RID: 75
		private GCHandle _managedJobGcHandle;
	}
}
