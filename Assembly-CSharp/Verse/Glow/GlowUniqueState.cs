using System;
using LudeonTK;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Verse.Glow
{
	// Token: 0x020007B8 RID: 1976
	public struct GlowUniqueState : IDisposable
	{
		// Token: 0x06003F96 RID: 16278 RVA: 0x001974C8 File Offset: 0x001956C8
		public static GlowUniqueState AllocateNew()
		{
			GlowUniqueState result = new GlowUniqueState
			{
				queue = new UnsafeHeap<IntVec3, GlowCellComparer>(Allocator.Persistent, 324, default(GlowCellComparer)),
				area = new UnsafeList<GlowCell>(6561, Allocator.Persistent, NativeArrayOptions.UninitializedMemory),
				blockers = new UnsafeBitArray(8, Allocator.Persistent)
			};
			result.area.Resize(6561, NativeArrayOptions.ClearMemory);
			return result;
		}

		// Token: 0x06003F97 RID: 16279 RVA: 0x00197533 File Offset: 0x00195733
		public void PrepareComparer(ref GlowLight light)
		{
			this.queue.Comparator = new GlowCellComparer(this.area, ref light);
		}

		// Token: 0x06003F98 RID: 16280 RVA: 0x0019754C File Offset: 0x0019574C
		public void Clear()
		{
			NativeArrayUtility.MemClear<GlowCell>(this.area);
			this.queue.Clear();
			this.blockers.Clear();
		}

		// Token: 0x06003F99 RID: 16281 RVA: 0x0019756F File Offset: 0x0019576F
		public void Dispose()
		{
			ref this.area.EnsureDisposed<GlowCell>();
			ref this.queue.EnsureDisposed<IntVec3, GlowCellComparer>();
			ref this.blockers.EnsureDisposed();
		}

		// Token: 0x04002A21 RID: 10785
		[NativeDisableParallelForRestriction]
		public UnsafeHeap<IntVec3, GlowCellComparer> queue;

		// Token: 0x04002A22 RID: 10786
		[NativeDisableParallelForRestriction]
		public UnsafeList<GlowCell> area;

		// Token: 0x04002A23 RID: 10787
		[NativeDisableParallelForRestriction]
		public UnsafeBitArray blockers;
	}
}
