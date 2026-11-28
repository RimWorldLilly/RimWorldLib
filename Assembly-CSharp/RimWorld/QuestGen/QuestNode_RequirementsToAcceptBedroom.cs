using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200222F RID: 8751
	public class QuestNode_RequirementsToAcceptBedroom : QuestNode
	{
		// Token: 0x0600D8BF RID: 55487 RVA: 0x00505408 File Offset: 0x00503608
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			QuestPart_RequirementsToAcceptBedroom questPart_RequirementsToAcceptBedroom = new QuestPart_RequirementsToAcceptBedroom();
			questPart_RequirementsToAcceptBedroom.targetPawns = (from p in this.pawns.GetValue(QuestGen.slate)
			where p.royalty != null && p.royalty.HighestTitleWithBedroomRequirements() != null
			orderby p.royalty.HighestTitleWithBedroomRequirements().def.seniority descending
			select p).ToList<Pawn>();
			questPart_RequirementsToAcceptBedroom.mapParent = slate.Get<Map>("map", null, false).Parent;
			quest.AddPart(questPart_RequirementsToAcceptBedroom);
		}

		// Token: 0x0600D8C0 RID: 55488 RVA: 0x005054A5 File Offset: 0x005036A5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x04008A6D RID: 35437
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
