using System;

namespace Verse
{
	// Token: 0x0200045F RID: 1119
	public class HediffCompProperties_MessageStageIncreased : HediffCompProperties
	{
		// Token: 0x06001FC3 RID: 8131 RVA: 0x000C06BD File Offset: 0x000BE8BD
		public HediffCompProperties_MessageStageIncreased()
		{
			this.compClass = typeof(HediffComp_MessageStageIncreased);
		}

		// Token: 0x040018B9 RID: 6329
		[MustTranslate]
		public string message;

		// Token: 0x040018BA RID: 6330
		public MessageTypeDef messageType;
	}
}
