using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010C2 RID: 4290
	public class ExtraFaction : IExposable
	{
		// Token: 0x06006B3A RID: 27450 RVA: 0x0025A1CD File Offset: 0x002583CD
		public ExtraFaction()
		{
		}

		// Token: 0x06006B3B RID: 27451 RVA: 0x0025A1D5 File Offset: 0x002583D5
		public ExtraFaction(Faction faction, ExtraFactionType factionType)
		{
			this.faction = faction;
			this.factionType = factionType;
		}

		// Token: 0x06006B3C RID: 27452 RVA: 0x0025A1EB File Offset: 0x002583EB
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<ExtraFactionType>(ref this.factionType, "factionType", ExtraFactionType.HomeFaction, false);
		}

		// Token: 0x0400442B RID: 17451
		public Faction faction;

		// Token: 0x0400442C RID: 17452
		public ExtraFactionType factionType;
	}
}
