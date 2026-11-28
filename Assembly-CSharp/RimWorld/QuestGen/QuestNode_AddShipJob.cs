using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200219A RID: 8602
	public class QuestNode_AddShipJob : QuestNode
	{
		// Token: 0x170025FC RID: 9724
		// (get) Token: 0x0600D6A1 RID: 54945 RVA: 0x004FB573 File Offset: 0x004F9773
		protected virtual ShipJobDef DefaultShipJobDef
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600D6A2 RID: 54946 RVA: 0x004FB578 File Offset: 0x004F9778
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			ShipJob shipJob = ShipJobMaker.MakeShipJob(this.jobDef.GetValue(slate) ?? this.DefaultShipJobDef);
			this.AddJobVars(shipJob, slate);
			QuestPart_AddShipJob part = new QuestPart_AddShipJob
			{
				inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				shipJob = shipJob,
				shipJobStartMode = this.shipJobStartMode.GetValue(slate).GetValueOrDefault(),
				transportShip = this.transportShip.GetValue(slate)
			};
			QuestGen.quest.AddPart(part);
		}

		// Token: 0x0600D6A3 RID: 54947 RVA: 0x004FB61F File Offset: 0x004F981F
		protected virtual void AddJobVars(ShipJob shipJob, Slate slate)
		{
		}

		// Token: 0x0600D6A4 RID: 54948 RVA: 0x004FB621 File Offset: 0x004F9821
		protected override bool TestRunInt(Slate slate)
		{
			return this.jobDef.GetValue(slate) != null || this.DefaultShipJobDef != null;
		}

		// Token: 0x040087F0 RID: 34800
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040087F1 RID: 34801
		public SlateRef<TransportShip> transportShip;

		// Token: 0x040087F2 RID: 34802
		public SlateRef<ShipJobDef> jobDef;

		// Token: 0x040087F3 RID: 34803
		public SlateRef<ShipJobStartMode?> shipJobStartMode;
	}
}
