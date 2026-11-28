using System;
using LudeonTK;

namespace Verse.ProfilerUI
{
	// Token: 0x020007A8 RID: 1960
	public static class DebugActionsFrameProfiler
	{
		// Token: 0x06003F38 RID: 16184 RVA: 0x001948A1 File Offset: 0x00192AA1
		[DebugAction("General", "Frame profiler", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Playing)]
		private static void OpenFrameProfiler()
		{
			DebugActionsFrameProfiler.Toggle();
		}

		// Token: 0x06003F39 RID: 16185 RVA: 0x001948A8 File Offset: 0x00192AA8
		[DebugAction("General", "Frame profiler", false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Entry)]
		private static void OpenFrameProfilerEntry()
		{
			DebugActionsFrameProfiler.Toggle();
		}

		// Token: 0x06003F3A RID: 16186 RVA: 0x001948AF File Offset: 0x00192AAF
		private static void Toggle()
		{
			if (!Find.WindowStack.TryRemove(typeof(Window_FrameProfiler), true))
			{
				Find.WindowStack.Add(new Window_FrameProfiler());
			}
		}
	}
}
