using System;
using System.Collections.Generic;
using LudeonTK;
using Unity.Collections;

namespace Verse
{
	// Token: 0x02000374 RID: 884
	public class FogSource : SimpleBoolPathFinderDataSource
	{
		// Token: 0x06001B0E RID: 6926 RVA: 0x000A6978 File Offset: 0x000A4B78
		public FogSource(Map map) : base(map)
		{
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x000A6981 File Offset: 0x000A4B81
		public override void ComputeAll(IEnumerable<PathRequest> _)
		{
			this.data.CopyFrom(this.map.fogGrid.FogGrid_Unsafe);
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x000A69A0 File Offset: 0x000A4BA0
		public override bool UpdateIncrementally(IEnumerable<PathRequest> _, List<IntVec3> cellDeltas)
		{
			CellIndices cellIndices = this.map.cellIndices;
			NativeBitArray fogGrid_Unsafe = this.map.fogGrid.FogGrid_Unsafe;
			foreach (IntVec3 c in cellDeltas)
			{
				int pos = cellIndices.CellToIndex(c);
				this.data.Set(pos, fogGrid_Unsafe.IsSet(pos));
			}
			return false;
		}
	}
}
