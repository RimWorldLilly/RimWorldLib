using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200105F RID: 4191
	public class QuestPart_Filter_AllRequiredThingsLoaded : QuestPart_Filter
	{
		// Token: 0x06006961 RID: 26977 RVA: 0x00252EEC File Offset: 0x002510EC
		protected override bool Pass(SignalArgs args)
		{
			if (this.shuttle == null)
			{
				return false;
			}
			CompShuttle compShuttle = this.shuttle.TryGetComp<CompShuttle>();
			return compShuttle != null && compShuttle.AllRequiredThingsLoaded;
		}

		// Token: 0x06006962 RID: 26978 RVA: 0x00252F1A File Offset: 0x0025111A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
		}

		// Token: 0x0400430C RID: 17164
		public Thing shuttle;
	}
}
