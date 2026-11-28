using System;

namespace Verse
{
	// Token: 0x020002EC RID: 748
	public struct FloodFillRange
	{
		// Token: 0x060016C9 RID: 5833 RVA: 0x00088919 File Offset: 0x00086B19
		public FloodFillRange(int minX, int maxX, int y)
		{
			this.minX = minX;
			this.maxX = maxX;
			this.z = y;
		}

		// Token: 0x04001382 RID: 4994
		public int minX;

		// Token: 0x04001383 RID: 4995
		public int maxX;

		// Token: 0x04001384 RID: 4996
		public int z;
	}
}
