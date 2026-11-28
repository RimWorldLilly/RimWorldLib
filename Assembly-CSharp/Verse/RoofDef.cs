using System;

namespace Verse
{
	// Token: 0x020001B0 RID: 432
	public class RoofDef : Def
	{
		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000C0E RID: 3086 RVA: 0x000402D0 File Offset: 0x0003E4D0
		public bool VanishOnCollapse
		{
			get
			{
				return !this.isThickRoof;
			}
		}

		// Token: 0x04000B64 RID: 2916
		public bool isNatural;

		// Token: 0x04000B65 RID: 2917
		public bool isThickRoof;

		// Token: 0x04000B66 RID: 2918
		public ThingDef collapseLeavingThingDef;

		// Token: 0x04000B67 RID: 2919
		public ThingDef filthLeaving;

		// Token: 0x04000B68 RID: 2920
		public SoundDef soundPunchThrough;

		// Token: 0x04000B69 RID: 2921
		public bool canCollapse = true;
	}
}
