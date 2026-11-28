using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010F8 RID: 4344
	public class QuestPart_SetSitePartThreatPointsToCurrent : QuestPart
	{
		// Token: 0x06006C69 RID: 27753 RVA: 0x002604D4 File Offset: 0x0025E6D4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.useMapParentThreatPoints != null && this.useMapParentThreatPoints.HasMap && this.site != null && this.sitePartDef != null)
			{
				List<SitePart> parts = this.site.parts;
				for (int i = 0; i < parts.Count; i++)
				{
					if (parts[i].def == this.sitePartDef)
					{
						parts[i].parms.threatPoints = StorytellerUtility.DefaultThreatPointsNow(this.useMapParentThreatPoints.Map) * this.threatPointsFactor;
					}
				}
			}
		}

		// Token: 0x06006C6A RID: 27754 RVA: 0x0026057C File Offset: 0x0025E77C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Defs.Look<SitePartDef>(ref this.sitePartDef, "sitePartDef");
			Scribe_References.Look<Site>(ref this.site, "site", false);
			Scribe_References.Look<MapParent>(ref this.useMapParentThreatPoints, "useMapParentThreatPoints", false);
			Scribe_Values.Look<float>(ref this.threatPointsFactor, "threatPointsFactor", 0f, false);
		}

		// Token: 0x040044FC RID: 17660
		public string inSignal;

		// Token: 0x040044FD RID: 17661
		public SitePartDef sitePartDef;

		// Token: 0x040044FE RID: 17662
		public Site site;

		// Token: 0x040044FF RID: 17663
		public MapParent useMapParentThreatPoints;

		// Token: 0x04004500 RID: 17664
		public float threatPointsFactor = 1f;
	}
}
