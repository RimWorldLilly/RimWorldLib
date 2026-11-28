using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Verse
{
	// Token: 0x020006C0 RID: 1728
	public static class ProfilerPairValidation
	{
		// Token: 0x060036D5 RID: 14037 RVA: 0x0015FE5D File Offset: 0x0015E05D
		public static void BeginSample(string token)
		{
			ProfilerPairValidation.profilerSignatures.Push(new StackTrace(1, true));
		}

		// Token: 0x060036D6 RID: 14038 RVA: 0x0015FE70 File Offset: 0x0015E070
		public static void EndSample()
		{
			StackTrace stackTrace = ProfilerPairValidation.profilerSignatures.Pop();
			StackTrace stackTrace2 = new StackTrace(1, true);
			if (stackTrace2.FrameCount != stackTrace.FrameCount)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(12, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Mismatch:\n");
				defaultInterpolatedStringHandler.AppendFormatted<StackTrace>(stackTrace);
				defaultInterpolatedStringHandler.AppendLiteral("\n\n");
				defaultInterpolatedStringHandler.AppendFormatted<StackTrace>(stackTrace2);
				Log.Message(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			for (int i = 0; i < stackTrace2.FrameCount; i++)
			{
				if (stackTrace2.GetFrame(i).GetMethod() != stackTrace.GetFrame(i).GetMethod() && (!(stackTrace.GetFrame(i).GetMethod().DeclaringType == typeof(ProfilerThreadCheck)) || !(stackTrace2.GetFrame(i).GetMethod().DeclaringType == typeof(ProfilerThreadCheck))) && (!(stackTrace.GetFrame(i).GetMethod() == typeof(PathFinder).GetMethod("PfProfilerBeginSample", BindingFlags.Instance | BindingFlags.NonPublic)) || !(stackTrace2.GetFrame(i).GetMethod() == typeof(PathFinder).GetMethod("PfProfilerEndSample", BindingFlags.Instance | BindingFlags.NonPublic))))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(12, 2);
					defaultInterpolatedStringHandler2.AppendLiteral("Mismatch:\n");
					defaultInterpolatedStringHandler2.AppendFormatted<StackTrace>(stackTrace);
					defaultInterpolatedStringHandler2.AppendLiteral("\n\n");
					defaultInterpolatedStringHandler2.AppendFormatted<StackTrace>(stackTrace2);
					Log.Message(defaultInterpolatedStringHandler2.ToStringAndClear());
					return;
				}
			}
		}

		// Token: 0x0400240E RID: 9230
		public static Stack<StackTrace> profilerSignatures = new Stack<StackTrace>();
	}
}
