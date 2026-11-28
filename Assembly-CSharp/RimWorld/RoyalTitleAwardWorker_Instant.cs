using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FFE RID: 8190
	public class RoyalTitleAwardWorker_Instant : RoyalTitleAwardWorker
	{
		// Token: 0x0600CF70 RID: 53104 RVA: 0x004C6163 File Offset: 0x004C4363
		public override void DoAward(Pawn pawn, Faction faction, RoyalTitleDef currentTitle, RoyalTitleDef newTitle)
		{
			pawn.royalty.TryUpdateTitle(faction, true, this.def);
		}
	}
}
