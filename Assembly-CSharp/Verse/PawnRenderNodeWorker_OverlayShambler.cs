using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003CE RID: 974
	public class PawnRenderNodeWorker_OverlayShambler : PawnRenderNodeWorker_Overlay
	{
		// Token: 0x06001CC0 RID: 7360 RVA: 0x000B0899 File Offset: 0x000AEA99
		protected override PawnOverlayDrawer OverlayDrawer(Pawn pawn)
		{
			return pawn.Drawer.renderer.ShamblerScarDrawer;
		}

		// Token: 0x06001CC1 RID: 7361 RVA: 0x000B08AB File Offset: 0x000AEAAB
		public override bool ShouldListOnGraph(PawnRenderNode node, PawnDrawParms parms)
		{
			return parms.pawn.IsShambler;
		}

		// Token: 0x06001CC2 RID: 7362 RVA: 0x000B08B8 File Offset: 0x000AEAB8
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			return base.CanDrawNow(node, parms) && parms.pawn.IsShambler && (parms.pawn.mutant == null || parms.pawn.mutant.HasTurned);
		}
	}
}
