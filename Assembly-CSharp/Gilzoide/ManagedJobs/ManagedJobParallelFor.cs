using System;
using System.Runtime.InteropServices;
using Unity.Jobs;

namespace Gilzoide.ManagedJobs
{
	// Token: 0x02000025 RID: 37
	public struct ManagedJobParallelFor : IJobParallelFor, IDisposable
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600010B RID: 267 RVA: 0x000092A0 File Offset: 0x000074A0
		public IJobParallelFor Job
		{
			get
			{
				if (!this._managedJobGcHandle.IsAllocated)
				{
					return null;
				}
				return (IJobParallelFor)this._managedJobGcHandle.Target;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600010C RID: 268 RVA: 0x000092C1 File Offset: 0x000074C1
		public bool HasJob
		{
			get
			{
				return this.Job != null;
			}
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000092CC File Offset: 0x000074CC
		public ManagedJobParallelFor(IJobParallelFor managedJob)
		{
			this._managedJobGcHandle = ((managedJob != null) ? GCHandle.Alloc(managedJob) : default(GCHandle));
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000092F3 File Offset: 0x000074F3
		public void Execute(int index)
		{
			IJobParallelFor job = this.Job;
			if (job == null)
			{
				return;
			}
			job.Execute(index);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00009306 File Offset: 0x00007506
		public void Dispose()
		{
			if (this._managedJobGcHandle.IsAllocated)
			{
				this._managedJobGcHandle.Free();
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00009320 File Offset: 0x00007520
		public JobHandle Schedule(int arrayLength, int innerloopBatchCount, JobHandle dependsOn = default(JobHandle))
		{
			JobHandle jobHandle = this.Schedule(arrayLength, innerloopBatchCount, dependsOn);
			new DisposeJob<ManagedJobParallelFor>(this).Schedule(jobHandle);
			return jobHandle;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00009350 File Offset: 0x00007550
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

		// Token: 0x0400004D RID: 77
		private GCHandle _managedJobGcHandle;
	}
}
