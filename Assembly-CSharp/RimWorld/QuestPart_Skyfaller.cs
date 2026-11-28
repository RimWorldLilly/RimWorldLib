using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001119 RID: 4377
	public class QuestPart_Skyfaller : QuestPart
	{
		// Token: 0x170012C7 RID: 4807
		// (get) Token: 0x06006D7A RID: 28026 RVA: 0x00265BB8 File Offset: 0x00263DB8
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_Skyfaller.<get_QuestLookTargets>d__6 <get_QuestLookTargets>d__ = new QuestPart_Skyfaller.<get_QuestLookTargets>d__6(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006D7B RID: 28027 RVA: 0x00265BC8 File Offset: 0x00263DC8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (this.mapParent == null || !this.mapParent.HasMap || !this.quest.IsParentSuitableForQuest(this.mapParent))
			{
				this.mapParent = this.quest.TryFindNewSuitableMapParentForRetarget();
			}
			if (signal.tag == this.inSignal && this.mapParent != null && this.mapParent.HasMap)
			{
				this.items.RemoveAll((Thing x) => x.Destroyed);
				Map map = this.mapParent.Map;
				SkyfallerMaker.SpawnSkyfaller(this.skyfallerDef, this.items, this.dropSpot, map);
			}
		}

		// Token: 0x040045BC RID: 17852
		public ThingDef skyfallerDef;

		// Token: 0x040045BD RID: 17853
		public string inSignal;

		// Token: 0x040045BE RID: 17854
		public IntVec3 dropSpot = IntVec3.Invalid;

		// Token: 0x040045BF RID: 17855
		public MapParent mapParent;

		// Token: 0x040045C0 RID: 17856
		private List<Thing> items = new List<Thing>();
	}
}
