using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001089 RID: 4233
	public class QuestPart_PawnsAvailable : QuestPartActivable
	{
		// Token: 0x06006A0F RID: 27151 RVA: 0x00255774 File Offset: 0x00253974
		public override void QuestPartTick()
		{
			if (this.mapParent == null || this.mapParent.Destroyed)
			{
				this.mapParent = this.quest.TryFindNewSuitableMapParentForRetarget();
			}
			if (this.requiredCount > 0 && Find.TickManager.TicksAbs % 500 == 0)
			{
				int num = 0;
				IReadOnlyList<Pawn> allPawnsSpawned = this.mapParent.Map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].def == this.race && allPawnsSpawned[i].Faction == null)
					{
						num++;
					}
				}
				if (num < this.requiredCount)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalPawnsNotAvailable, false));
				}
			}
		}

		// Token: 0x06006A10 RID: 27152 RVA: 0x00255833 File Offset: 0x00253A33
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignalDecrement)
			{
				this.requiredCount--;
			}
		}

		// Token: 0x06006A11 RID: 27153 RVA: 0x00255860 File Offset: 0x00253A60
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.race, "race");
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<int>(ref this.requiredCount, "requiredCount", 0, false);
			Scribe_Values.Look<string>(ref this.inSignalDecrement, "inSignalChangeCount", null, false);
			Scribe_Values.Look<string>(ref this.outSignalPawnsNotAvailable, "outSignalPawnsNotAvailable", null, false);
		}

		// Token: 0x04004368 RID: 17256
		public ThingDef race;

		// Token: 0x04004369 RID: 17257
		public int requiredCount;

		// Token: 0x0400436A RID: 17258
		public MapParent mapParent;

		// Token: 0x0400436B RID: 17259
		public string inSignalDecrement;

		// Token: 0x0400436C RID: 17260
		public string outSignalPawnsNotAvailable;

		// Token: 0x0400436D RID: 17261
		private const int CheckInterval = 500;
	}
}
