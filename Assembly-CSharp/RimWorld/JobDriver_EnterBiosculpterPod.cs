using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A2B RID: 2603
	public class JobDriver_EnterBiosculpterPod : JobDriver
	{
		// Token: 0x17000E4A RID: 3658
		// (get) Token: 0x06004E98 RID: 20120 RVA: 0x001DE3B3 File Offset: 0x001DC5B3
		private CompBiosculpterPod BiosculpterPod
		{
			get
			{
				return this.job.targetA.Thing.TryGetComp<CompBiosculpterPod>();
			}
		}

		// Token: 0x06004E99 RID: 20121 RVA: 0x001DE3CC File Offset: 0x001DC5CC
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false))
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

		// Token: 0x06004E9A RID: 20122 RVA: 0x001DE43E File Offset: 0x001DC63E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_EnterBiosculpterPod.<MakeNewToils>d__7 <MakeNewToils>d__ = new JobDriver_EnterBiosculpterPod.<MakeNewToils>d__7(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004E9B RID: 20123 RVA: 0x001DE44E File Offset: 0x001DC64E
		public override string GetReport()
		{
			if (!this.BiosculpterPod.PawnCarryingExtraCycleIngredients(this.pawn, this.job.biosculpterCycleKey, false))
			{
				return "BiosculpterJobReportCollectIngredients".Translate();
			}
			return base.GetReport();
		}

		// Token: 0x06004E9C RID: 20124 RVA: 0x001DE485 File Offset: 0x001DC685
		public static IEnumerable<Toil> CollectIngredientsToilsHelper(TargetIndex ingredientIndex, Pawn carrier, List<Thing> pickedUpIngredients)
		{
			JobDriver_EnterBiosculpterPod.<CollectIngredientsToilsHelper>d__9 <CollectIngredientsToilsHelper>d__ = new JobDriver_EnterBiosculpterPod.<CollectIngredientsToilsHelper>d__9(-2);
			<CollectIngredientsToilsHelper>d__.<>3__ingredientIndex = ingredientIndex;
			<CollectIngredientsToilsHelper>d__.<>3__carrier = carrier;
			<CollectIngredientsToilsHelper>d__.<>3__pickedUpIngredients = pickedUpIngredients;
			return <CollectIngredientsToilsHelper>d__;
		}

		// Token: 0x06004E9D RID: 20125 RVA: 0x001DE4A4 File Offset: 0x001DC6A4
		private static Toil HaulBiosculpterIngredientInInventory(Pawn carrier, List<Thing> pickedUpIngredients)
		{
			Toil toil = ToilMaker.MakeToil("HaulBiosculpterIngredientInInventory");
			toil.initAction = delegate()
			{
				Thing carriedThing = carrier.carryTracker.CarriedThing;
				Thing thing;
				carrier.carryTracker.innerContainer.TryTransferToContainer(carriedThing, carrier.inventory.innerContainer, carriedThing.stackCount, out thing, true);
				if (thing != null)
				{
					pickedUpIngredients.Add(thing);
				}
			};
			return toil;
		}

		// Token: 0x06004E9E RID: 20126 RVA: 0x001DE4E4 File Offset: 0x001DC6E4
		public static Toil PrepareToEnterToil(TargetIndex podIndex)
		{
			Toil prepare = Toils_General.Wait(70, TargetIndex.None);
			prepare.FailOnCannotTouch(podIndex, PathEndMode.InteractionCell);
			prepare.WithProgressBarToilDelay(podIndex, false, -0.5f);
			prepare.PlaySustainerOrSound(delegate()
			{
				Thing thing = prepare.actor.CurJob.GetTarget(podIndex).Thing;
				if (thing == null)
				{
					return null;
				}
				CompBiosculpterPod compBiosculpterPod = thing.TryGetComp<CompBiosculpterPod>();
				if (compBiosculpterPod == null)
				{
					return null;
				}
				return compBiosculpterPod.Props.enterSound;
			}, 1f);
			return prepare;
		}

		// Token: 0x06004E9F RID: 20127 RVA: 0x001DE55A File Offset: 0x001DC75A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.pickedUpIngredients, "pickedUpIngredients", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x040030C2 RID: 12482
		public const int EnterPodDelay = 70;

		// Token: 0x040030C3 RID: 12483
		private List<Thing> pickedUpIngredients = new List<Thing>();

		// Token: 0x040030C4 RID: 12484
		private const TargetIndex BiosculpterIndex = TargetIndex.A;

		// Token: 0x040030C5 RID: 12485
		private const TargetIndex IngredientInd = TargetIndex.B;
	}
}
