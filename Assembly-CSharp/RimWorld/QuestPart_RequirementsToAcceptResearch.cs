using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010EB RID: 4331
	public class QuestPart_RequirementsToAcceptResearch : QuestPart_RequirementsToAccept
	{
		// Token: 0x06006C31 RID: 27697 RVA: 0x0025FB00 File Offset: 0x0025DD00
		public override AcceptanceReport CanAccept()
		{
			ResearchProjectDef researchProjectDef = this.project;
			if (researchProjectDef != null && !researchProjectDef.IsFinished)
			{
				return new AcceptanceReport("QuestRequiresResearch".Translate());
			}
			return true;
		}

		// Token: 0x06006C32 RID: 27698 RVA: 0x0025FB3A File Offset: 0x0025DD3A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ResearchProjectDef>(ref this.project, "project");
		}

		// Token: 0x040044E4 RID: 17636
		public ResearchProjectDef project;
	}
}
