using System;

namespace Verse.AI
{
	// Token: 0x02000816 RID: 2070
	[Flags]
	public enum JobCondition : byte
	{
		// Token: 0x04002B16 RID: 11030
		None = 0,
		// Token: 0x04002B17 RID: 11031
		Ongoing = 1,
		// Token: 0x04002B18 RID: 11032
		Succeeded = 2,
		// Token: 0x04002B19 RID: 11033
		Incompletable = 4,
		// Token: 0x04002B1A RID: 11034
		InterruptOptional = 8,
		// Token: 0x04002B1B RID: 11035
		InterruptForced = 16,
		// Token: 0x04002B1C RID: 11036
		QueuedNoLongerValid = 32,
		// Token: 0x04002B1D RID: 11037
		Errored = 64,
		// Token: 0x04002B1E RID: 11038
		ErroredPather = 128
	}
}
