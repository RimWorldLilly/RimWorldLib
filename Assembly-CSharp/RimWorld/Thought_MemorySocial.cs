using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020015E0 RID: 5600
	public class Thought_MemorySocial : Thought_Memory, ISocialThought
	{
		// Token: 0x1700181A RID: 6170
		// (get) Token: 0x06008B23 RID: 35619 RVA: 0x00326CFB File Offset: 0x00324EFB
		public override bool ShouldDiscard
		{
			get
			{
				return this.otherPawn == null || this.opinionOffset == 0f || base.ShouldDiscard;
			}
		}

		// Token: 0x1700181B RID: 6171
		// (get) Token: 0x06008B24 RID: 35620 RVA: 0x00326D1A File Offset: 0x00324F1A
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && this.MoodOffset() != 0f;
			}
		}

		// Token: 0x1700181C RID: 6172
		// (get) Token: 0x06008B25 RID: 35621 RVA: 0x00326D36 File Offset: 0x00324F36
		private float AgePct
		{
			get
			{
				return (float)this.age / (float)this.DurationTicks;
			}
		}

		// Token: 0x1700181D RID: 6173
		// (get) Token: 0x06008B26 RID: 35622 RVA: 0x00326D47 File Offset: 0x00324F47
		private float AgeFactor
		{
			get
			{
				return Mathf.InverseLerp(1f, this.def.lerpOpinionToZeroAfterDurationPct, this.AgePct);
			}
		}

		// Token: 0x06008B27 RID: 35623 RVA: 0x00326D64 File Offset: 0x00324F64
		public virtual float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			if (this.ShouldDiscard)
			{
				return 0f;
			}
			return this.opinionOffset * this.AgeFactor;
		}

		// Token: 0x06008B28 RID: 35624 RVA: 0x00326D9A File Offset: 0x00324F9A
		public Pawn OtherPawn()
		{
			return this.otherPawn;
		}

		// Token: 0x06008B29 RID: 35625 RVA: 0x00326DA2 File Offset: 0x00324FA2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.opinionOffset, "opinionOffset", 0f, false);
		}

		// Token: 0x06008B2A RID: 35626 RVA: 0x00326DC0 File Offset: 0x00324FC0
		public override void Init()
		{
			base.Init();
			this.opinionOffset = base.CurStage.baseOpinionOffset;
		}

		// Token: 0x06008B2B RID: 35627 RVA: 0x00326DD9 File Offset: 0x00324FD9
		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			showBubble = false;
			return false;
		}

		// Token: 0x06008B2C RID: 35628 RVA: 0x00326DE0 File Offset: 0x00324FE0
		public override bool GroupsWith(Thought other)
		{
			Thought_MemorySocial thought_MemorySocial = other as Thought_MemorySocial;
			return thought_MemorySocial != null && base.GroupsWith(other) && this.otherPawn == thought_MemorySocial.otherPawn;
		}

		// Token: 0x0400558F RID: 21903
		public float opinionOffset;
	}
}
