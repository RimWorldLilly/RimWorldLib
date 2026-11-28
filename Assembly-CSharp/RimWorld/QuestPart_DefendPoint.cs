using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001099 RID: 4249
	public class QuestPart_DefendPoint : QuestPart_MakeLord
	{
		// Token: 0x06006A65 RID: 27237 RVA: 0x00256B64 File Offset: 0x00254D64
		protected override Lord MakeLord()
		{
			return LordMaker.MakeNewLord(this.faction, new LordJob_DefendPoint(this.point, this.wanderRadius, this.defendRadius, this.isCaravanSendable, this.addFleeToil), base.Map, null);
		}

		// Token: 0x06006A66 RID: 27238 RVA: 0x00256B9C File Offset: 0x00254D9C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
			Scribe_Values.Look<float?>(ref this.wanderRadius, "wanderRadius", null, false);
			Scribe_Values.Look<float?>(ref this.defendRadius, "defendRadius", null, false);
			Scribe_Values.Look<bool>(ref this.isCaravanSendable, "isCaravanSendable", false, false);
			Scribe_Values.Look<bool>(ref this.addFleeToil, "addFleeToil", false, false);
		}

		// Token: 0x04004393 RID: 17299
		public IntVec3 point;

		// Token: 0x04004394 RID: 17300
		public float? wanderRadius;

		// Token: 0x04004395 RID: 17301
		public float? defendRadius;

		// Token: 0x04004396 RID: 17302
		public bool isCaravanSendable;

		// Token: 0x04004397 RID: 17303
		public bool addFleeToil = true;
	}
}
