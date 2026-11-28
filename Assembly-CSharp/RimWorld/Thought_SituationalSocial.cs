using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015E4 RID: 5604
	public class Thought_SituationalSocial : Thought_Situational, ISocialThought
	{
		// Token: 0x17001824 RID: 6180
		// (get) Token: 0x06008B41 RID: 35649 RVA: 0x0032726C File Offset: 0x0032546C
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && this.MoodOffset() != 0f;
			}
		}

		// Token: 0x17001825 RID: 6181
		// (get) Token: 0x06008B42 RID: 35650 RVA: 0x00327288 File Offset: 0x00325488
		public override string LabelCap
		{
			get
			{
				if (!this.reason.NullOrEmpty())
				{
					TaggedString taggedString = base.CurStage.label.Formatted(this.reason.Named("REASON"), this.pawn.Named("PAWN"), this.otherPawn.Named("OTHERPAWN")).CapitalizeFirst();
					if (this.def.Worker != null)
					{
						taggedString = this.def.Worker.PostProcessLabel(this.pawn, taggedString);
					}
					return taggedString;
				}
				if (this.def.Worker == null)
				{
					return base.CurStage.LabelCap.Formatted(this.pawn.Named("PAWN"), this.otherPawn.Named("OTHERPAWN")).CapitalizeFirst();
				}
				return base.LabelCap;
			}
		}

		// Token: 0x17001826 RID: 6182
		// (get) Token: 0x06008B43 RID: 35651 RVA: 0x00327374 File Offset: 0x00325574
		public override string LabelCapSocial
		{
			get
			{
				if (base.CurStage.labelSocial != null)
				{
					return base.CurStage.LabelSocialCap.Formatted(this.pawn.Named("PAWN"), this.otherPawn.Named("OTHERPAWN"), this.reason.Named("REASON"));
				}
				return base.LabelCapSocial;
			}
		}

		// Token: 0x06008B44 RID: 35652 RVA: 0x003273DA File Offset: 0x003255DA
		public Pawn OtherPawn()
		{
			return this.otherPawn;
		}

		// Token: 0x06008B45 RID: 35653 RVA: 0x003273E4 File Offset: 0x003255E4
		public virtual float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			float num = base.CurStage.baseOpinionOffset;
			if (this.def.effectMultiplyingStat != null)
			{
				num *= this.pawn.GetStatValue(this.def.effectMultiplyingStat, true, -1) * this.otherPawn.GetStatValue(this.def.effectMultiplyingStat, true, -1);
			}
			return num;
		}

		// Token: 0x06008B46 RID: 35654 RVA: 0x00327458 File Offset: 0x00325658
		public override bool GroupsWith(Thought other)
		{
			Thought_SituationalSocial thought_SituationalSocial = other as Thought_SituationalSocial;
			return thought_SituationalSocial != null && base.GroupsWith(other) && this.otherPawn == thought_SituationalSocial.otherPawn;
		}

		// Token: 0x06008B47 RID: 35655 RVA: 0x0032748A File Offset: 0x0032568A
		protected override ThoughtState CurrentStateInternal()
		{
			return this.def.Worker.CurrentSocialState(this.pawn, this.otherPawn);
		}

		// Token: 0x04005594 RID: 21908
		public Pawn otherPawn;
	}
}
