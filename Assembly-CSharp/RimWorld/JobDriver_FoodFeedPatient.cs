using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AA0 RID: 2720
	public class JobDriver_FoodFeedPatient : JobDriver
	{
		// Token: 0x17000EDD RID: 3805
		// (get) Token: 0x060051D9 RID: 20953 RVA: 0x001E83E6 File Offset: 0x001E65E6
		protected Thing Food
		{
			get
			{
				return this.job.targetA.Thing;
			}
		}

		// Token: 0x17000EDE RID: 3806
		// (get) Token: 0x060051DA RID: 20954 RVA: 0x001E83F8 File Offset: 0x001E65F8
		protected Pawn Deliveree
		{
			get
			{
				return this.job.targetB.Pawn;
			}
		}

		// Token: 0x17000EDF RID: 3807
		// (get) Token: 0x060051DB RID: 20955 RVA: 0x001E840A File Offset: 0x001E660A
		protected Pawn_InventoryTracker FoodHolderInventory
		{
			get
			{
				Thing food = this.Food;
				return ((food != null) ? food.ParentHolder : null) as Pawn_InventoryTracker;
			}
		}

		// Token: 0x17000EE0 RID: 3808
		// (get) Token: 0x060051DC RID: 20956 RVA: 0x001E8423 File Offset: 0x001E6623
		protected Pawn FoodHolder
		{
			get
			{
				return this.job.targetC.Pawn;
			}
		}

		// Token: 0x060051DD RID: 20957 RVA: 0x001E8438 File Offset: 0x001E6638
		public override string GetReport()
		{
			if (this.job.GetTarget(TargetIndex.A).Thing is Building_NutrientPasteDispenser && this.Deliveree != null)
			{
				return JobUtility.GetResolvedJobReportRaw(this.job.def.reportString, ThingDefOf.MealNutrientPaste.label, ThingDefOf.MealNutrientPaste, this.Deliveree.LabelShort, this.Deliveree, "", "");
			}
			return base.GetReport();
		}

		// Token: 0x060051DE RID: 20958 RVA: 0x001E84B0 File Offset: 0x001E66B0
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!this.pawn.Reserve(this.Deliveree, this.job, 1, -1, null, errorOnFailed, false))
			{
				return false;
			}
			if (!(base.TargetThingA is Building_NutrientPasteDispenser) && (this.pawn.inventory == null || !this.pawn.inventory.Contains(base.TargetThingA)))
			{
				int maxAmountToPickup = FoodUtility.GetMaxAmountToPickup(this.Food, this.pawn, this.job.count);
				if (!this.pawn.Reserve(this.Food, this.job, 10, maxAmountToPickup, null, errorOnFailed, false))
				{
					return false;
				}
				this.job.count = maxAmountToPickup;
			}
			return true;
		}

		// Token: 0x060051DF RID: 20959 RVA: 0x001E8564 File Offset: 0x001E6764
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_FoodFeedPatient.<MakeNewToils>d__15 <MakeNewToils>d__ = new JobDriver_FoodFeedPatient.<MakeNewToils>d__15(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x040031A4 RID: 12708
		private const TargetIndex FoodSourceInd = TargetIndex.A;

		// Token: 0x040031A5 RID: 12709
		private const TargetIndex DelivereeInd = TargetIndex.B;

		// Token: 0x040031A6 RID: 12710
		private const TargetIndex FoodHolderInd = TargetIndex.C;

		// Token: 0x040031A7 RID: 12711
		private const float FeedDurationMultiplier = 1.5f;

		// Token: 0x040031A8 RID: 12712
		private const float MetalhorrorInfectionChance = 0.3f;
	}
}
