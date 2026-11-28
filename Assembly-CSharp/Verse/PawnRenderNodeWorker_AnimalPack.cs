using System;

namespace Verse
{
	// Token: 0x020003D1 RID: 977
	public class PawnRenderNodeWorker_AnimalPack : PawnRenderNodeWorker
	{
		// Token: 0x06001CCB RID: 7371 RVA: 0x000B09CC File Offset: 0x000AEBCC
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			return base.CanDrawNow(node, parms) && !parms.Portrait && parms.pawn.RaceProps.Animal && parms.pawn.inventory != null && parms.pawn.inventory.innerContainer.Count > 0;
		}
	}
}
