using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000704 RID: 1796
	public class MapCellsInRandomOrder
	{
		// Token: 0x060038EE RID: 14574 RVA: 0x0016D7C8 File Offset: 0x0016B9C8
		public MapCellsInRandomOrder(Map map)
		{
			this.map = map;
		}

		// Token: 0x060038EF RID: 14575 RVA: 0x0016D7D7 File Offset: 0x0016B9D7
		public List<IntVec3> GetAll()
		{
			this.CreateListIfShould();
			return this.randomizedCells;
		}

		// Token: 0x060038F0 RID: 14576 RVA: 0x0016D7E5 File Offset: 0x0016B9E5
		public IntVec3 Get(int index)
		{
			this.CreateListIfShould();
			return this.randomizedCells[index];
		}

		// Token: 0x060038F1 RID: 14577 RVA: 0x0016D7FC File Offset: 0x0016B9FC
		private void CreateListIfShould()
		{
			if (this.randomizedCells != null)
			{
				return;
			}
			this.randomizedCells = new List<IntVec3>(this.map.Area);
			foreach (IntVec3 item in this.map.AllCells)
			{
				this.randomizedCells.Add(item);
			}
			Rand.PushState();
			Rand.Seed = (Find.World.info.Seed ^ this.map.Tile.GetHashCode());
			this.randomizedCells.Shuffle<IntVec3>();
			Rand.PopState();
		}

		// Token: 0x04002501 RID: 9473
		private Map map;

		// Token: 0x04002502 RID: 9474
		private List<IntVec3> randomizedCells;
	}
}
