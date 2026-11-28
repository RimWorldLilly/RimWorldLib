using System;
using System.Collections.Generic;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x020015E1 RID: 5601
	public class Thought_MemorySocialCumulative : Thought_MemorySocial
	{
		// Token: 0x1700181E RID: 6174
		// (get) Token: 0x06008B2E RID: 35630 RVA: 0x00326E1A File Offset: 0x0032501A
		public override bool ShouldDiscard
		{
			get
			{
				return this.opinionOffset == 0f;
			}
		}

		// Token: 0x06008B2F RID: 35631 RVA: 0x00326E29 File Offset: 0x00325029
		public override float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			if (this.ShouldDiscard)
			{
				return 0f;
			}
			return Mathf.Min(this.opinionOffset, this.def.maxCumulatedOpinionOffset);
		}

		// Token: 0x06008B30 RID: 35632 RVA: 0x00326E68 File Offset: 0x00325068
		public override void ThoughtInterval()
		{
			base.ThoughtInterval();
			if (this.age >= 60000)
			{
				if (this.opinionOffset < 0f)
				{
					this.opinionOffset += 1f;
					if (this.opinionOffset > 0f)
					{
						this.opinionOffset = 0f;
					}
				}
				else if (this.opinionOffset > 0f)
				{
					this.opinionOffset -= 1f;
					if (this.opinionOffset < 0f)
					{
						this.opinionOffset = 0f;
					}
				}
				this.age = 0;
			}
		}

		// Token: 0x06008B31 RID: 35633 RVA: 0x00326F00 File Offset: 0x00325100
		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			showBubble = false;
			List<Thought_Memory> memories = this.pawn.needs.mood.thoughts.memories.Memories;
			for (int i = 0; i < memories.Count; i++)
			{
				if (memories[i].def == this.def)
				{
					Thought_MemorySocialCumulative thought_MemorySocialCumulative = (Thought_MemorySocialCumulative)memories[i];
					if (thought_MemorySocialCumulative.OtherPawn() == this.otherPawn)
					{
						thought_MemorySocialCumulative.opinionOffset += this.opinionOffset;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04005590 RID: 21904
		private const float OpinionOffsetChangePerDay = 1f;
	}
}
