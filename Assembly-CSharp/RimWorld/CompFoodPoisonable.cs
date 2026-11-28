using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001896 RID: 6294
	public class CompFoodPoisonable : ThingComp
	{
		// Token: 0x17001AFE RID: 6910
		// (get) Token: 0x06009DA4 RID: 40356 RVA: 0x0038A676 File Offset: 0x00388876
		public float PoisonPercent
		{
			get
			{
				return this.poisonPct;
			}
		}

		// Token: 0x17001AFF RID: 6911
		// (get) Token: 0x06009DA5 RID: 40357 RVA: 0x0038A67E File Offset: 0x0038887E
		public FoodPoisonCause Cause
		{
			get
			{
				return this.cause;
			}
		}

		// Token: 0x06009DA6 RID: 40358 RVA: 0x0038A686 File Offset: 0x00388886
		public void SetPoisoned(FoodPoisonCause newCause)
		{
			this.poisonPct = 1f;
			this.cause = newCause;
		}

		// Token: 0x06009DA7 RID: 40359 RVA: 0x0038A69A File Offset: 0x0038889A
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.poisonPct, "poisonPct", 0f, false);
			Scribe_Values.Look<FoodPoisonCause>(ref this.cause, "cause", FoodPoisonCause.Unknown, false);
		}

		// Token: 0x06009DA8 RID: 40360 RVA: 0x0038A6CA File Offset: 0x003888CA
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			CompFoodPoisonable compFoodPoisonable = piece.TryGetComp<CompFoodPoisonable>();
			compFoodPoisonable.poisonPct = this.poisonPct;
			compFoodPoisonable.cause = this.cause;
		}

		// Token: 0x06009DA9 RID: 40361 RVA: 0x0038A6F0 File Offset: 0x003888F0
		public override void Notify_RecipeProduced(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_All);
			if (Rand.Chance((room != null) ? room.GetStat(RoomStatDefOf.FoodPoisonChance) : RoomStatDefOf.FoodPoisonChance.roomlessScore))
			{
				this.SetPoisoned(FoodPoisonCause.FilthyKitchen);
				return;
			}
			if (Rand.Chance(pawn.GetStatValue(StatDefOf.FoodPoisonChance, true, -1)))
			{
				this.SetPoisoned(FoodPoisonCause.IncompetentCook);
			}
		}

		// Token: 0x06009DAA RID: 40362 RVA: 0x0038A74C File Offset: 0x0038894C
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			base.PreAbsorbStack(otherStack, count);
			CompFoodPoisonable compFoodPoisonable = otherStack.TryGetComp<CompFoodPoisonable>();
			if (this.cause == FoodPoisonCause.Unknown && compFoodPoisonable.cause != FoodPoisonCause.Unknown)
			{
				this.cause = compFoodPoisonable.cause;
			}
			else if (compFoodPoisonable.cause != FoodPoisonCause.Unknown && this.cause != FoodPoisonCause.Unknown)
			{
				float num = this.poisonPct * (float)this.parent.stackCount;
				float num2 = compFoodPoisonable.poisonPct * (float)count;
				this.cause = ((num > num2) ? this.cause : compFoodPoisonable.cause);
			}
			this.poisonPct = GenMath.WeightedAverage(this.poisonPct, (float)this.parent.stackCount, compFoodPoisonable.poisonPct, (float)count);
		}

		// Token: 0x06009DAB RID: 40363 RVA: 0x0038A7F1 File Offset: 0x003889F1
		public override void PostIngested(Pawn ingester)
		{
			if (Rand.Chance(this.poisonPct * FoodUtility.GetFoodPoisonChanceFactor(ingester)))
			{
				FoodUtility.AddFoodPoisoningHediff(ingester, this.parent, this.cause);
			}
		}

		// Token: 0x04005D60 RID: 23904
		private float poisonPct;

		// Token: 0x04005D61 RID: 23905
		private FoodPoisonCause cause;
	}
}
