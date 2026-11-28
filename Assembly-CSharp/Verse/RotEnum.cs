using System;

namespace Verse
{
	// Token: 0x0200006F RID: 111
	[Flags]
	public enum RotEnum : byte
	{
		// Token: 0x04000192 RID: 402
		North = 1,
		// Token: 0x04000193 RID: 403
		East = 2,
		// Token: 0x04000194 RID: 404
		South = 4,
		// Token: 0x04000195 RID: 405
		West = 8,
		// Token: 0x04000196 RID: 406
		All = 15
	}
}
