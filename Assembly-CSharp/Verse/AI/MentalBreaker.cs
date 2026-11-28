using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x02000855 RID: 2133
	public class MentalBreaker : IExposable
	{
		// Token: 0x17000CB9 RID: 3257
		// (get) Token: 0x060042FA RID: 17146 RVA: 0x001A713D File Offset: 0x001A533D
		public float BreakThresholdExtreme
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true, 5) * 0.14285715f;
			}
		}

		// Token: 0x17000CBA RID: 3258
		// (get) Token: 0x060042FB RID: 17147 RVA: 0x001A7157 File Offset: 0x001A5357
		public float BreakThresholdMajor
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true, 5) * 0.5714286f;
			}
		}

		// Token: 0x17000CBB RID: 3259
		// (get) Token: 0x060042FC RID: 17148 RVA: 0x001A7171 File Offset: 0x001A5371
		public float BreakThresholdMinor
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true, 5);
			}
		}

		// Token: 0x17000CBC RID: 3260
		// (get) Token: 0x060042FD RID: 17149 RVA: 0x001A7188 File Offset: 0x001A5388
		public bool CanDoRandomMentalBreaks
		{
			get
			{
				LifeStageDef curLifeStage = this.pawn.ageTracker.CurLifeStage;
				if (curLifeStage == null)
				{
					return this.pawn.RaceProps.Humanlike && (this.pawn.Spawned || this.pawn.IsCaravanMember());
				}
				return curLifeStage.canDoRandomMentalBreaks;
			}
		}

		// Token: 0x17000CBD RID: 3261
		// (get) Token: 0x060042FE RID: 17150 RVA: 0x001A71DD File Offset: 0x001A53DD
		public bool BreakExtremeIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && this.CurMood < this.BreakThresholdExtreme && !this.Blocked;
			}
		}

		// Token: 0x17000CBE RID: 3262
		// (get) Token: 0x060042FF RID: 17151 RVA: 0x001A7205 File Offset: 0x001A5405
		public bool BreakMajorIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && !this.BreakExtremeIsImminent && this.CurMood < this.BreakThresholdMajor && !this.Blocked;
			}
		}

		// Token: 0x17000CBF RID: 3263
		// (get) Token: 0x06004300 RID: 17152 RVA: 0x001A7235 File Offset: 0x001A5435
		public bool BreakMinorIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && !this.BreakExtremeIsImminent && !this.BreakMajorIsImminent && this.CurMood < this.BreakThresholdMinor && !this.Blocked;
			}
		}

		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x06004301 RID: 17153 RVA: 0x001A726D File Offset: 0x001A546D
		public bool BreakExtremeIsApproaching
		{
			get
			{
				return this.pawn.MentalStateDef == null && this.BreakExtremeIsImminent && this.CurMood < this.BreakThresholdExtreme + 0.1f && !this.Blocked;
			}
		}

		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x06004302 RID: 17154 RVA: 0x001A72A3 File Offset: 0x001A54A3
		public bool Blocked
		{
			get
			{
				return this.pawn.mindState.mentalStateHandler.MentalBreaksBlocked();
			}
		}

		// Token: 0x17000CC2 RID: 3266
		// (get) Token: 0x06004303 RID: 17155 RVA: 0x001A72BA File Offset: 0x001A54BA
		public float CurMood
		{
			get
			{
				if (this.pawn.needs.mood == null)
				{
					return 0.5f;
				}
				return this.pawn.needs.mood.CurLevel;
			}
		}

		// Token: 0x17000CC3 RID: 3267
		// (get) Token: 0x06004304 RID: 17156 RVA: 0x001A72E9 File Offset: 0x001A54E9
		private IEnumerable<MentalBreakDef> CurrentPossibleMoodBreaks
		{
			get
			{
				MentalBreaker.<get_CurrentPossibleMoodBreaks>d__34 <get_CurrentPossibleMoodBreaks>d__ = new MentalBreaker.<get_CurrentPossibleMoodBreaks>d__34(-2);
				<get_CurrentPossibleMoodBreaks>d__.<>4__this = this;
				return <get_CurrentPossibleMoodBreaks>d__;
			}
		}

		// Token: 0x17000CC4 RID: 3268
		// (get) Token: 0x06004305 RID: 17157 RVA: 0x001A72FC File Offset: 0x001A54FC
		private bool ChooseAnomalyBreak
		{
			get
			{
				if (!ModsConfig.AnomalyActive)
				{
					return false;
				}
				if (this.pawn.IsCreepJoiner)
				{
					return true;
				}
				MonolithLevelDef levelDef = Find.Anomaly.LevelDef;
				return levelDef != null && Rand.Chance(levelDef.anomalyMentalBreakChance);
			}
		}

		// Token: 0x17000CC5 RID: 3269
		// (get) Token: 0x06004306 RID: 17158 RVA: 0x001A733C File Offset: 0x001A553C
		private MentalBreakIntensity CurrentDesiredMoodBreakIntensity
		{
			get
			{
				if (this.ticksBelowExtreme >= 2000)
				{
					return MentalBreakIntensity.Extreme;
				}
				if (this.ticksBelowMajor >= 2000)
				{
					return MentalBreakIntensity.Major;
				}
				if (this.ticksBelowMinor >= 2000)
				{
					return MentalBreakIntensity.Minor;
				}
				return MentalBreakIntensity.None;
			}
		}

		// Token: 0x06004307 RID: 17159 RVA: 0x001A736C File Offset: 0x001A556C
		public MentalBreaker()
		{
		}

		// Token: 0x06004308 RID: 17160 RVA: 0x001A7374 File Offset: 0x001A5574
		public MentalBreaker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06004309 RID: 17161 RVA: 0x001A7383 File Offset: 0x001A5583
		internal void Reset()
		{
			this.ticksBelowExtreme = 0;
			this.ticksBelowMajor = 0;
			this.ticksBelowMinor = 0;
		}

		// Token: 0x0600430A RID: 17162 RVA: 0x001A739C File Offset: 0x001A559C
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksUntilCanDoMentalBreak, "ticksUntilCanDoMentalBreak", 0, false);
			Scribe_Values.Look<int>(ref this.ticksBelowExtreme, "ticksBelowExtreme", 0, false);
			Scribe_Values.Look<int>(ref this.ticksBelowMajor, "ticksBelowMajor", 0, false);
			Scribe_Values.Look<int>(ref this.ticksBelowMinor, "ticksBelowMinor", 0, false);
		}

		// Token: 0x0600430B RID: 17163 RVA: 0x001A73F1 File Offset: 0x001A55F1
		private IEnumerable<MentalBreakDef> GetBreaksForIntensity(MentalBreakIntensity intensity, bool anomalyBreak, bool tryNonAnomalyBreakIfNoneAvailable = true)
		{
			MentalBreaker.<GetBreaksForIntensity>d__43 <GetBreaksForIntensity>d__ = new MentalBreaker.<GetBreaksForIntensity>d__43(-2);
			<GetBreaksForIntensity>d__.<>4__this = this;
			<GetBreaksForIntensity>d__.<>3__intensity = intensity;
			<GetBreaksForIntensity>d__.<>3__anomalyBreak = anomalyBreak;
			<GetBreaksForIntensity>d__.<>3__tryNonAnomalyBreakIfNoneAvailable = tryNonAnomalyBreakIfNoneAvailable;
			return <GetBreaksForIntensity>d__;
		}

		// Token: 0x0600430C RID: 17164 RVA: 0x001A7418 File Offset: 0x001A5618
		public void MentalBreakerTickInterval(int delta)
		{
			if (this.ticksUntilCanDoMentalBreak > 0 && this.pawn.Awake())
			{
				this.ticksUntilCanDoMentalBreak -= delta;
			}
			if (this.CanDoRandomMentalBreaks && this.pawn.MentalStateDef == null && this.pawn.IsHashIntervalTick(150, delta))
			{
				if (!DebugSettings.enableRandomMentalStates)
				{
					return;
				}
				if (this.CurMood < this.BreakThresholdExtreme)
				{
					this.ticksBelowExtreme += 150;
				}
				else
				{
					this.ticksBelowExtreme = 0;
				}
				if (this.CurMood < this.BreakThresholdMajor)
				{
					this.ticksBelowMajor += 150;
				}
				else
				{
					this.ticksBelowMajor = 0;
				}
				if (this.CurMood < this.BreakThresholdMinor)
				{
					this.ticksBelowMinor += 150;
				}
				else
				{
					this.ticksBelowMinor = 0;
				}
				if (this.TestMoodMentalBreak())
				{
					if (AlwaysBerserkUtility.TryTriggerBerserkBloodRage(this.pawn) || AlwaysBerserkUtility.TryTriggerBerserkFrenzyInducer(this.pawn))
					{
						return;
					}
					if (this.TryDoRandomMoodCausedMentalBreak())
					{
						return;
					}
				}
				if (this.pawn.story != null)
				{
					List<Trait> allTraits = this.pawn.story.traits.allTraits;
					for (int i = 0; i < allTraits.Count; i++)
					{
						if (!allTraits[i].Suppressed && allTraits[i].CurrentData.MentalStateGiver.CheckGive(this.pawn, 150))
						{
							return;
						}
					}
				}
			}
		}

		// Token: 0x0600430D RID: 17165 RVA: 0x001A7590 File Offset: 0x001A5790
		private bool TestMoodMentalBreak()
		{
			if (this.ticksUntilCanDoMentalBreak > 0)
			{
				return false;
			}
			if (this.ticksBelowExtreme > 2000)
			{
				return Rand.MTBEventOccurs(0.5f, 60000f, 150f);
			}
			if (this.ticksBelowMajor > 2000)
			{
				return Rand.MTBEventOccurs(0.8f, 60000f, 150f);
			}
			return this.ticksBelowMinor > 2000 && Rand.MTBEventOccurs(4f, 60000f, 150f);
		}

		// Token: 0x0600430E RID: 17166 RVA: 0x001A7610 File Offset: 0x001A5810
		public bool TryDoRandomMoodCausedMentalBreak()
		{
			if (!this.CanHaveMentalBreak())
			{
				return false;
			}
			MentalBreakDef breakDef;
			if (!this.CurrentPossibleMoodBreaks.TryRandomElementByWeight((MentalBreakDef d) => d.Worker.CommonalityFor(this.pawn, true), out breakDef))
			{
				return false;
			}
			Thought thought = this.RandomFinalStraw();
			TaggedString taggedString = "MentalStateReason_Mood".Translate();
			if (thought != null)
			{
				taggedString += "\n\n" + "FinalStraw".Translate(thought.LabelCap);
			}
			return this.TryDoMentalBreak(taggedString, breakDef);
		}

		// Token: 0x0600430F RID: 17167 RVA: 0x001A768C File Offset: 0x001A588C
		public bool TryDoMentalBreak(string reason, MentalBreakDef breakDef)
		{
			return this.CanHaveMentalBreak() && breakDef.Worker.TryStart(this.pawn, reason, true);
		}

		// Token: 0x06004310 RID: 17168 RVA: 0x001A76AB File Offset: 0x001A58AB
		public bool TryGetRandomMentalBreak(MentalBreakIntensity intensity, out MentalBreakDef breakDef)
		{
			return this.GetBreaksForIntensity(intensity, this.ChooseAnomalyBreak, true).TryRandomElementByWeight((MentalBreakDef d) => d.Worker.CommonalityFor(this.pawn, true), out breakDef);
		}

		// Token: 0x06004311 RID: 17169 RVA: 0x001A76D0 File Offset: 0x001A58D0
		private bool CanHaveMentalBreak()
		{
			return this.CanDoRandomMentalBreaks && !this.pawn.Downed && this.pawn.Awake() && !this.pawn.InMentalState && (!this.pawn.IsMutant || !this.pawn.mutant.Def.preventsMentalBreaks) && (this.pawn.Faction == Faction.OfPlayer || this.CurrentDesiredMoodBreakIntensity == MentalBreakIntensity.Extreme) && !QuestUtility.AnyQuestDisablesRandomMoodCausedMentalBreaksFor(this.pawn) && !this.Blocked;
		}

		// Token: 0x06004312 RID: 17170 RVA: 0x001A776C File Offset: 0x001A596C
		private Thought RandomFinalStraw()
		{
			this.pawn.needs.mood.thoughts.GetAllMoodThoughts(MentalBreaker.tmpThoughts);
			float num = 0f;
			for (int i = 0; i < MentalBreaker.tmpThoughts.Count; i++)
			{
				float num2 = MentalBreaker.tmpThoughts[i].MoodOffset();
				if (num2 < num)
				{
					num = num2;
				}
			}
			float maxMoodOffset = num * 0.5f;
			Thought result;
			(from x in MentalBreaker.tmpThoughts
			where x.MoodOffset() <= maxMoodOffset
			select x).TryRandomElementByWeight((Thought x) => -x.MoodOffset(), out result);
			MentalBreaker.tmpThoughts.Clear();
			return result;
		}

		// Token: 0x06004313 RID: 17171 RVA: 0x001A7828 File Offset: 0x001A5A28
		public void Notify_RecoveredFromMentalState()
		{
			this.ticksUntilCanDoMentalBreak = 15000;
		}

		// Token: 0x06004314 RID: 17172 RVA: 0x001A7835 File Offset: 0x001A5A35
		public float MentalBreakThresholdFor(MentalBreakIntensity intensity)
		{
			switch (intensity)
			{
			case MentalBreakIntensity.Minor:
				return this.BreakThresholdMinor;
			case MentalBreakIntensity.Major:
				return this.BreakThresholdMajor;
			case MentalBreakIntensity.Extreme:
				return this.BreakThresholdExtreme;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06004315 RID: 17173 RVA: 0x001A7868 File Offset: 0x001A5A68
		internal string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.pawn.ToString());
			stringBuilder.AppendLine("   ticksUntilCanDoMentalBreak=" + this.ticksUntilCanDoMentalBreak.ToString());
			stringBuilder.AppendLine("   ticksBelowExtreme=" + this.ticksBelowExtreme.ToString() + "/" + 2000.ToString());
			stringBuilder.AppendLine("   ticksBelowSerious=" + this.ticksBelowMajor.ToString() + "/" + 2000.ToString());
			stringBuilder.AppendLine("   ticksBelowMinor=" + this.ticksBelowMinor.ToString() + "/" + 2000.ToString());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Current desired mood break intensity: " + this.CurrentDesiredMoodBreakIntensity.ToString());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Current possible mood breaks:");
			float num = (from d in this.CurrentPossibleMoodBreaks
			select d.Worker.CommonalityFor(this.pawn, true)).Sum();
			foreach (MentalBreakDef mentalBreakDef in this.CurrentPossibleMoodBreaks)
			{
				float num2 = mentalBreakDef.Worker.CommonalityFor(this.pawn, true);
				StringBuilder stringBuilder2 = stringBuilder;
				string str = "   ";
				MentalBreakDef mentalBreakDef2 = mentalBreakDef;
				stringBuilder2.AppendLine(str + ((mentalBreakDef2 != null) ? mentalBreakDef2.ToString() : null) + "     " + (num2 / num).ToStringPercent());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06004316 RID: 17174 RVA: 0x001A7A18 File Offset: 0x001A5C18
		internal void LogPossibleMentalBreaks()
		{
			StringBuilder stringBuilder = new StringBuilder();
			MentalBreakIntensity currentDesiredMoodBreakIntensity = this.CurrentDesiredMoodBreakIntensity;
			StringBuilder stringBuilder2 = stringBuilder;
			Pawn pawn = this.pawn;
			stringBuilder2.AppendLine(((pawn != null) ? pawn.ToString() : null) + " current possible mood mental breaks:").AppendLine("CurrentDesiredMoodBreakIntensity: " + currentDesiredMoodBreakIntensity.ToString());
			foreach (MentalBreakDef mentalBreakDef in this.GetBreaksForIntensity(currentDesiredMoodBreakIntensity, false, true))
			{
				StringBuilder stringBuilder3 = stringBuilder;
				string str = "  ";
				MentalBreakDef mentalBreakDef2 = mentalBreakDef;
				stringBuilder3.AppendLine(str + ((mentalBreakDef2 != null) ? mentalBreakDef2.ToString() : null));
			}
			if (ModsConfig.AnomalyActive)
			{
				MonolithLevelDef levelDef = Find.Anomaly.LevelDef;
				if (levelDef != null && levelDef.anomalyMentalBreakChance > 0f)
				{
					foreach (MentalBreakDef mentalBreakDef3 in this.GetBreaksForIntensity(currentDesiredMoodBreakIntensity, true, false))
					{
						StringBuilder stringBuilder4 = stringBuilder;
						string str2 = "  ";
						MentalBreakDef mentalBreakDef4 = mentalBreakDef3;
						stringBuilder4.AppendLine(str2 + ((mentalBreakDef4 != null) ? mentalBreakDef4.ToString() : null));
					}
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04002BDD RID: 11229
		private Pawn pawn;

		// Token: 0x04002BDE RID: 11230
		private int ticksUntilCanDoMentalBreak;

		// Token: 0x04002BDF RID: 11231
		private int ticksBelowExtreme;

		// Token: 0x04002BE0 RID: 11232
		private int ticksBelowMajor;

		// Token: 0x04002BE1 RID: 11233
		private int ticksBelowMinor;

		// Token: 0x04002BE2 RID: 11234
		private const int CheckInterval = 150;

		// Token: 0x04002BE3 RID: 11235
		private const float ExtremeBreakMTBDays = 0.5f;

		// Token: 0x04002BE4 RID: 11236
		private const float MajorBreakMTBDays = 0.8f;

		// Token: 0x04002BE5 RID: 11237
		private const float MinorBreakMTBDays = 4f;

		// Token: 0x04002BE6 RID: 11238
		private const int MinTicksBelowToBreak = 2000;

		// Token: 0x04002BE7 RID: 11239
		private const int MinTicksSinceRecoveryToBreak = 15000;

		// Token: 0x04002BE8 RID: 11240
		private const float MajorBreakMoodFraction = 0.5714286f;

		// Token: 0x04002BE9 RID: 11241
		private const float ExtremeBreakMoodFraction = 0.14285715f;

		// Token: 0x04002BEA RID: 11242
		private static List<Thought> tmpThoughts = new List<Thought>();
	}
}
