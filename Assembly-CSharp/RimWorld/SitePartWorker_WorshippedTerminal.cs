using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001783 RID: 6019
	public class SitePartWorker_WorshippedTerminal : SitePartWorker
	{
		// Token: 0x060095C3 RID: 38339 RVA: 0x00366DDC File Offset: 0x00364FDC
		public override void Init(Site site, SitePart sitePart)
		{
			base.Init(site, sitePart);
			sitePart.things = new ThingOwner<Thing>(sitePart);
			Thing item = ThingMaker.MakeThing(ThingDefOf.AncientTerminal_Worshipful, null);
			sitePart.things.TryAdd(item, true);
		}

		// Token: 0x060095C4 RID: 38340 RVA: 0x00366E18 File Offset: 0x00365018
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			return base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets).Formatted(map.Parent.GetComponent<TimedMakeFactionHostile>().TicksLeft.Value.ToStringTicksToPeriod(true, false, true, true, false).Named("TIMER"));
		}
	}
}
