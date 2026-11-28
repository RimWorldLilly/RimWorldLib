using System;

namespace Verse
{
	// Token: 0x02000087 RID: 135
	public struct ProfilerBlock : IDisposable
	{
		// Token: 0x06000613 RID: 1555 RVA: 0x0002419B File Offset: 0x0002239B
		public ProfilerBlock(string blockName)
		{
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x0002419D File Offset: 0x0002239D
		public void Dispose()
		{
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x0002419F File Offset: 0x0002239F
		public static ProfilerBlock Scope(string blockName)
		{
			return new ProfilerBlock(blockName);
		}
	}
}
