using System;
using System.Threading;
using Verse;

// Token: 0x0200001C RID: 28
public class FleckParallelizationInfo
{
	// Token: 0x0400003E RID: 62
	public int startIndex;

	// Token: 0x0400003F RID: 63
	public int endIndex;

	// Token: 0x04000040 RID: 64
	public object data;

	// Token: 0x04000041 RID: 65
	public DrawBatch drawBatch = new DrawBatch();

	// Token: 0x04000042 RID: 66
	public ManualResetEvent doneEvent = new ManualResetEvent(false);
}
