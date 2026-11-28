using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003CF RID: 975
	public class PawnRenderNodeWorker_OverlayScaria : PawnRenderNodeWorker_Overlay
	{
		// Token: 0x06001CC4 RID: 7364 RVA: 0x000B08FA File Offset: 0x000AEAFA
		protected override PawnOverlayDrawer OverlayDrawer(Pawn pawn)
		{
			return pawn.Drawer.renderer.ScariaSoreDrawer;
		}

		// Token: 0x06001CC5 RID: 7365 RVA: 0x000B090C File Offset: 0x000AEB0C
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			return base.CanDrawNow(node, parms) && parms.rotDrawMode != RotDrawMode.Dessicated;
		}
	}
}
