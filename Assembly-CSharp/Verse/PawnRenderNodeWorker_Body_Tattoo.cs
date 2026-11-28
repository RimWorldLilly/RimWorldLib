using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003C3 RID: 963
	public class PawnRenderNodeWorker_Body_Tattoo : PawnRenderNodeWorker_Body
	{
		// Token: 0x06001C9E RID: 7326 RVA: 0x000B0270 File Offset: 0x000AE470
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			if (ModsConfig.IdeologyActive)
			{
				Pawn_StyleTracker style = parms.pawn.style;
				if (((style != null) ? style.BodyTattoo : null) != null)
				{
					return base.CanDrawNow(node, parms) && (parms.pawn.style.BodyTattoo.visibleNorth || !(parms.facing == Rot4.North));
				}
			}
			return false;
		}
	}
}
