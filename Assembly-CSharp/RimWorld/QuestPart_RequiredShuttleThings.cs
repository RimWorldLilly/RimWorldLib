using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010DB RID: 4315
	public class QuestPart_RequiredShuttleThings : QuestPart
	{
		// Token: 0x06006BE4 RID: 27620 RVA: 0x0025DD10 File Offset: 0x0025BF10
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				CompShuttle compShuttle = this.shuttle.TryGetComp<CompShuttle>();
				if (compShuttle != null)
				{
					compShuttle.requireAllColonistsOnMap = this.requireAllColonistsOnMap;
					compShuttle.requiredColonistCount = this.requiredColonistCount;
				}
			}
		}

		// Token: 0x06006BE5 RID: 27621 RVA: 0x0025DD60 File Offset: 0x0025BF60
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.requireAllColonistsOnMap, "requireAllColonistsOnMap", false, false);
			Scribe_Values.Look<int>(ref this.requiredColonistCount, "requiredColonistCount", -1, false);
		}

		// Token: 0x0400449C RID: 17564
		public Thing shuttle;

		// Token: 0x0400449D RID: 17565
		public MapParent mapParent;

		// Token: 0x0400449E RID: 17566
		public bool requireAllColonistsOnMap;

		// Token: 0x0400449F RID: 17567
		public int requiredColonistCount = -1;

		// Token: 0x040044A0 RID: 17568
		public string inSignal;
	}
}
