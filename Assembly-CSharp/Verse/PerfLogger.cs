using System;
using System.Diagnostics;
using System.Text;

namespace Verse
{
	// Token: 0x0200070F RID: 1807
	public static class PerfLogger
	{
		// Token: 0x0600391F RID: 14623 RVA: 0x0016EAC9 File Offset: 0x0016CCC9
		public static void Reset()
		{
			PerfLogger.currentLog = null;
			PerfLogger.start = Stopwatch.GetTimestamp();
			PerfLogger.current = PerfLogger.start;
		}

		// Token: 0x06003920 RID: 14624 RVA: 0x0016EAE5 File Offset: 0x0016CCE5
		public static void Flush()
		{
			Log.Message((PerfLogger.currentLog != null) ? PerfLogger.currentLog.ToString() : "");
			PerfLogger.Reset();
		}

		// Token: 0x06003921 RID: 14625 RVA: 0x0016EB0C File Offset: 0x0016CD0C
		public static void Record(string label)
		{
			long timestamp = Stopwatch.GetTimestamp();
			if (PerfLogger.currentLog == null)
			{
				PerfLogger.currentLog = new StringBuilder();
			}
			PerfLogger.currentLog.AppendLine(string.Format("{0}: {3}{1} ({2})", new object[]
			{
				(timestamp - PerfLogger.start) * 1000L / Stopwatch.Frequency,
				label,
				(timestamp - PerfLogger.current) * 1000L / Stopwatch.Frequency,
				new string(' ', PerfLogger.indent * 2)
			}));
			PerfLogger.current = timestamp;
		}

		// Token: 0x06003922 RID: 14626 RVA: 0x0016EB9E File Offset: 0x0016CD9E
		public static void Indent()
		{
			PerfLogger.indent++;
		}

		// Token: 0x06003923 RID: 14627 RVA: 0x0016EBAC File Offset: 0x0016CDAC
		public static void Outdent()
		{
			PerfLogger.indent--;
		}

		// Token: 0x06003924 RID: 14628 RVA: 0x0016EBBA File Offset: 0x0016CDBA
		public static float Duration()
		{
			return (float)(Stopwatch.GetTimestamp() - PerfLogger.start) / (float)Stopwatch.Frequency;
		}

		// Token: 0x040025A1 RID: 9633
		public static StringBuilder currentLog = new StringBuilder();

		// Token: 0x040025A2 RID: 9634
		private static long start;

		// Token: 0x040025A3 RID: 9635
		private static long current;

		// Token: 0x040025A4 RID: 9636
		private static int indent;
	}
}
