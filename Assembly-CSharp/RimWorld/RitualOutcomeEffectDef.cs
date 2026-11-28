using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020016DA RID: 5850
	public class RitualOutcomeEffectDef : Def
	{
		// Token: 0x17001921 RID: 6433
		// (get) Token: 0x06009258 RID: 37464 RVA: 0x003572EC File Offset: 0x003554EC
		public string Description
		{
			get
			{
				if (this.minMoodCached == 3.4028235E+38f && !this.outcomeChances.NullOrEmpty<RitualOutcomePossibility>())
				{
					foreach (RitualOutcomePossibility ritualOutcomePossibility in this.outcomeChances)
					{
						if (ritualOutcomePossibility.memory != null)
						{
							float baseMoodEffect = ritualOutcomePossibility.memory.stages[0].baseMoodEffect;
							if (baseMoodEffect > this.maxMoodCached)
							{
								this.maxMoodCached = baseMoodEffect;
							}
							if (baseMoodEffect < this.minMoodCached)
							{
								this.minMoodCached = baseMoodEffect;
							}
							this.moodDurationCached = ritualOutcomePossibility.memory.durationDays;
						}
					}
				}
				return this.description.Formatted(this.minMoodCached.Named("MINMOOD"), this.maxMoodCached.ToStringWithSign("0.##").Named("MAXMOOD"), this.moodDurationCached.Named("MOODDAYS"));
			}
		}

		// Token: 0x17001922 RID: 6434
		// (get) Token: 0x06009259 RID: 37465 RVA: 0x003573FC File Offset: 0x003555FC
		public RitualOutcomePossibility BestOutcome
		{
			get
			{
				if (this.bestOutcomeCached == null)
				{
					this.bestOutcomeCached = this.outcomeChances.MaxBy((RitualOutcomePossibility o) => o.positivityIndex);
				}
				return this.bestOutcomeCached;
			}
		}

		// Token: 0x17001923 RID: 6435
		// (get) Token: 0x0600925A RID: 37466 RVA: 0x0035743C File Offset: 0x0035563C
		public RitualOutcomePossibility WorstOutcome
		{
			get
			{
				if (this.worstOutcomeCached == null)
				{
					this.worstOutcomeCached = this.outcomeChances.MinBy((RitualOutcomePossibility o) => o.positivityIndex);
				}
				return this.worstOutcomeCached;
			}
		}

		// Token: 0x0600925B RID: 37467 RVA: 0x0035747C File Offset: 0x0035567C
		public RitualOutcomeEffectWorker GetInstance()
		{
			return (RitualOutcomeEffectWorker)Activator.CreateInstance(this.workerClass, new object[]
			{
				this
			});
		}

		// Token: 0x0600925C RID: 37468 RVA: 0x00357498 File Offset: 0x00355698
		public string OutcomeMoodBreakdown(RitualOutcomePossibility outcome)
		{
			if (outcome.memory != null && outcome.memory.stages[0].baseMoodEffect != 0f)
			{
				return "RitualOutcomeExtraDesc_Mood".Translate(outcome.memory.stages[0].baseMoodEffect.ToStringWithSign("0.##"), outcome.memory.durationDays);
			}
			return "";
		}

		// Token: 0x0600925D RID: 37469 RVA: 0x00357514 File Offset: 0x00355714
		public override IEnumerable<string> ConfigErrors()
		{
			RitualOutcomeEffectDef.<ConfigErrors>d__38 <ConfigErrors>d__ = new RitualOutcomeEffectDef.<ConfigErrors>d__38(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x040058A4 RID: 22692
		public Type workerClass;

		// Token: 0x040058A5 RID: 22693
		public float startingQuality;

		// Token: 0x040058A6 RID: 22694
		public float minQuality;

		// Token: 0x040058A7 RID: 22695
		public float maxQuality = 1f;

		// Token: 0x040058A8 RID: 22696
		public bool givesDevelopmentPoints = true;

		// Token: 0x040058A9 RID: 22697
		public ThoughtDef memoryDef;

		// Token: 0x040058AA RID: 22698
		public List<RitualOutcomePossibility> outcomeChances = new List<RitualOutcomePossibility>();

		// Token: 0x040058AB RID: 22699
		public SimpleCurve honorFromQuality;

		// Token: 0x040058AC RID: 22700
		[MustTranslate]
		public List<string> extraPredictedOutcomeDescriptions;

		// Token: 0x040058AD RID: 22701
		[MustTranslate]
		public List<string> extraInfoLines;

		// Token: 0x040058AE RID: 22702
		public List<RitualOutcomeEffectDef.ExtraOutcomeChanceDescription> extraOutcomeDescriptions;

		// Token: 0x040058AF RID: 22703
		public EffecterDef effecter;

		// Token: 0x040058B0 RID: 22704
		public FleckDef fleckDef;

		// Token: 0x040058B1 RID: 22705
		public int flecksPerCell;

		// Token: 0x040058B2 RID: 22706
		public FloatRange fleckRotationRange = new FloatRange(0f, 360f);

		// Token: 0x040058B3 RID: 22707
		public FloatRange fleckScaleRange = FloatRange.One;

		// Token: 0x040058B4 RID: 22708
		public FloatRange fleckVelocityAngle = FloatRange.Zero;

		// Token: 0x040058B5 RID: 22709
		public FloatRange fleckVelocitySpeed = FloatRange.Zero;

		// Token: 0x040058B6 RID: 22710
		public ThingDef filthDefToSpawn;

		// Token: 0x040058B7 RID: 22711
		public IntRange filthCountToSpawn = IntRange.Zero;

		// Token: 0x040058B8 RID: 22712
		public List<RitualOutcomeComp> comps;

		// Token: 0x040058B9 RID: 22713
		public bool warnOnLowQuality = true;

		// Token: 0x040058BA RID: 22714
		public bool allowAttachableOutcome = true;

		// Token: 0x040058BB RID: 22715
		public bool allowOutcomeWithNoAttendance;

		// Token: 0x040058BC RID: 22716
		private float minMoodCached = float.MaxValue;

		// Token: 0x040058BD RID: 22717
		private float maxMoodCached = float.MinValue;

		// Token: 0x040058BE RID: 22718
		private float moodDurationCached = -1f;

		// Token: 0x040058BF RID: 22719
		private RitualOutcomePossibility bestOutcomeCached;

		// Token: 0x040058C0 RID: 22720
		private RitualOutcomePossibility worstOutcomeCached;

		// Token: 0x02003505 RID: 13573
		public class ExtraOutcomeChanceDescription
		{
			// Token: 0x0400D443 RID: 54339
			[MustTranslate]
			public string description;

			// Token: 0x0400D444 RID: 54340
			public Func<float, float> qualityToValue;
		}
	}
}
