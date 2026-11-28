using System;

namespace Verse
{
	// Token: 0x020000A7 RID: 167
	public struct RandBlock : IDisposable
	{
		// Token: 0x060006F6 RID: 1782 RVA: 0x00027DDD File Offset: 0x00025FDD
		public RandBlock(int seed)
		{
			Rand.PushState(seed);
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x00027DE5 File Offset: 0x00025FE5
		public void Dispose()
		{
			Rand.PopState();
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x00027DEC File Offset: 0x00025FEC
		public static RandBlock Seed(int seed)
		{
			return new RandBlock(seed);
		}
	}
}
