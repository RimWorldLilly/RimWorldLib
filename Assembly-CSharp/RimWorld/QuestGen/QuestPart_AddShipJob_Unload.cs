using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021A3 RID: 8611
	public class QuestPart_AddShipJob_Unload : QuestPart_AddShipJob
	{
		// Token: 0x0600D6C6 RID: 54982 RVA: 0x004FBD70 File Offset: 0x004F9F70
		public override ShipJob GetShipJob()
		{
			ShipJob_Unload shipJob_Unload = (ShipJob_Unload)ShipJobMaker.MakeShipJob(this.shipJobDef);
			shipJob_Unload.unforbidAll = this.unforbidAll;
			return shipJob_Unload;
		}

		// Token: 0x0600D6C7 RID: 54983 RVA: 0x004FBD8E File Offset: 0x004F9F8E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.unforbidAll, "unforbidAll", false, false);
		}

		// Token: 0x0400880D RID: 34829
		public bool unforbidAll = true;
	}
}
