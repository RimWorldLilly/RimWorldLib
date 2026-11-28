using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E1E RID: 7710
	public class Tutor : IExposable
	{
		// Token: 0x0600C855 RID: 51285 RVA: 0x00497030 File Offset: 0x00495230
		public void ExposeData()
		{
			Scribe_Deep.Look<ActiveLessonHandler>(ref this.activeLesson, "activeLesson", Array.Empty<object>());
			Scribe_Deep.Look<LearningReadout>(ref this.learningReadout, "learningReadout", Array.Empty<object>());
			Scribe_Deep.Look<TutorialState>(ref this.tutorialState, "tutorialState", Array.Empty<object>());
		}

		// Token: 0x0600C856 RID: 51286 RVA: 0x0049707C File Offset: 0x0049527C
		internal void TutorUpdate()
		{
			this.activeLesson.ActiveLessonUpdate();
			this.learningReadout.LearningReadoutUpdate();
		}

		// Token: 0x0600C857 RID: 51287 RVA: 0x00497094 File Offset: 0x00495294
		internal void TutorOnGUI()
		{
			this.activeLesson.ActiveLessonOnGUI();
			this.learningReadout.LearningReadoutOnGUI();
		}

		// Token: 0x04007201 RID: 29185
		public ActiveLessonHandler activeLesson = new ActiveLessonHandler();

		// Token: 0x04007202 RID: 29186
		public LearningReadout learningReadout = new LearningReadout();

		// Token: 0x04007203 RID: 29187
		public TutorialState tutorialState = new TutorialState();
	}
}
