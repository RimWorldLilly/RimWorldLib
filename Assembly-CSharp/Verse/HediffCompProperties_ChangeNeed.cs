using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000415 RID: 1045
	public class HediffCompProperties_ChangeNeed : HediffCompProperties
	{
		// Token: 0x06001EC4 RID: 7876 RVA: 0x000BCAD3 File Offset: 0x000BACD3
		public HediffCompProperties_ChangeNeed()
		{
			this.compClass = typeof(HediffComp_ChangeNeed);
		}

		// Token: 0x04001828 RID: 6184
		public NeedDef needDef;

		// Token: 0x04001829 RID: 6185
		public float percentPerDay;
	}
}
