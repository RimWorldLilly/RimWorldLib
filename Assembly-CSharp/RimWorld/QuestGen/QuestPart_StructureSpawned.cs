using System;
using RimWorld.Utility;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020022A2 RID: 8866
	public class QuestPart_StructureSpawned : QuestPart
	{
		// Token: 0x0600DAE0 RID: 56032 RVA: 0x00518884 File Offset: 0x00516A84
		public QuestPart_StructureSpawned()
		{
		}

		// Token: 0x0600DAE1 RID: 56033 RVA: 0x0051888C File Offset: 0x00516A8C
		public QuestPart_StructureSpawned(string spawnedSignal, Thing structure)
		{
			this.spawnedSignal = spawnedSignal;
			this.structure = structure;
		}

		// Token: 0x0600DAE2 RID: 56034 RVA: 0x005188A2 File Offset: 0x00516AA2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.spawnedSignal, "spawnedSignal", null, false);
			Scribe_References.Look<Thing>(ref this.structure, "structure", false);
		}

		// Token: 0x0600DAE3 RID: 56035 RVA: 0x005188D0 File Offset: 0x00516AD0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag != this.spawnedSignal)
			{
				return;
			}
			VoidAwakeningUtility.SpawnVoidMetalAround(this.structure, 50, QuestPart_StructureSpawned.NumMassesRange.RandomInRange, true, null);
			VoidAwakeningUtility.SpawnMetalhorrorDefenders(this.structure);
		}

		// Token: 0x04008C19 RID: 35865
		private const int VoidMetalPatchSize = 50;

		// Token: 0x04008C1A RID: 35866
		private static readonly IntRange NumMassesRange = new IntRange(3, 4);

		// Token: 0x04008C1B RID: 35867
		private string spawnedSignal;

		// Token: 0x04008C1C RID: 35868
		private Thing structure;
	}
}
