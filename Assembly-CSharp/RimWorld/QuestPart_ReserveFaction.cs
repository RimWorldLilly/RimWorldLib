using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010F0 RID: 4336
	public class QuestPart_ReserveFaction : QuestPart
	{
		// Token: 0x06006C45 RID: 27717 RVA: 0x0025FF13 File Offset: 0x0025E113
		public override bool QuestPartReserves(Faction f)
		{
			return f != null && f == this.faction;
		}

		// Token: 0x06006C46 RID: 27718 RVA: 0x0025FF23 File Offset: 0x0025E123
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x06006C47 RID: 27719 RVA: 0x0025FF35 File Offset: 0x0025E135
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x040044ED RID: 17645
		public Faction faction;
	}
}
