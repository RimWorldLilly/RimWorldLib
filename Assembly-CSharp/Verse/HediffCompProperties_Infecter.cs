using System;

namespace Verse
{
	// Token: 0x0200044B RID: 1099
	public class HediffCompProperties_Infecter : HediffCompProperties
	{
		// Token: 0x06001F75 RID: 8053 RVA: 0x000BF270 File Offset: 0x000BD470
		public HediffCompProperties_Infecter()
		{
			this.compClass = typeof(HediffComp_Infecter);
		}

		// Token: 0x04001889 RID: 6281
		public float infectionChance = 0.5f;
	}
}
