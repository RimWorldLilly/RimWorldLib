using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016C9 RID: 5833
	public class RitualOutcomeComp_DataThingPresence : RitualOutcomeComp_Data
	{
		// Token: 0x060091FF RID: 37375 RVA: 0x003554C9 File Offset: 0x003536C9
		public override void Reset()
		{
			this.presentForTicks.Clear();
		}

		// Token: 0x06009200 RID: 37376 RVA: 0x003554D8 File Offset: 0x003536D8
		public override void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.presentForTicks.RemoveAll((KeyValuePair<Thing, float> x) => x.Key.Destroyed);
			}
			Scribe_Collections.Look<Thing, float>(ref this.presentForTicks, "presentForTicks", LookMode.Reference, LookMode.Value, ref this.tmpPresentThing, ref this.tmpPresentTicks, true, false, false);
		}

		// Token: 0x04005885 RID: 22661
		public Dictionary<Thing, float> presentForTicks = new Dictionary<Thing, float>();

		// Token: 0x04005886 RID: 22662
		private List<Thing> tmpPresentThing;

		// Token: 0x04005887 RID: 22663
		private List<float> tmpPresentTicks;
	}
}
