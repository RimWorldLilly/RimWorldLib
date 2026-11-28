using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015E2 RID: 5602
	public class Thought_PsychicRitualGuilt : Thought_Memory
	{
		// Token: 0x1700181F RID: 6175
		// (get) Token: 0x06008B33 RID: 35635 RVA: 0x00326F90 File Offset: 0x00325190
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.ritualDef.label.Named("RITUAL")).CapitalizeFirst();
			}
		}

		// Token: 0x17001820 RID: 6176
		// (get) Token: 0x06008B34 RID: 35636 RVA: 0x00326FCF File Offset: 0x003251CF
		public override int CurStageIndex
		{
			get
			{
				if (!ModsConfig.IdeologyActive)
				{
					return 0;
				}
				if (this.pawn.Ideo.HasPrecept(PreceptDefOf.PsychicRituals_Disapproved))
				{
					return 1;
				}
				if (this.pawn.Ideo.HasPrecept(PreceptDefOf.PsychicRituals_Abhorrent))
				{
					return 2;
				}
				return 0;
			}
		}

		// Token: 0x06008B35 RID: 35637 RVA: 0x0032700D File Offset: 0x0032520D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualDef>(ref this.ritualDef, "ritualDef");
		}

		// Token: 0x06008B36 RID: 35638 RVA: 0x00327028 File Offset: 0x00325228
		public override void CopyFrom(Thought_Memory m)
		{
			base.CopyFrom(m);
			Thought_PsychicRitualGuilt thought_PsychicRitualGuilt = m as Thought_PsychicRitualGuilt;
			if (thought_PsychicRitualGuilt != null)
			{
				this.ritualDef = thought_PsychicRitualGuilt.ritualDef;
			}
		}

		// Token: 0x06008B37 RID: 35639 RVA: 0x00327054 File Offset: 0x00325254
		public override bool GroupsWith(Thought other)
		{
			if (base.GroupsWith(other))
			{
				Thought_PsychicRitualGuilt thought_PsychicRitualGuilt = other as Thought_PsychicRitualGuilt;
				if (thought_PsychicRitualGuilt != null)
				{
					return this.ritualDef == thought_PsychicRitualGuilt.ritualDef;
				}
			}
			return false;
		}

		// Token: 0x04005591 RID: 21905
		public PsychicRitualDef ritualDef;
	}
}
