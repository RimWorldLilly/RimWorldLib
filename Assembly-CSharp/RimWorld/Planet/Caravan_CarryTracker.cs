using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023A7 RID: 9127
	public class Caravan_CarryTracker : IExposable
	{
		// Token: 0x170027E6 RID: 10214
		// (get) Token: 0x0600E31C RID: 58140 RVA: 0x0054798A File Offset: 0x00545B8A
		public List<Pawn> CarriedPawnsListForReading
		{
			get
			{
				return this.carriedPawns;
			}
		}

		// Token: 0x0600E31D RID: 58141 RVA: 0x00547992 File Offset: 0x00545B92
		public Caravan_CarryTracker()
		{
		}

		// Token: 0x0600E31E RID: 58142 RVA: 0x005479A5 File Offset: 0x00545BA5
		public Caravan_CarryTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x0600E31F RID: 58143 RVA: 0x005479BF File Offset: 0x00545BBF
		public void CarryTrackerTickInterval(int delta)
		{
			this.RecalculateCarriedPawns();
		}

		// Token: 0x0600E320 RID: 58144 RVA: 0x005479C7 File Offset: 0x00545BC7
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecalculateCarriedPawns();
			}
		}

		// Token: 0x0600E321 RID: 58145 RVA: 0x005479D7 File Offset: 0x00545BD7
		public bool IsCarried(Pawn p)
		{
			return this.carriedPawns.Contains(p);
		}

		// Token: 0x0600E322 RID: 58146 RVA: 0x005479E8 File Offset: 0x00545BE8
		private void RecalculateCarriedPawns()
		{
			this.carriedPawns.Clear();
			if (!this.caravan.Spawned)
			{
				return;
			}
			if (this.caravan.pather.MovingNow)
			{
				Caravan_CarryTracker.tmpPawnsWhoCanCarry.Clear();
				this.CalculatePawnsWhoCanCarry(Caravan_CarryTracker.tmpPawnsWhoCanCarry);
				int num = 0;
				while (num < this.caravan.pawns.Count && Caravan_CarryTracker.tmpPawnsWhoCanCarry.Any<Pawn>())
				{
					Pawn pawn = this.caravan.pawns[num];
					if (this.WantsToBeCarried(pawn) && Caravan_CarryTracker.tmpPawnsWhoCanCarry.Any<Pawn>())
					{
						this.carriedPawns.Add(pawn);
						Caravan_CarryTracker.tmpPawnsWhoCanCarry.RemoveLast<Pawn>();
					}
					num++;
				}
				Caravan_CarryTracker.tmpPawnsWhoCanCarry.Clear();
			}
		}

		// Token: 0x0600E323 RID: 58147 RVA: 0x00547AA6 File Offset: 0x00545CA6
		public void Notify_CaravanSpawned()
		{
			this.RecalculateCarriedPawns();
		}

		// Token: 0x0600E324 RID: 58148 RVA: 0x00547AAE File Offset: 0x00545CAE
		public void Notify_PawnRemoved()
		{
			this.RecalculateCarriedPawns();
		}

		// Token: 0x0600E325 RID: 58149 RVA: 0x00547AB8 File Offset: 0x00545CB8
		private void CalculatePawnsWhoCanCarry(List<Pawn> outPawns)
		{
			outPawns.Clear();
			for (int i = 0; i < this.caravan.pawns.Count; i++)
			{
				Pawn pawn = this.caravan.pawns[i];
				if (pawn.RaceProps.Humanlike && !pawn.Downed && !pawn.InMentalState && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) && !this.WantsToBeCarried(pawn))
				{
					outPawns.Add(pawn);
				}
			}
		}

		// Token: 0x0600E326 RID: 58150 RVA: 0x00547B3C File Offset: 0x00545D3C
		private bool WantsToBeCarried(Pawn p)
		{
			return p.health.beCarriedByCaravanIfSick && CaravanCarryUtility.WouldBenefitFromBeingCarried(p);
		}

		// Token: 0x0600E327 RID: 58151 RVA: 0x00547B54 File Offset: 0x00545D54
		public string GetInspectStringLine()
		{
			if (!this.carriedPawns.Any<Pawn>())
			{
				return null;
			}
			Caravan_CarryTracker.tmpPawnLabels.Clear();
			int num = 0;
			for (int i = 0; i < this.carriedPawns.Count; i++)
			{
				Caravan_CarryTracker.tmpPawnLabels.Add(this.carriedPawns[i].LabelShort);
				if (this.caravan.beds.IsInBed(this.carriedPawns[i]))
				{
					num++;
				}
			}
			string str = (Caravan_CarryTracker.tmpPawnLabels.Count > 5) ? (Caravan_CarryTracker.tmpPawnLabels.Take(5).ToCommaList(false, false) + "...") : Caravan_CarryTracker.tmpPawnLabels.ToCommaList(true, false);
			string result = CaravanBedUtility.AppendUsingBedsLabel("BeingCarriedDueToIllness".Translate() + ": " + str.CapitalizeFirst(), num);
			Caravan_CarryTracker.tmpPawnLabels.Clear();
			return result;
		}

		// Token: 0x040090E9 RID: 37097
		public Caravan caravan;

		// Token: 0x040090EA RID: 37098
		private List<Pawn> carriedPawns = new List<Pawn>();

		// Token: 0x040090EB RID: 37099
		private static List<Pawn> tmpPawnsWhoCanCarry = new List<Pawn>();

		// Token: 0x040090EC RID: 37100
		private static readonly List<string> tmpPawnLabels = new List<string>();
	}
}
