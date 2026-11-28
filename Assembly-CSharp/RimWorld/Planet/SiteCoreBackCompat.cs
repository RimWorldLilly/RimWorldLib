using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023BC RID: 9148
	public class SiteCoreBackCompat : IExposable
	{
		// Token: 0x0600E40C RID: 58380 RVA: 0x0054C0AF File Offset: 0x0054A2AF
		public void ExposeData()
		{
			Scribe_Defs.Look<SitePartDef>(ref this.def, "def");
			Scribe_Deep.Look<SitePartParams>(ref this.parms, "parms", Array.Empty<object>());
		}

		// Token: 0x04009125 RID: 37157
		public SitePartDef def;

		// Token: 0x04009126 RID: 37158
		public SitePartParams parms;
	}
}
