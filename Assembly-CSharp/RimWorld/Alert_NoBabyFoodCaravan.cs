using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BCA RID: 7114
	public class Alert_NoBabyFoodCaravan : Alert
	{
		// Token: 0x0600B64F RID: 46671 RVA: 0x00403236 File Offset: 0x00401436
		public Alert_NoBabyFoodCaravan()
		{
			this.defaultLabel = "AlertNoBabyFoodCaravan".Translate();
			this.defaultPriority = AlertPriority.High;
			this.requireBiotech = true;
		}

		// Token: 0x0600B650 RID: 46672 RVA: 0x0040326D File Offset: 0x0040146D
		public override TaggedString GetExplanation()
		{
			return "AlertNoBabyFoodCaravanDesc".Translate(Faction.OfPlayer.Named("FACTION"));
		}

		// Token: 0x0600B651 RID: 46673 RVA: 0x00403288 File Offset: 0x00401488
		public override AlertReport GetReport()
		{
			return new AlertReport
			{
				active = this.LowBabyFoodNutrition(this.caravansReportOut),
				culpritsCaravans = this.caravansReportOut
			};
		}

		// Token: 0x0600B652 RID: 46674 RVA: 0x004032C0 File Offset: 0x004014C0
		private bool LowBabyFoodNutrition(List<Caravan> foodlessCaravansOut)
		{
			foodlessCaravansOut.Clear();
			foreach (Caravan caravan in Find.WorldObjects.Caravans)
			{
				bool flag = false;
				for (int i = 0; i < caravan.PawnsListForReading.Count; i++)
				{
					Pawn pawn = caravan.PawnsListForReading[i];
					ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
					if (ChildcareUtility.CanSuckle(pawn, out breastfeedFailReason))
					{
						Predicate<Pawn, Pawn> feederPredicate = delegate(Pawn _baby, Pawn _mom)
						{
							ChildcareUtility.BreastfeedFailReason? breastfeedFailReason2;
							return ChildcareUtility.CanMomBreastfeedBaby(_mom, _baby, out breastfeedFailReason2);
						};
						if (!pawn.mindState.AnyAutofeeder(AutofeedMode.Urgent, feederPredicate, caravan.PawnsListForReading) && !pawn.mindState.AnyAutofeeder(AutofeedMode.Childcare, feederPredicate, caravan.PawnsListForReading))
						{
							flag = true;
						}
					}
				}
				if (flag)
				{
					if (!CaravanInventoryUtility.AllInventoryItems(caravan).Any((Thing thing) => thing.def.IsNutritionGivingIngestibleForHumanlikeBabies))
					{
						foodlessCaravansOut.Add(caravan);
					}
				}
			}
			return foodlessCaravansOut.Count > 0;
		}

		// Token: 0x040068B8 RID: 26808
		private List<Caravan> caravansReportOut = new List<Caravan>(8);
	}
}
