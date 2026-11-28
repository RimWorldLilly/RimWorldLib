using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023C6 RID: 9158
	public abstract class SitePartWorker_DistressCall : SitePartWorker
	{
		// Token: 0x0600E462 RID: 58466 RVA: 0x0054DB68 File Offset: 0x0054BD68
		public override void Init(Site site, SitePart sitePart)
		{
			base.Init(site, sitePart);
			site.customLabel = sitePart.def.label.Formatted(site.Faction.Named("FACTION"));
		}

		// Token: 0x0600E463 RID: 58467 RVA: 0x0054DBA0 File Offset: 0x0054BDA0
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			return "Hostiles".Translate() + ": " + "Unknown".Translate().CapitalizeFirst() + ("\n" + "Contains".Translate() + ": " + "Unknown".Translate().CapitalizeFirst());
		}
	}
}
