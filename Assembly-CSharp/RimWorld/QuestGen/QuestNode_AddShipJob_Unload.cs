using System;

namespace RimWorld.QuestGen
{
	// Token: 0x0200219D RID: 8605
	public class QuestNode_AddShipJob_Unload : QuestNode_AddShipJob
	{
		// Token: 0x170025FE RID: 9726
		// (get) Token: 0x0600D6AB RID: 54955 RVA: 0x004FB767 File Offset: 0x004F9967
		protected override ShipJobDef DefaultShipJobDef
		{
			get
			{
				return ShipJobDefOf.Unload;
			}
		}

		// Token: 0x0600D6AC RID: 54956 RVA: 0x004FB770 File Offset: 0x004F9970
		protected override void AddJobVars(ShipJob shipJob, Slate slate)
		{
			ShipJob_Unload shipJob_Unload = shipJob as ShipJob_Unload;
			if (shipJob_Unload != null)
			{
				shipJob_Unload.dropMode = this.dropMode.GetValue(slate).GetValueOrDefault(TransportShipDropMode.All);
			}
		}

		// Token: 0x040087FA RID: 34810
		public SlateRef<TransportShipDropMode?> dropMode;
	}
}
