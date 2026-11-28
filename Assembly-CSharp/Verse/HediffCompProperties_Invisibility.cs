using System;

namespace Verse
{
	// Token: 0x0200044D RID: 1101
	public class HediffCompProperties_Invisibility : HediffCompProperties
	{
		// Token: 0x06001F7F RID: 8063 RVA: 0x000BF685 File Offset: 0x000BD885
		public HediffCompProperties_Invisibility()
		{
			this.compClass = typeof(HediffComp_Invisibility);
		}

		// Token: 0x04001893 RID: 6291
		public bool visibleToPlayer;

		// Token: 0x04001894 RID: 6292
		public int fadeDurationTicks;

		// Token: 0x04001895 RID: 6293
		public int recoverFromDisruptedTicks;

		// Token: 0x04001896 RID: 6294
		public bool affectedByDisruptor = true;
	}
}
