using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009A5 RID: 2469
	public abstract class JobDriver_InteractAnimal : JobDriver
	{
		// Token: 0x17000DA5 RID: 3493
		// (get) Token: 0x06004AC7 RID: 19143 RVA: 0x001D1C5B File Offset: 0x001CFE5B
		protected Pawn Animal
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x17000DA6 RID: 3494
		// (get) Token: 0x06004AC8 RID: 19144 RVA: 0x001D1C72 File Offset: 0x001CFE72
		protected virtual bool CanInteractNow
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000DA7 RID: 3495
		// (get) Token: 0x06004AC9 RID: 19145 RVA: 0x001D1C75 File Offset: 0x001CFE75
		protected virtual bool CanFeedEver
		{
			get
			{
				Pawn animal = this.Animal;
				object obj;
				if (animal == null)
				{
					obj = null;
				}
				else
				{
					Pawn_NeedsTracker needs = animal.needs;
					obj = ((needs != null) ? needs.food : null);
				}
				return obj != null;
			}
		}

		// Token: 0x06004ACA RID: 19146 RVA: 0x001D1C98 File Offset: 0x001CFE98
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.feedNutritionLeft, "feedNutritionLeft", 0f, false);
		}

		// Token: 0x06004ACB RID: 19147 RVA: 0x001D1CB6 File Offset: 0x001CFEB6
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Animal, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004ACC RID: 19148 RVA: 0x001D1CD9 File Offset: 0x001CFED9
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_InteractAnimal.<MakeNewToils>d__17 <MakeNewToils>d__ = new JobDriver_InteractAnimal.<MakeNewToils>d__17(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004ACD RID: 19149 RVA: 0x001D1CE9 File Offset: 0x001CFEE9
		public static float RequiredNutritionPerFeed(Pawn animal)
		{
			if (animal.needs.food == null)
			{
				return 0f;
			}
			return Mathf.Min(animal.needs.food.MaxLevel * 0.15f, 0.3f);
		}

		// Token: 0x06004ACE RID: 19150 RVA: 0x001D1D1E File Offset: 0x001CFF1E
		private IEnumerable<Toil> FeedToils()
		{
			JobDriver_InteractAnimal.<FeedToils>d__19 <FeedToils>d__ = new JobDriver_InteractAnimal.<FeedToils>d__19(-2);
			<FeedToils>d__.<>4__this = this;
			return <FeedToils>d__;
		}

		// Token: 0x06004ACF RID: 19151 RVA: 0x001D1D30 File Offset: 0x001CFF30
		private Toil TalkToAnimal(TargetIndex tameeInd)
		{
			Toil toil = ToilMaker.MakeToil("TalkToAnimal");
			toil.initAction = delegate()
			{
				Pawn actor = toil.GetActor();
				Pawn recipient = (Pawn)((Thing)actor.CurJob.GetTarget(tameeInd));
				actor.interactions.TryInteractWith(recipient, InteractionDefOf.AnimalChat);
			};
			toil.FailOn(() => !this.CanInteractNow);
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 270;
			toil.activeSkill = (() => SkillDefOf.Animals);
			return toil;
		}

		// Token: 0x06004AD0 RID: 19152 RVA: 0x001D1DDC File Offset: 0x001CFFDC
		private Toil StartFeedAnimal(TargetIndex tameeInd)
		{
			Toil toil = ToilMaker.MakeToil("StartFeedAnimal");
			toil.initAction = delegate()
			{
				Pawn actor = toil.GetActor();
				Pawn pawn = (Pawn)((Thing)actor.CurJob.GetTarget(tameeInd));
				PawnUtility.ForceWait(pawn, 270, actor, false, false);
				Thing thing = FoodUtility.BestFoodInInventory(actor, pawn, FoodPreferability.NeverForNutrition, FoodPreferability.RawTasty, 0f, false, false);
				if (thing == null)
				{
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				actor.mindState.lastInventoryRawFoodUseTick = Find.TickManager.TicksGame;
				int num = FoodUtility.StackCountForNutrition(this.feedNutritionLeft, thing.GetStatValue(StatDefOf.Nutrition, true, -1));
				int stackCount = thing.stackCount;
				Thing thing2 = actor.inventory.innerContainer.Take(thing, Mathf.Min(num, stackCount));
				actor.carryTracker.TryStartCarry(thing2);
				actor.CurJob.SetTarget(TargetIndex.B, thing2);
				float num2 = (float)thing2.stackCount * thing2.GetStatValue(StatDefOf.Nutrition, true, -1);
				this.ticksLeftThisToil = Mathf.CeilToInt(270f * (num2 / JobDriver_InteractAnimal.RequiredNutritionPerFeed(pawn)));
				if (num <= stackCount)
				{
					this.feedNutritionLeft = 0f;
					return;
				}
				this.feedNutritionLeft -= num2;
				if (this.feedNutritionLeft < 0.001f)
				{
					this.feedNutritionLeft = 0f;
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.activeSkill = (() => SkillDefOf.Animals);
			return toil;
		}

		// Token: 0x04002FA3 RID: 12195
		protected const TargetIndex AnimalInd = TargetIndex.A;

		// Token: 0x04002FA4 RID: 12196
		private const TargetIndex FoodHandInd = TargetIndex.B;

		// Token: 0x04002FA5 RID: 12197
		private const int FeedDuration = 270;

		// Token: 0x04002FA6 RID: 12198
		private const int TalkDuration = 270;

		// Token: 0x04002FA7 RID: 12199
		private const float NutritionPercentagePerFeed = 0.15f;

		// Token: 0x04002FA8 RID: 12200
		private const float MaxMinNutritionPerFeed = 0.3f;

		// Token: 0x04002FA9 RID: 12201
		public const int FeedCount = 2;

		// Token: 0x04002FAA RID: 12202
		public const FoodPreferability MaxFoodPreferability = FoodPreferability.RawTasty;

		// Token: 0x04002FAB RID: 12203
		private float feedNutritionLeft;
	}
}
