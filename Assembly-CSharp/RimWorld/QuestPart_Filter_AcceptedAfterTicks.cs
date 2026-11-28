using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200105B RID: 4187
	public class QuestPart_Filter_AcceptedAfterTicks : QuestPart_Filter
	{
		// Token: 0x06006952 RID: 26962 RVA: 0x00252B01 File Offset: 0x00250D01
		protected override bool Pass(SignalArgs args)
		{
			return this.quest.TicksSinceAccepted > this.timeTicks;
		}

		// Token: 0x06006953 RID: 26963 RVA: 0x00252B16 File Offset: 0x00250D16
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.timeTicks, "timeTicks", 0, false);
		}

		// Token: 0x04004305 RID: 17157
		public int timeTicks;
	}
}
