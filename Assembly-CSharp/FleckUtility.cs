using System;
using System.Collections.Generic;
using Verse;

// Token: 0x0200001D RID: 29
public static class FleckUtility
{
	// Token: 0x060000DD RID: 221 RVA: 0x0000876D File Offset: 0x0000696D
	public static FleckParallelizationInfo GetParallelizationInfo()
	{
		if (FleckUtility.parallelizationInfosPool.Count == 0)
		{
			return new FleckParallelizationInfo();
		}
		return FleckUtility.parallelizationInfosPool.Pop<FleckParallelizationInfo>();
	}

	// Token: 0x060000DE RID: 222 RVA: 0x0000878B File Offset: 0x0000698B
	public static void ReturnParallelizationInfo(FleckParallelizationInfo info)
	{
		info.doneEvent.Reset();
		info.drawBatch.Flush(false);
		FleckUtility.parallelizationInfosPool.Add(info);
	}

	// Token: 0x04000043 RID: 67
	private static List<FleckParallelizationInfo> parallelizationInfosPool = new List<FleckParallelizationInfo>();
}
