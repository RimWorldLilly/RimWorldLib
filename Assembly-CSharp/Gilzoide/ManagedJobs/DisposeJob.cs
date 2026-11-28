using System;
using Unity.Jobs;

namespace Gilzoide.ManagedJobs
{
	// Token: 0x02000022 RID: 34
	public struct DisposeJob<TDisposable> : IJob where TDisposable : struct, IDisposable
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00009071 File Offset: 0x00007271
		public readonly TDisposable Disposable { get; }

		// Token: 0x060000FA RID: 250 RVA: 0x00009079 File Offset: 0x00007279
		public DisposeJob(TDisposable disposable)
		{
			this.Disposable = disposable;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00009084 File Offset: 0x00007284
		public void Execute()
		{
			TDisposable disposable = this.Disposable;
			disposable.Dispose();
		}
	}
}
