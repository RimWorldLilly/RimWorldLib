using System;

namespace Verse
{
	// Token: 0x02000497 RID: 1175
	public class HediffCompProperties_SurgeryInspectable : HediffCompProperties
	{
		// Token: 0x0600208C RID: 8332 RVA: 0x000C3273 File Offset: 0x000C1473
		public HediffCompProperties_SurgeryInspectable()
		{
			this.compClass = typeof(HediffComp_SurgeryInspectable);
		}

		// Token: 0x0400192A RID: 6442
		[MustTranslate]
		public string surgicalDetectionDesc;

		// Token: 0x0400192B RID: 6443
		public bool preventLetterIfPreviouslyDetected;
	}
}
