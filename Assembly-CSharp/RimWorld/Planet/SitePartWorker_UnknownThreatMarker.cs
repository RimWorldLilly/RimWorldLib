using System;

namespace RimWorld.Planet
{
	// Token: 0x020023CF RID: 9167
	public class SitePartWorker_UnknownThreatMarker : SitePartWorker
	{
		// Token: 0x0600E47E RID: 58494 RVA: 0x0054E8F8 File Offset: 0x0054CAF8
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			if (site.MainSitePartDef == SitePartDefOf.PreciousLump)
			{
				return null;
			}
			return base.GetPostProcessedThreatLabel(site, sitePart);
		}
	}
}
