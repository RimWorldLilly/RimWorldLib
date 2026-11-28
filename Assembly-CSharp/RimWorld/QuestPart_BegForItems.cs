using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001096 RID: 4246
	public class QuestPart_BegForItems : QuestPart_MakeLord
	{
		// Token: 0x06006A59 RID: 27225 RVA: 0x0025668C File Offset: 0x0025488C
		protected override Lord MakeLord()
		{
			IntVec3 idleSpot;
			if (!RCellFinder.TryFindRandomSpotJustOutsideColony(this.pawns[0], out idleSpot))
			{
				idleSpot = CellFinder.RandomCell(this.pawns[0].Map);
			}
			return LordMaker.MakeNewLord(this.faction, new LordJob_BegForItems(this.faction, idleSpot, this.target, this.thingDef, this.amount, this.outSignalItemsReceived), base.Map, null);
		}

		// Token: 0x06006A5A RID: 27226 RVA: 0x002566FB File Offset: 0x002548FB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
		}

		// Token: 0x04004388 RID: 17288
		public Pawn target;

		// Token: 0x04004389 RID: 17289
		public ThingDef thingDef;

		// Token: 0x0400438A RID: 17290
		public int amount;

		// Token: 0x0400438B RID: 17291
		public string outSignalItemsReceived;
	}
}
