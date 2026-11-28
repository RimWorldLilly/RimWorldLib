using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001582 RID: 5506
	public static class RestCategoryUtility
	{
		// Token: 0x060086F4 RID: 34548 RVA: 0x0030C678 File Offset: 0x0030A878
		public static string GetLabel(this RestCategory fatigue)
		{
			switch (fatigue)
			{
			case RestCategory.Rested:
				return "HungerLevel_Rested".Translate();
			case RestCategory.Tired:
				return "HungerLevel_Tired".Translate();
			case RestCategory.VeryTired:
				return "HungerLevel_VeryTired".Translate();
			case RestCategory.Exhausted:
				return "HungerLevel_Exhausted".Translate();
			default:
				throw new InvalidOperationException();
			}
		}
	}
}
