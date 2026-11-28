using System;
using System.Collections.Generic;
using Unity.Collections;

namespace Verse
{
	// Token: 0x02000361 RID: 865
	public abstract class SimpleBoolPathFinderDataSource : IPathFinderDataSource, IDisposable
	{
		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06001A7D RID: 6781 RVA: 0x000A1CA1 File Offset: 0x0009FEA1
		public NativeBitArray.ReadOnly Data
		{
			get
			{
				return this.data.AsReadOnly();
			}
		}

		// Token: 0x06001A7E RID: 6782 RVA: 0x000A1CAE File Offset: 0x0009FEAE
		public SimpleBoolPathFinderDataSource(Map map)
		{
			this.map = map;
			this.cellCount = map.cellIndices.NumGridCells;
			this.data = new NativeBitArray(this.cellCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x000A1CE6 File Offset: 0x0009FEE6
		public virtual void Dispose()
		{
			this.data.Dispose();
		}

		// Token: 0x06001A80 RID: 6784
		public abstract void ComputeAll(IEnumerable<PathRequest> requests);

		// Token: 0x06001A81 RID: 6785
		public abstract bool UpdateIncrementally(IEnumerable<PathRequest> requests, List<IntVec3> cellDeltas);

		// Token: 0x04001591 RID: 5521
		protected readonly Map map;

		// Token: 0x04001592 RID: 5522
		protected readonly int cellCount;

		// Token: 0x04001593 RID: 5523
		protected NativeBitArray data;
	}
}
