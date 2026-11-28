using System;
using Unity.Collections;
using Verse;

namespace LudeonTK
{
	// Token: 0x0200005F RID: 95
	public static class ThreadResultPool
	{
		// Token: 0x06000299 RID: 665 RVA: 0x00013EE3 File Offset: 0x000120E3
		public static void EnsureReady()
		{
			ThreadResultPool.EnsureAllocated();
			ThreadResultPool.Reset();
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00013EF0 File Offset: 0x000120F0
		private static void EnsureAllocated()
		{
			if (ThreadResultPool.allocated)
			{
				return;
			}
			ThreadResultPool.allocated = true;
			UnityData.DisposeStatic += ThreadResultPool.Dispose;
			ThreadResultPool.closestResults = new NativeArray<int>(128, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			ThreadResultPool.closestSqrDists = new NativeArray<float>(128, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00013F40 File Offset: 0x00012140
		private static void Reset()
		{
			if (!ThreadResultPool.closestResults.IsCreated)
			{
				return;
			}
			for (int i = 0; i < ThreadResultPool.closestResults.Length; i++)
			{
				ThreadResultPool.closestResults[i] = -1;
				ThreadResultPool.closestSqrDists[i] = float.MaxValue;
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00013F8B File Offset: 0x0001218B
		private static void Dispose()
		{
			if (!ThreadResultPool.allocated)
			{
				return;
			}
			ThreadResultPool.closestResults.Dispose();
			ThreadResultPool.closestSqrDists.Dispose();
			ThreadResultPool.allocated = false;
		}

		// Token: 0x04000167 RID: 359
		public static NativeArray<int> closestResults;

		// Token: 0x04000168 RID: 360
		public static NativeArray<float> closestSqrDists;

		// Token: 0x04000169 RID: 361
		private static bool allocated;
	}
}
