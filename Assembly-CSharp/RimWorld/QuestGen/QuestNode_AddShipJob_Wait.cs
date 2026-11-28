using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200219C RID: 8604
	public class QuestNode_AddShipJob_Wait : QuestNode_AddShipJob
	{
		// Token: 0x0600D6A9 RID: 54953 RVA: 0x004FB708 File Offset: 0x004F9908
		protected override void AddJobVars(ShipJob shipJob, Slate slate)
		{
			ShipJob_Wait shipJob_Wait = shipJob as ShipJob_Wait;
			if (shipJob_Wait != null)
			{
				shipJob_Wait.leaveImmediatelyWhenSatisfied = this.leaveImmediatelyWhenSatisfied.GetValue(slate);
				shipJob_Wait.sendAwayIfAllDespawned = this.sendAwayIfAllDespawned.GetValue(slate);
			}
			ShipJob_WaitTime shipJob_WaitTime = shipJob as ShipJob_WaitTime;
			if (shipJob_WaitTime != null)
			{
				shipJob_WaitTime.duration = this.ticks.GetValue(slate);
			}
		}

		// Token: 0x040087F7 RID: 34807
		public SlateRef<int> ticks;

		// Token: 0x040087F8 RID: 34808
		public SlateRef<bool> leaveImmediatelyWhenSatisfied;

		// Token: 0x040087F9 RID: 34809
		public SlateRef<List<Thing>> sendAwayIfAllDespawned;
	}
}
