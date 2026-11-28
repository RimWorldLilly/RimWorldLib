using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013D1 RID: 5073
	public struct PreCastAction
	{
		// Token: 0x04004F3E RID: 20286
		public Action<LocalTargetInfo, LocalTargetInfo> action;

		// Token: 0x04004F3F RID: 20287
		public int ticksAwayFromCast;
	}
}
