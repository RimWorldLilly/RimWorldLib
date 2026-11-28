using System;

namespace Verse
{
	// Token: 0x02000432 RID: 1074
	public class HediffCompProperties_GetsPermanent : HediffCompProperties
	{
		// Token: 0x06001F2E RID: 7982 RVA: 0x000BDFFC File Offset: 0x000BC1FC
		public HediffCompProperties_GetsPermanent()
		{
			this.compClass = typeof(HediffComp_GetsPermanent);
		}

		// Token: 0x04001857 RID: 6231
		public float becomePermanentChanceFactor = 1f;

		// Token: 0x04001858 RID: 6232
		[MustTranslate]
		public string permanentLabel;

		// Token: 0x04001859 RID: 6233
		[MustTranslate]
		public string instantlyPermanentLabel;
	}
}
