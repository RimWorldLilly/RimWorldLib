using System;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002218 RID: 8728
	public class QuestNode_GiveTechprints : QuestNode
	{
		// Token: 0x0600D877 RID: 55415 RVA: 0x00503E30 File Offset: 0x00502030
		protected override bool TestRunInt(Slate slate)
		{
			ResearchProjectDef researchProjectDef = this.FindTargetProject(slate);
			if (researchProjectDef == null || researchProjectDef.TechprintRequirementMet)
			{
				return false;
			}
			if (this.storeProjectAs.GetValue(slate) != null)
			{
				slate.Set<ResearchProjectDef>(this.storeProjectAs.GetValue(slate), researchProjectDef, false);
			}
			return true;
		}

		// Token: 0x0600D878 RID: 55416 RVA: 0x00503E78 File Offset: 0x00502078
		private ResearchProjectDef FindTargetProject(Slate slate)
		{
			if (this.fixedProject.GetValue(slate) != null)
			{
				return this.fixedProject.GetValue(slate);
			}
			return (from p in DefDatabase<ResearchProjectDef>.AllDefsListForReading
			where !p.IsFinished && !p.TechprintRequirementMet
			select p).RandomElement<ResearchProjectDef>();
		}

		// Token: 0x0600D879 RID: 55417 RVA: 0x00503ED0 File Offset: 0x005020D0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			ResearchProjectDef researchProjectDef = this.FindTargetProject(slate);
			QuestPart_GiveTechprints questPart_GiveTechprints = new QuestPart_GiveTechprints();
			questPart_GiveTechprints.amount = 1;
			questPart_GiveTechprints.project = researchProjectDef;
			questPart_GiveTechprints.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_GiveTechprints.outSignalWasGiven = QuestGenUtility.HardcodedSignalWithQuestID("AddedTechprints");
			QuestGen.quest.AddPart(questPart_GiveTechprints);
			if (this.storeProjectAs.GetValue(slate) != null)
			{
				QuestGen.slate.Set<ResearchProjectDef>(this.storeProjectAs.GetValue(slate), researchProjectDef, false);
			}
		}

		// Token: 0x040089FE RID: 35326
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040089FF RID: 35327
		public SlateRef<ResearchProjectDef> fixedProject;

		// Token: 0x04008A00 RID: 35328
		[NoTranslate]
		public SlateRef<string> storeProjectAs;
	}
}
