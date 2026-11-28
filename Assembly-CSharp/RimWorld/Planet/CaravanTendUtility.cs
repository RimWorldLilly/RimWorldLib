using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002391 RID: 9105
	public static class CaravanTendUtility
	{
		// Token: 0x0600E275 RID: 57973 RVA: 0x00541428 File Offset: 0x0053F628
		public static void CheckTend(Caravan caravan, int delta)
		{
			for (int i = 0; i < caravan.pawns.Count; i++)
			{
				Pawn pawn = caravan.pawns[i];
				if (CaravanTendUtility.IsValidDoctorFor(pawn, null, caravan) && pawn.IsHashIntervalTick(1250, delta))
				{
					CaravanTendUtility.TryTendToAnyPawn(caravan);
				}
			}
		}

		// Token: 0x0600E276 RID: 57974 RVA: 0x00541478 File Offset: 0x0053F678
		public static void TryTendToAnyPawn(Caravan caravan)
		{
			CaravanTendUtility.FindPawnsNeedingTend(caravan, CaravanTendUtility.tmpPawnsNeedingTreatment);
			if (!CaravanTendUtility.tmpPawnsNeedingTreatment.Any<Pawn>())
			{
				return;
			}
			CaravanTendUtility.tmpPawnsNeedingTreatment.SortByDescending(new Func<Pawn, float>(CaravanTendUtility.GetTendPriority));
			Pawn patient = null;
			Pawn pawn = null;
			for (int i = 0; i < CaravanTendUtility.tmpPawnsNeedingTreatment.Count; i++)
			{
				patient = CaravanTendUtility.tmpPawnsNeedingTreatment[i];
				pawn = CaravanTendUtility.FindBestDoctorFor(caravan, patient);
				if (pawn != null)
				{
					break;
				}
			}
			if (pawn == null)
			{
				return;
			}
			Medicine medicine = null;
			Pawn pawn2 = null;
			CaravanInventoryUtility.TryGetBestMedicine(caravan, patient, out medicine, out pawn2);
			TendUtility.DoTend(pawn, patient, medicine);
			if (medicine != null && medicine.Destroyed && pawn2 != null)
			{
				pawn2.inventory.innerContainer.Remove(medicine);
			}
			CaravanTendUtility.tmpPawnsNeedingTreatment.Clear();
		}

		// Token: 0x0600E277 RID: 57975 RVA: 0x00541530 File Offset: 0x0053F730
		private static void FindPawnsNeedingTend(Caravan caravan, List<Pawn> outPawnsNeedingTend)
		{
			outPawnsNeedingTend.Clear();
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Pawn pawn = pawnsListForReading[i];
				if ((pawn.playerSettings == null || pawn.playerSettings.medCare > MedicalCareCategory.NoCare) && pawn.health.HasHediffsNeedingTend(false))
				{
					outPawnsNeedingTend.Add(pawn);
				}
			}
		}

		// Token: 0x0600E278 RID: 57976 RVA: 0x00541590 File Offset: 0x0053F790
		private static Pawn FindBestDoctorFor(Caravan caravan, Pawn patient)
		{
			float num = 0f;
			Pawn pawn = null;
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Pawn pawn2 = pawnsListForReading[i];
				if (CaravanTendUtility.IsValidDoctorFor(pawn2, patient, caravan))
				{
					float statValue = pawn2.GetStatValue(StatDefOf.MedicalTendQuality, true, -1);
					if (statValue > num || pawn == null)
					{
						num = statValue;
						pawn = pawn2;
					}
				}
			}
			return pawn;
		}

		// Token: 0x0600E279 RID: 57977 RVA: 0x005415F0 File Offset: 0x0053F7F0
		private static bool IsValidDoctorFor(Pawn doctor, Pawn patient, Caravan caravan)
		{
			if (!doctor.RaceProps.Humanlike)
			{
				if (!doctor.IsColonyMechPlayerControlled || !doctor.RaceProps.mechEnabledWorkTypes.NotNullAndContains(WorkTypeDefOf.Doctor))
				{
					return false;
				}
			}
			else if (!caravan.IsOwner(doctor))
			{
				return false;
			}
			return (doctor != patient || (doctor.IsColonist && doctor.playerSettings.selfTend)) && !doctor.Downed && !doctor.InMentalState && (doctor.story == null || !doctor.WorkTypeIsDisabled(WorkTypeDefOf.Doctor));
		}

		// Token: 0x0600E27A RID: 57978 RVA: 0x0054167C File Offset: 0x0053F87C
		private static float GetTendPriority(Pawn patient)
		{
			int num = HealthUtility.TicksUntilDeathDueToBloodLoss(patient);
			if (num < 15000)
			{
				if (patient.RaceProps.Humanlike)
				{
					return GenMath.LerpDouble(0f, 15000f, 5f, 4f, (float)num);
				}
				return GenMath.LerpDouble(0f, 15000f, 4f, 3f, (float)num);
			}
			else
			{
				int i = 0;
				while (i < patient.health.hediffSet.hediffs.Count)
				{
					Hediff hediff = patient.health.hediffSet.hediffs[i];
					if ((hediff.IsCurrentlyLifeThreatening || hediff.IsLethal) && hediff.TendableNow(false))
					{
						if (patient.RaceProps.Humanlike)
						{
							return 2.5f;
						}
						return 2f;
					}
					else
					{
						i++;
					}
				}
				if (patient.health.hediffSet.BleedRateTotal >= 0.0001f)
				{
					if (patient.RaceProps.Humanlike)
					{
						return 1.5f;
					}
					return 1f;
				}
				else
				{
					if (patient.RaceProps.Humanlike)
					{
						return 0.5f;
					}
					return 0f;
				}
			}
		}

		// Token: 0x0400908B RID: 37003
		private static readonly List<Pawn> tmpPawnsNeedingTreatment = new List<Pawn>();

		// Token: 0x0400908C RID: 37004
		private const int TendIntervalTicks = 1250;
	}
}
