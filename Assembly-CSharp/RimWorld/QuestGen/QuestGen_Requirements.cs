using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02002162 RID: 8546
	public static class QuestGen_Requirements
	{
		// Token: 0x0600D5E0 RID: 54752 RVA: 0x004F68F4 File Offset: 0x004F4AF4
		public static QuestPart_RequirementsToAcceptFactionRelation RequirementsToAcceptFactionRelation(this Quest quest, Faction faction, FactionRelationKind relationKind, bool acceptIfDefeated = false)
		{
			QuestPart_RequirementsToAcceptFactionRelation questPart_RequirementsToAcceptFactionRelation = new QuestPart_RequirementsToAcceptFactionRelation();
			questPart_RequirementsToAcceptFactionRelation.otherFaction = faction;
			questPart_RequirementsToAcceptFactionRelation.relationKind = relationKind;
			questPart_RequirementsToAcceptFactionRelation.acceptIfDefeated = acceptIfDefeated;
			quest.AddPart(questPart_RequirementsToAcceptFactionRelation);
			return questPart_RequirementsToAcceptFactionRelation;
		}
	}
}
