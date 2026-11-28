using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000832 RID: 2098
	public class JobDriver_DoBill : JobDriver
	{
		// Token: 0x060041A5 RID: 16805 RVA: 0x0019FC68 File Offset: 0x0019DE68
		public override string GetReport()
		{
			if (ModsConfig.BiotechActive)
			{
				Bill_Mech bill_Mech = this.job.bill as Bill_Mech;
				if (bill_Mech != null)
				{
					return MechanitorUtility.GetMechGestationJobString(this, this.pawn, bill_Mech);
				}
			}
			if (this.job.RecipeDef != null)
			{
				return this.ReportStringProcessed(this.job.RecipeDef.jobString);
			}
			return base.GetReport();
		}

		// Token: 0x17000C8F RID: 3215
		// (get) Token: 0x060041A6 RID: 16806 RVA: 0x0019FCC8 File Offset: 0x0019DEC8
		public IBillGiver BillGiver
		{
			get
			{
				IBillGiver billGiver = this.job.GetTarget(TargetIndex.A).Thing as IBillGiver;
				if (billGiver == null)
				{
					throw new InvalidOperationException("DoBill on non-Billgiver.");
				}
				return billGiver;
			}
		}

		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x060041A7 RID: 16807 RVA: 0x0019FCFC File Offset: 0x0019DEFC
		public bool AnyIngredientsQueued
		{
			get
			{
				return !this.job.GetTargetQueue(TargetIndex.B).NullOrEmpty<LocalTargetInfo>();
			}
		}

		// Token: 0x060041A8 RID: 16808 RVA: 0x0019FD12 File Offset: 0x0019DF12
		public override bool IsContinuation(Job j)
		{
			return j.bill == this.job.bill;
		}

		// Token: 0x060041A9 RID: 16809 RVA: 0x0019FD28 File Offset: 0x0019DF28
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Values.Look<int>(ref this.billStartTick, "billStartTick", 0, false);
			Scribe_Values.Look<int>(ref this.ticksSpentDoingRecipeWork, "ticksSpentDoingRecipeWork", 0, false);
		}

		// Token: 0x060041AA RID: 16810 RVA: 0x0019FD78 File Offset: 0x0019DF78
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
			if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false))
			{
				return false;
			}
			if (thing != null && thing.def.hasInteractionCell && !this.pawn.ReserveSittableOrSpot(thing.InteractionCell, this.job, errorOnFailed))
			{
				return false;
			}
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x060041AB RID: 16811 RVA: 0x0019FE0D File Offset: 0x0019E00D
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_DoBill.<MakeNewToils>d__15 <MakeNewToils>d__ = new JobDriver_DoBill.<MakeNewToils>d__15(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x060041AC RID: 16812 RVA: 0x0019FE1D File Offset: 0x0019E01D
		public static IEnumerable<Toil> CollectIngredientsToils(TargetIndex ingredientInd, TargetIndex billGiverInd, TargetIndex ingredientPlaceCellInd, bool subtractNumTakenFromJobCount = false, bool failIfStackCountLessThanJobCount = true, bool placeInBillGiver = false)
		{
			JobDriver_DoBill.<CollectIngredientsToils>d__16 <CollectIngredientsToils>d__ = new JobDriver_DoBill.<CollectIngredientsToils>d__16(-2);
			<CollectIngredientsToils>d__.<>3__ingredientInd = ingredientInd;
			<CollectIngredientsToils>d__.<>3__billGiverInd = billGiverInd;
			<CollectIngredientsToils>d__.<>3__ingredientPlaceCellInd = ingredientPlaceCellInd;
			<CollectIngredientsToils>d__.<>3__subtractNumTakenFromJobCount = subtractNumTakenFromJobCount;
			<CollectIngredientsToils>d__.<>3__failIfStackCountLessThanJobCount = failIfStackCountLessThanJobCount;
			<CollectIngredientsToils>d__.<>3__placeInBillGiver = placeInBillGiver;
			return <CollectIngredientsToils>d__;
		}

		// Token: 0x060041AD RID: 16813 RVA: 0x0019FE54 File Offset: 0x0019E054
		private static Toil JumpIfTargetInsideBillGiver(Toil jumpToil, TargetIndex ingredient, TargetIndex billGiver)
		{
			Toil toil = ToilMaker.MakeToil("JumpIfTargetInsideBillGiver");
			toil.initAction = delegate()
			{
				Thing thing = toil.actor.CurJob.GetTarget(billGiver).Thing;
				if (thing == null || !thing.Spawned)
				{
					return;
				}
				Thing thing2 = toil.actor.jobs.curJob.GetTarget(ingredient).Thing;
				if (thing2 == null)
				{
					return;
				}
				ThingOwner thingOwner = thing.TryGetInnerInteractableThingOwner();
				if (thingOwner != null && thingOwner.Contains(thing2))
				{
					HaulAIUtility.UpdateJobWithPlacedThings(toil.actor.jobs.curJob, thing2, thing2.stackCount);
					toil.actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x060041AE RID: 16814 RVA: 0x0019FEAC File Offset: 0x0019E0AC
		public static Toil JumpToCollectNextIntoHandsForBill(Toil gotoGetTargetToil, TargetIndex ind)
		{
			Toil toil = ToilMaker.MakeToil("JumpToCollectNextIntoHandsForBill");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				if (actor.carryTracker.CarriedThing == null)
				{
					string str = "JumpToAlsoCollectTargetInQueue run on ";
					Pawn pawn = actor;
					Log.Error(str + ((pawn != null) ? pawn.ToString() : null) + " who is not carrying something.");
					return;
				}
				if (actor.carryTracker.Full)
				{
					return;
				}
				Job curJob = actor.jobs.curJob;
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
				if (targetQueue.NullOrEmpty<LocalTargetInfo>())
				{
					return;
				}
				for (int i = 0; i < targetQueue.Count; i++)
				{
					if (GenAI.CanUseItemForWork(actor, targetQueue[i].Thing) && targetQueue[i].Thing.CanStackWith(actor.carryTracker.CarriedThing) && (float)(actor.Position - targetQueue[i].Thing.Position).LengthHorizontalSquared <= 64f)
					{
						int num = (actor.carryTracker.CarriedThing == null) ? 0 : actor.carryTracker.CarriedThing.stackCount;
						int num2 = curJob.countQueue[i];
						num2 = Mathf.Min(num2, targetQueue[i].Thing.def.stackLimit - num);
						num2 = Mathf.Min(num2, actor.carryTracker.AvailableStackSpace(targetQueue[i].Thing.def));
						if (num2 > 0)
						{
							curJob.count = num2;
							curJob.SetTarget(ind, targetQueue[i].Thing);
							List<int> countQueue = curJob.countQueue;
							int index = i;
							countQueue[index] -= num2;
							if (curJob.countQueue[i] <= 0)
							{
								curJob.countQueue.RemoveAt(i);
								targetQueue.RemoveAt(i);
							}
							actor.jobs.curDriver.JumpToToil(gotoGetTargetToil);
							return;
						}
					}
				}
			};
			return toil;
		}

		// Token: 0x04002B2D RID: 11053
		public float workLeft;

		// Token: 0x04002B2E RID: 11054
		public int billStartTick;

		// Token: 0x04002B2F RID: 11055
		public int ticksSpentDoingRecipeWork;

		// Token: 0x04002B30 RID: 11056
		public const PathEndMode GotoIngredientPathEndMode = PathEndMode.ClosestTouch;

		// Token: 0x04002B31 RID: 11057
		public const TargetIndex BillGiverInd = TargetIndex.A;

		// Token: 0x04002B32 RID: 11058
		public const TargetIndex IngredientInd = TargetIndex.B;

		// Token: 0x04002B33 RID: 11059
		public const TargetIndex IngredientPlaceCellInd = TargetIndex.C;
	}
}
