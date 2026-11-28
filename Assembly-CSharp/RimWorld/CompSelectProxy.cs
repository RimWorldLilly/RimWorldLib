using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001964 RID: 6500
	public class CompSelectProxy : ThingComp
	{
		// Token: 0x0600A327 RID: 41767 RVA: 0x003A4598 File Offset: 0x003A2798
		public override void PostExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thingToSelect, "thingToSelect", false);
		}

		// Token: 0x04006061 RID: 24673
		public Thing thingToSelect;
	}
}
