using System;

namespace Verse.AI.Group
{
	// Token: 0x0200096E RID: 2414
	public class Trigger_Signal : Trigger
	{
		// Token: 0x06004983 RID: 18819 RVA: 0x001CA8D5 File Offset: 0x001C8AD5
		public Trigger_Signal(string signal)
		{
			this.signal = signal;
		}

		// Token: 0x06004984 RID: 18820 RVA: 0x001CA8E4 File Offset: 0x001C8AE4
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Signal && signal.signal.tag == this.signal;
		}

		// Token: 0x04002F32 RID: 12082
		private string signal;
	}
}
