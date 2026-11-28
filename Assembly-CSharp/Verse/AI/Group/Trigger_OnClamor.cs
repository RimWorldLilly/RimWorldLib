using System;

namespace Verse.AI.Group
{
	// Token: 0x0200096F RID: 2415
	public class Trigger_OnClamor : Trigger
	{
		// Token: 0x06004985 RID: 18821 RVA: 0x001CA908 File Offset: 0x001C8B08
		public Trigger_OnClamor(ClamorDef clamorType)
		{
			this.clamorType = clamorType;
		}

		// Token: 0x06004986 RID: 18822 RVA: 0x001CA917 File Offset: 0x001C8B17
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Clamor && signal.clamorType == this.clamorType;
		}

		// Token: 0x04002F33 RID: 12083
		private ClamorDef clamorType;
	}
}
