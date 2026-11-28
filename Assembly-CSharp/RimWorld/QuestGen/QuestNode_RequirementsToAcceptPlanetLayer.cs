using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002231 RID: 8753
	public class QuestNode_RequirementsToAcceptPlanetLayer : QuestNode
	{
		// Token: 0x0600D8C5 RID: 55493 RVA: 0x00505504 File Offset: 0x00503704
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.quest.AddPart(new QuestPart_RequirementsToAcceptPlanetLayer
			{
				canBeSpace = this.canBeSpace.GetValue(slate),
				layerWhitelist = this.layerWhitelist.GetValue(slate),
				layerBlacklist = this.layerBlacklist.GetValue(slate),
				mapParent = slate.Get<Map>("map", null, false).Parent
			});
		}

		// Token: 0x0600D8C6 RID: 55494 RVA: 0x00505574 File Offset: 0x00503774
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x04008A70 RID: 35440
		public SlateRef<bool> canBeSpace;

		// Token: 0x04008A71 RID: 35441
		public SlateRef<List<PlanetLayerDef>> layerWhitelist;

		// Token: 0x04008A72 RID: 35442
		public SlateRef<List<PlanetLayerDef>> layerBlacklist;
	}
}
