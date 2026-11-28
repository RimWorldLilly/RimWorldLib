using System;

namespace Verse
{
	// Token: 0x02000592 RID: 1426
	[Flags]
	public enum ProjectileHitFlags
	{
		// Token: 0x04001D1E RID: 7454
		None = 0,
		// Token: 0x04001D1F RID: 7455
		IntendedTarget = 1,
		// Token: 0x04001D20 RID: 7456
		NonTargetPawns = 2,
		// Token: 0x04001D21 RID: 7457
		NonTargetWorld = 4,
		// Token: 0x04001D22 RID: 7458
		All = -1
	}
}
