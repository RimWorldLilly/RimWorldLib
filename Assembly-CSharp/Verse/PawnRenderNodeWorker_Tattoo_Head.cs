using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003CA RID: 970
	public class PawnRenderNodeWorker_Tattoo_Head : PawnRenderNodeWorker_FlipWhenCrawling
	{
		// Token: 0x06001CB2 RID: 7346 RVA: 0x000B0724 File Offset: 0x000AE924
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			if (ModsConfig.IdeologyActive)
			{
				Pawn_StyleTracker style = parms.pawn.style;
				if (((style != null) ? style.FaceTattoo : null) != null)
				{
					return base.CanDrawNow(node, parms) && (parms.pawn.style.FaceTattoo.visibleNorth || !(parms.facing == Rot4.North));
				}
			}
			return false;
		}
	}
}
