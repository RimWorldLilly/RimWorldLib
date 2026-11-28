using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002253 RID: 8787
	public class QuestPart_MechRelay : QuestPart
	{
		// Token: 0x0600D939 RID: 55609 RVA: 0x00507CBD File Offset: 0x00505EBD
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				Thing thing = this.relay;
				if (thing == null)
				{
					return;
				}
				CompMechRelay compMechRelay = thing.TryGetComp<CompMechRelay>();
				if (compMechRelay == null)
				{
					return;
				}
				compMechRelay.Deactivate();
			}
		}

		// Token: 0x0600D93A RID: 55610 RVA: 0x00507CEC File Offset: 0x00505EEC
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Thing>(ref this.relay, "relay", false);
		}

		// Token: 0x04008ADC RID: 35548
		public string inSignal;

		// Token: 0x04008ADD RID: 35549
		public Thing relay;
	}
}
