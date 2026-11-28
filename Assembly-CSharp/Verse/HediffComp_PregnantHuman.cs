using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000466 RID: 1126
	public class HediffComp_PregnantHuman : HediffComp
	{
		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06001FCF RID: 8143 RVA: 0x000C08A9 File Offset: 0x000BEAA9
		public PregnancyAttitude? Attitude
		{
			get
			{
				return this.pregnancyAttitude;
			}
		}

		// Token: 0x06001FD0 RID: 8144 RVA: 0x000C08B1 File Offset: 0x000BEAB1
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.SetupPregnancyAttitude();
		}

		// Token: 0x06001FD1 RID: 8145 RVA: 0x000C08C0 File Offset: 0x000BEAC0
		private void SetupPregnancyAttitude()
		{
			this.pregnancyAttitude = new PregnancyAttitude?((Rand.Value < 0.5f) ? PregnancyAttitude.Positive : PregnancyAttitude.Negative);
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x000C08E0 File Offset: 0x000BEAE0
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (!ModLister.CheckBiotech("Human pregnancy"))
			{
				return;
			}
			if (base.Pawn.IsHashIntervalTick(60, delta))
			{
				if ((this.lastGivenMorningSicknessTick == -1 || GenTicks.TicksGame - this.lastGivenMorningSicknessTick >= 48000) && Rand.MTBEventOccurs(HediffComp_PregnantHuman.MorningSicknessMTBDaysPerStage[this.parent.CurStageIndex], 60000f, 60f))
				{
					base.Pawn.health.AddHediff(HediffDefOf.MorningSickness, null, null, null);
					this.lastGivenMorningSicknessTick = GenTicks.TicksGame;
				}
				if ((this.lastGivenPregnancyMoodTick == -1 || GenTicks.TicksGame - this.lastGivenPregnancyMoodTick >= 48000) && Rand.MTBEventOccurs(HediffComp_PregnantHuman.PregnancyMoodMTBDaysPerStage[this.parent.CurStageIndex], 60000f, 60f))
				{
					base.Pawn.health.AddHediff(HediffDefOf.PregnancyMood, base.Pawn.health.hediffSet.GetBrain(), null, null);
					this.lastGivenPregnancyMoodTick = GenTicks.TicksGame;
				}
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06001FD3 RID: 8147 RVA: 0x000C09F8 File Offset: 0x000BEBF8
		public override string CompTipStringExtra
		{
			get
			{
				Hediff_Pregnant hediff_Pregnant = (Hediff_Pregnant)this.parent;
				TaggedString t = "\n" + "FatherTip".Translate() + ": " + ((hediff_Pregnant.Father != null) ? hediff_Pregnant.Father.LabelShort.Colorize(ColoredText.NameColor) : "Unknown".Translate().ToString()).CapitalizeFirst();
				if (hediff_Pregnant.Mother != null && hediff_Pregnant.Mother != this.parent.pawn)
				{
					t += "\n" + "MotherTip".Translate() + ": " + hediff_Pregnant.Mother.LabelShort.CapitalizeFirst().Colorize(ColoredText.NameColor);
				}
				return t.Resolve();
			}
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x000C0AD8 File Offset: 0x000BECD8
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<int>(ref this.lastGivenMorningSicknessTick, "lastGivenMorningSicknessTick", -1, false);
			Scribe_Values.Look<int>(ref this.lastGivenPregnancyMoodTick, "lastGivenPregnancyMoodTick", -1, false);
			Scribe_Values.Look<PregnancyAttitude?>(ref this.pregnancyAttitude, "pregnancyAttitude", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.pregnancyAttitude == null)
			{
				this.SetupPregnancyAttitude();
			}
		}

		// Token: 0x040018C9 RID: 6345
		private int lastGivenMorningSicknessTick = -1;

		// Token: 0x040018CA RID: 6346
		private int lastGivenPregnancyMoodTick = -1;

		// Token: 0x040018CB RID: 6347
		private PregnancyAttitude? pregnancyAttitude;

		// Token: 0x040018CC RID: 6348
		private const int MorningSicknessMinIntervalTicks = 48000;

		// Token: 0x040018CD RID: 6349
		private static readonly float[] MorningSicknessMTBDaysPerStage = new float[]
		{
			4f,
			8f,
			8f
		};

		// Token: 0x040018CE RID: 6350
		private const int PregnancyMoodMinIntervalTicks = 48000;

		// Token: 0x040018CF RID: 6351
		private static readonly float[] PregnancyMoodMTBDaysPerStage = new float[]
		{
			4f,
			8f,
			8f
		};
	}
}
