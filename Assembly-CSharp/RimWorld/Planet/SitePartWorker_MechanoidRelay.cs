using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023CC RID: 9164
	public class SitePartWorker_MechanoidRelay : SitePartWorker
	{
		// Token: 0x0600E476 RID: 58486 RVA: 0x0054E694 File Offset: 0x0054C894
		public override void Init(Site site, SitePart sitePart)
		{
			base.Init(site, sitePart);
			sitePart.things = new ThingOwner<Thing>(sitePart);
			sitePart.things.TryAdd(ThingMaker.MakeThing(ThingDefOf.MechRelay, null), true);
			for (int i = 0; i < sitePart.parms.stabilizerCount; i++)
			{
				sitePart.things.TryAdd(ThingMaker.MakeThing(ThingDefOf.MechStabilizer, null), true);
			}
		}
	}
}
