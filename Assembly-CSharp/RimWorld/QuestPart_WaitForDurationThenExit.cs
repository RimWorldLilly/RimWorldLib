using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020010A0 RID: 4256
	public class QuestPart_WaitForDurationThenExit : QuestPart_MakeLord
	{
		// Token: 0x06006A81 RID: 27265 RVA: 0x002572C1 File Offset: 0x002554C1
		protected override Lord MakeLord()
		{
			return LordMaker.MakeNewLord(this.faction, new LordJob_WaitForDurationThenExit(this.point, this.durationTicks), base.Map, null);
		}

		// Token: 0x06006A82 RID: 27266 RVA: 0x002572E8 File Offset: 0x002554E8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.durationTicks, "durationTicks", 0, false);
		}

		// Token: 0x040043B4 RID: 17332
		public IntVec3 point;

		// Token: 0x040043B5 RID: 17333
		public int durationTicks;
	}
}
