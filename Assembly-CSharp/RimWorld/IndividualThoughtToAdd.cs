using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014C7 RID: 5319
	public struct IndividualThoughtToAdd
	{
		// Token: 0x17001632 RID: 5682
		// (get) Token: 0x0600820A RID: 33290 RVA: 0x002E6A68 File Offset: 0x002E4C68
		public string LabelCap
		{
			get
			{
				string text = this.thought.LabelCap;
				float num = this.thought.MoodOffset();
				if (num != 0f)
				{
					text = text + " " + Mathf.RoundToInt(num).ToStringWithSign();
				}
				return text;
			}
		}

		// Token: 0x0600820B RID: 33291 RVA: 0x002E6AB0 File Offset: 0x002E4CB0
		public IndividualThoughtToAdd(ThoughtDef thoughtDef, Pawn addTo, Pawn otherPawn = null, float moodPowerFactor = 1f, float opinionOffsetFactor = 1f)
		{
			this.addTo = addTo;
			this.otherPawn = otherPawn;
			this.thought = (Thought_Memory)ThoughtMaker.MakeThought(thoughtDef);
			this.thought.moodPowerFactor = moodPowerFactor;
			this.thought.otherPawn = otherPawn;
			this.thought.pawn = addTo;
			Thought_MemorySocial thought_MemorySocial = this.thought as Thought_MemorySocial;
			if (thought_MemorySocial != null)
			{
				thought_MemorySocial.opinionOffset *= opinionOffsetFactor;
			}
		}

		// Token: 0x0600820C RID: 33292 RVA: 0x002E6B20 File Offset: 0x002E4D20
		public void Add()
		{
			if (this.addTo.needs != null && this.addTo.needs.mood != null)
			{
				this.addTo.needs.mood.thoughts.memories.TryGainMemory(this.thought, this.otherPawn);
			}
		}

		// Token: 0x0400508A RID: 20618
		public Thought_Memory thought;

		// Token: 0x0400508B RID: 20619
		public Pawn addTo;

		// Token: 0x0400508C RID: 20620
		private Pawn otherPawn;
	}
}
