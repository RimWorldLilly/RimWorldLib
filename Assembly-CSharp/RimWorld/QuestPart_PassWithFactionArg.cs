using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001057 RID: 4183
	public class QuestPart_PassWithFactionArg : QuestPart_Pass
	{
		// Token: 0x06006943 RID: 26947 RVA: 0x002527A0 File Offset: 0x002509A0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				SignalArgs args = new SignalArgs(signal.args);
				if (this.outSignalOutcomeArg != null)
				{
					args.Add(this.outSignalOutcomeArg.Value.Named("OUTCOME"));
				}
				args.Add(this.faction.Named("FACTION"));
				Find.SignalManager.SendSignal(new Signal(this.outSignal, args, false));
			}
		}

		// Token: 0x06006944 RID: 26948 RVA: 0x00252829 File Offset: 0x00250A29
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x06006945 RID: 26949 RVA: 0x0025283B File Offset: 0x00250A3B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x040042FD RID: 17149
		public Faction faction;
	}
}
