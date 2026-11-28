using System;

namespace Verse.AI.Group
{
	// Token: 0x0200096D RID: 2413
	public class Trigger_ChanceOnSignal : Trigger
	{
		// Token: 0x06004981 RID: 18817 RVA: 0x001CA8A0 File Offset: 0x001C8AA0
		public Trigger_ChanceOnSignal(TriggerSignalType signalType, float chance)
		{
			this.signalType = signalType;
			this.chance = chance;
		}

		// Token: 0x06004982 RID: 18818 RVA: 0x001CA8B6 File Offset: 0x001C8AB6
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == this.signalType && Rand.Value < this.chance;
		}

		// Token: 0x04002F30 RID: 12080
		private TriggerSignalType signalType;

		// Token: 0x04002F31 RID: 12081
		private float chance;
	}
}
