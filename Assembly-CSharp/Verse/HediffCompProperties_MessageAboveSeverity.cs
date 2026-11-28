using System;

namespace Verse
{
	// Token: 0x02000457 RID: 1111
	public class HediffCompProperties_MessageAboveSeverity : HediffCompProperties
	{
		// Token: 0x06001FAE RID: 8110 RVA: 0x000C0288 File Offset: 0x000BE488
		public HediffCompProperties_MessageAboveSeverity()
		{
			this.compClass = typeof(HediffComp_MessageAboveSeverity);
		}

		// Token: 0x040018A9 RID: 6313
		public float severity;

		// Token: 0x040018AA RID: 6314
		public MessageTypeDef messageType;

		// Token: 0x040018AB RID: 6315
		[MustTranslate]
		public string message;
	}
}
