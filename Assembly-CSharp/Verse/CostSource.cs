using System;
using System.Collections.Generic;
using Unity.Collections;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000370 RID: 880
	public class CostSource : SimplePathFinderDataSource<int>
	{
		// Token: 0x06001AFA RID: 6906 RVA: 0x000A5FF5 File Offset: 0x000A41F5
		public CostSource(Map map, PathingContext context) : base(map)
		{
			this.context = context;
		}

		// Token: 0x06001AFB RID: 6907 RVA: 0x000A6005 File Offset: 0x000A4205
		public override void ComputeAll(IEnumerable<PathRequest> _)
		{
			this.data.CopyFrom(this.context.pathGrid.Grid_Unsafe);
		}

		// Token: 0x06001AFC RID: 6908 RVA: 0x000A6024 File Offset: 0x000A4224
		public override bool UpdateIncrementally(IEnumerable<PathRequest> _, List<IntVec3> cellDeltas)
		{
			CellIndices cellIndices = this.map.cellIndices;
			NativeArray<int> grid_Unsafe = this.context.pathGrid.Grid_Unsafe;
			foreach (IntVec3 c in cellDeltas)
			{
				int index = cellIndices.CellToIndex(c);
				this.data[index] = grid_Unsafe[index];
			}
			return false;
		}

		// Token: 0x0400162D RID: 5677
		protected readonly PathingContext context;
	}
}
