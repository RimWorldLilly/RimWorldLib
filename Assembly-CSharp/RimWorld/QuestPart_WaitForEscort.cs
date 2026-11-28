using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020010A2 RID: 4258
	public class QuestPart_WaitForEscort : QuestPart_MakeLord
	{
		// Token: 0x06006A87 RID: 27271 RVA: 0x002573EF File Offset: 0x002555EF
		protected override Lord MakeLord()
		{
			return LordMaker.MakeNewLord(this.faction, new LordJob_WaitForEscort(this.point, this.addFleeToil), base.Map, null);
		}

		// Token: 0x06006A88 RID: 27272 RVA: 0x00257414 File Offset: 0x00255614
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.addFleeToil, "addFleeToil", false, false);
		}

		// Token: 0x040043B8 RID: 17336
		public IntVec3 point;

		// Token: 0x040043B9 RID: 17337
		public bool addFleeToil = true;
	}
}
