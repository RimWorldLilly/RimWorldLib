using System;

namespace Verse.AI.Group
{
	// Token: 0x02000982 RID: 2434
	public class Trigger_Memo : Trigger
	{
		// Token: 0x060049B0 RID: 18864 RVA: 0x001CB195 File Offset: 0x001C9395
		public Trigger_Memo(string memo)
		{
			this.memo = memo;
		}

		// Token: 0x060049B1 RID: 18865 RVA: 0x001CB1A4 File Offset: 0x001C93A4
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Memo && signal.memo == this.memo;
		}

		// Token: 0x04002F44 RID: 12100
		private string memo;
	}
}
