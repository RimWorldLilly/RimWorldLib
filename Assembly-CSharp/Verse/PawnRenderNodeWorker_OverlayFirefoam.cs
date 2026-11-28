using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003CC RID: 972
	public class PawnRenderNodeWorker_OverlayFirefoam : PawnRenderNodeWorker_Overlay
	{
		// Token: 0x06001CB9 RID: 7353 RVA: 0x000B07FE File Offset: 0x000AE9FE
		protected override PawnOverlayDrawer OverlayDrawer(Pawn pawn)
		{
			return pawn.Drawer.renderer.FirefoamOverlays;
		}

		// Token: 0x06001CBA RID: 7354 RVA: 0x000B0810 File Offset: 0x000AEA10
		public override bool ShouldListOnGraph(PawnRenderNode node, PawnDrawParms parms)
		{
			return parms.pawn.Drawer.renderer.FirefoamOverlays.coveredInFoam;
		}

		// Token: 0x06001CBB RID: 7355 RVA: 0x000B082C File Offset: 0x000AEA2C
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			return base.CanDrawNow(node, parms) && parms.rotDrawMode == RotDrawMode.Fresh && parms.pawn.Drawer.renderer.FirefoamOverlays.coveredInFoam;
		}
	}
}
