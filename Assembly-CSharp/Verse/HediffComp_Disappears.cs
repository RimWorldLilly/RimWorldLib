using System;
using RimWorld;
using UnityEngine;
using Verse.Noise;

namespace Verse
{
	// Token: 0x0200041C RID: 1052
	public class HediffComp_Disappears : HediffComp
	{
		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06001EDC RID: 7900 RVA: 0x000BCEBB File Offset: 0x000BB0BB
		public HediffCompProperties_Disappears Props
		{
			get
			{
				return (HediffCompProperties_Disappears)this.props;
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06001EDD RID: 7901 RVA: 0x000BCEC8 File Offset: 0x000BB0C8
		public override bool CompShouldRemove
		{
			get
			{
				return base.CompShouldRemove || this.ticksToDisappear <= 0 || (this.Props.requiredMentalState != null && base.Pawn.MentalStateDef != this.Props.requiredMentalState);
			}
		}

		// Token: 0x06001EDE RID: 7902 RVA: 0x000BCF08 File Offset: 0x000BB108
		private static float AddNoiseToProgress(float progress, int seed)
		{
			float num = (float)Perlin.GetValue((double)progress, 0.0, 0.0, 9.0, seed, 2.0, 0.5, 6, false, false, QualityMode.Medium);
			float num2 = 0.25f * (1f - progress);
			return Mathf.Clamp01(progress + num2 * num);
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06001EDF RID: 7903 RVA: 0x000BCF68 File Offset: 0x000BB168
		public float Progress
		{
			get
			{
				return 1f - (float)this.ticksToDisappear / (float)Math.Max(1, this.disappearsAfterTicks);
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06001EE0 RID: 7904 RVA: 0x000BCF85 File Offset: 0x000BB185
		public int EffectiveTicksToDisappear
		{
			get
			{
				return this.ticksToDisappear / this.TicksLostPerTick;
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06001EE1 RID: 7905 RVA: 0x000BCF94 File Offset: 0x000BB194
		public float NoisyProgress
		{
			get
			{
				return HediffComp_Disappears.AddNoiseToProgress(this.Progress, this.seed);
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06001EE2 RID: 7906 RVA: 0x000BCFA7 File Offset: 0x000BB1A7
		public virtual int TicksLostPerTick
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06001EE3 RID: 7907 RVA: 0x000BCFAC File Offset: 0x000BB1AC
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (!this.Props.showRemainingTime)
				{
					return base.CompLabelInBracketsExtra;
				}
				if (this.EffectiveTicksToDisappear < 2500)
				{
					return this.EffectiveTicksToDisappear.ToStringSecondsFromTicks("F0");
				}
				return this.EffectiveTicksToDisappear.ToStringTicksToPeriod(true, true, true, true, this.Props.canUseDecimalsShortForm);
			}
		}

		// Token: 0x06001EE4 RID: 7908 RVA: 0x000BD005 File Offset: 0x000BB205
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.disappearsAfterTicks = this.Props.disappearsAfterTicks.RandomInRange;
			this.seed = Rand.Int;
			this.ticksToDisappear = this.disappearsAfterTicks;
		}

		// Token: 0x06001EE5 RID: 7909 RVA: 0x000BD03A File Offset: 0x000BB23A
		public void ResetElapsedTicks()
		{
			this.ticksToDisappear = this.disappearsAfterTicks;
		}

		// Token: 0x06001EE6 RID: 7910 RVA: 0x000BD048 File Offset: 0x000BB248
		public void SetDuration(int ticks)
		{
			this.disappearsAfterTicks = ticks;
			this.ticksToDisappear = ticks;
		}

		// Token: 0x06001EE7 RID: 7911 RVA: 0x000BD058 File Offset: 0x000BB258
		public override void CompPostPostRemoved()
		{
			if (!this.Props.leaveFreshWounds)
			{
				foreach (BodyPartRecord part in this.parent.Part.GetPartAndAllChildParts())
				{
					Hediff_MissingPart hediff_MissingPart = base.Pawn.health.hediffSet.GetMissingPartFor(part) as Hediff_MissingPart;
					if (hediff_MissingPart != null)
					{
						hediff_MissingPart.IsFresh = false;
					}
				}
			}
			if (this.CompShouldRemove && !this.Props.messageOnDisappear.NullOrEmpty() && PawnUtility.ShouldSendNotificationAbout(base.Pawn))
			{
				Messages.Message(this.Props.messageOnDisappear.Formatted(base.Pawn.Named("PAWN")), base.Pawn, MessageTypeDefOf.PositiveEvent, true);
			}
			if (!this.Props.letterTextOnDisappear.NullOrEmpty() && !this.Props.letterLabelOnDisappear.NullOrEmpty() && PawnUtility.ShouldSendNotificationAbout(base.Pawn) && (!base.Pawn.Dead || this.Props.sendLetterOnDisappearIfDead))
			{
				Find.LetterStack.ReceiveLetter(this.Props.letterLabelOnDisappear.Formatted(base.Pawn.Named("PAWN")), this.Props.letterTextOnDisappear.Formatted(base.Pawn.Named("PAWN")), LetterDefOf.PositiveEvent, base.Pawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x06001EE8 RID: 7912 RVA: 0x000BD1F0 File Offset: 0x000BB3F0
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksToDisappear -= this.TicksLostPerTick;
		}

		// Token: 0x06001EE9 RID: 7913 RVA: 0x000BD208 File Offset: 0x000BB408
		public override void CompPostMerged(Hediff other)
		{
			base.CompPostMerged(other);
			HediffComp_Disappears hediffComp_Disappears = other.TryGetComp<HediffComp_Disappears>();
			if (hediffComp_Disappears != null && hediffComp_Disappears.ticksToDisappear > this.ticksToDisappear)
			{
				this.ticksToDisappear = hediffComp_Disappears.ticksToDisappear;
			}
		}

		// Token: 0x06001EEA RID: 7914 RVA: 0x000BD240 File Offset: 0x000BB440
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksToDisappear, "ticksToDisappear", 0, false);
			Scribe_Values.Look<int>(ref this.disappearsAfterTicks, "disappearsAfterTicks", 0, false);
			Scribe_Values.Look<int>(ref this.seed, "seed", 0, false);
		}

		// Token: 0x06001EEB RID: 7915 RVA: 0x000BD278 File Offset: 0x000BB478
		public override string CompDebugString()
		{
			return "ticksToDisappear: " + this.ticksToDisappear.ToString();
		}

		// Token: 0x06001EEC RID: 7916 RVA: 0x000BD290 File Offset: 0x000BB490
		public override void CopyFrom(HediffComp other)
		{
			HediffComp_Disappears hediffComp_Disappears = other as HediffComp_Disappears;
			if (hediffComp_Disappears != null)
			{
				this.ticksToDisappear = hediffComp_Disappears.ticksToDisappear;
				this.disappearsAfterTicks = hediffComp_Disappears.disappearsAfterTicks;
			}
		}

		// Token: 0x0400183C RID: 6204
		public int ticksToDisappear;

		// Token: 0x0400183D RID: 6205
		public int disappearsAfterTicks;

		// Token: 0x0400183E RID: 6206
		public int seed;

		// Token: 0x0400183F RID: 6207
		private const float NoiseScale = 0.25f;

		// Token: 0x04001840 RID: 6208
		private const float NoiseWiggliness = 9f;
	}
}
