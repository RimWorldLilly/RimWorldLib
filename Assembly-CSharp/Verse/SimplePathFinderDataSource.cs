using System;
using System.Collections.Generic;
using Unity.Collections;

namespace Verse
{
	// Token: 0x02000360 RID: 864
	public abstract class SimplePathFinderDataSource<T> : IPathFinderDataSource, IDisposable where T : struct
	{
		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06001A78 RID: 6776 RVA: 0x000A1C54 File Offset: 0x0009FE54
		public NativeArray<T>.ReadOnly Data
		{
			get
			{
				return this.data.AsReadOnly();
			}
		}

		// Token: 0x06001A79 RID: 6777 RVA: 0x000A1C61 File Offset: 0x0009FE61
		public SimplePathFinderDataSource(Map map)
		{
			this.map = map;
			this.cellCount = map.cellIndices.NumGridCells;
			this.data = new NativeArray<T>(this.cellCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06001A7A RID: 6778 RVA: 0x000A1C94 File Offset: 0x0009FE94
		public virtual void Dispose()
		{
			this.data.Dispose();
		}

		// Token: 0x06001A7B RID: 6779
		public abstract void ComputeAll(IEnumerable<PathRequest> requests);

		// Token: 0x06001A7C RID: 6780
		public abstract bool UpdateIncrementally(IEnumerable<PathRequest> requests, List<IntVec3> cellDeltas);

		// Token: 0x0400158E RID: 5518
		protected readonly Map map;

		// Token: 0x0400158F RID: 5519
		protected readonly int cellCount;

		// Token: 0x04001590 RID: 5520
		protected NativeArray<T> data;
	}
}
