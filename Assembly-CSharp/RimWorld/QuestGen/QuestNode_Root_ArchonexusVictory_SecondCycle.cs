using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02002266 RID: 8806
	public class QuestNode_Root_ArchonexusVictory_SecondCycle : QuestNode_Root_ArchonexusVictory_Cycle
	{
		// Token: 0x17002613 RID: 9747
		// (get) Token: 0x0600D990 RID: 55696 RVA: 0x00509F4E File Offset: 0x0050814E
		protected override int ArchonexusCycle
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x0600D991 RID: 55697 RVA: 0x00509F54 File Offset: 0x00508154
		protected override void RunInt()
		{
			base.RunInt();
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			base.TryAddStudyRequirement(quest, slate, ThingDefOf.MajorArchotechStructureStudiable);
			quest.DialogWithCloseBehavior("[resolvedQuestDescription]", null, quest.AddedSignal, null, null, QuestPart.SignalListenMode.NotYetAcceptedOnly, QuestPartDialogCloseAction.CloseActionKey.ArchonexusVictorySound2nd);
			Faction faction = slate.Get<Faction>("roughOutlander", null, false);
			if (faction != null)
			{
				quest.RequirementsToAcceptFactionRelation(faction, FactionRelationKind.Ally, true);
			}
			base.PickNewColony(faction, WorldObjectDefOf.Settlement_ThirdArchonexusCycle, SoundDefOf.GameStartSting_SecondArchonexusCycle, 2);
			slate.Set<bool>("factionless", faction == null, false);
		}

		// Token: 0x04008B06 RID: 35590
		private const int MaxAllowedRelicsToTake = 2;
	}
}
