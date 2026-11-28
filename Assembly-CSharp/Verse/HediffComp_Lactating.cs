using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000452 RID: 1106
	[StaticConstructorOnStartup]
	public class HediffComp_Lactating : HediffComp_Chargeable
	{
		// Token: 0x06001F9C RID: 8092 RVA: 0x000BFDB8 File Offset: 0x000BDFB8
		public override void TryCharge(float desiredChargeAmount)
		{
			Pawn pawn = base.Pawn;
			bool flag;
			if (pawn == null)
			{
				flag = (null != null);
			}
			else
			{
				Pawn_NeedsTracker needs = pawn.needs;
				flag = (((needs != null) ? needs.food : null) != null);
			}
			if (!flag)
			{
				return;
			}
			desiredChargeAmount *= PawnUtility.BodyResourceGrowthSpeed(base.Pawn);
			float num = Mathf.Min(desiredChargeAmount, base.Pawn.needs.food.CurLevel);
			base.Pawn.needs.food.CurLevel -= num;
			base.TryCharge(num);
		}

		// Token: 0x06001F9D RID: 8093 RVA: 0x000BFE35 File Offset: 0x000BE035
		public float AddedNutritionPerDay()
		{
			return base.Props.fullChargeAmount * 60000f / (float)base.Props.ticksToFullCharge;
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06001F9E RID: 8094 RVA: 0x000BFE58 File Offset: 0x000BE058
		public override string CompTipStringExtra
		{
			get
			{
				if (base.Charge >= base.Props.fullChargeAmount)
				{
					return "LactatingStoppedBecauseFull".Translate();
				}
				float num = PawnUtility.BodyResourceGrowthSpeed(base.Pawn);
				if (num == 0f)
				{
					return "LactatingStoppedBecauseHungry".Translate().Colorize(ColorLibrary.RedReadable);
				}
				float f = this.AddedNutritionPerDay() * num;
				return "LactatingAddedNutritionPerDay".Translate(f.ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute), num);
			}
		}
	}
}
