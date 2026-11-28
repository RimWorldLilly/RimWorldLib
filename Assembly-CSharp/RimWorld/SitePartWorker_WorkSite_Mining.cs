using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001780 RID: 6016
	public class SitePartWorker_WorkSite_Mining : SitePartWorker_WorkSite
	{
		// Token: 0x1700198B RID: 6539
		// (get) Token: 0x060095AB RID: 38315 RVA: 0x003667F0 File Offset: 0x003649F0
		public override IEnumerable<PreceptDef> DisallowedPrecepts
		{
			get
			{
				return from p in DefDatabase<PreceptDef>.AllDefs
				where p.disallowMiningCamps
				select p;
			}
		}

		// Token: 0x1700198C RID: 6540
		// (get) Token: 0x060095AC RID: 38316 RVA: 0x0036681B File Offset: 0x00364A1B
		public override PawnGroupKindDef WorkerGroupKind
		{
			get
			{
				return PawnGroupKindDefOf.Miners;
			}
		}

		// Token: 0x060095AD RID: 38317 RVA: 0x00366824 File Offset: 0x00364A24
		public override bool CanSpawnOn(PlanetTile tile)
		{
			Hilliness hilliness = Find.WorldGrid[tile].hilliness;
			return hilliness >= Hilliness.LargeHills && hilliness < Hilliness.Impassable;
		}

		// Token: 0x060095AE RID: 38318 RVA: 0x0036684C File Offset: 0x00364A4C
		protected override void OnLootChosen(Site site, SitePart sitePart, SitePartWorker_WorkSite.CampLootThingStruct loot)
		{
			site.customLabel = sitePart.def.label.Formatted(loot.thing.Named("THING"));
		}
	}
}
