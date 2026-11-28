using System;

namespace RimWorld.QuestGen
{
	// Token: 0x0200219F RID: 8607
	public class QuestNode_SendTransportShipAwayOnCleanup : QuestNode
	{
		// Token: 0x0600D6B1 RID: 54961 RVA: 0x004FB80C File Offset: 0x004F9A0C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_SendTransportShipAwayOnCleanup part = new QuestPart_SendTransportShipAwayOnCleanup
			{
				transportShip = this.transportShip.GetValue(slate),
				unsatisfiedDropMode = this.unsatisfiedDropMode.GetValue(slate).GetValueOrDefault(TransportShipDropMode.NonRequired),
				unloadContents = this.unloadContents.GetValue(slate)
			};
			QuestGen.quest.AddPart(part);
		}

		// Token: 0x0600D6B2 RID: 54962 RVA: 0x004FB86F File Offset: 0x004F9A6F
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x040087FD RID: 34813
		public SlateRef<TransportShip> transportShip;

		// Token: 0x040087FE RID: 34814
		public SlateRef<bool> unloadContents;

		// Token: 0x040087FF RID: 34815
		public SlateRef<TransportShipDropMode?> unsatisfiedDropMode;
	}
}
