using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000376 RID: 886
	public class PersistentDangerSource : SimpleBoolPathFinderDataSource
	{
		// Token: 0x06001B17 RID: 6935 RVA: 0x000A6C10 File Offset: 0x000A4E10
		public PersistentDangerSource(Map map) : base(map)
		{
		}

		// Token: 0x06001B18 RID: 6936 RVA: 0x000A6C1C File Offset: 0x000A4E1C
		public override void ComputeAll(IEnumerable<PathRequest> _)
		{
			this.data.Clear();
			for (int i = 0; i < this.cellCount; i++)
			{
				IntVec3 c = this.map.cellIndices.IndexToCell(i);
				TerrainDef terrain = c.GetTerrain(this.map);
				List<Thing> thingList = c.GetThingList(this.map);
				bool flag = false;
				for (int j = 0; j < thingList.Count; j++)
				{
					if (thingList[j].def.pathfinderDangerous && !(thingList[j] is Pawn))
					{
						flag = true;
						break;
					}
				}
				this.data.Set(i, terrain.dangerous || flag);
			}
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x000A6CC8 File Offset: 0x000A4EC8
		public override bool UpdateIncrementally(IEnumerable<PathRequest> requests, List<IntVec3> cellDeltas)
		{
			foreach (IntVec3 c in cellDeltas)
			{
				int pos = this.map.cellIndices.CellToIndex(c);
				List<Thing> thingList = c.GetThingList(this.map);
				bool flag = false;
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i].def.pathfinderDangerous && !(thingList[i] is Pawn))
					{
						flag = true;
						break;
					}
				}
				this.data.Set(pos, flag || c.GetTerrain(this.map).dangerous);
			}
			return false;
		}
	}
}
