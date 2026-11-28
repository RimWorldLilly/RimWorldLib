using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021A5 RID: 8613
	public class QuestPart_AddShipJob_WaitTime : QuestPart_AddShipJob_Wait
	{
		// Token: 0x0600D6CB RID: 54987 RVA: 0x004FBE94 File Offset: 0x004FA094
		public override ShipJob GetShipJob()
		{
			ShipJob_WaitTime shipJob_WaitTime = (ShipJob_WaitTime)ShipJobMaker.MakeShipJob(this.shipJobDef);
			shipJob_WaitTime.duration = this.duration;
			shipJob_WaitTime.leaveImmediatelyWhenSatisfied = this.leaveImmediatelyWhenSatisfied;
			shipJob_WaitTime.showGizmos = this.showGizmos;
			shipJob_WaitTime.sendAwayIfAllDespawned = this.sendAwayIfAllDespawned;
			shipJob_WaitTime.sendAwayIfAnyDespawnedDownedOrDead = this.sendAwayIfAnyDespawnedDownedOrDead;
			return shipJob_WaitTime;
		}

		// Token: 0x0600D6CC RID: 54988 RVA: 0x004FBEED File Offset: 0x004FA0ED
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
		}

		// Token: 0x04008812 RID: 34834
		public int duration;
	}
}
