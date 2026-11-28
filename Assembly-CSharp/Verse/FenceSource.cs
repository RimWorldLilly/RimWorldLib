using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000373 RID: 883
	public class FenceSource : SimpleBoolPathFinderDataSource
	{
		// Token: 0x06001B0B RID: 6923 RVA: 0x000A6842 File Offset: 0x000A4A42
		public FenceSource(Map map) : base(map)
		{
		}

		// Token: 0x06001B0C RID: 6924 RVA: 0x000A684C File Offset: 0x000A4A4C
		public override void ComputeAll(IEnumerable<PathRequest> _)
		{
			this.data.Clear();
			CellIndices cellIndices = this.map.cellIndices;
			foreach (Thing thing in ((IEnumerable<Thing>)this.map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial)))
			{
				if (thing.def.IsFence)
				{
					int pos = cellIndices.CellToIndex(thing.Position);
					this.data.Set(pos, true);
				}
			}
		}

		// Token: 0x06001B0D RID: 6925 RVA: 0x000A68E0 File Offset: 0x000A4AE0
		public override bool UpdateIncrementally(IEnumerable<PathRequest> _, List<IntVec3> cellDeltas)
		{
			CellIndices cellIndices = this.map.cellIndices;
			Building[] innerArray = this.map.edificeGrid.InnerArray;
			foreach (IntVec3 c in cellDeltas)
			{
				int num = cellIndices.CellToIndex(c);
				int pos = num;
				Building building = innerArray[num];
				this.data.Set(pos, building != null && building.def.IsFence);
			}
			return false;
		}
	}
}
