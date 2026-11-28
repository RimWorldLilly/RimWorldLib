using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001623 RID: 5667
	public class IdeoDevelopmentTracker : IExposable
	{
		// Token: 0x17001886 RID: 6278
		// (get) Token: 0x06008DB0 RID: 36272 RVA: 0x0033B59F File Offset: 0x0033979F
		public int Points
		{
			get
			{
				return this.points;
			}
		}

		// Token: 0x17001887 RID: 6279
		// (get) Token: 0x06008DB1 RID: 36273 RVA: 0x0033B5A7 File Offset: 0x003397A7
		public bool CanReformNow
		{
			get
			{
				return this.Points >= this.NextReformationDevelopmentPoints;
			}
		}

		// Token: 0x17001888 RID: 6280
		// (get) Token: 0x06008DB2 RID: 36274 RVA: 0x0033B5BA File Offset: 0x003397BA
		public int NextReformationDevelopmentPoints
		{
			get
			{
				return Mathf.FloorToInt(IdeoDevelopmentTracker.DevelopmentPointsOverReformCountCurve.Evaluate((float)this.reformCount));
			}
		}

		// Token: 0x17001889 RID: 6281
		// (get) Token: 0x06008DB3 RID: 36275 RVA: 0x0033B5D2 File Offset: 0x003397D2
		public bool CanBeDevelopedNow
		{
			get
			{
				return this.ideo.ColonistBelieverCountCached > 0;
			}
		}

		// Token: 0x06008DB4 RID: 36276 RVA: 0x0033B5E2 File Offset: 0x003397E2
		public IdeoDevelopmentTracker()
		{
		}

		// Token: 0x06008DB5 RID: 36277 RVA: 0x0033B5F5 File Offset: 0x003397F5
		public IdeoDevelopmentTracker(Ideo ideo)
		{
			this.ideo = ideo;
		}

		// Token: 0x06008DB6 RID: 36278 RVA: 0x0033B60F File Offset: 0x0033980F
		public void Notify_MemberGainedByConversion()
		{
			this.TryAddDevelopmentPoints(1);
		}

		// Token: 0x06008DB7 RID: 36279 RVA: 0x0033B61C File Offset: 0x0033981C
		public void Notify_Reformed()
		{
			this.ResetDevelopmentPoints();
			this.reformCount++;
			foreach (Precept precept in this.ideo.PreceptsListForReading)
			{
				precept.Notify_IdeoReformed();
			}
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoCryptosleep)
			{
				if (pawn.Ideo == this.ideo && !pawn.IsSlave && !pawn.IsQuestLodger())
				{
					pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
				}
			}
		}

		// Token: 0x06008DB8 RID: 36280 RVA: 0x0033B6F8 File Offset: 0x003398F8
		public void Notify_PreReform(Ideo newIdeo)
		{
			if (!IdeoDevelopmentTracker.<Notify_PreReform>g__ApprovesOfRaiding|18_0(this.ideo) && IdeoDevelopmentTracker.<Notify_PreReform>g__ApprovesOfRaiding|18_0(newIdeo))
			{
				this.lastTickRaidingApproved = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06008DB9 RID: 36281 RVA: 0x0033B720 File Offset: 0x00339920
		public bool TryGainDevelopmentPointsForRitualOutcome(Precept_Ritual ritual, int outcomeIndex, out int developmentPoints)
		{
			developmentPoints = 0;
			if (!this.CanBeDevelopedNow)
			{
				return false;
			}
			SimpleCurve developmentPointsOverOutcomeIndexCurveForRitual = IdeoDevelopmentUtility.GetDevelopmentPointsOverOutcomeIndexCurveForRitual(this.ideo, ritual);
			if (developmentPointsOverOutcomeIndexCurveForRitual != null)
			{
				developmentPoints = Mathf.FloorToInt(developmentPointsOverOutcomeIndexCurveForRitual.Evaluate((float)outcomeIndex));
				int value = this.Points;
				if (developmentPoints > 0 && this.TryAddDevelopmentPoints(developmentPoints))
				{
					Messages.Message("MessageDevelopmentPointsEarned".Translate(value, this.Points, ritual.Label), MessageTypeDefOf.PositiveEvent, true);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06008DBA RID: 36282 RVA: 0x0033B7A8 File Offset: 0x003399A8
		public bool TryAddDevelopmentPoints(int pointsToAdd)
		{
			if (pointsToAdd <= 0 || this.CanReformNow)
			{
				return false;
			}
			bool canReformNow = this.CanReformNow;
			this.points = Mathf.Min(this.points + pointsToAdd, this.NextReformationDevelopmentPoints);
			if (!canReformNow && this.CanReformNow)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelReformIdeo".Translate(), "LetterTextReformIdeo".Translate(this.ideo), LetterDefOf.PositiveEvent, null, 0, true);
			}
			return true;
		}

		// Token: 0x06008DBB RID: 36283 RVA: 0x0033B81E File Offset: 0x00339A1E
		public void ResetDevelopmentPoints()
		{
			this.points = 0;
		}

		// Token: 0x06008DBC RID: 36284 RVA: 0x0033B827 File Offset: 0x00339A27
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.points, "points", 0, false);
			Scribe_Values.Look<int>(ref this.reformCount, "reformCount", 0, false);
			Scribe_Values.Look<int>(ref this.lastTickRaidingApproved, "lastTickRaidingApproved", -9999999, false);
		}

		// Token: 0x06008DBD RID: 36285 RVA: 0x0033B863 File Offset: 0x00339A63
		public void CopyTo(IdeoDevelopmentTracker other)
		{
			other.points = this.points;
			other.reformCount = this.reformCount;
			other.lastTickRaidingApproved = this.lastTickRaidingApproved;
		}

		// Token: 0x06008DBF RID: 36287 RVA: 0x0033B928 File Offset: 0x00339B28
		[CompilerGenerated]
		internal static bool <Notify_PreReform>g__ApprovesOfRaiding|18_0(Ideo ideo)
		{
			List<Precept> preceptsListForReading = ideo.PreceptsListForReading;
			for (int i = 0; i < preceptsListForReading.Count; i++)
			{
				if (preceptsListForReading[i].def.approvesOfRaiding)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040056DA RID: 22234
		private const int DevelopmentPointsPerConversion = 1;

		// Token: 0x040056DB RID: 22235
		private static readonly SimpleCurve DevelopmentPointsOverReformCountCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 10f),
				true
			},
			{
				new CurvePoint(1f, 12f),
				true
			},
			{
				new CurvePoint(2f, 14f),
				true
			},
			{
				new CurvePoint(3f, 16f),
				true
			},
			{
				new CurvePoint(4f, 18f),
				true
			},
			{
				new CurvePoint(5f, 20f),
				true
			}
		};

		// Token: 0x040056DC RID: 22236
		public Ideo ideo;

		// Token: 0x040056DD RID: 22237
		public int points;

		// Token: 0x040056DE RID: 22238
		public int reformCount;

		// Token: 0x040056DF RID: 22239
		public int lastTickRaidingApproved = -9999999;
	}
}
