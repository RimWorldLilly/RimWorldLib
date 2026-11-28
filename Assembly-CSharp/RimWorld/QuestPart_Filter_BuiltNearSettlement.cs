using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001070 RID: 4208
	public class QuestPart_Filter_BuiltNearSettlement : QuestPart_Filter
	{
		// Token: 0x06006997 RID: 27031 RVA: 0x00253A79 File Offset: 0x00251C79
		protected override bool Pass(SignalArgs args)
		{
			return this.settlementFaction != null && args.GetArg<Thing>("SUBJECT").MapHeld.Parent == this.mapParent;
		}

		// Token: 0x06006998 RID: 27032 RVA: 0x00253AA3 File Offset: 0x00251CA3
		public override void Notify_FactionRemoved(Faction faction)
		{
			base.Notify_FactionRemoved(faction);
			if (this.settlementFaction == faction)
			{
				this.settlementFaction = null;
			}
		}

		// Token: 0x06006999 RID: 27033 RVA: 0x00253ABC File Offset: 0x00251CBC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<Faction>(ref this.settlementFaction, "settlementFaction", false);
		}

		// Token: 0x04004326 RID: 17190
		public MapParent mapParent;

		// Token: 0x04004327 RID: 17191
		public Faction settlementFaction;
	}
}
