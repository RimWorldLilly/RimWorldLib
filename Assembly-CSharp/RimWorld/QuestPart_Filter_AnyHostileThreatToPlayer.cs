using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001065 RID: 4197
	public class QuestPart_Filter_AnyHostileThreatToPlayer : QuestPart_Filter
	{
		// Token: 0x06006971 RID: 26993 RVA: 0x002531A1 File Offset: 0x002513A1
		protected override bool Pass(SignalArgs args)
		{
			return this.mapParent != null && this.mapParent.Map != null && GenHostility.AnyHostileActiveThreatToPlayer(this.mapParent.Map, this.countDormantPawnsAsHostile, false);
		}

		// Token: 0x06006972 RID: 26994 RVA: 0x002531D1 File Offset: 0x002513D1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "parent", false);
			Scribe_Values.Look<bool>(ref this.countDormantPawnsAsHostile, "countDormantPawnsAsHostile", false, false);
		}

		// Token: 0x04004310 RID: 17168
		public MapParent mapParent;

		// Token: 0x04004311 RID: 17169
		public bool countDormantPawnsAsHostile;
	}
}
