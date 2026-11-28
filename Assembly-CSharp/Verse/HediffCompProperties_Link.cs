using System;

namespace Verse
{
	// Token: 0x02000455 RID: 1109
	public class HediffCompProperties_Link : HediffCompProperties
	{
		// Token: 0x06001FA5 RID: 8101 RVA: 0x000BFFF9 File Offset: 0x000BE1F9
		public HediffCompProperties_Link()
		{
			this.compClass = typeof(HediffComp_Link);
		}

		// Token: 0x040018A2 RID: 6306
		public bool showName = true;

		// Token: 0x040018A3 RID: 6307
		public float maxDistance = -1f;

		// Token: 0x040018A4 RID: 6308
		public bool requireLinkOnOtherPawn = true;

		// Token: 0x040018A5 RID: 6309
		public ThingDef customMote;
	}
}
