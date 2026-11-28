using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003CD RID: 973
	public class PawnRenderNodeWorker_OverlayWounds : PawnRenderNodeWorker_Overlay
	{
		// Token: 0x06001CBD RID: 7357 RVA: 0x000B0865 File Offset: 0x000AEA65
		protected override PawnOverlayDrawer OverlayDrawer(Pawn pawn)
		{
			return pawn.Drawer.renderer.WoundOverlays;
		}

		// Token: 0x06001CBE RID: 7358 RVA: 0x000B0877 File Offset: 0x000AEA77
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			return base.CanDrawNow(node, parms) && parms.rotDrawMode != RotDrawMode.Dessicated;
		}
	}
}
