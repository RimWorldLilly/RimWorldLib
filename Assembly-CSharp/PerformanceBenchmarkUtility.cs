using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

// Token: 0x0200001B RID: 27
public static class PerformanceBenchmarkUtility
{
	// Token: 0x060000D9 RID: 217 RVA: 0x000084E3 File Offset: 0x000066E3
	public static void StartBenchmark()
	{
		PerformanceBenchmarkUtility.startBenchmarkTime = Time.realtimeSinceStartup;
		PerformanceBenchmarkUtility.startBenchmarkTicks = Find.TickManager.TicksGame;
		PerformanceBenchmarkUtility.startBenchmarkFrames = Time.frameCount;
	}

	// Token: 0x060000DA RID: 218 RVA: 0x00008508 File Offset: 0x00006708
	public static void CheckBenchmark()
	{
		if (PerformanceBenchmarkUtility.startBenchmarkTime > 0f && PerformanceBenchmarkUtility.startBenchmarkTime + 30f < Time.realtimeSinceStartup)
		{
			float num = Time.realtimeSinceStartup - PerformanceBenchmarkUtility.startBenchmarkTime;
			int num2 = Time.frameCount - PerformanceBenchmarkUtility.startBenchmarkFrames;
			int num3 = Find.TickManager.TicksGame - PerformanceBenchmarkUtility.startBenchmarkTicks;
			string[] array = new string[6];
			int num4 = 0;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(84, 4);
			defaultInterpolatedStringHandler.AppendLiteral("Frames per second: ");
			defaultInterpolatedStringHandler.AppendFormatted<float>((float)num2 / num);
			defaultInterpolatedStringHandler.AppendLiteral("\n");
			defaultInterpolatedStringHandler.AppendLiteral("Ticks per second: ");
			defaultInterpolatedStringHandler.AppendFormatted<float>((float)num3 / num);
			defaultInterpolatedStringHandler.AppendLiteral("\n");
			defaultInterpolatedStringHandler.AppendLiteral("Ticks + Frames per second: ");
			defaultInterpolatedStringHandler.AppendFormatted<float>((float)(num3 + num2) / num);
			defaultInterpolatedStringHandler.AppendLiteral("\n");
			defaultInterpolatedStringHandler.AppendLiteral("Ticks / Frame: ");
			defaultInterpolatedStringHandler.AppendFormatted<float>((float)num3 / (float)num2);
			defaultInterpolatedStringHandler.AppendLiteral("\n\n");
			array[num4] = defaultInterpolatedStringHandler.ToStringAndClear();
			array[1] = "----RAW----\n";
			int num5 = 2;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(16, 1);
			defaultInterpolatedStringHandler2.AppendLiteral("Time elapsed: ");
			defaultInterpolatedStringHandler2.AppendFormatted<float>(num);
			defaultInterpolatedStringHandler2.AppendLiteral("s\n");
			array[num5] = defaultInterpolatedStringHandler2.ToStringAndClear();
			int num6 = 3;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(9, 1);
			defaultInterpolatedStringHandler3.AppendLiteral("Frames: ");
			defaultInterpolatedStringHandler3.AppendFormatted<int>(num2);
			defaultInterpolatedStringHandler3.AppendLiteral("\n");
			array[num6] = defaultInterpolatedStringHandler3.ToStringAndClear();
			int num7 = 4;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(14, 1);
			defaultInterpolatedStringHandler4.AppendLiteral("Game Ticks: ");
			defaultInterpolatedStringHandler4.AppendFormatted<int>(num3);
			defaultInterpolatedStringHandler4.AppendLiteral("\n\n");
			array[num7] = defaultInterpolatedStringHandler4.ToStringAndClear();
			int num8 = 5;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler5 = new DefaultInterpolatedStringHandler(376, 1);
			defaultInterpolatedStringHandler5.AppendLiteral("Note: Each frame the game tries to do <tickrate> ticks or as many ticks as it can before ");
			defaultInterpolatedStringHandler5.AppendFormatted<int>(Mathf.RoundToInt(45.454544f));
			defaultInterpolatedStringHandler5.AppendLiteral("ms elapses. This means that sometimes tickrate, not framerate will increase as performance improves if the game is consistently not completing <tickrate> ticks per frame. Framerate can increase if performance improves while the game is consistently completing <tickrate> ticks per frame.");
			array[num8] = defaultInterpolatedStringHandler5.ToStringAndClear();
			Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation(string.Concat(array), delegate()
			{
			}, false, null, WindowLayer.Dialog);
			PerformanceBenchmarkUtility.startBenchmarkTime = -1f;
			Find.WindowStack.Add(window);
		}
	}

	// Token: 0x0400003A RID: 58
	public const float BenchmarkSeconds = 30f;

	// Token: 0x0400003B RID: 59
	public static float startBenchmarkTime = -1f;

	// Token: 0x0400003C RID: 60
	public static int startBenchmarkFrames = -1;

	// Token: 0x0400003D RID: 61
	public static int startBenchmarkTicks = -1;
}
