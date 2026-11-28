using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200176E RID: 5998
	public class SitePartWorker_AncientComplex : SitePartWorker
	{
		// Token: 0x06009569 RID: 38249 RVA: 0x003659C9 File Offset: 0x00363BC9
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			if (site.MainSitePartDef == this.def)
			{
				return null;
			}
			return base.GetPostProcessedThreatLabel(site, sitePart);
		}
	}
}
