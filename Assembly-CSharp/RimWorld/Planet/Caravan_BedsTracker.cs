using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023A6 RID: 9126
	public class Caravan_BedsTracker : IExposable
	{
		// Token: 0x0600E30E RID: 58126 RVA: 0x005474F4 File Offset: 0x005456F4
		public Caravan_BedsTracker()
		{
		}

		// Token: 0x0600E30F RID: 58127 RVA: 0x00547507 File Offset: 0x00545707
		public Caravan_BedsTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x0600E310 RID: 58128 RVA: 0x00547524 File Offset: 0x00545724
		public void BedsTrackerTickInterval(int delta)
		{
			this.RecalculateUsedBeds();
			foreach (KeyValuePair<Pawn, Building_Bed> keyValuePair in this.usedBeds)
			{
				PawnUtility.GainComfortFromThingIfPossible(keyValuePair.Key, keyValuePair.Value, delta);
			}
		}

		// Token: 0x0600E311 RID: 58129 RVA: 0x0054758C File Offset: 0x0054578C
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecalculateUsedBeds();
			}
		}

		// Token: 0x0600E312 RID: 58130 RVA: 0x0054759C File Offset: 0x0054579C
		private void RecalculateUsedBeds()
		{
			this.usedBeds.Clear();
			if (!this.caravan.Spawned)
			{
				return;
			}
			Caravan_BedsTracker.tmpUsableBeds.Clear();
			this.GetUsableBeds(Caravan_BedsTracker.tmpUsableBeds);
			if (!this.caravan.pather.MovingNow)
			{
				Caravan_BedsTracker.tmpUsableBeds.SortByDescending((Building_Bed x) => x.GetStatValue(StatDefOf.BedRestEffectiveness, true, -1));
				for (int i = 0; i < this.caravan.pawns.Count; i++)
				{
					Pawn pawn = this.caravan.pawns[i];
					if (pawn.needs != null && pawn.needs.rest != null)
					{
						Building_Bed andRemoveFirstAvailableBedFor = this.GetAndRemoveFirstAvailableBedFor(pawn, Caravan_BedsTracker.tmpUsableBeds);
						if (andRemoveFirstAvailableBedFor != null)
						{
							this.usedBeds.Add(pawn, andRemoveFirstAvailableBedFor);
						}
					}
				}
			}
			else
			{
				Caravan_BedsTracker.tmpUsableBeds.SortByDescending((Building_Bed x) => x.GetStatValue(StatDefOf.ImmunityGainSpeedFactor, true, -1));
				for (int j = 0; j < this.caravan.pawns.Count; j++)
				{
					Pawn pawn2 = this.caravan.pawns[j];
					if (pawn2.needs != null && pawn2.needs.rest != null && CaravanBedUtility.WouldBenefitFromRestingInBed(pawn2) && (!this.caravan.pather.MovingNow || pawn2.CarriedByCaravan()))
					{
						Building_Bed andRemoveFirstAvailableBedFor2 = this.GetAndRemoveFirstAvailableBedFor(pawn2, Caravan_BedsTracker.tmpUsableBeds);
						if (andRemoveFirstAvailableBedFor2 != null)
						{
							this.usedBeds.Add(pawn2, andRemoveFirstAvailableBedFor2);
						}
					}
				}
			}
			Caravan_BedsTracker.tmpUsableBeds.Clear();
		}

		// Token: 0x0600E313 RID: 58131 RVA: 0x0054773A File Offset: 0x0054593A
		public void Notify_CaravanSpawned()
		{
			this.RecalculateUsedBeds();
		}

		// Token: 0x0600E314 RID: 58132 RVA: 0x00547742 File Offset: 0x00545942
		public void Notify_PawnRemoved()
		{
			this.RecalculateUsedBeds();
		}

		// Token: 0x0600E315 RID: 58133 RVA: 0x0054774C File Offset: 0x0054594C
		public Building_Bed GetBedUsedBy(Pawn p)
		{
			Building_Bed building_Bed;
			if (this.usedBeds.TryGetValue(p, out building_Bed) && !building_Bed.DestroyedOrNull())
			{
				return building_Bed;
			}
			return null;
		}

		// Token: 0x0600E316 RID: 58134 RVA: 0x00547774 File Offset: 0x00545974
		public bool IsInBed(Pawn p)
		{
			return this.GetBedUsedBy(p) != null;
		}

		// Token: 0x0600E317 RID: 58135 RVA: 0x00547780 File Offset: 0x00545980
		public int GetUsedBedCount()
		{
			return this.usedBeds.Count;
		}

		// Token: 0x0600E318 RID: 58136 RVA: 0x00547790 File Offset: 0x00545990
		private void GetUsableBeds(List<Building_Bed> outBeds)
		{
			outBeds.Clear();
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(this.caravan);
			for (int i = 0; i < list.Count; i++)
			{
				Building_Bed building_Bed = list[i].GetInnerIfMinified() as Building_Bed;
				if (building_Bed != null && building_Bed.def.building.bed_caravansCanUse)
				{
					for (int j = 0; j < list[i].stackCount; j++)
					{
						for (int k = 0; k < building_Bed.SleepingSlotsCount; k++)
						{
							outBeds.Add(building_Bed);
						}
					}
				}
			}
		}

		// Token: 0x0600E319 RID: 58137 RVA: 0x0054781C File Offset: 0x00545A1C
		private Building_Bed GetAndRemoveFirstAvailableBedFor(Pawn p, List<Building_Bed> beds)
		{
			for (int i = 0; i < beds.Count; i++)
			{
				if (RestUtility.CanUseBedEver(p, beds[i].def))
				{
					Building_Bed result = beds[i];
					beds.RemoveAt(i);
					return result;
				}
			}
			return null;
		}

		// Token: 0x0600E31A RID: 58138 RVA: 0x00547860 File Offset: 0x00545A60
		public string GetInBedForMedicalReasonsInspectStringLine()
		{
			if (this.usedBeds.Count == 0)
			{
				return null;
			}
			Caravan_BedsTracker.tmpPawnLabels.Clear();
			foreach (KeyValuePair<Pawn, Building_Bed> keyValuePair in this.usedBeds)
			{
				if (!this.caravan.carryTracker.IsCarried(keyValuePair.Key) && CaravanBedUtility.WouldBenefitFromRestingInBed(keyValuePair.Key))
				{
					Caravan_BedsTracker.tmpPawnLabels.Add(keyValuePair.Key.LabelShort);
				}
			}
			if (!Caravan_BedsTracker.tmpPawnLabels.Any<string>())
			{
				return null;
			}
			string t = (Caravan_BedsTracker.tmpPawnLabels.Count > 5) ? (Caravan_BedsTracker.tmpPawnLabels.Take(5).ToCommaList(false, false) + "...") : Caravan_BedsTracker.tmpPawnLabels.ToCommaList(true, false);
			Caravan_BedsTracker.tmpPawnLabels.Clear();
			return "UsingBedrollsDueToIllness".Translate() + ": " + t;
		}

		// Token: 0x040090E5 RID: 37093
		public Caravan caravan;

		// Token: 0x040090E6 RID: 37094
		private Dictionary<Pawn, Building_Bed> usedBeds = new Dictionary<Pawn, Building_Bed>();

		// Token: 0x040090E7 RID: 37095
		private static List<Building_Bed> tmpUsableBeds = new List<Building_Bed>();

		// Token: 0x040090E8 RID: 37096
		private static List<string> tmpPawnLabels = new List<string>();
	}
}
