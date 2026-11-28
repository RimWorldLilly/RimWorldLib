using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003C5 RID: 965
	public class PawnRenderNodeWorker_Swaddle : PawnRenderNodeWorker_Body
	{
		// Token: 0x06001CA2 RID: 7330 RVA: 0x000B0303 File Offset: 0x000AE503
		public override bool ShouldListOnGraph(PawnRenderNode node, PawnDrawParms parms)
		{
			return parms.pawn.DevelopmentalStage.Baby();
		}

		// Token: 0x06001CA3 RID: 7331 RVA: 0x000B0315 File Offset: 0x000AE515
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			return base.CanDrawNow(node, parms) && parms.pawn.SwaddleBaby();
		}
	}
}
