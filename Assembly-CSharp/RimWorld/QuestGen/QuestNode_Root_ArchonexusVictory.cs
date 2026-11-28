using System;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002263 RID: 8803
	public class QuestNode_Root_ArchonexusVictory : QuestNode
	{
		// Token: 0x1700260E RID: 9742
		// (get) Token: 0x0600D981 RID: 55681 RVA: 0x0050999D File Offset: 0x00507B9D
		public Faction CivilOutlander
		{
			get
			{
				return Find.FactionManager.AllFactionsVisible.FirstOrDefault((Faction f) => f.def == FactionDefOf.OutlanderCivil);
			}
		}

		// Token: 0x1700260F RID: 9743
		// (get) Token: 0x0600D982 RID: 55682 RVA: 0x005099CD File Offset: 0x00507BCD
		public Faction RoughOutlander
		{
			get
			{
				return Find.FactionManager.AllFactionsVisible.FirstOrDefault((Faction f) => f.def == FactionDefOf.OutlanderRough);
			}
		}

		// Token: 0x17002610 RID: 9744
		// (get) Token: 0x0600D983 RID: 55683 RVA: 0x005099FD File Offset: 0x00507BFD
		public Faction RoughTribe
		{
			get
			{
				return Find.FactionManager.AllFactionsVisible.FirstOrDefault((Faction f) => f.def == FactionDefOf.TribeRough);
			}
		}

		// Token: 0x0600D984 RID: 55684 RVA: 0x00509A30 File Offset: 0x00507C30
		protected override void RunInt()
		{
			if (!ModLister.CheckIdeology("Archonexus victory"))
			{
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			quest.AddPart(new QuestPart_SubquestGenerator_ArchonexusVictory
			{
				inSignalEnable = slate.Get<string>("inSignal", null, false),
				interval = new IntRange(0, 0),
				maxSuccessfulSubquests = 3,
				maxActiveSubquests = 1,
				civilOutlander = this.CivilOutlander,
				roughOutlander = this.RoughOutlander,
				roughTribe = this.RoughTribe,
				subquestDefs = 
				{
					QuestScriptDefOf.EndGame_ArchonexusVictory_FirstCycle,
					QuestScriptDefOf.EndGame_ArchonexusVictory_SecondCycle,
					QuestScriptDefOf.EndGame_ArchonexusVictory_ThirdCycle
				}
			});
		}

		// Token: 0x0600D985 RID: 55685 RVA: 0x00509AE3 File Offset: 0x00507CE3
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}
	}
}
