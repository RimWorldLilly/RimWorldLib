using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002283 RID: 8835
	public class QuestPart_SpawnMonolith : QuestPart
	{
		// Token: 0x0600DA39 RID: 55865 RVA: 0x00511A94 File Offset: 0x0050FC94
		public QuestPart_SpawnMonolith()
		{
		}

		// Token: 0x0600DA3A RID: 55866 RVA: 0x00511A9C File Offset: 0x0050FC9C
		public QuestPart_SpawnMonolith(string inSignal)
		{
			this.inSignal = inSignal;
		}

		// Token: 0x0600DA3B RID: 55867 RVA: 0x00511AAC File Offset: 0x0050FCAC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag != this.inSignal)
			{
				return;
			}
			Map randomSurfacePlayerHomeMap = Find.RandomSurfacePlayerHomeMap;
			if (randomSurfacePlayerHomeMap == null)
			{
				Log.Message("Tried to spawn monolith but no map found");
				this.quest.End(QuestEndOutcome.Fail, true, true);
				return;
			}
			IntVec3 loc;
			Find.Anomaly.TryGetCellForMonolithSpawn(randomSurfacePlayerHomeMap, out loc, false);
			GenSpawn.Spawn(ThingDefOf.MonolithIncoming, loc, randomSurfacePlayerHomeMap, WipeMode.Vanish);
		}

		// Token: 0x0600DA3C RID: 55868 RVA: 0x00511B0C File Offset: 0x0050FD0C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
		}

		// Token: 0x04008B81 RID: 35713
		private string inSignal;
	}
}
