using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010F6 RID: 4342
	public class QuestPart_SetFaction : QuestPart
	{
		// Token: 0x17001285 RID: 4741
		// (get) Token: 0x06006C5C RID: 27740 RVA: 0x002602E1 File Offset: 0x0025E4E1
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				QuestPart_SetFaction.<get_InvolvedFactions>d__4 <get_InvolvedFactions>d__ = new QuestPart_SetFaction.<get_InvolvedFactions>d__4(-2);
				<get_InvolvedFactions>d__.<>4__this = this;
				return <get_InvolvedFactions>d__;
			}
		}

		// Token: 0x06006C5D RID: 27741 RVA: 0x002602F4 File Offset: 0x0025E4F4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.things.Count; i++)
				{
					if (this.things[i].Faction != this.faction)
					{
						this.things[i].SetFaction(this.faction, null);
					}
				}
			}
		}

		// Token: 0x06006C5E RID: 27742 RVA: 0x00260362 File Offset: 0x0025E562
		public override void Notify_FactionRemoved(Faction f)
		{
			if (f == this.faction)
			{
				this.faction = null;
			}
		}

		// Token: 0x06006C5F RID: 27743 RVA: 0x00260374 File Offset: 0x0025E574
		public override bool QuestPartReserves(Faction f)
		{
			return f == this.faction;
		}

		// Token: 0x06006C60 RID: 27744 RVA: 0x00260380 File Offset: 0x0025E580
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x040044F6 RID: 17654
		public string inSignal;

		// Token: 0x040044F7 RID: 17655
		public Faction faction;

		// Token: 0x040044F8 RID: 17656
		public List<Thing> things = new List<Thing>();
	}
}
