using System;
using System.Diagnostics;

namespace Verse
{
	// Token: 0x02000722 RID: 1826
	public static class ProfilerThreadCheck
	{
		// Token: 0x060039BE RID: 14782 RVA: 0x0016FEC7 File Offset: 0x0016E0C7
		[Conditional("UNITY_EDITOR")]
		[Conditional("BUILD_AND_RUN")]
		public static void BeginSample(string name)
		{
			bool isInMainThread = UnityData.IsInMainThread;
		}

		// Token: 0x060039BF RID: 14783 RVA: 0x0016FECF File Offset: 0x0016E0CF
		[Conditional("UNITY_EDITOR")]
		[Conditional("BUILD_AND_RUN")]
		public static void EndSample()
		{
			bool isInMainThread = UnityData.IsInMainThread;
		}
	}
}
