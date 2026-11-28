using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E11 RID: 7697
	public class Instruction_LearnConcept : Lesson_Instruction
	{
		// Token: 0x170024ED RID: 9453
		// (get) Token: 0x0600C7F8 RID: 51192 RVA: 0x00495E53 File Offset: 0x00494053
		protected override float ProgressPercent
		{
			get
			{
				return PlayerKnowledgeDatabase.GetKnowledge(this.def.concept);
			}
		}

		// Token: 0x0600C7F9 RID: 51193 RVA: 0x00495E65 File Offset: 0x00494065
		public override void OnActivated()
		{
			PlayerKnowledgeDatabase.SetKnowledge(this.def.concept, 0f);
			base.OnActivated();
		}

		// Token: 0x0600C7FA RID: 51194 RVA: 0x00495E82 File Offset: 0x00494082
		public override void LessonUpdate()
		{
			base.LessonUpdate();
			if (PlayerKnowledgeDatabase.IsComplete(this.def.concept))
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
