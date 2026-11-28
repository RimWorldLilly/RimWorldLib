using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009BD RID: 2493
	public class JobDriver_BottleFeedBaby : JobDriver_FeedBaby
	{
		// Token: 0x17000DBE RID: 3518
		// (get) Token: 0x06004BA9 RID: 19369 RVA: 0x001D51A5 File Offset: 0x001D33A5
		protected Thing BabyFood
		{
			get
			{
				return base.TargetThingB;
			}
		}

		// Token: 0x17000DBF RID: 3519
		// (get) Token: 0x06004BAA RID: 19370 RVA: 0x001D51AD File Offset: 0x001D33AD
		protected LocalTargetInfo BabyFoodTarget
		{
			get
			{
				return base.TargetB;
			}
		}

		// Token: 0x17000DC0 RID: 3520
		// (get) Token: 0x06004BAB RID: 19371 RVA: 0x001D51B5 File Offset: 0x001D33B5
		// (set) Token: 0x06004BAC RID: 19372 RVA: 0x001D51BD File Offset: 0x001D33BD
		protected override Toil FeedingToil { get; set; }

		// Token: 0x06004BAD RID: 19373 RVA: 0x001D51C6 File Offset: 0x001D33C6
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_BottleFeedBaby.<MakeNewToils>d__12 <MakeNewToils>d__ = new JobDriver_BottleFeedBaby.<MakeNewToils>d__12(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004BAE RID: 19374 RVA: 0x001D51D6 File Offset: 0x001D33D6
		private Toil FailIfNoBabyFoodInInventory()
		{
			Toil toil = ToilMaker.MakeToil("FailIfNoBabyFoodInInventory");
			toil.FailOn(() => FoodUtility.BestFoodInInventory(this.pawn, base.Baby, FoodPreferability.NeverForNutrition, FoodPreferability.MealLavish, 0f, false, false) == null);
			return toil;
		}

		// Token: 0x06004BAF RID: 19375 RVA: 0x001D51F5 File Offset: 0x001D33F5
		protected override IEnumerable<Toil> FeedBaby()
		{
			JobDriver_BottleFeedBaby.<FeedBaby>d__14 <FeedBaby>d__ = new JobDriver_BottleFeedBaby.<FeedBaby>d__14(-2);
			<FeedBaby>d__.<>4__this = this;
			return <FeedBaby>d__;
		}

		// Token: 0x06004BB0 RID: 19376 RVA: 0x001D5208 File Offset: 0x001D3408
		private Toil FeedBabyFoodFromInventory()
		{
			this.FeedingToil = ToilMaker.MakeToil("FeedBabyFoodFromInventory");
			this.FeedingToil.initAction = delegate()
			{
				this.initialNutritionNeeded = base.Baby.needs.food.NutritionWanted;
				this.initialFoodPercentage = base.Baby.needs.food.CurLevelPercentage;
				base.Baby.jobs.StartJob(ChildcareUtility.MakeBabySuckleJob(this.pawn), JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
			};
			this.FeedingToil.tickIntervalAction = delegate(int delta)
			{
				float num = base.Baby.needs.food.MaxLevel / 5000f;
				float num2 = Mathf.Min(base.Baby.needs.food.NutritionWanted, num * (float)delta);
				this.bottleNutrition += num2;
				this.totalBottleNutrition += num2;
				this.pawn.GainComfortFromCellIfPossible(delta, false);
				Pawn_IdeoTracker ideo = base.Baby.ideo;
				if (ideo != null)
				{
					ideo.IncreaseIdeoExposureIfBabyTick(this.pawn.Ideo, 1);
				}
				if (!this.pawn.Downed && this.pawn.Rotation == Rot4.North)
				{
					this.pawn.Rotation = Rot4.East;
				}
				for (;;)
				{
					Thing thing = FoodUtility.BestFoodInInventory(this.pawn, base.Baby, FoodPreferability.NeverForNutrition, FoodPreferability.MealLavish, 0f, false, false);
					if (thing == null)
					{
						break;
					}
					if (this.bottleNutrition >= base.Baby.needs.food.NutritionWanted)
					{
						float num3 = thing.Ingested(base.Baby, this.bottleNutrition);
						base.Baby.records.AddTo(RecordDefOf.NutritionEaten, num3);
						this.bottleNutrition -= num3;
						base.Baby.needs.food.CurLevel = Mathf.Clamp(base.Baby.needs.food.CurLevel + num3, 0f, base.Baby.needs.food.MaxLevel);
						if (base.Baby.needs.food.CurLevel >= base.Baby.needs.food.MaxLevel)
						{
							goto Block_6;
						}
					}
					else
					{
						float num4 = FoodUtility.NutritionForEater(base.Baby, thing);
						if (this.bottleNutrition < num4)
						{
							return;
						}
						float num5 = thing.Ingested(base.Baby, num4);
						base.Baby.records.AddTo(RecordDefOf.NutritionEaten, num5);
						this.bottleNutrition -= num5;
						base.Baby.needs.food.CurLevel = Mathf.Clamp(base.Baby.needs.food.CurLevel + num5, 0f, base.Baby.needs.food.MaxLevel);
					}
				}
				base.ReadyForNextToil();
				return;
				Block_6:
				base.ReadyForNextToil();
			};
			this.FeedingToil.AddFinishAction(delegate
			{
				if (base.Baby.needs != null && base.Baby.needs.food.CurLevelPercentage - this.initialFoodPercentage > 0.6f)
				{
					base.Baby.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FedMe, this.pawn, null);
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FedBaby, base.Baby, null);
				}
				if (base.Baby.CurJobDef == JobDefOf.BabySuckle)
				{
					base.Baby.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
				}
			});
			this.FeedingToil.handlingFacing = true;
			this.FeedingToil.WithProgressBar(TargetIndex.A, () => this.totalBottleNutrition / this.initialNutritionNeeded, false, -0.5f, false);
			this.FeedingToil.defaultCompleteMode = ToilCompleteMode.Never;
			this.FeedingToil.WithEffect(EffecterDefOf.Breastfeeding, TargetIndex.A, null);
			return this.FeedingToil;
		}

		// Token: 0x06004BB1 RID: 19377 RVA: 0x001D52C4 File Offset: 0x001D34C4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.bottleNutrition, "bottleNutrition", 0f, false);
			Scribe_Values.Look<float>(ref this.totalBottleNutrition, "totalBottleNutrition", 0f, false);
			Scribe_Values.Look<float>(ref this.initialNutritionNeeded, "initialNutritionNeeded", 0f, false);
		}

		// Token: 0x04002FE0 RID: 12256
		protected const TargetIndex BabyFoodInd = TargetIndex.B;

		// Token: 0x04002FE2 RID: 12258
		private float bottleNutrition;

		// Token: 0x04002FE3 RID: 12259
		private float totalBottleNutrition;

		// Token: 0x04002FE4 RID: 12260
		private float initialNutritionNeeded;
	}
}
