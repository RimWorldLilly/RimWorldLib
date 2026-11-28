using System;

namespace Verse
{
	// Token: 0x02000410 RID: 1040
	public class HediffCompProperties_ChanceToRemove : HediffCompProperties
	{
		// Token: 0x06001EB5 RID: 7861 RVA: 0x000BC7D2 File Offset: 0x000BA9D2
		public HediffCompProperties_ChanceToRemove()
		{
			this.compClass = typeof(HediffComp_ChanceToRemove);
		}

		// Token: 0x0400181E RID: 6174
		public int intervalTicks;

		// Token: 0x0400181F RID: 6175
		public float chance;
	}
}
