using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001062 RID: 4194
	public class QuestPart_Filter_AnyColonistAlive : QuestPart_Filter
	{
		// Token: 0x06006969 RID: 26985 RVA: 0x00253097 File Offset: 0x00251297
		protected override bool Pass(SignalArgs args)
		{
			return this.mapParent != null && this.mapParent.HasMap && this.mapParent.Map.mapPawns.ColonistCount != 0;
		}

		// Token: 0x0600696A RID: 26986 RVA: 0x002530C8 File Offset: 0x002512C8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
		}

		// Token: 0x0400430E RID: 17166
		public MapParent mapParent;
	}
}
