using System;

namespace Verse
{
	// Token: 0x020003C9 RID: 969
	public class PawnRenderNodeWorker_Stump : PawnRenderNodeWorker_Head
	{
		// Token: 0x06001CAF RID: 7343 RVA: 0x000B06F1 File Offset: 0x000AE8F1
		public override bool ShouldListOnGraph(PawnRenderNode node, PawnDrawParms parms)
		{
			return parms.flags.FlagSet(PawnRenderFlags.HeadStump);
		}

		// Token: 0x06001CB0 RID: 7344 RVA: 0x000B06FF File Offset: 0x000AE8FF
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			return base.CanDrawNow(node, parms) && parms.flags.FlagSet(PawnRenderFlags.HeadStump);
		}
	}
}
