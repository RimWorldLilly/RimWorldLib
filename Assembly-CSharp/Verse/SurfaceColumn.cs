using System;

namespace Verse
{
	// Token: 0x020006AA RID: 1706
	public struct SurfaceColumn
	{
		// Token: 0x0600361E RID: 13854 RVA: 0x0015D5F3 File Offset: 0x0015B7F3
		public SurfaceColumn(float x, SimpleCurve y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x0400232B RID: 9003
		public float x;

		// Token: 0x0400232C RID: 9004
		public SimpleCurve y;
	}
}
