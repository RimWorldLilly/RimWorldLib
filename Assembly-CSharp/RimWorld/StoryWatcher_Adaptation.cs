using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011FB RID: 4603
	public class StoryWatcher_Adaptation : IExposable
	{
		// Token: 0x1700136A RID: 4970
		// (get) Token: 0x06007264 RID: 29284 RVA: 0x00280E97 File Offset: 0x0027F097
		public float TotalThreatPointsFactor
		{
			get
			{
				return this.StorytellerDef.pointsFactorFromAdaptDays.Evaluate(this.adaptDays);
			}
		}

		// Token: 0x1700136B RID: 4971
		// (get) Token: 0x06007265 RID: 29285 RVA: 0x00280EAF File Offset: 0x0027F0AF
		public float AdaptDays
		{
			get
			{
				return this.adaptDays;
			}
		}

		// Token: 0x1700136C RID: 4972
		// (get) Token: 0x06007266 RID: 29286 RVA: 0x00280EB7 File Offset: 0x0027F0B7
		private int Population
		{
			get
			{
				return PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists.Count<Pawn>();
			}
		}

		// Token: 0x1700136D RID: 4973
		// (get) Token: 0x06007267 RID: 29287 RVA: 0x00280EC3 File Offset: 0x0027F0C3
		private StorytellerDef StorytellerDef
		{
			get
			{
				return Find.Storyteller.def;
			}
		}

		// Token: 0x06007268 RID: 29288 RVA: 0x00280ED0 File Offset: 0x0027F0D0
		public void Notify_PawnEvent(Pawn p, AdaptationEvent ev, DamageInfo? dinfo = null)
		{
			if (!p.RaceProps.Humanlike || !p.IsColonist || p.IsPrisoner)
			{
				return;
			}
			if (ev != AdaptationEvent.Downed)
			{
				this.ResolvePawnEvent(p, ev);
				return;
			}
			if (dinfo == null || !dinfo.Value.Def.ExternalViolenceFor(p))
			{
				return;
			}
			this.pawnsJustDownedThisTick.Add(p);
		}

		// Token: 0x06007269 RID: 29289 RVA: 0x00280F34 File Offset: 0x0027F134
		private void ResolvePawnEvent(Pawn p, AdaptationEvent ev)
		{
			float num;
			if (ev == AdaptationEvent.Downed)
			{
				num = this.StorytellerDef.adaptDaysLossFromColonistViolentlyDownedByPopulation.Evaluate((float)this.Population);
			}
			else
			{
				if (this.pawnsJustDownedThisTick.Contains(p))
				{
					this.pawnsJustDownedThisTick.Remove(p);
				}
				int num2 = this.Population - 1;
				num = this.StorytellerDef.adaptDaysLossFromColonistLostByPostPopulation.Evaluate((float)num2);
			}
			if (DebugViewSettings.writeStoryteller)
			{
				Log.Message(string.Concat(new string[]
				{
					"Adaptation event: ",
					(p != null) ? p.ToString() : null,
					" ",
					ev.ToString(),
					". Loss: ",
					num.ToString("F1"),
					" from ",
					this.adaptDays.ToString("F1")
				}));
			}
			this.adaptDays = Mathf.Max(this.StorytellerDef.adaptDaysMin, this.adaptDays - num);
		}

		// Token: 0x0600726A RID: 29290 RVA: 0x0028102C File Offset: 0x0027F22C
		public void AdaptationWatcherTick()
		{
			for (int i = 0; i < this.pawnsJustDownedThisTick.Count; i++)
			{
				this.ResolvePawnEvent(this.pawnsJustDownedThisTick[i], AdaptationEvent.Downed);
			}
			this.pawnsJustDownedThisTick.Clear();
			if (Find.TickManager.TicksGame % 30000 == 0)
			{
				if (this.adaptDays >= 0f && (float)GenDate.DaysPassedSinceSettle < this.StorytellerDef.adaptDaysGameStartGraceDays)
				{
					return;
				}
				float num = 0.5f * this.StorytellerDef.adaptDaysGrowthRateCurve.Evaluate(this.adaptDays);
				if (this.adaptDays > 0f)
				{
					num *= Find.Storyteller.difficulty.adaptationGrowthRateFactorOverZero;
				}
				this.adaptDays += num;
				this.adaptDays = Mathf.Min(this.adaptDays, this.StorytellerDef.adaptDaysMax);
			}
		}

		// Token: 0x0600726B RID: 29291 RVA: 0x00281109 File Offset: 0x0027F309
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.adaptDays, "adaptDays", 0f, false);
		}

		// Token: 0x0600726C RID: 29292 RVA: 0x00281121 File Offset: 0x0027F321
		public void Debug_OffsetAdaptDays(float days)
		{
			this.adaptDays += days;
		}

		// Token: 0x0600726D RID: 29293 RVA: 0x00281131 File Offset: 0x0027F331
		public void ResetAdaptDays()
		{
			this.adaptDays = 0f;
		}

		// Token: 0x04004869 RID: 18537
		private float adaptDays;

		// Token: 0x0400486A RID: 18538
		private List<Pawn> pawnsJustDownedThisTick = new List<Pawn>();

		// Token: 0x0400486B RID: 18539
		private const int UpdateInterval = 30000;
	}
}
