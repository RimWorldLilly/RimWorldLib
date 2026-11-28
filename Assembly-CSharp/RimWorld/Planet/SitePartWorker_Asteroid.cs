using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023C5 RID: 9157
	public class SitePartWorker_Asteroid : SitePartWorker
	{
		// Token: 0x0600E460 RID: 58464 RVA: 0x0054DB26 File Offset: 0x0054BD26
		public override void Init(Site site, SitePart sitePart)
		{
			base.Init(site, sitePart);
			site.customLabel = sitePart.def.label.Formatted(sitePart.parms.preciousLumpResources.Named("RESOURCE"));
		}
	}
}
