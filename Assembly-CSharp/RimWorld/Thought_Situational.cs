using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015E3 RID: 5603
	public class Thought_Situational : Thought
	{
		// Token: 0x17001821 RID: 6177
		// (get) Token: 0x06008B39 RID: 35641 RVA: 0x0032708C File Offset: 0x0032528C
		public bool Active
		{
			get
			{
				return this.curStageIndex >= 0;
			}
		}

		// Token: 0x17001822 RID: 6178
		// (get) Token: 0x06008B3A RID: 35642 RVA: 0x0032709A File Offset: 0x0032529A
		public override int CurStageIndex
		{
			get
			{
				return this.curStageIndex;
			}
		}

		// Token: 0x17001823 RID: 6179
		// (get) Token: 0x06008B3B RID: 35643 RVA: 0x003270A4 File Offset: 0x003252A4
		public override string LabelCap
		{
			get
			{
				if (!this.reason.NullOrEmpty())
				{
					string text = base.CurStage.label.Formatted(this.reason.Named("REASON"), this.pawn.Named("PAWN")).CapitalizeFirst();
					if (this.def.Worker != null)
					{
						text = this.def.Worker.PostProcessLabel(this.pawn, text);
					}
					return text;
				}
				return base.LabelCap;
			}
		}

		// Token: 0x06008B3C RID: 35644 RVA: 0x0032712C File Offset: 0x0032532C
		public void RecalculateState()
		{
			bool active = this.Active;
			ThoughtState thoughtState = this.CurrentStateInternal();
			if (thoughtState.ActiveFor(this.def))
			{
				this.curStageIndex = thoughtState.StageIndexFor(this.def);
				this.reason = thoughtState.Reason;
			}
			else
			{
				this.curStageIndex = -1;
			}
			bool active2 = this.Active;
			if (active && !active2)
			{
				this.Notify_BecameInactive();
			}
			if (!active && active2)
			{
				this.Notify_BecameActive();
			}
		}

		// Token: 0x06008B3D RID: 35645 RVA: 0x003271A0 File Offset: 0x003253A0
		protected virtual void Notify_BecameActive()
		{
			if (this.def.producesMemoryThought != null)
			{
				Pawn_NeedsTracker needs = this.pawn.needs;
				if (needs == null)
				{
					return;
				}
				Need_Mood mood = needs.mood;
				if (mood == null)
				{
					return;
				}
				mood.thoughts.memories.RemoveMemoriesOfDef(this.def.producesMemoryThought);
			}
		}

		// Token: 0x06008B3E RID: 35646 RVA: 0x003271F0 File Offset: 0x003253F0
		protected virtual void Notify_BecameInactive()
		{
			if (this.def.producesMemoryThought != null)
			{
				Pawn_NeedsTracker needs = this.pawn.needs;
				if (needs == null)
				{
					return;
				}
				Need_Mood mood = needs.mood;
				if (mood == null)
				{
					return;
				}
				mood.thoughts.memories.TryGainMemory(this.def.producesMemoryThought, null, this.sourcePrecept);
			}
		}

		// Token: 0x06008B3F RID: 35647 RVA: 0x00327245 File Offset: 0x00325445
		protected virtual ThoughtState CurrentStateInternal()
		{
			return this.def.Worker.CurrentState(this.pawn);
		}

		// Token: 0x04005592 RID: 21906
		private int curStageIndex = -1;

		// Token: 0x04005593 RID: 21907
		protected string reason;
	}
}
