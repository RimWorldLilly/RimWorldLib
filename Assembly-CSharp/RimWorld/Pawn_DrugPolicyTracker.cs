using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020015B7 RID: 5559
	public class Pawn_DrugPolicyTracker : IExposable
	{
		// Token: 0x170017C0 RID: 6080
		// (get) Token: 0x060089BD RID: 35261 RVA: 0x0031DE10 File Offset: 0x0031C010
		// (set) Token: 0x060089BE RID: 35262 RVA: 0x0031DE66 File Offset: 0x0031C066
		public DrugPolicy CurrentPolicy
		{
			get
			{
				if (this.pawn.IsMutant && this.pawn.mutant.Def.disablePolicies)
				{
					return null;
				}
				if (this.curPolicy == null)
				{
					this.curPolicy = Current.Game.drugPolicyDatabase.DefaultDrugPolicy();
				}
				return this.curPolicy;
			}
			set
			{
				if (this.curPolicy == value)
				{
					return;
				}
				this.curPolicy = value;
			}
		}

		// Token: 0x170017C1 RID: 6081
		// (get) Token: 0x060089BF RID: 35263 RVA: 0x0031DE7C File Offset: 0x0031C07C
		private float DayPercentNotSleeping
		{
			get
			{
				if (this.pawn.IsCaravanMember())
				{
					return Mathf.InverseLerp(6f, 22f, GenLocalDate.HourFloat(this.pawn));
				}
				if (this.pawn.timetable == null)
				{
					return GenLocalDate.DayPercent(this.pawn);
				}
				float hoursPerDayNotSleeping = this.HoursPerDayNotSleeping;
				if (hoursPerDayNotSleeping == 0f)
				{
					return 1f;
				}
				float num = 0f;
				int num2 = GenLocalDate.HourOfDay(this.pawn);
				for (int i = 0; i < num2; i++)
				{
					if (this.pawn.timetable.times[i] != TimeAssignmentDefOf.Sleep)
					{
						num += 1f;
					}
				}
				if (this.pawn.timetable.CurrentAssignment != TimeAssignmentDefOf.Sleep)
				{
					float num3 = (float)(Find.TickManager.TicksAbs % 2500) / 2500f;
					num += num3;
				}
				return num / hoursPerDayNotSleeping;
			}
		}

		// Token: 0x170017C2 RID: 6082
		// (get) Token: 0x060089C0 RID: 35264 RVA: 0x0031DF5C File Offset: 0x0031C15C
		private float HoursPerDayNotSleeping
		{
			get
			{
				if (this.pawn.IsCaravanMember())
				{
					return 16f;
				}
				int num = 0;
				for (int i = 0; i < 24; i++)
				{
					if (this.pawn.timetable.times[i] != TimeAssignmentDefOf.Sleep)
					{
						num++;
					}
				}
				return (float)num;
			}
		}

		// Token: 0x060089C1 RID: 35265 RVA: 0x0031DFAE File Offset: 0x0031C1AE
		public Pawn_DrugPolicyTracker()
		{
		}

		// Token: 0x060089C2 RID: 35266 RVA: 0x0031DFC1 File Offset: 0x0031C1C1
		public Pawn_DrugPolicyTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060089C3 RID: 35267 RVA: 0x0031DFDC File Offset: 0x0031C1DC
		public void ExposeData()
		{
			Scribe_References.Look<DrugPolicy>(ref this.curPolicy, "curAssignedDrugs", false);
			Scribe_Collections.Look<DrugTakeRecord>(ref this.drugTakeRecords, "drugTakeRecords", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.drugTakeRecords.RemoveAll((DrugTakeRecord x) => x.drug == null) != 0)
				{
					Log.ErrorOnce("Removed some null drugs from drug policy tracker", 816929737);
				}
			}
		}

		// Token: 0x060089C4 RID: 35268 RVA: 0x0031E054 File Offset: 0x0031C254
		public bool HasEverTaken(ThingDef drug)
		{
			if (!drug.IsDrug)
			{
				ThingDef drug2 = drug;
				Log.Warning(((drug2 != null) ? drug2.ToString() : null) + " is not a drug.");
				return false;
			}
			return this.drugTakeRecords.Any((DrugTakeRecord x) => x.drug == drug);
		}

		// Token: 0x060089C5 RID: 35269 RVA: 0x0031E0B8 File Offset: 0x0031C2B8
		public bool AllowedToTakeToInventory(ThingDef thingDef)
		{
			if (!thingDef.IsIngestible)
			{
				Log.Error(((thingDef != null) ? thingDef.ToString() : null) + " is not ingestible.");
				return false;
			}
			if (!thingDef.IsDrug)
			{
				Log.Error("AllowedToTakeScheduledEver on non-drug " + ((thingDef != null) ? thingDef.ToString() : null));
				return false;
			}
			if (thingDef.IsNonMedicalDrug && !this.pawn.CanTakeDrug(thingDef))
			{
				return false;
			}
			DrugPolicyEntry drugPolicyEntry = this.CurrentPolicy[thingDef];
			return !drugPolicyEntry.allowScheduled && drugPolicyEntry.takeToInventory > 0 && !this.pawn.inventory.innerContainer.Contains(thingDef);
		}

		// Token: 0x060089C6 RID: 35270 RVA: 0x0031E164 File Offset: 0x0031C364
		public bool AllowedToTakeScheduledEver(ThingDef thingDef)
		{
			if (!thingDef.IsIngestible)
			{
				Log.Error(((thingDef != null) ? thingDef.ToString() : null) + " is not ingestible.");
				return false;
			}
			if (!thingDef.IsDrug)
			{
				Log.Error("AllowedToTakeScheduledEver on non-drug " + ((thingDef != null) ? thingDef.ToString() : null));
				return false;
			}
			return this.CurrentPolicy[thingDef].allowScheduled && (!thingDef.IsNonMedicalDrug || this.pawn.CanTakeDrug(thingDef)) && (thingDef.ingestible.drugCategory != DrugCategory.Hard || new HistoryEvent(HistoryEventDefOf.IngestedHardDrug, this.pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo());
		}

		// Token: 0x060089C7 RID: 35271 RVA: 0x0031E21C File Offset: 0x0031C41C
		public bool AllowedToTakeScheduledNow(ThingDef thingDef)
		{
			if (!thingDef.IsIngestible)
			{
				ThingDef thingDef2 = thingDef;
				Log.Error(((thingDef2 != null) ? thingDef2.ToString() : null) + " is not ingestible.");
				return false;
			}
			if (!thingDef.IsDrug)
			{
				string str = "AllowedToTakeScheduledEver on non-drug ";
				ThingDef thingDef3 = thingDef;
				Log.Error(str + ((thingDef3 != null) ? thingDef3.ToString() : null));
				return false;
			}
			if (!this.AllowedToTakeScheduledEver(thingDef))
			{
				return false;
			}
			DrugPolicyEntry drugPolicyEntry = this.CurrentPolicy[thingDef];
			if (drugPolicyEntry.onlyIfMoodBelow < 1f && this.pawn.needs.mood != null && this.pawn.needs.mood.CurLevelPercentage >= drugPolicyEntry.onlyIfMoodBelow)
			{
				return false;
			}
			if (drugPolicyEntry.onlyIfJoyBelow < 1f && this.pawn.needs.joy != null && this.pawn.needs.joy.CurLevelPercentage >= drugPolicyEntry.onlyIfJoyBelow)
			{
				return false;
			}
			DrugTakeRecord drugTakeRecord = this.drugTakeRecords.Find((DrugTakeRecord x) => x.drug == thingDef);
			if (drugTakeRecord != null)
			{
				if (drugPolicyEntry.daysFrequency < 1f)
				{
					int num = Mathf.RoundToInt(1f / drugPolicyEntry.daysFrequency);
					if (drugTakeRecord.TimesTakenThisDay >= num)
					{
						return false;
					}
				}
				else
				{
					int num2 = Mathf.Abs(GenDate.DaysPassed - drugTakeRecord.LastTakenDays);
					int num3 = Mathf.RoundToInt(drugPolicyEntry.daysFrequency);
					if (num2 < num3)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060089C8 RID: 35272 RVA: 0x0031E3A0 File Offset: 0x0031C5A0
		public bool ShouldTryToTakeScheduledNow(ThingDef ingestible)
		{
			if (!ingestible.IsDrug)
			{
				return false;
			}
			if (!this.AllowedToTakeScheduledNow(ingestible))
			{
				return false;
			}
			Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DrugOverdose, false);
			if (firstHediffOfDef != null && firstHediffOfDef.Severity > 0.5f && this.CanCauseOverdose(ingestible))
			{
				int num = this.LastTicksWhenTakenDrugWhichCanCauseOverdose();
				if (Find.TickManager.TicksGame - num < 1250)
				{
					return false;
				}
			}
			DrugTakeRecord drugTakeRecord = this.drugTakeRecords.Find((DrugTakeRecord x) => x.drug == ingestible);
			if (drugTakeRecord == null)
			{
				return true;
			}
			DrugPolicyEntry drugPolicyEntry = this.CurrentPolicy[ingestible];
			if (drugPolicyEntry.daysFrequency < 1f)
			{
				int num2 = Mathf.RoundToInt(1f / drugPolicyEntry.daysFrequency);
				float num3 = 1f / (float)(num2 + 1);
				int num4 = 0;
				float dayPercentNotSleeping = this.DayPercentNotSleeping;
				for (int i = 0; i < num2; i++)
				{
					if (dayPercentNotSleeping > (float)(i + 1) * num3 - num3 * 0.5f)
					{
						num4++;
					}
				}
				return drugTakeRecord.TimesTakenThisDay < num4 && (drugTakeRecord.TimesTakenThisDay == 0 || (float)(Find.TickManager.TicksGame - drugTakeRecord.lastTakenTicks) / (this.HoursPerDayNotSleeping * 2500f) >= 0.6f * num3);
			}
			float dayPercentNotSleeping2 = this.DayPercentNotSleeping;
			Rand.PushState();
			Rand.Seed = Gen.HashCombineInt(GenDate.DaysPassed, this.pawn.thingIDNumber);
			bool result = dayPercentNotSleeping2 >= Rand.Range(0.1f, 0.35f);
			Rand.PopState();
			return result;
		}

		// Token: 0x060089C9 RID: 35273 RVA: 0x0031E548 File Offset: 0x0031C748
		public void Notify_DrugIngested(Thing drug)
		{
			DrugTakeRecord drugTakeRecord = this.drugTakeRecords.Find((DrugTakeRecord x) => x.drug == drug.def);
			if (drugTakeRecord == null)
			{
				drugTakeRecord = new DrugTakeRecord();
				drugTakeRecord.drug = drug.def;
				this.drugTakeRecords.Add(drugTakeRecord);
			}
			drugTakeRecord.lastTakenTicks = Find.TickManager.TicksGame;
			DrugTakeRecord drugTakeRecord2 = drugTakeRecord;
			int timesTakenThisDay = drugTakeRecord2.TimesTakenThisDay;
			drugTakeRecord2.TimesTakenThisDay = timesTakenThisDay + 1;
		}

		// Token: 0x060089CA RID: 35274 RVA: 0x0031E5C0 File Offset: 0x0031C7C0
		public void Notify_LeftSuspension(int suspendedTicks)
		{
			foreach (DrugTakeRecord drugTakeRecord in this.drugTakeRecords)
			{
				drugTakeRecord.Notify_LeftSuspension(suspendedTicks);
			}
		}

		// Token: 0x060089CB RID: 35275 RVA: 0x0031E614 File Offset: 0x0031C814
		private int LastTicksWhenTakenDrugWhichCanCauseOverdose()
		{
			int num = -999999;
			for (int i = 0; i < this.drugTakeRecords.Count; i++)
			{
				if (this.CanCauseOverdose(this.drugTakeRecords[i].drug))
				{
					num = Mathf.Max(num, this.drugTakeRecords[i].lastTakenTicks);
				}
			}
			return num;
		}

		// Token: 0x060089CC RID: 35276 RVA: 0x0031E670 File Offset: 0x0031C870
		private bool CanCauseOverdose(ThingDef drug)
		{
			CompProperties_Drug compProperties = drug.GetCompProperties<CompProperties_Drug>();
			return compProperties != null && compProperties.CanCauseOverdose;
		}

		// Token: 0x040054D6 RID: 21718
		public Pawn pawn;

		// Token: 0x040054D7 RID: 21719
		private DrugPolicy curPolicy;

		// Token: 0x040054D8 RID: 21720
		private List<DrugTakeRecord> drugTakeRecords = new List<DrugTakeRecord>();

		// Token: 0x040054D9 RID: 21721
		private const float DangerousDrugOverdoseSeverity = 0.5f;
	}
}
