using System;

namespace Verse
{
	// Token: 0x020003C6 RID: 966
	public class PawnRenderNodeWorker_Apparel_Head : PawnRenderNodeWorker_FlipWhenCrawling
	{
		// Token: 0x06001CA5 RID: 7333 RVA: 0x000B0338 File Offset: 0x000AE538
		public override bool CanDrawNow(PawnRenderNode n, PawnDrawParms parms)
		{
			return base.CanDrawNow(n, parms) && parms.flags.FlagSet(PawnRenderFlags.Clothes) && parms.flags.FlagSet(PawnRenderFlags.Headgear) && PawnRenderNodeWorker_Apparel_Head.HeadgearVisible(parms) && (!parms.Portrait || !Prefs.HatsOnlyOnMap || parms.flags.FlagSet(PawnRenderFlags.StylingStation));
		}

		// Token: 0x06001CA6 RID: 7334 RVA: 0x000B03A0 File Offset: 0x000AE5A0
		public static bool HeadgearVisible(PawnDrawParms parms)
		{
			return parms.flags.FlagSet(PawnRenderFlags.Clothes) && parms.flags.FlagSet(PawnRenderFlags.Headgear) && (parms.Portrait || parms.bed == null || parms.bed.def.building.bed_showSleeperBody) && (!parms.Portrait || !Prefs.HatsOnlyOnMap || parms.flags.FlagSet(PawnRenderFlags.StylingStation));
		}
	}
}
