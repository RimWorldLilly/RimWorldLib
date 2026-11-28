using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200199E RID: 6558
	public class CompStudiableMonolith : CompStudiable
	{
		// Token: 0x17001CA5 RID: 7333
		// (get) Token: 0x0600A4BB RID: 42171 RVA: 0x003ADF0D File Offset: 0x003AC10D
		public override float AnomalyKnowledge
		{
			get
			{
				if (!Find.Anomaly.QuestlineEnded)
				{
					return base.AnomalyKnowledge;
				}
				return 4f;
			}
		}

		// Token: 0x17001CA6 RID: 7334
		// (get) Token: 0x0600A4BC RID: 42172 RVA: 0x003ADF27 File Offset: 0x003AC127
		public override KnowledgeCategoryDef KnowledgeCategory
		{
			get
			{
				return Find.Anomaly.LevelDef.monolithStudyCategory;
			}
		}

		// Token: 0x0600A4BD RID: 42173 RVA: 0x003ADF38 File Offset: 0x003AC138
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.anomalyKnowledgeGained = Find.Anomaly.monolithAnomalyKnowledge;
		}

		// Token: 0x0600A4BE RID: 42174 RVA: 0x003ADF51 File Offset: 0x003AC151
		public override void Study(Pawn studier, float studyAmount, float anomalyKnowledgeAmount = 0f)
		{
			base.Study(studier, studyAmount, anomalyKnowledgeAmount);
			Find.Anomaly.monolithAnomalyKnowledge = this.anomalyKnowledgeGained;
		}

		// Token: 0x04006161 RID: 24929
		private const float FinishedStudyAmount = 4f;
	}
}
