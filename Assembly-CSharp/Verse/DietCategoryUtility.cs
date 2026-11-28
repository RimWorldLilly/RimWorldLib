using System;

namespace Verse
{
	// Token: 0x0200014A RID: 330
	public static class DietCategoryUtility
	{
		// Token: 0x06000A25 RID: 2597 RVA: 0x00038A08 File Offset: 0x00036C08
		public static string ToStringHuman(this DietCategory diet)
		{
			switch (diet)
			{
			case DietCategory.NeverEats:
				return "DietCategory_NeverEats".Translate();
			case DietCategory.Herbivorous:
				return "DietCategory_Herbivorous".Translate();
			case DietCategory.Dendrovorous:
				return "DietCategory_Dendrovorous".Translate();
			case DietCategory.Ovivorous:
				return "DietCategory_Ovivorous".Translate();
			case DietCategory.Omnivorous:
				return "DietCategory_Omnivorous".Translate();
			case DietCategory.Carnivorous:
				return "DietCategory_Carnivorous".Translate();
			default:
				return "error";
			}
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x00038A9C File Offset: 0x00036C9C
		public static string ToStringHumanShort(this DietCategory diet)
		{
			switch (diet)
			{
			case DietCategory.NeverEats:
				return "DietCategory_NeverEats_Short".Translate();
			case DietCategory.Herbivorous:
				return "DietCategory_Herbivorous_Short".Translate();
			case DietCategory.Dendrovorous:
				return "DietCategory_Dendrovorous_Short".Translate();
			case DietCategory.Ovivorous:
				return "DietCategory_Ovivorous_Short".Translate();
			case DietCategory.Omnivorous:
				return "DietCategory_Omnivorous_Short".Translate();
			case DietCategory.Carnivorous:
				return "DietCategory_Carnivorous_Short".Translate();
			default:
				return "error";
			}
		}
	}
}
