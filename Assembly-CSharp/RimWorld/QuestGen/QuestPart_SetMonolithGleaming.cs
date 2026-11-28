using System;
using RimWorld.Utility;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020022A7 RID: 8871
	public class QuestPart_SetMonolithGleaming : QuestPart
	{
		// Token: 0x0600DB06 RID: 56070 RVA: 0x00519816 File Offset: 0x00517A16
		public QuestPart_SetMonolithGleaming()
		{
		}

		// Token: 0x0600DB07 RID: 56071 RVA: 0x0051981E File Offset: 0x00517A1E
		public QuestPart_SetMonolithGleaming(Building_VoidMonolith monolith, string inSignal)
		{
			this.monolith = monolith;
			this.inSignal = inSignal;
		}

		// Token: 0x0600DB08 RID: 56072 RVA: 0x00519834 File Offset: 0x00517A34
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag != this.inSignal)
			{
				return;
			}
			Find.Anomaly.SetLevel(MonolithLevelDefOf.Gleaming, true);
			CellRect cellRect = this.monolith.OccupiedRect();
			CellRect value = new CellRect(cellRect.minX, cellRect.minZ - 3, 3, 3);
			VoidAwakeningUtility.SpawnVoidMetalAround(this.monolith, 300, QuestPart_SetMonolithGleaming.NumMassesRange.RandomInRange, true, new CellRect?(value));
			VoidAwakeningUtility.SpawnMetalhorrorDefenders(this.monolith);
		}

		// Token: 0x0600DB09 RID: 56073 RVA: 0x005198B7 File Offset: 0x00517AB7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Building_VoidMonolith>(ref this.monolith, "monolith", false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
		}

		// Token: 0x04008C38 RID: 35896
		private const int VoidMetalPatchSize = 300;

		// Token: 0x04008C39 RID: 35897
		private static readonly IntRange NumMassesRange = new IntRange(3, 4);

		// Token: 0x04008C3A RID: 35898
		private Building_VoidMonolith monolith;

		// Token: 0x04008C3B RID: 35899
		private string inSignal;
	}
}
