using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A18 RID: 2584
	public class JobDriver_CarryToBiosculpterPod : JobDriver
	{
		// Token: 0x17000E31 RID: 3633
		// (get) Token: 0x06004E08 RID: 19976 RVA: 0x001DC604 File Offset: 0x001DA804
		private Pawn Takee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000E32 RID: 3634
		// (get) Token: 0x06004E09 RID: 19977 RVA: 0x001DC62C File Offset: 0x001DA82C
		private CompBiosculpterPod Pod
		{
			get
			{
				return this.job.GetTarget(TargetIndex.C).Thing.TryGetComp<CompBiosculpterPod>();
			}
		}

		// Token: 0x06004E0A RID: 19978 RVA: 0x001DC654 File Offset: 0x001DA854
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!this.pawn.Reserve(this.Takee, this.job, 1, -1, null, errorOnFailed, false) || !this.pawn.Reserve(this.Pod.parent, this.job, 1, -1, null, errorOnFailed, false))
			{
				return false;
			}
			List<LocalTargetInfo> targetQueue = this.job.GetTargetQueue(TargetIndex.B);
			for (int i = 0; i < targetQueue.Count; i++)
			{
				if (!this.pawn.Reserve(targetQueue[i], this.job, 1, -1, null, errorOnFailed, false))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004E0B RID: 19979 RVA: 0x001DC6EE File Offset: 0x001DA8EE
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_CarryToBiosculpterPod.<MakeNewToils>d__10 <MakeNewToils>d__ = new JobDriver_CarryToBiosculpterPod.<MakeNewToils>d__10(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004E0C RID: 19980 RVA: 0x001DC6FE File Offset: 0x001DA8FE
		private Toil TransferIngredientsAndPrepareCarryDownedPawn()
		{
			Toil toil = ToilMaker.MakeToil("TransferIngredientsAndPrepareCarryDownedPawn");
			toil.initAction = delegate()
			{
				List<ThingDefCountClass> extraRequiredIngredients = this.Pod.GetCycle(this.job.biosculpterCycleKey).Props.extraRequiredIngredients;
				if (extraRequiredIngredients != null && !this.Pod.devFillPodLatch)
				{
					ThingOwner<Thing> innerContainer = this.pawn.inventory.innerContainer;
					foreach (ThingDefCountClass thingDefCountClass in extraRequiredIngredients)
					{
						if (this.Takee.inventory.Count(thingDefCountClass.thingDef) < thingDefCountClass.count)
						{
							if (this.pawn.inventory.Count(thingDefCountClass.thingDef) < thingDefCountClass.count)
							{
								base.EndJobWith(JobCondition.Incompletable);
								return;
							}
							this.thingsToTransfer.Clear();
							int num = 0;
							foreach (Thing thing in innerContainer)
							{
								if (num >= thingDefCountClass.count)
								{
									break;
								}
								if (thing.def == thingDefCountClass.thingDef)
								{
									int num2 = Mathf.Min(thingDefCountClass.count - num, thing.stackCount);
									this.thingsToTransfer.Add(new ThingCount(thing, Mathf.Min(thingDefCountClass.count - num, thing.stackCount), false));
									num += num2;
								}
							}
							foreach (ThingCount thingCount in this.thingsToTransfer)
							{
								int num3 = this.Takee.inventory.innerContainer.TryAddOrTransfer(thingCount.Thing, thingCount.Count, true);
								if (num3 != thingCount.Count)
								{
									DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(70, 3);
									defaultInterpolatedStringHandler.AppendLiteral("Only able to transfer x");
									defaultInterpolatedStringHandler.AppendFormatted<int>(num3);
									defaultInterpolatedStringHandler.AppendLiteral(" of the expected x");
									defaultInterpolatedStringHandler.AppendFormatted<int>(thingCount.Count);
									defaultInterpolatedStringHandler.AppendLiteral(" of ");
									defaultInterpolatedStringHandler.AppendFormatted(thingCount.Thing.Label);
									defaultInterpolatedStringHandler.AppendLiteral(" while CarryToBiosculpter");
									Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
									base.EndJobWith(JobCondition.Incompletable);
									return;
								}
							}
						}
					}
				}
				this.job.count = 1;
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x06004E0D RID: 19981 RVA: 0x001DC723 File Offset: 0x001DA923
		private Toil DropCarryToGrabIngredients()
		{
			Toil toil = ToilMaker.MakeToil("DropCarryToGrabIngredients");
			toil.initAction = delegate()
			{
				if (this.pawn.carryTracker.CarriedThing != null)
				{
					Thing thing;
					this.pawn.carryTracker.TryDropCarriedThing(this.pawn.Position, ThingPlaceMode.Near, out thing, null);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x06004E0E RID: 19982 RVA: 0x001DC748 File Offset: 0x001DA948
		public override string GetReport()
		{
			if (!this.Pod.PawnCarryingExtraCycleIngredients(this.Takee, this.job.biosculpterCycleKey, false) && !this.Pod.PawnCarryingExtraCycleIngredients(this.pawn, this.job.biosculpterCycleKey, false))
			{
				return "BiosculpterJobReportCollectIngredients".Translate();
			}
			return base.GetReport();
		}

		// Token: 0x06004E0F RID: 19983 RVA: 0x001DC7A9 File Offset: 0x001DA9A9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.pickedUpIngredients, "pickedUpIngredients", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x04003095 RID: 12437
		private const TargetIndex TakeeInd = TargetIndex.A;

		// Token: 0x04003096 RID: 12438
		private const TargetIndex IngredientInd = TargetIndex.B;

		// Token: 0x04003097 RID: 12439
		private const TargetIndex PodInd = TargetIndex.C;

		// Token: 0x04003098 RID: 12440
		private List<Thing> pickedUpIngredients = new List<Thing>();

		// Token: 0x04003099 RID: 12441
		private List<ThingCount> thingsToTransfer = new List<ThingCount>();
	}
}
