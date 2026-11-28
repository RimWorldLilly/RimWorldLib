using System;
using System.Collections.Generic;
using System.Threading;

namespace Verse
{
	// Token: 0x020006BB RID: 1723
	public static class DeepProfiler
	{
		// Token: 0x060036B5 RID: 14005 RVA: 0x0015F2FC File Offset: 0x0015D4FC
		public static ThreadLocalDeepProfiler Get()
		{
			object deepProfilersLock = DeepProfiler.DeepProfilersLock;
			ThreadLocalDeepProfiler result;
			lock (deepProfilersLock)
			{
				int managedThreadId = Thread.CurrentThread.ManagedThreadId;
				ThreadLocalDeepProfiler threadLocalDeepProfiler;
				if (!DeepProfiler.deepProfilers.TryGetValue(managedThreadId, out threadLocalDeepProfiler))
				{
					threadLocalDeepProfiler = new ThreadLocalDeepProfiler();
					DeepProfiler.deepProfilers.Add(managedThreadId, threadLocalDeepProfiler);
					result = threadLocalDeepProfiler;
				}
				else
				{
					result = threadLocalDeepProfiler;
				}
			}
			return result;
		}

		// Token: 0x060036B6 RID: 14006 RVA: 0x0015F36C File Offset: 0x0015D56C
		public static void Start(string label = null)
		{
			if (!DeepProfiler.enabled || !Prefs.LogVerbose)
			{
				return;
			}
			DeepProfiler.Get().Start(label);
		}

		// Token: 0x060036B7 RID: 14007 RVA: 0x0015F38A File Offset: 0x0015D58A
		public static void End()
		{
			if (!DeepProfiler.enabled || !Prefs.LogVerbose)
			{
				return;
			}
			DeepProfiler.Get().End();
		}

		// Token: 0x040023FE RID: 9214
		public static volatile bool enabled = true;

		// Token: 0x040023FF RID: 9215
		private static Dictionary<int, ThreadLocalDeepProfiler> deepProfilers = new Dictionary<int, ThreadLocalDeepProfiler>();

		// Token: 0x04002400 RID: 9216
		private static readonly object DeepProfilersLock = new object();
	}
}
