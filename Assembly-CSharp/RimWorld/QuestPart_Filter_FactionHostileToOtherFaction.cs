using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001072 RID: 4210
	public class QuestPart_Filter_FactionHostileToOtherFaction : QuestPart_Filter
	{
		// Token: 0x0600699D RID: 27037 RVA: 0x00253B08 File Offset: 0x00251D08
		protected override bool Pass(SignalArgs args)
		{
			return this.faction != null && this.other != null && this.faction != this.other && this.faction.HostileTo(this.other);
		}

		// Token: 0x0600699E RID: 27038 RVA: 0x00253B3B File Offset: 0x00251D3B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<Faction>(ref this.other, "other", false);
		}

		// Token: 0x04004328 RID: 17192
		public Faction faction;

		// Token: 0x04004329 RID: 17193
		public Faction other;
	}
}
