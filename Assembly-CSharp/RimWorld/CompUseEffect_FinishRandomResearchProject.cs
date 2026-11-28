using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A37 RID: 6711
	public class CompUseEffect_FinishRandomResearchProject : CompUseEffect
	{
		// Token: 0x0600A861 RID: 43105 RVA: 0x003BD434 File Offset: 0x003BB634
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			ResearchProjectDef project = Find.ResearchManager.GetProject(null);
			if (project != null)
			{
				this.FinishInstantly(project, usedBy);
			}
		}

		// Token: 0x0600A862 RID: 43106 RVA: 0x003BD45F File Offset: 0x003BB65F
		public override AcceptanceReport CanBeUsedBy(Pawn p)
		{
			if (Find.ResearchManager.GetProject(null) == null)
			{
				return "NoActiveResearchProjectToFinish".Translate();
			}
			return true;
		}

		// Token: 0x0600A863 RID: 43107 RVA: 0x003BD484 File Offset: 0x003BB684
		private void FinishInstantly(ResearchProjectDef proj, Pawn usedBy)
		{
			Find.ResearchManager.FinishProject(proj, true, null, true);
			Messages.Message("MessageResearchProjectFinishedByItem".Translate(proj.LabelCap), usedBy, MessageTypeDefOf.PositiveEvent, true);
		}
	}
}
