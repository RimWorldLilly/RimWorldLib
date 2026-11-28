using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001571 RID: 5489
	public static class HungerLevelUtility
	{
		// Token: 0x0600869D RID: 34461 RVA: 0x0030AA54 File Offset: 0x00308C54
		public static string GetLabel(this HungerCategory hunger)
		{
			switch (hunger)
			{
			case HungerCategory.Fed:
				return "HungerLevel_Fed".Translate();
			case HungerCategory.Hungry:
				return "HungerLevel_Hungry".Translate();
			case HungerCategory.UrgentlyHungry:
				return "HungerLevel_UrgentlyHungry".Translate();
			case HungerCategory.Starving:
				return "HungerLevel_Starving".Translate();
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x0600869E RID: 34462 RVA: 0x0030AABE File Offset: 0x00308CBE
		public static float HungerMultiplier(this HungerCategory cat)
		{
			switch (cat)
			{
			case HungerCategory.Fed:
				return 1f;
			case HungerCategory.Hungry:
				return 0.5f;
			case HungerCategory.UrgentlyHungry:
				return 0.25f;
			case HungerCategory.Starving:
				return 0f;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x040052FE RID: 21246
		public const float FallPerTickFactor_Hungry = 0.5f;

		// Token: 0x040052FF RID: 21247
		public const float FallPerTickFactor_UrgentlyHungry = 0.25f;
	}
}
