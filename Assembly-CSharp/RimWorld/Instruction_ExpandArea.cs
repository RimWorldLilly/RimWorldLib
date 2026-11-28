using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E0D RID: 7693
	public abstract class Instruction_ExpandArea : Lesson_Instruction
	{
		// Token: 0x170024E8 RID: 9448
		// (get) Token: 0x0600C7EA RID: 51178
		protected abstract Area MyArea { get; }

		// Token: 0x170024E9 RID: 9449
		// (get) Token: 0x0600C7EB RID: 51179 RVA: 0x00495C68 File Offset: 0x00493E68
		protected override float ProgressPercent
		{
			get
			{
				return (float)(this.MyArea.TrueCount - this.startingAreaCount) / (float)this.def.targetCount;
			}
		}

		// Token: 0x0600C7EC RID: 51180 RVA: 0x00495C8A File Offset: 0x00493E8A
		public override void OnActivated()
		{
			base.OnActivated();
			this.startingAreaCount = this.MyArea.TrueCount;
		}

		// Token: 0x0600C7ED RID: 51181 RVA: 0x00495CA3 File Offset: 0x00493EA3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.startingAreaCount, "startingAreaCount", 0, false);
		}

		// Token: 0x0600C7EE RID: 51182 RVA: 0x00495CBD File Offset: 0x00493EBD
		public override void LessonUpdate()
		{
			if (this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x040071DD RID: 29149
		private int startingAreaCount = -1;
	}
}
