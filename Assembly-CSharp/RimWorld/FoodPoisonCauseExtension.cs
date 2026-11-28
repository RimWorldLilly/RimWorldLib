using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001895 RID: 6293
	public static class FoodPoisonCauseExtension
	{
		// Token: 0x06009DA3 RID: 40355 RVA: 0x0038A5E8 File Offset: 0x003887E8
		public static string ToStringHuman(this FoodPoisonCause cause)
		{
			switch (cause)
			{
			case FoodPoisonCause.Unknown:
				return "UnknownLower".Translate().CapitalizeFirst();
			case FoodPoisonCause.IncompetentCook:
				return "FoodPoisonCause_IncompetentCook".Translate();
			case FoodPoisonCause.FilthyKitchen:
				return "FoodPoisonCause_FilthyKitchen".Translate();
			case FoodPoisonCause.Rotten:
				return "FoodPoisonCause_Rotten".Translate();
			case FoodPoisonCause.DangerousFoodType:
				return "FoodPoisonCause_DangerousFoodType".Translate();
			default:
				return cause.ToString();
			}
		}
	}
}
