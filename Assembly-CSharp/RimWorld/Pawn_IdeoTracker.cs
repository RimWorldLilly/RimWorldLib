using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001599 RID: 5529
	public class Pawn_IdeoTracker : IExposable
	{
		// Token: 0x17001755 RID: 5973
		// (get) Token: 0x06008822 RID: 34850 RVA: 0x0031429C File Offset: 0x0031249C
		public Ideo Ideo
		{
			get
			{
				return this.ideo;
			}
		}

		// Token: 0x17001756 RID: 5974
		// (get) Token: 0x06008823 RID: 34851 RVA: 0x003142A4 File Offset: 0x003124A4
		public List<Ideo> PreviousIdeos
		{
			get
			{
				return this.previousIdeos;
			}
		}

		// Token: 0x17001757 RID: 5975
		// (get) Token: 0x06008824 RID: 34852 RVA: 0x003142AC File Offset: 0x003124AC
		// (set) Token: 0x06008825 RID: 34853 RVA: 0x003142D1 File Offset: 0x003124D1
		public float Certainty
		{
			get
			{
				if (this.pawn.DevelopmentalStage.Baby())
				{
					this.certaintyInt = 0f;
				}
				return this.certaintyInt;
			}
			private set
			{
				if (this.pawn.DevelopmentalStage.Baby())
				{
					this.certaintyInt = 0f;
				}
				else
				{
					this.certaintyInt = value;
				}
				this.certaintyInt = Mathf.Clamp01(this.certaintyInt);
			}
		}

		// Token: 0x17001758 RID: 5976
		// (get) Token: 0x06008826 RID: 34854 RVA: 0x0031430C File Offset: 0x0031250C
		private float CertaintyChangeFactor
		{
			get
			{
				if (!ModsConfig.BiotechActive)
				{
					return 1f;
				}
				if (Pawn_IdeoTracker.pawnAgeCertaintyCurve == null)
				{
					Pawn_IdeoTracker.pawnAgeCertaintyCurve = new SimpleCurve
					{
						{
							new CurvePoint(this.pawn.ageTracker.LifeStageMinAge(LifeStageDefOf.HumanlikeChild), 2f),
							true
						},
						{
							new CurvePoint(this.pawn.ageTracker.LifeStageMinAge(LifeStageDefOf.HumanlikeAdult), 1f),
							true
						}
					};
				}
				return Pawn_IdeoTracker.pawnAgeCertaintyCurve.Evaluate(this.pawn.ageTracker.AgeBiologicalYearsFloat);
			}
		}

		// Token: 0x17001759 RID: 5977
		// (get) Token: 0x06008827 RID: 34855 RVA: 0x0031439D File Offset: 0x0031259D
		public float CertaintyChangePerDay
		{
			get
			{
				if (this.pawn.needs.mood != null)
				{
					return ConversionTuning.CertaintyPerDayByMoodCurve.Evaluate(this.pawn.needs.mood.CurLevelPercentage);
				}
				return 0f;
			}
		}

		// Token: 0x1700175A RID: 5978
		// (get) Token: 0x06008828 RID: 34856 RVA: 0x003143D6 File Offset: 0x003125D6
		public List<Pawn_IdeoTracker.IdeoExposureWeight> BabyIdeoExposureSorted
		{
			get
			{
				if (!this.IsBabyIdeoExposureSorted())
				{
					this.babyIdeoExposure.Sort();
					this.isIdeoExposureSortDirty = false;
				}
				return this.babyIdeoExposure;
			}
		}

		// Token: 0x1700175B RID: 5979
		// (get) Token: 0x06008829 RID: 34857 RVA: 0x003143F8 File Offset: 0x003125F8
		public float BabyIdeoExposureTotal
		{
			get
			{
				if (this.babyIdeoExposure.NullOrEmpty<Pawn_IdeoTracker.IdeoExposureWeight>())
				{
					return 0f;
				}
				float num = 0f;
				foreach (Pawn_IdeoTracker.IdeoExposureWeight ideoExposureWeight in this.babyIdeoExposure)
				{
					num += ideoExposureWeight.exposure;
				}
				return num;
			}
		}

		// Token: 0x0600882A RID: 34858 RVA: 0x00314468 File Offset: 0x00312668
		public Pawn_IdeoTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.previousIdeos = new List<Ideo>();
		}

		// Token: 0x0600882B RID: 34859 RVA: 0x00314489 File Offset: 0x00312689
		public Pawn_IdeoTracker()
		{
		}

		// Token: 0x0600882C RID: 34860 RVA: 0x00314498 File Offset: 0x00312698
		private float ApplyCertaintyChangeFactor(float certaintyOffset)
		{
			if (certaintyOffset <= 0f)
			{
				return certaintyOffset * this.CertaintyChangeFactor;
			}
			return certaintyOffset / this.CertaintyChangeFactor;
		}

		// Token: 0x0600882D RID: 34861 RVA: 0x003144B4 File Offset: 0x003126B4
		public void IdeoTrackerTickInterval(int delta)
		{
			if (!this.pawn.Destroyed && !this.pawn.InMentalState && this.ideo != null && !Find.IdeoManager.classicMode && !this.pawn.Deathresting)
			{
				this.Certainty += this.ApplyCertaintyChangeFactor(this.CertaintyChangePerDay / 60000f) * (float)delta;
			}
			if (this.ideo != null && this.ideo.currentCacheId != this.lastCacheId)
			{
				this.lastCacheId = this.ideo.currentCacheId;
				this.RecacheIdeoComponents();
			}
		}

		// Token: 0x0600882E RID: 34862 RVA: 0x00314550 File Offset: 0x00312750
		public void SetIdeo(Ideo ideo)
		{
			if (this.ideo == ideo || this.pawn.DevelopmentalStage.Baby())
			{
				return;
			}
			if (this.ideo != null)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.ChangedIdeo, this.pawn.Named(HistoryEventArgsNames.Doer)), true);
			}
			if (this.previousIdeos.Contains(ideo))
			{
				this.previousIdeos.Remove(ideo);
			}
			if (this.ideo != null)
			{
				this.previousIdeos.Add(this.ideo);
			}
			Ideo ideo2 = this.ideo;
			if (this.pawn.Faction != null && this.pawn.Faction.IsPlayer)
			{
				Ideo ideo3 = this.ideo;
				if (ideo3 != null)
				{
					ideo3.Notify_MemberLost(this.pawn, this.pawn.Map);
				}
				Ideo ideo4 = this.ideo;
				if (ideo4 != null)
				{
					ideo4.RecacheColonistBelieverCount();
				}
			}
			this.ideo = ideo;
			this.Certainty = Mathf.Clamp01(ConversionTuning.InitialCertaintyRange.RandomInRange);
			if (this.pawn.Faction != null && this.pawn.Faction.IsPlayer)
			{
				this.pawn.Faction.ideos.Notify_ColonistChangedIdeo();
				if (ideo2 != null)
				{
					ideo2.RecacheColonistBelieverCount();
				}
				Ideo ideo5 = this.ideo;
				if (ideo5 != null)
				{
					ideo5.RecacheColonistBelieverCount();
				}
			}
			Ideo ideo6 = this.ideo;
			if (ideo6 != null)
			{
				ideo6.Notify_MemberGained(this.pawn);
			}
			if (this.pawn.ownership.OwnedBed != null && this.pawn.ownership.OwnedBed.CompAssignableToPawn.IdeoligionForbids(this.pawn))
			{
				this.pawn.ownership.UnclaimBed();
			}
			SpouseRelationUtility.RemoveSpousesAsForbiddenByIdeo(this.pawn);
			if (ideo != null && !ideo.MemberWillingToDo(new HistoryEvent(HistoryEventDefOf.Bonded)))
			{
				List<Pawn> list = new List<Pawn>();
				List<DirectPawnRelation> directRelations = this.pawn.relations.DirectRelations;
				for (int i = directRelations.Count - 1; i >= 0; i--)
				{
					DirectPawnRelation directPawnRelation = directRelations[i];
					if (directPawnRelation.def == PawnRelationDefOf.Bond)
					{
						list.Add(directPawnRelation.otherPawn);
						this.pawn.relations.RemoveDirectRelation(directPawnRelation);
					}
				}
				if (list.Count > 0)
				{
					Find.LetterStack.ReceiveLetter("LetterBondRemoved".Translate(), "LetterBondRemovedDesc".Translate(ideo.Named("IDEO"), this.pawn.Named("PAWN"), (from b in list
					select b.LabelCap).ToLineList(null, false).Named("BONDS")), LetterDefOf.NeutralEvent, new LookTargets(list.Concat(new Pawn[]
					{
						this.pawn
					})), null, null, null, null, 0, true);
				}
			}
			this.joinTick = Find.TickManager.TicksGame;
			this.RecacheIdeoComponents();
			Pawn_AgeTracker ageTracker = this.pawn.ageTracker;
			if (ageTracker == null)
			{
				return;
			}
			ageTracker.Notify_IdeoChanged();
		}

		// Token: 0x0600882F RID: 34863 RVA: 0x00314858 File Offset: 0x00312A58
		private void RecacheIdeoComponents()
		{
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (needs != null)
			{
				Need_Mood mood = needs.mood;
				if (mood != null)
				{
					mood.thoughts.situational.Notify_SituationalThoughtsDirty();
				}
			}
			Pawn_NeedsTracker needs2 = this.pawn.needs;
			if (needs2 != null)
			{
				needs2.AddOrRemoveNeedsAsAppropriate();
			}
			Pawn_ApparelTracker apparel = this.pawn.apparel;
			if (apparel != null)
			{
				apparel.Notify_IdeoChanged();
			}
			Pawn_AbilityTracker abilities = this.pawn.abilities;
			if (abilities == null)
			{
				return;
			}
			abilities.Notify_TemporaryAbilitiesChanged();
		}

		// Token: 0x06008830 RID: 34864 RVA: 0x003148D4 File Offset: 0x00312AD4
		public bool IdeoConversionAttempt(float certaintyReduction, Ideo initiatorIdeo, bool applyCertaintyFactor = true)
		{
			if (!ModLister.CheckIdeology("Ideoligion conversion") || this.pawn.DevelopmentalStage.Baby())
			{
				return false;
			}
			if (Find.IdeoManager.classicMode)
			{
				return false;
			}
			float num = Mathf.Clamp01(this.Certainty + (applyCertaintyFactor ? this.ApplyCertaintyChangeFactor(-certaintyReduction) : (-certaintyReduction)));
			if (this.pawn.Spawned)
			{
				string text = "Certainty".Translate() + "\n" + this.Certainty.ToStringPercent() + " -> " + num.ToStringPercent();
				MoteMaker.ThrowText(this.pawn.DrawPos, this.pawn.Map, text, 8f);
			}
			this.Certainty = num;
			if (this.Certainty <= 0f)
			{
				bool flag = this.PreviousIdeos.Contains(initiatorIdeo);
				this.SetIdeo(initiatorIdeo);
				this.Certainty = 0.5f;
				initiatorIdeo.Notify_MemberGainedByConversion();
				if (!flag)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.ConvertedNewMember, this.pawn.Named(HistoryEventArgsNames.Doer), initiatorIdeo.Named(HistoryEventArgsNames.Ideo)), true);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06008831 RID: 34865 RVA: 0x00314A05 File Offset: 0x00312C05
		public void IncreaseIdeoExposureIfBabyTick(Ideo ideo, int ticks = 1)
		{
			this.IncreaseIdeoExposureIfBaby(ideo, (float)ticks * 0.0004f);
		}

		// Token: 0x06008832 RID: 34866 RVA: 0x00314A18 File Offset: 0x00312C18
		public void IncreaseIdeoExposureIfBaby(Ideo ideo, float exposure)
		{
			if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive || ideo == null || Find.IdeoManager.classicMode || !this.pawn.DevelopmentalStage.Baby())
			{
				return;
			}
			if (ModsConfig.AnomalyActive && ideo == Find.IdeoManager.Horaxian)
			{
				return;
			}
			if (this.babyIdeoExposure == null)
			{
				this.babyIdeoExposure = new List<Pawn_IdeoTracker.IdeoExposureWeight>();
			}
			this.isIdeoExposureSortDirty = true;
			foreach (Pawn_IdeoTracker.IdeoExposureWeight ideoExposureWeight in this.babyIdeoExposure)
			{
				if (ideoExposureWeight.ideo == ideo)
				{
					ideoExposureWeight.exposure += exposure;
					return;
				}
			}
			this.babyIdeoExposure.Add(new Pawn_IdeoTracker.IdeoExposureWeight(ideo, exposure));
		}

		// Token: 0x06008833 RID: 34867 RVA: 0x00314AF0 File Offset: 0x00312CF0
		private bool IsBabyIdeoExposureSorted()
		{
			if (this.babyIdeoExposure.NullOrEmpty<Pawn_IdeoTracker.IdeoExposureWeight>() || !this.isIdeoExposureSortDirty)
			{
				return true;
			}
			float exposure = this.babyIdeoExposure[0].exposure;
			foreach (Pawn_IdeoTracker.IdeoExposureWeight ideoExposureWeight in this.babyIdeoExposure)
			{
				if (exposure < ideoExposureWeight.exposure)
				{
					return false;
				}
				exposure = ideoExposureWeight.exposure;
			}
			this.isIdeoExposureSortDirty = false;
			return true;
		}

		// Token: 0x06008834 RID: 34868 RVA: 0x00314B84 File Offset: 0x00312D84
		public bool TryJoinIdeoFromExposures()
		{
			if (this.Ideo != null)
			{
				return false;
			}
			Ideo ideo;
			if (!this.babyIdeoExposure.NullOrEmpty<Pawn_IdeoTracker.IdeoExposureWeight>() && !Find.IdeoManager.classicMode)
			{
				Pawn_IdeoTracker.IdeoExposureWeight ideoExposureWeight;
				this.babyIdeoExposure.TryRandomElementByWeight((Pawn_IdeoTracker.IdeoExposureWeight i) => i.exposure, out ideoExposureWeight);
				ideo = ideoExposureWeight.ideo;
			}
			else
			{
				ideo = this.FallbackIdeo(null);
			}
			this.SetIdeo(ideo);
			if (this.Ideo != null)
			{
				this.babyIdeoExposure = null;
				return true;
			}
			return false;
		}

		// Token: 0x06008835 RID: 34869 RVA: 0x00314C0B File Offset: 0x00312E0B
		public void Reassure(float certaintyGain)
		{
			this.OffsetCertainty(certaintyGain);
		}

		// Token: 0x06008836 RID: 34870 RVA: 0x00314C14 File Offset: 0x00312E14
		public void OffsetCertainty(float offset)
		{
			if (!ModLister.CheckIdeology("Ideoligion certainty"))
			{
				return;
			}
			if (Find.IdeoManager.classicMode)
			{
				return;
			}
			float num = Mathf.Clamp01(this.Certainty + this.ApplyCertaintyChangeFactor(offset));
			if (this.pawn.Spawned)
			{
				string text = "Certainty".Translate() + "\n" + this.Certainty.ToStringPercent() + " -> " + num.ToStringPercent();
				MoteMaker.ThrowText(this.pawn.DrawPos, this.pawn.Map, text, 8f);
			}
			this.Certainty = num;
		}

		// Token: 0x06008837 RID: 34871 RVA: 0x00314CC3 File Offset: 0x00312EC3
		public void Notify_IdeoRemoved(Ideo removedIdeo)
		{
			if (this.Ideo == removedIdeo)
			{
				this.SetIdeo(this.FallbackIdeo(removedIdeo));
			}
			if (this.previousIdeos.Contains(removedIdeo))
			{
				this.previousIdeos.Remove(removedIdeo);
			}
		}

		// Token: 0x06008838 RID: 34872 RVA: 0x00314CF8 File Offset: 0x00312EF8
		private Ideo FallbackIdeo(Ideo excludeIdeo = null)
		{
			Faction faction = this.pawn.Faction;
			if (((faction != null) ? faction.ideos : null) != null && this.pawn.Faction.ideos.PrimaryIdeo != excludeIdeo && this.pawn.Faction.ideos.PrimaryIdeo != null)
			{
				return this.pawn.Faction.ideos.PrimaryIdeo;
			}
			return Find.IdeoManager.IdeosListForReading.RandomElement<Ideo>();
		}

		// Token: 0x06008839 RID: 34873 RVA: 0x00314D72 File Offset: 0x00312F72
		public void Debug_ReduceCertainty(float amt)
		{
			this.Certainty -= amt;
		}

		// Token: 0x0600883A RID: 34874 RVA: 0x00314D84 File Offset: 0x00312F84
		public void ExposeData()
		{
			Scribe_References.Look<Ideo>(ref this.ideo, "ideo", false);
			Scribe_Collections.Look<Ideo>(ref this.previousIdeos, "previousIdeos", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<float>(ref this.certaintyInt, "certainty", 0f, false);
			Scribe_Values.Look<int>(ref this.joinTick, "joinTick", 0, false);
			Scribe_Collections.Look<Pawn_IdeoTracker.IdeoExposureWeight>(ref this.babyIdeoExposure, "babyIdeoExposure", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.previousIdeos.RemoveAll((Ideo x) => x == null) != 0)
				{
					Log.Error("Removed null ideos");
				}
				if (this.Certainty <= 0f)
				{
					this.Certainty = Mathf.Clamp01(ConversionTuning.InitialCertaintyRange.RandomInRange);
				}
				if (this.Ideo == null && this.pawn.ShouldHaveIdeo)
				{
					Ideo value = this.FallbackIdeo(null);
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 2);
					defaultInterpolatedStringHandler.AppendFormatted(this.pawn.ToStringSafe<Pawn>());
					defaultInterpolatedStringHandler.AppendLiteral(" did not have an ideo set; assigning fallback ideo ");
					defaultInterpolatedStringHandler.AppendFormatted<Ideo>(value);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
					this.SetIdeo(value);
				}
			}
		}

		// Token: 0x040053F5 RID: 21493
		private Pawn pawn;

		// Token: 0x040053F6 RID: 21494
		public int joinTick;

		// Token: 0x040053F7 RID: 21495
		private Ideo ideo;

		// Token: 0x040053F8 RID: 21496
		private List<Ideo> previousIdeos;

		// Token: 0x040053F9 RID: 21497
		private float certaintyInt;

		// Token: 0x040053FA RID: 21498
		private List<Pawn_IdeoTracker.IdeoExposureWeight> babyIdeoExposure;

		// Token: 0x040053FB RID: 21499
		private bool isIdeoExposureSortDirty = true;

		// Token: 0x040053FC RID: 21500
		private int lastCacheId;

		// Token: 0x040053FD RID: 21501
		private const float ExposurePointsPerHour = 1f;

		// Token: 0x040053FE RID: 21502
		public const float ExposurePointsPerTick = 0.0004f;

		// Token: 0x040053FF RID: 21503
		private static SimpleCurve pawnAgeCertaintyCurve;

		// Token: 0x020033A6 RID: 13222
		public class IdeoExposureWeight : IExposable, IComparable<Pawn_IdeoTracker.IdeoExposureWeight>
		{
			// Token: 0x06013106 RID: 78086 RVA: 0x0064FE27 File Offset: 0x0064E027
			public IdeoExposureWeight()
			{
			}

			// Token: 0x06013107 RID: 78087 RVA: 0x0064FE2F File Offset: 0x0064E02F
			public IdeoExposureWeight(Ideo ideo, float exposure)
			{
				this.ideo = ideo;
				this.exposure = exposure;
			}

			// Token: 0x06013108 RID: 78088 RVA: 0x0064FE45 File Offset: 0x0064E045
			public int CompareTo(Pawn_IdeoTracker.IdeoExposureWeight other)
			{
				return other.exposure.CompareTo(this.exposure);
			}

			// Token: 0x06013109 RID: 78089 RVA: 0x0064FE58 File Offset: 0x0064E058
			public void ExposeData()
			{
				Scribe_References.Look<Ideo>(ref this.ideo, "ideo", false);
				Scribe_Values.Look<float>(ref this.exposure, "exposure", 0f, false);
			}

			// Token: 0x0400CF59 RID: 53081
			public Ideo ideo;

			// Token: 0x0400CF5A RID: 53082
			public float exposure;
		}
	}
}
