using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02002265 RID: 8805
	public class QuestNode_Root_ArchonexusVictory_FirstCycle : QuestNode_Root_ArchonexusVictory_Cycle
	{
		// Token: 0x17002612 RID: 9746
		// (get) Token: 0x0600D98D RID: 55693 RVA: 0x00509EE8 File Offset: 0x005080E8
		protected override int ArchonexusCycle
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600D98E RID: 55694 RVA: 0x00509EEC File Offset: 0x005080EC
		protected override void RunInt()
		{
			base.RunInt();
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Faction faction = slate.Get<Faction>("civilOutlander", null, false);
			if (faction != null)
			{
				quest.RequirementsToAcceptFactionRelation(faction, FactionRelationKind.Ally, true);
			}
			base.PickNewColony(faction, WorldObjectDefOf.Settlement_SecondArchonexusCycle, SoundDefOf.GameStartSting_FirstArchonexusCycle, 1);
			slate.Set<bool>("factionless", faction == null, false);
		}
	}
}
