using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020021A6 RID: 8614
	public class QuestPart_AddShipJob_WaitForever : QuestPart_AddShipJob_Wait
	{
		// Token: 0x0600D6CE RID: 54990 RVA: 0x004FBF10 File Offset: 0x004FA110
		public override ShipJob GetShipJob()
		{
			ShipJob_WaitForever shipJob_WaitForever = (ShipJob_WaitForever)ShipJobMaker.MakeShipJob(this.shipJobDef);
			shipJob_WaitForever.leaveImmediatelyWhenSatisfied = this.leaveImmediatelyWhenSatisfied;
			shipJob_WaitForever.showGizmos = this.showGizmos;
			shipJob_WaitForever.sendAwayIfAllDespawned = this.sendAwayIfAllDespawned;
			shipJob_WaitForever.sendAwayIfAnyDespawnedDownedOrDead = this.sendAwayIfAnyDespawnedDownedOrDead;
			return shipJob_WaitForever;
		}
	}
}
