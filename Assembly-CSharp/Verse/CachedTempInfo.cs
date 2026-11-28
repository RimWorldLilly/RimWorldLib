using System;

namespace Verse
{
	// Token: 0x02000318 RID: 792
	public struct CachedTempInfo
	{
		// Token: 0x06001864 RID: 6244 RVA: 0x00092934 File Offset: 0x00090B34
		public static CachedTempInfo NewCachedTempInfo()
		{
			CachedTempInfo result = default(CachedTempInfo);
			result.Reset();
			return result;
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x00092951 File Offset: 0x00090B51
		public void Reset()
		{
			this.roomID = -1;
			this.numCells = 0;
			this.temperature = 0f;
			this.vacuum = 0f;
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x00092977 File Offset: 0x00090B77
		public CachedTempInfo(int roomID, int numCells, float temperature, float vacuum)
		{
			this.roomID = roomID;
			this.numCells = numCells;
			this.temperature = temperature;
			this.vacuum = vacuum;
		}

		// Token: 0x04001457 RID: 5207
		public int roomID;

		// Token: 0x04001458 RID: 5208
		public int numCells;

		// Token: 0x04001459 RID: 5209
		public float temperature;

		// Token: 0x0400145A RID: 5210
		public float vacuum;
	}
}
