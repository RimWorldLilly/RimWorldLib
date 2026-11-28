using System;

namespace Verse
{
	// Token: 0x0200044F RID: 1103
	public class HediffCompProperties_KillAfterDays : HediffCompProperties
	{
		// Token: 0x06001F94 RID: 8084 RVA: 0x000BFCB5 File Offset: 0x000BDEB5
		public HediffCompProperties_KillAfterDays()
		{
			this.compClass = typeof(HediffComp_KillAfterDays);
		}

		// Token: 0x0400189C RID: 6300
		public int days;
	}
}
