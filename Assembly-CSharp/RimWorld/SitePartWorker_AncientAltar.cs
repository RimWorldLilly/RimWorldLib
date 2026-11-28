using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200176D RID: 5997
	public class SitePartWorker_AncientAltar : SitePartWorker
	{
		// Token: 0x06009564 RID: 38244 RVA: 0x003658D6 File Offset: 0x00363AD6
		public override void Init(Site site, SitePart sitePart)
		{
			base.Init(site, sitePart);
			sitePart.relicThing = sitePart.parms.relicThing;
		}

		// Token: 0x06009565 RID: 38245 RVA: 0x003658F1 File Offset: 0x00363AF1
		public bool ShouldKeepMapForRelic(SitePart sitePart)
		{
			return sitePart.relicThing != null && !sitePart.relicThing.DestroyedOrNull() && sitePart.relicThing.MapHeld == sitePart.site.Map;
		}

		// Token: 0x06009566 RID: 38246 RVA: 0x00365924 File Offset: 0x00363B24
		public override void Notify_SiteMapAboutToBeRemoved(SitePart sitePart)
		{
			base.Notify_SiteMapAboutToBeRemoved(sitePart);
			if (this.ShouldKeepMapForRelic(sitePart))
			{
				sitePart.relicThing.DeSpawnOrDeselect(DestroyMode.Vanish);
				if (sitePart.relicThing.holdingOwner != null)
				{
					sitePart.relicThing.holdingOwner.Remove(sitePart.relicThing);
				}
				sitePart.relicWasSpawned = false;
				return;
			}
			if (!sitePart.parms.relicLostSignal.NullOrEmpty())
			{
				Find.SignalManager.SendSignal(new Signal(sitePart.parms.relicLostSignal, false));
			}
		}

		// Token: 0x06009567 RID: 38247 RVA: 0x003659A7 File Offset: 0x00363BA7
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
