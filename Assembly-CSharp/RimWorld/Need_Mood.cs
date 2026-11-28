using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200157C RID: 5500
	public class Need_Mood : Need_Seeker
	{
		// Token: 0x17001703 RID: 5891
		// (get) Token: 0x060086E1 RID: 34529 RVA: 0x0030BE9C File Offset: 0x0030A09C
		public override float CurInstantLevel
		{
			get
			{
				if (this.lastInstantMoodCheckTick != Find.TickManager.TicksGame)
				{
					this.lastInstantMood = this.thoughts.TotalMoodOffset();
					if (this.pawn.IsColonist || this.pawn.IsPrisonerOfColony)
					{
						this.lastInstantMood += Find.Storyteller.difficulty.colonistMoodOffset;
					}
					this.lastInstantMood = Mathf.Clamp01((this.pawn.health.hediffSet.OverrideMoodBase ?? this.def.baseLevel) + this.lastInstantMood / 100f);
					this.lastInstantMoodCheckTick = Find.TickManager.TicksGame;
				}
				return this.lastInstantMood;
			}
		}

		// Token: 0x17001704 RID: 5892
		// (get) Token: 0x060086E2 RID: 34530 RVA: 0x0030BF68 File Offset: 0x0030A168
		public string MoodString
		{
			get
			{
				if (this.pawn.MentalStateDef != null)
				{
					return "Mood_MentalState".Translate();
				}
				float breakThresholdExtreme = this.pawn.mindState.mentalBreaker.BreakThresholdExtreme;
				if (this.CurLevel < breakThresholdExtreme)
				{
					return "Mood_AboutToBreak".Translate();
				}
				if (this.CurLevel < breakThresholdExtreme + 0.05f)
				{
					return "Mood_OnEdge".Translate();
				}
				if (this.CurLevel < this.pawn.mindState.mentalBreaker.BreakThresholdMinor)
				{
					return "Mood_Stressed".Translate();
				}
				if (this.CurLevel < 0.65f)
				{
					return "Mood_Neutral".Translate();
				}
				if (this.CurLevel < 0.9f)
				{
					return "Mood_Content".Translate();
				}
				return "Mood_Happy".Translate();
			}
		}

		// Token: 0x060086E3 RID: 34531 RVA: 0x0030C058 File Offset: 0x0030A258
		public Need_Mood(Pawn pawn) : base(pawn)
		{
			this.thoughts = new ThoughtHandler(pawn);
			this.observer = new PawnObserver(pawn);
			this.recentMemory = new PawnRecentMemory(pawn);
		}

		// Token: 0x060086E4 RID: 34532 RVA: 0x0030C0A8 File Offset: 0x0030A2A8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThoughtHandler>(ref this.thoughts, "thoughts", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<PawnRecentMemory>(ref this.recentMemory, "recentMemory", new object[]
			{
				this.pawn
			});
		}

		// Token: 0x060086E5 RID: 34533 RVA: 0x0030C0F9 File Offset: 0x0030A2F9
		public override void NeedInterval()
		{
			base.NeedInterval();
			this.recentMemory.RecentMemoryInterval();
			this.thoughts.ThoughtInterval();
			this.observer.ObserverInterval();
		}

		// Token: 0x060086E6 RID: 34534 RVA: 0x0030C124 File Offset: 0x0030A324
		public override string GetTipString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			LifeStageDef curLifeStage = this.pawn.ageTracker.CurLifeStage;
			string text = (curLifeStage != null) ? curLifeStage.customMoodTipString : null;
			if (text != null)
			{
				stringBuilder.AppendLine((base.LabelCap + ": " + base.CurLevelPercentage.ToStringPercent()).Colorize(ColoredText.TipSectionTitleColor));
				stringBuilder.AppendLine(text);
			}
			else
			{
				stringBuilder.AppendLine(base.GetTipString());
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("MentalBreakThresholdExtreme".Translate() + ": " + this.pawn.mindState.mentalBreaker.BreakThresholdExtreme.ToStringPercent());
				stringBuilder.AppendLine("MentalBreakThresholdMajor".Translate() + ": " + this.pawn.mindState.mentalBreaker.BreakThresholdMajor.ToStringPercent());
				stringBuilder.AppendLine("MentalBreakThresholdMinor".Translate() + ": " + this.pawn.mindState.mentalBreaker.BreakThresholdMinor.ToStringPercent());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060086E7 RID: 34535 RVA: 0x0030C268 File Offset: 0x0030A468
		public override void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true, Rect? rectForTooltip = null, bool drawLabel = true)
		{
			if (this.threshPercents == null)
			{
				this.threshPercents = new List<float>();
			}
			this.threshPercents.Clear();
			if (this.pawn.mindState.mentalBreaker.CanDoRandomMentalBreaks)
			{
				this.threshPercents.Add(this.pawn.mindState.mentalBreaker.BreakThresholdExtreme);
				this.threshPercents.Add(this.pawn.mindState.mentalBreaker.BreakThresholdMajor);
				this.threshPercents.Add(this.pawn.mindState.mentalBreaker.BreakThresholdMinor);
			}
			base.DrawOnGUI(rect, maxThresholdMarkers, customMargin, drawArrows, doTooltip, rectForTooltip, drawLabel);
		}

		// Token: 0x04005342 RID: 21314
		public ThoughtHandler thoughts;

		// Token: 0x04005343 RID: 21315
		public PawnObserver observer;

		// Token: 0x04005344 RID: 21316
		public PawnRecentMemory recentMemory;

		// Token: 0x04005345 RID: 21317
		private int lastInstantMoodCheckTick = -9999;

		// Token: 0x04005346 RID: 21318
		private float lastInstantMood = -1f;
	}
}
