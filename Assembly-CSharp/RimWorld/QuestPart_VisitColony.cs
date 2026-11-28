using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200109F RID: 4255
	public class QuestPart_VisitColony : QuestPart_MakeLord
	{
		// Token: 0x06006A7D RID: 27261 RVA: 0x002571F4 File Offset: 0x002553F4
		protected override Lord MakeLord()
		{
			IntVec3 chillSpot;
			if (!RCellFinder.TryFindRandomSpotJustOutsideColony(this.pawns[0], out chillSpot))
			{
				chillSpot = CellFinder.RandomCell(this.pawns[0].Map);
			}
			LordJob_VisitColony lordJob = new LordJob_VisitColony(this.faction ?? this.pawns[0].Faction, chillSpot, this.durationTicks);
			return LordMaker.MakeNewLord(this.faction ?? this.pawns[0].Faction, lordJob, this.mapParent.Map, null);
		}

		// Token: 0x06006A7E RID: 27262 RVA: 0x00257284 File Offset: 0x00255484
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int?>(ref this.durationTicks, "durationTicks", null, false);
		}

		// Token: 0x040043B3 RID: 17331
		public int? durationTicks;
	}
}
