using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020019A1 RID: 6561
	public class CompStudyUnlocks : ThingComp, IThingStudied
	{
		// Token: 0x17001CA7 RID: 7335
		// (get) Token: 0x0600A4C4 RID: 42180 RVA: 0x003ADFCA File Offset: 0x003AC1CA
		protected CompProperties_StudyUnlocks Props
		{
			get
			{
				return (CompProperties_StudyUnlocks)this.props;
			}
		}

		// Token: 0x17001CA8 RID: 7336
		// (get) Token: 0x0600A4C5 RID: 42181 RVA: 0x003ADFD8 File Offset: 0x003AC1D8
		protected CompStudiable StudyComp
		{
			get
			{
				CompStudiable result;
				if ((result = this.studyInt) == null)
				{
					result = (this.studyInt = this.parent.TryGetComp<CompStudiable>());
				}
				return result;
			}
		}

		// Token: 0x17001CA9 RID: 7337
		// (get) Token: 0x0600A4C6 RID: 42182 RVA: 0x003AE003 File Offset: 0x003AC203
		public IReadOnlyList<ChoiceLetter> Letters
		{
			get
			{
				return this.letters;
			}
		}

		// Token: 0x17001CAA RID: 7338
		// (get) Token: 0x0600A4C7 RID: 42183 RVA: 0x003AE00B File Offset: 0x003AC20B
		public int Progress
		{
			get
			{
				return this.studyProgress;
			}
		}

		// Token: 0x17001CAB RID: 7339
		// (get) Token: 0x0600A4C8 RID: 42184 RVA: 0x003AE013 File Offset: 0x003AC213
		public bool Completed
		{
			get
			{
				return this.nextIndex >= this.Props.studyNotes.Count;
			}
		}

		// Token: 0x17001CAC RID: 7340
		// (get) Token: 0x0600A4C9 RID: 42185 RVA: 0x003AE030 File Offset: 0x003AC230
		public float? StudyKnowledgeAmount
		{
			get
			{
				if (this.studyProgress == 0)
				{
					return this.Props.defaultStudyAmount;
				}
				return this.Props.studyNotes[this.studyProgress - 1].studyKnowledgeAmount;
			}
		}

		// Token: 0x0600A4CA RID: 42186 RVA: 0x003AE063 File Offset: 0x003AC263
		public override void PostPostMake()
		{
			this.SetupStudyThresholds();
		}

		// Token: 0x0600A4CB RID: 42187 RVA: 0x003AE06C File Offset: 0x003AC26C
		public void OnStudied(Pawn studier, float amount, KnowledgeCategoryDef category = null)
		{
			if (this.Completed)
			{
				return;
			}
			float anomalyKnowledgeGained = this.StudyComp.anomalyKnowledgeGained;
			int num = this.nextIndex;
			while (num < this.Props.studyNotes.Count && anomalyKnowledgeGained >= this.studyThresholds[num])
			{
				this.RegisterStudyLevel(studier, num);
				num++;
			}
		}

		// Token: 0x0600A4CC RID: 42188 RVA: 0x003AE0C8 File Offset: 0x003AC2C8
		private void RegisterStudyLevel(Pawn studier, int i)
		{
			if (this.nextIndex > i)
			{
				return;
			}
			StudyNote studyNote = this.Props.studyNotes[i];
			this.nextIndex = i + 1;
			this.studyProgress = this.nextIndex;
			TaggedString label = studyNote.label.Formatted(studier.Named("PAWN"));
			TaggedString text = studyNote.text.Formatted(studier.Named("PAWN"));
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, LetterDefOf.NeutralEvent, this.parent, null, null, null);
			Find.LetterStack.ReceiveLetter(let, null, 0, true);
			ChoiceLetter choiceLetter = LetterMaker.MakeLetter(label, text, LetterDefOf.NeutralEvent, this.parent, null, null, null);
			choiceLetter.arrivalTick = Find.TickManager.TicksGame;
			this.letters.Add(choiceLetter);
			this.Notify_StudyLevelChanged(choiceLetter);
		}

		// Token: 0x0600A4CD RID: 42189 RVA: 0x003AE199 File Offset: 0x003AC399
		protected virtual void Notify_StudyLevelChanged(ChoiceLetter keptLetter)
		{
		}

		// Token: 0x0600A4CE RID: 42190 RVA: 0x003AE19B File Offset: 0x003AC39B
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompStudyUnlocks.<CompGetGizmosExtra>d__21 <CompGetGizmosExtra>d__ = new CompStudyUnlocks.<CompGetGizmosExtra>d__21(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A4CF RID: 42191 RVA: 0x003AE1AC File Offset: 0x003AC3AC
		private void SetupStudyThresholds()
		{
			foreach (StudyNote studyNote in this.Props.studyNotes)
			{
				this.studyThresholds.Add((studyNote.threshold != 0f) ? studyNote.threshold : studyNote.thresholdRange.RandomInRange);
			}
		}

		// Token: 0x0600A4D0 RID: 42192 RVA: 0x003AE228 File Offset: 0x003AC428
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.nextIndex, "nextIndex", 0, false);
			Scribe_Values.Look<int>(ref this.studyProgress, "studyProgress", 0, false);
			Scribe_Collections.Look<float>(ref this.studyThresholds, "studyThresholds", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<ChoiceLetter>(ref this.letters, "letters", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.letters == null)
				{
					this.letters = new List<ChoiceLetter>();
				}
				if (this.studyThresholds == null)
				{
					this.studyThresholds = new List<float>();
					this.SetupStudyThresholds();
				}
			}
		}

		// Token: 0x0400616A RID: 24938
		private List<float> studyThresholds = new List<float>();

		// Token: 0x0400616B RID: 24939
		protected List<ChoiceLetter> letters = new List<ChoiceLetter>();

		// Token: 0x0400616C RID: 24940
		protected int nextIndex;

		// Token: 0x0400616D RID: 24941
		protected int studyProgress;

		// Token: 0x0400616E RID: 24942
		private CompStudiable studyInt;
	}
}
