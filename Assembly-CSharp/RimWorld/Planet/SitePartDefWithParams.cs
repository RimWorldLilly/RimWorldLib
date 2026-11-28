using System;

namespace RimWorld.Planet
{
	// Token: 0x020023BE RID: 9150
	public class SitePartDefWithParams
	{
		// Token: 0x0600E43B RID: 58427 RVA: 0x0054D020 File Offset: 0x0054B220
		public SitePartDefWithParams(SitePartDef def, SitePartParams parms)
		{
			this.def = def;
			this.parms = parms;
		}

		// Token: 0x04009135 RID: 37173
		public SitePartDef def;

		// Token: 0x04009136 RID: 37174
		public SitePartParams parms;
	}
}
