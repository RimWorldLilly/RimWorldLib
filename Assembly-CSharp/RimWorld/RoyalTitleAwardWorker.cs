using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FFD RID: 8189
	public class RoyalTitleAwardWorker
	{
		// Token: 0x0600CF6D RID: 53101 RVA: 0x004C6157 File Offset: 0x004C4357
		public virtual void OnPreAward(Pawn pawn, Faction faction, RoyalTitleDef currentTitle, RoyalTitleDef newTitle)
		{
		}

		// Token: 0x0600CF6E RID: 53102 RVA: 0x004C6159 File Offset: 0x004C4359
		public virtual void DoAward(Pawn pawn, Faction faction, RoyalTitleDef currentTitle, RoyalTitleDef newTitle)
		{
		}

		// Token: 0x040083F2 RID: 33778
		public RoyalTitleDef def;
	}
}
