using System;

namespace Verse
{
	// Token: 0x020006BC RID: 1724
	public class DeepProfilerScope : IDisposable
	{
		// Token: 0x060036B9 RID: 14009 RVA: 0x0015F3C5 File Offset: 0x0015D5C5
		public DeepProfilerScope(string label = null)
		{
			DeepProfiler.Start(label);
		}

		// Token: 0x060036BA RID: 14010 RVA: 0x0015F3D3 File Offset: 0x0015D5D3
		void IDisposable.Dispose()
		{
			DeepProfiler.End();
		}
	}
}
