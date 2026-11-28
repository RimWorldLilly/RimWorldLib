using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010F7 RID: 4343
	public class QuestPart_SetFactionHidden : QuestPart
	{
		// Token: 0x17001286 RID: 4742
		// (get) Token: 0x06006C63 RID: 27747 RVA: 0x0026041A File Offset: 0x0025E61A
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				QuestPart_SetFactionHidden.<get_InvolvedFactions>d__4 <get_InvolvedFactions>d__ = new QuestPart_SetFactionHidden.<get_InvolvedFactions>d__4(-2);
				<get_InvolvedFactions>d__.<>4__this = this;
				return <get_InvolvedFactions>d__;
			}
		}

		// Token: 0x06006C64 RID: 27748 RVA: 0x0026042C File Offset: 0x0025E62C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.faction != null && this.faction.Hidden != this.hidden)
			{
				this.faction.hidden = new bool?(this.hidden);
			}
		}

		// Token: 0x06006C65 RID: 27749 RVA: 0x00260484 File Offset: 0x0025E684
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x06006C66 RID: 27750 RVA: 0x00260496 File Offset: 0x0025E696
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x040044F9 RID: 17657
		public string inSignal;

		// Token: 0x040044FA RID: 17658
		public Faction faction;

		// Token: 0x040044FB RID: 17659
		public bool hidden;
	}
}
