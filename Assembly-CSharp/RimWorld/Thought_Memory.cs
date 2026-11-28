using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015DB RID: 5595
	public class Thought_Memory : Thought
	{
		// Token: 0x17001809 RID: 6153
		// (get) Token: 0x06008AFE RID: 35582 RVA: 0x0032658D File Offset: 0x0032478D
		public override bool VisibleInNeedsTab
		{
			get
			{
				return base.VisibleInNeedsTab && !this.ShouldDiscard;
			}
		}

		// Token: 0x1700180A RID: 6154
		// (get) Token: 0x06008AFF RID: 35583 RVA: 0x003265A2 File Offset: 0x003247A2
		public override int DurationTicks
		{
			get
			{
				if (this.durationTicksOverride < 0)
				{
					return this.def.DurationTicks;
				}
				return this.durationTicksOverride;
			}
		}

		// Token: 0x1700180B RID: 6155
		// (get) Token: 0x06008B00 RID: 35584 RVA: 0x003265BF File Offset: 0x003247BF
		public override int CurStageIndex
		{
			get
			{
				return this.forcedStage;
			}
		}

		// Token: 0x1700180C RID: 6156
		// (get) Token: 0x06008B01 RID: 35585 RVA: 0x003265C7 File Offset: 0x003247C7
		public virtual bool ShouldDiscard
		{
			get
			{
				return !this.permanent && this.age > this.DurationTicks;
			}
		}

		// Token: 0x1700180D RID: 6157
		// (get) Token: 0x06008B02 RID: 35586 RVA: 0x003265E4 File Offset: 0x003247E4
		public override string LabelCap
		{
			get
			{
				if (this.cachedLabelCap == null || this.cachedLabelCapForOtherPawn != this.otherPawn || this.cachedLabelCapForStageIndex != this.CurStageIndex)
				{
					if (this.otherPawn != null)
					{
						this.cachedLabelCap = base.CurStage.label.Formatted(this.otherPawn.LabelShort, this.otherPawn).CapitalizeFirst();
						if (this.def.Worker != null)
						{
							this.cachedLabelCap = this.def.Worker.PostProcessLabel(this.pawn, this.cachedLabelCap);
						}
					}
					else
					{
						this.cachedLabelCap = base.LabelCap;
					}
					this.cachedLabelCapForOtherPawn = this.otherPawn;
					this.cachedLabelCapForStageIndex = this.CurStageIndex;
				}
				return this.cachedLabelCap;
			}
		}

		// Token: 0x1700180E RID: 6158
		// (get) Token: 0x06008B03 RID: 35587 RVA: 0x003266BC File Offset: 0x003248BC
		public override string LabelCapSocial
		{
			get
			{
				string text;
				if (base.CurStage.labelSocial != null)
				{
					text = base.CurStage.LabelSocialCap.Formatted(this.pawn.Named("PAWN"), this.otherPawn.Named("OTHERPAWN"));
				}
				else
				{
					text = base.LabelCapSocial;
				}
				if (this.sourcePrecept != null)
				{
					text += " (" + "Ideo".Translate() + ")";
				}
				return text;
			}
		}

		// Token: 0x1700180F RID: 6159
		// (get) Token: 0x06008B04 RID: 35588 RVA: 0x00326748 File Offset: 0x00324948
		public virtual bool Save
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008B05 RID: 35589 RVA: 0x0032674B File Offset: 0x0032494B
		public void SetForcedStage(int stageIndex)
		{
			this.forcedStage = stageIndex;
		}

		// Token: 0x06008B06 RID: 35590 RVA: 0x00326754 File Offset: 0x00324954
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", true);
			Scribe_Values.Look<float>(ref this.moodPowerFactor, "moodPowerFactor", 1f, false);
			Scribe_Values.Look<int>(ref this.moodOffset, "moodOffset", 0, false);
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<int>(ref this.forcedStage, "stageIndex", 0, false);
			Scribe_Values.Look<int>(ref this.durationTicksOverride, "durationTicksOverride", -1, false);
			Scribe_Values.Look<bool>(ref this.permanent, "permanent", false, false);
		}

		// Token: 0x06008B07 RID: 35591 RVA: 0x003267E8 File Offset: 0x003249E8
		public virtual void ThoughtInterval()
		{
			this.age += 150;
		}

		// Token: 0x06008B08 RID: 35592 RVA: 0x003267FC File Offset: 0x003249FC
		public void Renew()
		{
			this.age = 0;
		}

		// Token: 0x06008B09 RID: 35593 RVA: 0x00326808 File Offset: 0x00324A08
		public virtual bool TryMergeWithExistingMemory(out bool showBubble)
		{
			ThoughtHandler thoughts = this.pawn.needs.mood.thoughts;
			if (thoughts.memories.NumMemoriesInGroup(this) >= this.def.stackLimit)
			{
				Thought_Memory thought_Memory = thoughts.memories.OldestMemoryInGroup(this);
				if (thought_Memory != null)
				{
					showBubble = (thought_Memory.age > thought_Memory.DurationTicks / 2);
					thought_Memory.Renew();
					return true;
				}
			}
			showBubble = true;
			return false;
		}

		// Token: 0x06008B0A RID: 35594 RVA: 0x00326874 File Offset: 0x00324A74
		public override bool GroupsWith(Thought other)
		{
			Thought_Memory thought_Memory = other as Thought_Memory;
			return thought_Memory != null && base.GroupsWith(other) && (this.otherPawn == thought_Memory.otherPawn || this.LabelCap == thought_Memory.LabelCap);
		}

		// Token: 0x06008B0B RID: 35595 RVA: 0x003268BC File Offset: 0x00324ABC
		public override float MoodOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			float num = base.MoodOffset();
			num *= this.moodPowerFactor;
			num += (float)this.moodOffset;
			if (this.def.lerpMoodToZero)
			{
				num *= 1f - (float)this.age / (float)this.DurationTicks;
			}
			return num;
		}

		// Token: 0x06008B0C RID: 35596 RVA: 0x00326922 File Offset: 0x00324B22
		public virtual void Notify_NewThoughtInGroupAdded(Thought_Memory memory)
		{
		}

		// Token: 0x06008B0D RID: 35597 RVA: 0x00326924 File Offset: 0x00324B24
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.def.defName,
				", moodPowerFactor=",
				this.moodPowerFactor.ToString(),
				", moodOffset=",
				this.moodOffset.ToString(),
				", age=",
				this.age.ToString(),
				")"
			});
		}

		// Token: 0x06008B0E RID: 35598 RVA: 0x003269A0 File Offset: 0x00324BA0
		public virtual void CopyFrom(Thought_Memory m)
		{
			this.age = m.age;
			this.pawn = m.pawn;
			this.sourcePrecept = m.sourcePrecept;
			this.moodOffset = m.moodOffset;
			this.otherPawn = m.otherPawn;
			this.durationTicksOverride = m.durationTicksOverride;
			this.moodPowerFactor = m.moodPowerFactor;
			this.SetForcedStage(m.CurStageIndex);
		}

		// Token: 0x04005580 RID: 21888
		public float moodPowerFactor = 1f;

		// Token: 0x04005581 RID: 21889
		public int moodOffset;

		// Token: 0x04005582 RID: 21890
		public Pawn otherPawn;

		// Token: 0x04005583 RID: 21891
		public bool permanent;

		// Token: 0x04005584 RID: 21892
		public int age;

		// Token: 0x04005585 RID: 21893
		private int forcedStage;

		// Token: 0x04005586 RID: 21894
		public int durationTicksOverride = -1;

		// Token: 0x04005587 RID: 21895
		private string cachedLabelCap;

		// Token: 0x04005588 RID: 21896
		private Pawn cachedLabelCapForOtherPawn;

		// Token: 0x04005589 RID: 21897
		private int cachedLabelCapForStageIndex = -1;
	}
}
