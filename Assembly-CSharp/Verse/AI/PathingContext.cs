using System;

namespace Verse.AI
{
	// Token: 0x020008A4 RID: 2212
	public class PathingContext : IDisposable
	{
		// Token: 0x06004469 RID: 17513 RVA: 0x001ACAB4 File Offset: 0x001AACB4
		public PathingContext(Map map, PathGrid pathGrid)
		{
			this.map = map;
			this.pathGrid = pathGrid;
		}

		// Token: 0x0600446A RID: 17514 RVA: 0x001ACACA File Offset: 0x001AACCA
		public void Dispose()
		{
			this.pathGrid.Dispose();
		}

		// Token: 0x04002C4D RID: 11341
		public readonly Map map;

		// Token: 0x04002C4E RID: 11342
		public readonly PathGrid pathGrid;
	}
}
