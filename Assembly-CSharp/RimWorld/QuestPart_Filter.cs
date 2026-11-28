using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200105A RID: 4186
	public abstract class QuestPart_Filter : QuestPart
	{
		// Token: 0x0600694D RID: 26957
		protected abstract bool Pass(SignalArgs args);

		// Token: 0x0600694E RID: 26958 RVA: 0x002529E4 File Offset: 0x00250BE4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				if (this.Pass(signal.args))
				{
					if (!this.outSignal.NullOrEmpty())
					{
						Find.SignalManager.SendSignal(new Signal(this.outSignal, signal.args, false));
						return;
					}
				}
				else if (!this.outSignalElse.NullOrEmpty())
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalElse, signal.args, false));
				}
			}
		}

		// Token: 0x0600694F RID: 26959 RVA: 0x00252A6C File Offset: 0x00250C6C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalElse, "outSignalElse", null, false);
		}

		// Token: 0x06006950 RID: 26960 RVA: 0x00252AAC File Offset: 0x00250CAC
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.outSignal = "DebugSignal" + Rand.Int.ToString();
		}

		// Token: 0x04004302 RID: 17154
		public string inSignal;

		// Token: 0x04004303 RID: 17155
		public string outSignal;

		// Token: 0x04004304 RID: 17156
		public string outSignalElse;
	}
}
