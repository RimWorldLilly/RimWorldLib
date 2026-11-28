using System;

namespace Verse.AI.Group
{
	// Token: 0x0200096B RID: 2411
	public class Trigger_FractionPawnsLost : Trigger
	{
		// Token: 0x0600497D RID: 18813 RVA: 0x001CA82B File Offset: 0x001C8A2B
		public Trigger_FractionPawnsLost(float fraction)
		{
			this.fraction = fraction;
		}

		// Token: 0x0600497E RID: 18814 RVA: 0x001CA845 File Offset: 0x001C8A45
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && (float)lord.numPawnsLostViolently >= (float)lord.numPawnsEverGained * this.fraction;
		}

		// Token: 0x04002F2E RID: 12078
		private float fraction = 0.5f;
	}
}
