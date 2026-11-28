using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001048 RID: 4168
	public class QuestPart_DelayRandom : QuestPart_Delay
	{
		// Token: 0x06006903 RID: 26883 RVA: 0x002517D1 File Offset: 0x0024F9D1
		protected override void Enable(SignalArgs receivedArgs)
		{
			this.delayTicks = this.delayTicksRange.RandomInRange;
			base.Enable(receivedArgs);
		}

		// Token: 0x06006904 RID: 26884 RVA: 0x002517EC File Offset: 0x0024F9EC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntRange>(ref this.delayTicksRange, "delayTicksRange", default(IntRange), false);
		}

		// Token: 0x06006905 RID: 26885 RVA: 0x00251819 File Offset: 0x0024FA19
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.delayTicksRange = new IntRange(833, 2500);
		}

		// Token: 0x040042DC RID: 17116
		public IntRange delayTicksRange;
	}
}
