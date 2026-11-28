using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DFF RID: 7679
	public class ActiveLessonHandler : IExposable
	{
		// Token: 0x170024D8 RID: 9432
		// (get) Token: 0x0600C791 RID: 51089 RVA: 0x004946D7 File Offset: 0x004928D7
		public Lesson Current
		{
			get
			{
				return this.activeLesson;
			}
		}

		// Token: 0x170024D9 RID: 9433
		// (get) Token: 0x0600C792 RID: 51090 RVA: 0x004946DF File Offset: 0x004928DF
		public bool ActiveLessonVisible
		{
			get
			{
				return this.activeLesson != null && !Find.WindowStack.WindowsPreventDrawTutor;
			}
		}

		// Token: 0x0600C793 RID: 51091 RVA: 0x004946F8 File Offset: 0x004928F8
		public void ExposeData()
		{
			Scribe_Deep.Look<Lesson>(ref this.activeLesson, "activeLesson", Array.Empty<object>());
		}

		// Token: 0x0600C794 RID: 51092 RVA: 0x00494710 File Offset: 0x00492910
		public void Activate(InstructionDef id)
		{
			Lesson_Instruction lesson_Instruction = this.activeLesson as Lesson_Instruction;
			if (lesson_Instruction != null && id == lesson_Instruction.def)
			{
				return;
			}
			Lesson_Instruction lesson_Instruction2 = (Lesson_Instruction)Activator.CreateInstance(id.instructionClass);
			lesson_Instruction2.def = id;
			this.activeLesson = lesson_Instruction2;
			this.activeLesson.OnActivated();
		}

		// Token: 0x0600C795 RID: 51093 RVA: 0x00494760 File Offset: 0x00492960
		public void Activate(Lesson lesson)
		{
			Lesson_Note lesson_Note = lesson as Lesson_Note;
			if (lesson_Note != null && this.activeLesson != null)
			{
				lesson_Note.doFadeIn = false;
			}
			this.activeLesson = lesson;
			this.activeLesson.OnActivated();
		}

		// Token: 0x0600C796 RID: 51094 RVA: 0x00494798 File Offset: 0x00492998
		public void Deactivate()
		{
			Lesson lesson = this.activeLesson;
			this.activeLesson = null;
			if (lesson != null)
			{
				lesson.PostDeactivated();
			}
		}

		// Token: 0x0600C797 RID: 51095 RVA: 0x004947BC File Offset: 0x004929BC
		public void ActiveLessonOnGUI()
		{
			if (Time.timeSinceLevelLoad < 0.01f || !this.ActiveLessonVisible)
			{
				return;
			}
			this.activeLesson.LessonOnGUI();
		}

		// Token: 0x0600C798 RID: 51096 RVA: 0x004947DE File Offset: 0x004929DE
		public void ActiveLessonUpdate()
		{
			if (Time.timeSinceLevelLoad < 0.01f || !this.ActiveLessonVisible)
			{
				return;
			}
			this.activeLesson.LessonUpdate();
		}

		// Token: 0x0600C799 RID: 51097 RVA: 0x00494800 File Offset: 0x00492A00
		public void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
			if (this.Current != null)
			{
				this.Current.Notify_KnowledgeDemonstrated(conc);
			}
		}

		// Token: 0x040071CB RID: 29131
		private Lesson activeLesson;
	}
}
