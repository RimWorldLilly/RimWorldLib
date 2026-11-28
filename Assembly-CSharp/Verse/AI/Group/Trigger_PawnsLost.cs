using System;

namespace Verse.AI.Group
{
	// Token: 0x0200096C RID: 2412
	public class Trigger_PawnsLost : Trigger
	{
		// Token: 0x0600497F RID: 18815 RVA: 0x001CA86C File Offset: 0x001C8A6C
		public Trigger_PawnsLost(int count)
		{
			this.count = count;
		}

		// Token: 0x06004980 RID: 18816 RVA: 0x001CA882 File Offset: 0x001C8A82
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && lord.numPawnsLostViolently >= this.count;
		}

		// Token: 0x04002F2F RID: 12079
		private int count = 1;
	}
}
