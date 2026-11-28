using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010D4 RID: 4308
	public class QuestPart_Notify_PlayerRaidedSomeone : QuestPart
	{
		// Token: 0x06006BBD RID: 27581 RVA: 0x0025D42C File Offset: 0x0025B62C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				IEnumerable<Pawn> enumerable = null;
				if (this.getRaidersFromMap != null)
				{
					enumerable = this.getRaidersFromMap.mapPawns.FreeColonistsSpawned;
				}
				if (this.getRaidersFromMapParent != null)
				{
					Map map = this.getRaidersFromMapParent.Map;
					if (map == null)
					{
						Log.Error("Cannot resolve map for QuestPart_NotifyPlayerRaidedSomeone.getRaidersFromWorldObjectMap!");
						return;
					}
					enumerable = map.mapPawns.FreeColonistsSpawned;
				}
				if (enumerable == null)
				{
					Log.Error("No raiders could be determined to notify ideoligons!");
					return;
				}
				IdeoUtility.Notify_PlayerRaidedSomeone(enumerable);
			}
		}

		// Token: 0x06006BBE RID: 27582 RVA: 0x0025D4A9 File Offset: 0x0025B6A9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Map>(ref this.getRaidersFromMap, "getRaidersFromMap", false);
			Scribe_References.Look<MapParent>(ref this.getRaidersFromMapParent, "getRaidersFromMapParent", false);
		}

		// Token: 0x0400447C RID: 17532
		public string inSignal;

		// Token: 0x0400447D RID: 17533
		public Map getRaidersFromMap;

		// Token: 0x0400447E RID: 17534
		public MapParent getRaidersFromMapParent;
	}
}
