using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019A3 RID: 6563
	public class CompStudyUnlocksMonolith : CompStudyUnlocks
	{
		// Token: 0x17001CAD RID: 7341
		// (get) Token: 0x0600A4D4 RID: 42196 RVA: 0x003AE317 File Offset: 0x003AC517
		protected new CompProperties_StudyUnlocksMonolith Props
		{
			get
			{
				return (CompProperties_StudyUnlocksMonolith)this.props;
			}
		}

		// Token: 0x0600A4D5 RID: 42197 RVA: 0x003AE324 File Offset: 0x003AC524
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.nextIndex = Find.Anomaly.MonolithNextIndex;
			this.studyProgress = Find.Anomaly.MonolithStudyProgress;
		}

		// Token: 0x0600A4D6 RID: 42198 RVA: 0x003AE346 File Offset: 0x003AC546
		protected override void Notify_StudyLevelChanged(ChoiceLetter keptLetter)
		{
			Find.Anomaly.Notify_MonolithStudyIncreased(keptLetter, this.nextIndex, this.studyProgress);
			this.letters.Clear();
		}
	}
}
