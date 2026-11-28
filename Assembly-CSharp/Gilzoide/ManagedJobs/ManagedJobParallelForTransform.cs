using System;
using System.Runtime.InteropServices;
using Unity.Jobs;
using UnityEngine.Jobs;

namespace Gilzoide.ManagedJobs
{
	// Token: 0x02000026 RID: 38
	public struct ManagedJobParallelForTransform : IJobParallelForTransform, IDisposable
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00009384 File Offset: 0x00007584
		public IJobParallelForTransform Job
		{
			get
			{
				if (!this._managedJobGcHandle.IsAllocated)
				{
					return null;
				}
				return (IJobParallelForTransform)this._managedJobGcHandle.Target;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000113 RID: 275 RVA: 0x000093A5 File Offset: 0x000075A5
		public bool HasJob
		{
			get
			{
				return this.Job != null;
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x000093B0 File Offset: 0x000075B0
		public ManagedJobParallelForTransform(IJobParallelForTransform managedJob)
		{
			this._managedJobGcHandle = ((managedJob != null) ? GCHandle.Alloc(managedJob) : default(GCHandle));
		}

		// Token: 0x06000115 RID: 277 RVA: 0x000093D7 File Offset: 0x000075D7
		public void Execute(int index, TransformAccess transform)
		{
			IJobParallelForTransform job = this.Job;
			if (job == null)
			{
				return;
			}
			job.Execute(index, transform);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000093EB File Offset: 0x000075EB
		public void Dispose()
		{
			if (this._managedJobGcHandle.IsAllocated)
			{
				this._managedJobGcHandle.Free();
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00009408 File Offset: 0x00007608
		public JobHandle Schedule(TransformAccessArray transforms, JobHandle dependsOn = default(JobHandle))
		{
			JobHandle jobHandle = this.Schedule(transforms, dependsOn);
			new DisposeJob<ManagedJobParallelForTransform>(this).Schedule(jobHandle);
			return jobHandle;
		}

		// Token: 0x0400004E RID: 78
		private GCHandle _managedJobGcHandle;
	}
}
