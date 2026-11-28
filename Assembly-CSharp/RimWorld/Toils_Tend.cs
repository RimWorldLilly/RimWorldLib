using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009EE RID: 2542
	public class Toils_Tend
	{
		// Token: 0x06004D13 RID: 19731 RVA: 0x001D90B0 File Offset: 0x001D72B0
		public static Toil ReserveMedicine(TargetIndex ind, Pawn injured)
		{
			Toil toil = ToilMaker.MakeToil("ReserveMedicine");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(ind).Thing;
				int num = actor.Map.reservationManager.CanReserveStack(actor, thing, 10, null, false);
				if (num <= 0 || !actor.Reserve(thing, curJob, 10, Mathf.Min(num, Medicine.GetMedicineCountToFullyHeal(injured)), null, true, false))
				{
					toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x06004D14 RID: 19732 RVA: 0x001D9118 File Offset: 0x001D7318
		public static Toil PickupMedicine(TargetIndex ind, Pawn injured)
		{
			Toil toil = ToilMaker.MakeToil("PickupMedicine");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(ind).Thing;
				int num = Medicine.GetMedicineCountToFullyHeal(injured);
				if (actor.carryTracker.CarriedThing != null)
				{
					num -= actor.carryTracker.CarriedThing.stackCount;
				}
				int num2 = Mathf.Min(actor.Map.reservationManager.CanReserveStack(actor, thing, 10, null, false), num);
				if (num2 > 0)
				{
					actor.carryTracker.TryStartCarry(thing, num2, true);
				}
				curJob.count = num - num2;
				if (thing.Spawned)
				{
					toil.actor.Map.reservationManager.Release(thing, actor, curJob);
				}
				curJob.SetTarget(ind, actor.carryTracker.CarriedThing);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x06004D15 RID: 19733 RVA: 0x001D9174 File Offset: 0x001D7374
		public static Toil FinalizeTend(Pawn patient)
		{
			Toil toil = ToilMaker.MakeToil("FinalizeTend");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Medicine medicine = (Medicine)actor.CurJob.targetB.Thing;
				if (actor.skills != null)
				{
					float num = patient.RaceProps.Animal ? 175f : 500f;
					float num2 = (medicine == null) ? 0.5f : medicine.def.MedicineTendXpGainFactor;
					actor.skills.Learn(SkillDefOf.Medicine, num * num2, false, false);
				}
				TendUtility.DoTend(actor, patient, medicine);
				if (medicine != null && medicine.Destroyed)
				{
					actor.CurJob.SetTarget(TargetIndex.B, LocalTargetInfo.Invalid);
				}
				if (toil.actor.CurJob.endAfterTendedOnce)
				{
					actor.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x04003044 RID: 12356
		public const int MaxMedicineReservations = 10;
	}
}
