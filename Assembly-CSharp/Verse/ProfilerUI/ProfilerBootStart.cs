using System;
using Waylith.Profiling;

namespace Verse.ProfilerUI
{
	// Token: 0x02000796 RID: 1942
	[StaticConstructorOnStartup]
	internal static class ProfilerBootStart
	{
		// Token: 0x06003DD9 RID: 15833 RVA: 0x00189B82 File Offset: 0x00187D82
		static ProfilerBootStart()
		{
			if (!ProfilerPrefs.Current.startOnBoot)
			{
				return;
			}
			FrameProfiler.SetEnabled(true);
			ProfilerPrefs.Apply();
		}
	}
}
